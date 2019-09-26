/*************************************************************************************
Controls the splash screen intro. Has to play some sound effects, as well as async load the
main menu.
Also, load everything in the game.

As of right now we're going to be trying to create all the play art that we need, through a .bat
file.

Okay there seem to be some issues. I think Unity takes a little bit to load in the files or recognize
them or whatever, so you have to load them in update once, which I hope is reliable.

mAudioSrc.clip.length;


Texture2D.LoadImage((pnghere)) seems promising. Apparently we can just shove png's into there.
Maybe then I can create sprites right then and there.
That's legacy. I probably have to create a color[] array, then make a texture using 
Texture2D.SetPixels();
Wait, I'm seeing ImageConversion.LoadImage(Texture2d tex, byte[] data, bool markNonReadable)
which apparently can be used to load in .png files.
Maybe not, you need to save that with the .bytes extension.
Alright, I think I might need to be using C# .Net image stuffs. File.ReadAllBytes(path)
might be a good place to start. Also, Bitmap is a great idea apparently. 

For the record, I had massive issues importing System.Drawing. Got the correct answer from
here: https://gamedev.stackexchange.com/questions/133372/system-drawing-dll-not-found/147506

Note, the %UnityFolder% referenced is the installation folder of Unity. My project now feels 
very fragile, but it does appear to be working.

Lol, turns out I might not have needed to do this anyways.

Ideally, I'd just save the new files as sprites, but I'm not sure how to do that.
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Diagnostics;
using System.IO;
using System.Drawing;

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

    public Text                     mDebugText;

    private SplashState             mState;

    public bool                     mPrintPlays = false;

    void Start()
    {
        IO_DefPlays.FLOAD_PLAYS();
        IO_PlayList.FLOAD_PLAYS();
        IO_RouteList.FLOAD_ROUTES();
        IO_Settings.FLOAD_SETTINGS();
        IO_ZoneList.FLOAD_ZONES();

        mDebugText.text = "Loading in stuffs";

        // IO_RouteList.FWRITE_ALL_ROUTES_AS_TEXT();
        DeleteOldFilesFromPlayArtDirectories();
        TransferCurrentFormationsAndPlaysIntoPlayArtDirectories();

        mDebugText.text = "Should have transfered text files with formation and plays";

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
            mDebugText.text = "Should have created all the plays";
            TransferCreatedFiles();
            mPrintPlays = false;
        }
    }

    // Once the jingle is done, we switch to the next screen.
    private void ENTER_Logo()
    {
        mDebugText.text = "ENTER_Logo has been called";

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
        mDebugText.text = "About to start the process";
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

    /*********************************************************************************
    This is going to convert those files textures. dfdfdfdf
    ******************************************************************************** */
    private void TransferCreatedFiles()
    {
        // string newDir = Application.dataPath+"/Resources/PlayArt/Offense/";
        string newDir = Application.dataPath+"/FILE_IO/PlayArt/Offense/";
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
            // byte[] fileData;
            // Texture2D tex = null;
            // fileData = File.ReadAllBytes(s);
            // tex = new Texture2D(256, 256);
            // tex.LoadImage(fileData);

            File.Copy(s, newPath, true);
        }
    }

    private void TransferCurrentFormationsAndPlaysIntoPlayArtDirectories()
    {
        string formDir = Application.dataPath+"/PLAYART_CREATION/Formations/";
        string textPlayDir = Application.dataPath+"/PLAYART_CREATION/OffensivePlays/";
        string oldFormDir = Application.dataPath+"/FILE_IO/Formations/";
        string oldPlayDir = Application.dataPath+"/FILE_IO/OffensivePlays/";

        string[] plyFiles = Directory.GetFiles(oldPlayDir, "*.txt");
        foreach(string s in plyFiles)
        {
            string playNameWithoutPath = s.Substring(oldPlayDir.Length);
            File.Copy(s, textPlayDir+playNameWithoutPath, true);
        }

        string[] formFiles = Directory.GetFiles(oldFormDir, "*.txt");
        foreach(string s in formFiles)
        {
            string formNameWithoutPath = s.Substring(oldFormDir.Length);
            File.Copy(s, formDir+formNameWithoutPath, true);
        }

    }
    /****************************************************************************
    I think I found the problem. There is no resources folder. Unity just uses that to make streamable 
    assets. We have to save to somewhere else. This works for the Editor, but the build will be broken.
    ************************************************************************** */
    // Called at start, deletes the old files from play art directory and resources.
    private void DeleteOldFilesFromPlayArtDirectories()
    {
        string resDir = Application.dataPath+"/FILE_IO/PlayArt/Offense/";
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

        string formDir = Application.dataPath+"/PLAYART_CREATION/Formations/";
        string textPlays = Application.dataPath+"/PLAYART_CREATION/OffensivePlays/";
        string[] textPlyFiles = Directory.GetFiles(textPlays);
        string[] formFiles = Directory.GetFiles(formDir);
        foreach(string s in textPlyFiles){
            File.Delete(s);
        }
        foreach(string s in formFiles){
            File.Delete(s);
        }
    }

}
