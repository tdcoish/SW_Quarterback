/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PE_Editor : MonoBehaviour
{
    // This needs to be a string, but save a play first.
    public string                           mDefaultPlay = "DEFAULT";

    public InputField                       rPlayNameField;

    public PE_JobAssigner                   rJobAssigner;

    private PE_PlayLoader                   cLoader;
    private PE_PlaySaver                    cSaver;

    void Start()
    {
        IO_PlayList.FLOAD_PLAYS();

        cLoader = GetComponent<PE_PlayLoader>();
        cSaver = GetComponent<PE_PlaySaver>();
    }

    // Spawn in a default play, with nothing in it, so we can edit that play.
    public void OnNewPlay()
    {
        cLoader.FLoadPlay(IO_PlayList.FLOAD_PLAY_BY_NAME("DEFAULT"));
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

    public void OnPlayerSelected()
    {
        rJobAssigner.gameObject.SetActive(true);
        rJobAssigner.FSetDropdownValues();
    }
    public void OnPlayerDeselected()
    {
        rJobAssigner.gameObject.SetActive(false);
    }
}
