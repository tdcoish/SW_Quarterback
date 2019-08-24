/*************************************************************************************

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PE_Editor : MonoBehaviour
{
    public DT_OffencePlay                   FL_CreatedPlay;
    public DT_OffencePlay                   FL_DefaultPlay;

    public InputField                       rPlayNameField;

    public PE_JobAssigner                   rJobAssigner;

    private PE_PlayLoader                   cLoader;
    private PE_PlaySaver                    cSaver;

    void Start()
    {
        cLoader = GetComponent<PE_PlayLoader>();
        cSaver = GetComponent<PE_PlaySaver>();
    }

    // Spawn in a default play, with nothing in it, so we can edit that play.
    public void OnNewPlay()
    {
        cLoader.FLoadPlay(FL_DefaultPlay);
    }

    public void OnPlaySaved()
    {
        if(rPlayNameField.text == string.Empty)
        {
            Debug.Log("Trying to save un-named play");
            return;
        }

        cSaver.FSavePlay(rPlayNameField.text, FL_CreatedPlay);
    }
}
