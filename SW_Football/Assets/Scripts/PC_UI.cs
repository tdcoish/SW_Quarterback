using UnityEngine;
using UnityEngine.UI;

public class PC_UI : MonoBehaviour
{

    public Image            mBar;

    // Start is called before the first frame update
    void Start()
    {
        mBar = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ThrowBar(float chrgPct)
    {
        mBar.fillAmount = chrgPct;
    }
}
