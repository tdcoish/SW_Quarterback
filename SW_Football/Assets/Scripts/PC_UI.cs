using UnityEngine;
using UnityEngine.UI;

public class PC_UI : MonoBehaviour
{

    public Image            mBar;

    // if the pc_controller is winding up.
    private bool            mIsWindingUp;

    [SerializeField]
    private GameObject          RefFootballPathNode;

    [SerializeField]
    private SO_Transform        RefPlayerPos;
    [SerializeField]
    private SO_Transform        RefPlayerCamera;

    private float               chargePct;

    // Start is called before the first frame update
    void Start()
    {
        mBar = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // render balls along trajectory
        if(mIsWindingUp){
            // shouldn't be too hard to calculate the x and y trajectory.
            float y = RefPlayerCamera.Val.position.y;
            Vector3 dir = RefPlayerCamera.Val.forward;

            // calculate the time it will take for the football to come down to the ground.
            // for now just hardcode the throw power as 10f
        }
    }

    public void ThrowBar(float chrgPct)
    {
        mBar.fillAmount = chrgPct;
        chargePct = chrgPct;
    }

    public void QB_Charging(){
        mIsWindingUp = true;
    }

    public void QB_ThrewBall(){
        mIsWindingUp = false;
    }
    
}
