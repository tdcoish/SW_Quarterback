/*************************************************************************************
This should theoretically give you a Vector2 representing your distance in game meters
from the snap spot.

Want to convert a unit in the gameworld, to a position, in yards, on the field.
*************************************************************************************/
using UnityEngine;

public class PE_PixToMeters : MonoBehaviour
{

    public Vector2 FConvertWorldSpaceToYardsFromCenter(float fFieldW, PE_Field rField, Vector2 vWorldPos)
    {
        float fMetersToPixels = fFieldW / rField.GetComponent<RectTransform>().rect.width;
        Vector2 vPos = new Vector2();
        vPos.x = (vWorldPos.x - rField.transform.position.x) / fMetersToPixels;
        vPos.y = (vWorldPos.y - rField.transform.position.y) / fMetersToPixels;

        return vPos;
    }
}