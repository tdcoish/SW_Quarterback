/*************************************************************************************
The size of the play can be variable, so we need some way of knowing the size.
Let's start with just throwing the players in the right spots, and trying to render multiples
of that.

So we have to load in a play, then throw this play onto a page.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PB_DisplayPlay : MonoBehaviour
{
    public PB_Play                      rField;
    public GameObject                   GFX_Player;
    public GameObject                   GFX_RouteNode;

    public string                       mPlayName;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)){
            FDisplayPlay();
        }
    }

    public void FDisplayPlay()
    {
        DATA_Play ply = IO_PlayList.FLOAD_PLAY_BY_NAME(mPlayName);
        if(ply == null){
            Debug.Log("That play doesn't exist");
            return;
        }

        foreach(DT_PlayerRole guy in ply.mPlayerRoles)
        {
            var clone = Instantiate(GFX_Player, transform.position, transform.rotation);
            Vector2 vGFXSpawn = new Vector2();
            vGFXSpawn = guy.mStart;
            // Now we do this conversion for the 1000x time.
            vGFXSpawn *= rField.GetComponent<RectTransform>().rect.width / 50f;     // field is 50 meters. 
            vGFXSpawn += (Vector2)rField.transform.position;
            vGFXSpawn *= 1000f;
            GFX_Player gfx = clone.GetComponentInChildren<GFX_Player>();
            Debug.Log("Current transform: " + gfx.GetComponent<RectTransform>().position);
            clone.GetComponentInChildren<RectTransform>().position = vGFXSpawn;
            clone.transform.position = vGFXSpawn;
            Debug.Log("New transform: " + gfx.GetComponent<RectTransform>().position);
        }
    }
}
