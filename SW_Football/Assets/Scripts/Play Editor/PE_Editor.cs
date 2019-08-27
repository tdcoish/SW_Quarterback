/*************************************************************************************
Okay. It's time to add some fucking states to this whole shebang.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PE_Editor : MonoBehaviour
{

    public enum EDITOR_STATE
    {
        SNOTHINGSELECTED,
        SPLAYERSELECTED,
        SROUTENODESELECTED
    }
    public EDITOR_STATE                     mState;

    // This needs to be a string, but save a play first.
    public string                           mDefaultPlay = "DEFAULT";

    public InputField                       rPlayNameField;

    public PE_JobAssigner                   rJobAssigner;

    private PE_PlayLoader                   cLoader;
    private PE_PlaySaver                    cSaver;
    private PE_DisplayPlayJobs              cJobDisplayer;

    void Start()
    {
        cLoader = GetComponent<PE_PlayLoader>();
        cSaver = GetComponent<PE_PlaySaver>();
        cJobDisplayer = GetComponent<PE_DisplayPlayJobs>();

        mState = EDITOR_STATE.SNOTHINGSELECTED;
    }

    // Spawn in a default play, with nothing in it, so we can edit that play.
    public void OnNewPlay()
    {
        cLoader.FLoadPlay(IO_PlayList.FLOAD_PLAY_BY_NAME(mDefaultPlay));
        
        // AND NOW WE HAVE TO WRITE SOMETHING LIKE: DISPLAYPLAY();
        cJobDisplayer.FDisplayJobs();
    }

    public void OnPlaySaved()
    {
        if(rPlayNameField.text == string.Empty)
        {
            Debug.Log("Trying to save un-named play");
            return;
        }

        // 2 stages
        // convert to DATA_Play
        // write that to disk
        IO_PlayList.FWRITE_PLAY(cSaver.FConvertPlayToDATA(rPlayNameField.text));
    }

    // Note, we should be greying out the job assigner screen.
    public void OnPlayerSelected()
    {
        mState = EDITOR_STATE.SPLAYERSELECTED;
        rJobAssigner.gameObject.SetActive(true);
        rJobAssigner.FSetDropdownValues();
        rJobAssigner.mState = PE_JobAssigner.ASSIGNER_STATE.SNEW_PLAYER_SELECTED;
    }
    public void OnPlayerDeselected()
    {
        mState = EDITOR_STATE.SNOTHINGSELECTED;
        rJobAssigner.mState = PE_JobAssigner.ASSIGNER_STATE.SNONE_SELECTED;
        rJobAssigner.gameObject.SetActive(false);
    }
}
