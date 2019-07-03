/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class RT_Player : MonoBehaviour
{
    [SerializeField]
    private GameObject      mClickedBar;

    public bool             mIsChosen = false;

    public string           mTag = "WR1";

    void Start()
    {
    }

    void Update()
    {

        // if we're active, then activate the clicked bar.
        mClickedBar.SetActive(mIsChosen);

        if(mIsChosen){
            //FollowMouse();
        }
    }

    // make the entity follow the mouse
    void FollowMouse()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = 100f;
        pos = Camera.main.ScreenToWorldPoint(pos);

        transform.position = Vector2.Lerp(transform.position, pos, 0.1f);
        Debug.Log("Mouse Pos: " + transform.position);
    }
}
