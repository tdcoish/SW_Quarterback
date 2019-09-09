/*************************************************************************************
Controls the splash screen intro. Has to play some sound effects, as well as async load the
main menu.
Also, load everything in the game.

mAudioSrc.clip.length;
*************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public AudioSource              sfx_logo;
    public AudioSource              sfx_copyright;

    private SplashState             mState;

    void Start()
    {
        IO_DefPlays.FLOAD_PLAYS();
        IO_PlayList.FLOAD_PLAYS();
        IO_RouteList.FLOAD_ROUTES();
        IO_Settings.FLOAD_SETTINGS();
        IO_ZoneList.FLOAD_ZONES();

        // IO_RouteList.FWRITE_ALL_ROUTES_AS_TEXT();

        mState = SplashState.SLOGO;

        ENTER_Logo();
    }

    void Update()
    {
        switch(mState)
        {
            case SplashState.SLOGO: RUN_Logo(); break;
            case SplashState.SCOPYRIGHT: RUN_Copyright(); break;
        }
    }

    // Once the jingle is done, we switch to the next screen.
    private void ENTER_Logo()
    {
        mState = SplashState.SLOGO;
        sfx_logo.Play();
        mTime = Time.time;
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
}
