/*************************************************************************************
Just a black image that fades in and out really quickly.    
*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class MN_Transition : MonoBehaviour
{

    private bool                mFadingIn = true;
    private Image               mImg;

    public float                mFadeSpd = 10f;     // so it takes 1/x seconds to fade from invisible to full
    public float                mMaxAlpha = 0.5f;

    public GameObject           PF_AudioSwoosh;

    void Start()
    {
        Instantiate(PF_AudioSwoosh);
        
        mImg = GetComponent<Image>();

        Color col = mImg.color;
        col.a = 0f;
        mImg.color = col;   
    }

    void Update()
    {
        if(mFadingIn)
        {
            Color col = mImg.color;
            col.a += Time.deltaTime * mFadeSpd;
            if(col.a > mMaxAlpha){
                mFadingIn = false;
            }
            mImg.color = col;
        }else{
            Color col = mImg.color;
            col.a -= Time.deltaTime * mFadeSpd;
            mImg.color = col;
            if(col.a < 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
