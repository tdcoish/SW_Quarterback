/*************************************************************************************
Makes a graphic follow the mouse around the screen.
*************************************************************************************/
using UnityEngine;

public class RT_Mouse : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 100f;
        transform.position = Camera.main.ScreenToWorldPoint(pos);

        // Debug.Log("Pos: " + transform.position);

    }
}
