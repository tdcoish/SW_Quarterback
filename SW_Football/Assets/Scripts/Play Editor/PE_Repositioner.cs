/*************************************************************************************
When they have selected a player, we reposition them using this script.
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class PE_Repositioner : MonoBehaviour
{
    private PE_Selector         cSelector;

    [SerializeField]
    private Text                rPositionTXT;

    private void Start()
    {
        cSelector = GetComponent<PE_Selector>();
    }
    
    public void BT_Up()
    {
        Debug.Log("Up pressed");
        RepositionStart(new Vector2(0f, 1f));
    }

    public void BT_Down()
    {
        Debug.Log("Down pressed");
        RepositionStart(new Vector2(0f, -1f));
    }

    public void BT_Right()
    {
        Debug.Log("Right pressed");
        RepositionStart(new Vector2(1f, 0f));
    }

    public void BT_Left()
    {
        Debug.Log("Left pressed");
        RepositionStart(new Vector2(-1f, 0f));
    }

    private void RepositionStart(Vector2 vMovement)
    {
        if(cSelector.mActivePlayer == -1)
        {
            Debug.Log("There is no active player");
            return;
        }

        Debug.Log("Shift active player by: " + vMovement);
        cSelector.rGuys[cSelector.mActivePlayer].GetComponent<PE_Role>().mStartPos += vMovement;
        cSelector.rGuys[cSelector.mActivePlayer].transform.position += (Vector3)vMovement / 10f;        // hacked conversion.
        // Again, unfortunately we have to convert from internal position to field position.

        UpdatePositionText();
    }

    // Probably make this public.
    private void UpdatePositionText()
    {
        if(cSelector.mActivePlayer == -1)
        {
            Debug.Log("There is no active player");
            return;
        }

        rPositionTXT.text = "POS: " + cSelector.rGuys[cSelector.mActivePlayer].GetComponent<PE_Role>().mStartPos;
    }
}
