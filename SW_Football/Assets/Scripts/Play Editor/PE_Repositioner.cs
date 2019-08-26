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
        RepositionStart(new Vector2(0f, 1f));
    }

    public void BT_Down()
    {
        RepositionStart(new Vector2(0f, -1f));
    }

    public void BT_Right()
    {
        RepositionStart(new Vector2(1f, 0f));
    }

    public void BT_Left()
    {
        RepositionStart(new Vector2(-1f, 0f));
    }

    public void BT_Up_More()
    {
        RepositionStart(new Vector2(0f, 5f));
    }

    public void BT_Down_More()
    {
        RepositionStart(new Vector2(0f, -5f));
    }

    public void BT_Right_More()
    {
        RepositionStart(new Vector2(5f, 0f));
    }

    public void BT_Left_More()
    {
        RepositionStart(new Vector2(-5f, 0f));
    }

    private void RepositionStart(Vector2 vMovement)
    {
        if(cSelector.mActivePlayer == -1)
        {
            Debug.Log("There is no active player");
            return;
        }

        Debug.Log("Shift active player by: " + vMovement);
        PE_Role role = cSelector.rGuys[cSelector.mActivePlayer].GetComponent<PE_Role>();
        role.mStartPos += vMovement;
        role.mStartPos.x = (float)System.Math.Round((double)role.mStartPos.x, 0);
        role.mStartPos.y = (float)System.Math.Round((double)role.mStartPos.y, 0);

        cSelector.rGuys[cSelector.mActivePlayer].transform.position += (Vector3)vMovement / 10f;        // hacked conversion.
        // Again, unfortunately we have to convert from internal position to field position.

        UpdatePositionText();
    }

    // Because you can't line up 5.3233490 yards to the left, only 5 or 6. Maybe I'll give them half yards. 
    private void SnapPosToYard()
    {

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
