/*************************************************************************************
Controls the splash screen intro. Has to play some sound effects, as well as async load the
main menu.
Also, load everything in the game.

As of right now we're going to be trying to create all the play art that we need, through a .bat
file.

Okay there seem to be some issues. I think Unity takes a little bit to load in the files or recognize
them or whatever, so you have to load them in update once, which I hope is reliable.

mAudioSrc.clip.length;
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Diagnostics;
using System.IO;

// For now only works with this, looking to make this more broadly
public abstract class tdcState
{
    public abstract void OnEnter(Object data);
    public abstract void Run(Object data);
    public abstract void OnExit(Object data);
}

public class MN_Splash : MonoBehaviour
{
    private enum SplashState{
        SLOGO,
        SCOPYRIGHT,
        SNOTHING
    }

    private float                   mTime;

    public GameObject               mLogoScreen;
    public GameObject               mCopyrightScreen;

    public AudioMixer               mAudioMixer;
    public AudioMixerSnapshot       SNP_Paused;
    public AudioSource              sfx_logo;
    public AudioSource              sfx_copyright;

    private SplashState             mState;

    public bool                     mPrintPlays = false;

    void Start()
    {
        IO_DefPlays.FLOAD_PLAYS();
        IO_PlayList.FLOAD_PLAYS();
        IO_RouteList.FLOAD_ROUTES();
        IO_Settings.FLOAD_SETTINGS();
        IO_ZoneList.FLOAD_ZONES();

        // IO_RouteList.FWRITE_ALL_ROUTES_AS_TEXT();
        DeleteOldFiles();

        mState = SplashState.SLOGO;
        mAudioMixer.SetFloat("MASTER_VOLUME", IO_Settings.mSet.lMasterVolume);

        ENTER_Logo();
    }

    void Update()
    {
        switch(mState)
        {
            case SplashState.SLOGO: RUN_Logo(); break;
            case SplashState.SCOPYRIGHT: RUN_Copyright(); break;
        }

        if(Input.anyKeyDown){
		    SNP_Paused.TransitionTo(0.5f);
            SceneManager.LoadScene("SN_MN_Main");
        }

        if(mPrintPlays){
            // PrintCreatedFiles();
            TransferCreatedFiles();
            mPrintPlays = false;
        }
    }

    // Once the jingle is done, we switch to the next screen.
    private void ENTER_Logo()
    {
        mState = SplashState.SLOGO;
        sfx_logo.Play();
        mTime = Time.time;

        // Now is as good a time as any to try to execute the program.
        Process p = new Process();
        p.StartInfo.UseShellExecute = false;
        string path = Application.dataPath+"/PLAYART_CREATION/PlayArtCreator.exe";
        p.StartInfo.FileName = path;
        p.StartInfo.CreateNoWindow = true;
        p.EnableRaisingEvents = true;
        p.Exited += new System.EventHandler(PROC_PlayArtExited);
        p.Start();
    }
    private void RUN_Logo()
    {
        if(Time.time - mTime > sfx_logo.clip.length)
        {
            EXIT_Logo();
            ENTER_Copyright();
        }
    }
    private void EXIT_Logo()
    {
        mLogoScreen.SetActive(false);
    }

    private void ENTER_Copyright()
    {
        mCopyrightScreen.SetActive(true);
        mState = SplashState.SCOPYRIGHT;
        sfx_copyright.Play();
        mTime = Time.time;
    }
    private void RUN_Copyright()
    {
        if(Time.time - mTime > sfx_copyright.clip.length)
        {
            SceneManager.LoadScene("SN_MN_Main");
        }
    }

    private void PROC_PlayArtExited(object sender, System.EventArgs e)
    {
        UnityEngine.Debug.Log("Plays have been created");
        UnityEngine.Debug.Log("This is where we should be transfering the files over");

        mPrintPlays = true;
    }

    private void PrintCreatedFiles()
    {
        string path = Application.dataPath+"/PLAYART_CREATION/PlayArt/Offense/";
        // Now I want to print out the directory where I should have made the files.
        string[] files = Directory.GetFiles(path, "*.png");
        foreach(string s in files)
        {
            UnityEngine.Debug.Log("Created this file: " + s);
        }
    }

    private void TransferCreatedFiles()
    {
        string newDir = Application.dataPath+"/Resources/PlayArt/Offense/";
        string oldDir = Application.dataPath+"/PLAYART_CREATION/PlayArt/Offense/";
        // gonna use the length of oldPath as a ghetto way of getting the filename using substring.

        string[] files = Directory.GetFiles(oldDir, "*.png");
        foreach(string s in files)
        {
            // transfer the png over.
            string playNameWithoutPath = s.Substring(oldDir.Length);
            UnityEngine.Debug.Log("Playname alone: " + playNameWithoutPath);
            string newPath = newDir + playNameWithoutPath;
            UnityEngine.Debug.Log("New Path: " + newPath);

            // I have to admit, I'm impressed it's that easy.
            System.IO.File.Copy(s, newPath);
        }
    }

    // Called at start, deletes the old files from play art directory and resources.
    private void DeleteOldFiles()
    {
        string resDir = Application.dataPath+"/Resources/PlayArt/Offense/";
        string plyDir = Application.dataPath+"/PLAYART_CREATION/PlayArt/Offense/";

        // Turns out we want to delete the .meta files as well.
        string[] plyFiles = Directory.GetFiles(plyDir);
        string[] resFiles = Directory.GetFiles(resDir);
        foreach(string s in plyFiles){
            File.Delete(s);
        }
        foreach(string s in resFiles){
            File.Delete(s);
        }
    }
}
