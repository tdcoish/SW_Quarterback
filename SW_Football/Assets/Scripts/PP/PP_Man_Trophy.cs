/*************************************************************************************

*************************************************************************************/
using UnityEngine;

public class PP_Man_Trophy : MonoBehaviour
{
    private PP_Manager          cPPMan;

    public Vector3              mTrophySpawnPos;
    public DT_TrophyValues      GB_TrophyValues;
    [HideInInspector]
    public string               mTrophyWon = "NONE";

    public Material             MAT_Bronze;
    public Material             MAT_Silver;
    public Material             MAT_Gold;

    public GameObject           PF_Particles;
    public PP_Trophy            PF_Trophy;
    
    private void Start()
    {
        cPPMan = GetComponent<PP_Manager>();
    }

    public void FHandleTrophyAfterScore()
    {
        string trophyWon = "NONE";
        if(cPPMan.mScore >= GB_TrophyValues.mGold){
            trophyWon = "GOLD";
        }else if(cPPMan.mScore >= GB_TrophyValues.mSilver){
            trophyWon = "SILVER";
        }else if(cPPMan.mScore >= GB_TrophyValues.mBronze){
            trophyWon = "BRONZE";
        }

        // This means they just won a trophy. Although they could un-win a trophy if they get sacked.
        if(trophyWon != mTrophyWon)
        {
            mTrophyWon = trophyWon;

            PP_Trophy[] trophies = FindObjectsOfType<PP_Trophy>();
            for(int i=0; i<trophies.Length; i++){
                Destroy(trophies[i].gameObject);
            }

            // Now we spawn a trophy?
            // Or if they un-won, then we laugh at them?
            if(mTrophyWon == "NONE"){
                Debug.Log("Haha you lost your trophy");
                return;
            }

            PP_Trophy clone = Instantiate(PF_Trophy, mTrophySpawnPos, transform.rotation);
            Instantiate(PF_Particles, mTrophySpawnPos, transform.rotation);
            if(mTrophyWon == "BRONZE"){
                clone.GetComponentInChildren<Renderer>().material = MAT_Bronze;
            }else if(mTrophyWon == "SILVER"){
                clone.GetComponentInChildren<Renderer>().material = MAT_Silver;
            }else if(mTrophyWon == "GOLD"){
                clone.GetComponentInChildren<Renderer>().material = MAT_Gold;                
            }else{
                Debug.Log("Should never get here, error trophy creation");
            }
        }
    }
}
