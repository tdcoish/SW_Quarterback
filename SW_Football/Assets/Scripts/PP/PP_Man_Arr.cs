/*************************************************************************************
Arrow manager/handler
*************************************************************************************/
using UnityEngine;

public class PP_Man_Arr : MonoBehaviour
{
    private PP_Manager          cPPMan;

    public GameObject           PF_Arrow;

    private void Start()
    {
        cPPMan = GetComponent<PP_Manager>();
    }

    public void FSpawnArrow(Vector3 vPos, Quaternion qRot)
    {
        var clone = Instantiate(PF_Arrow, vPos, qRot);
        SetArrowMaterialColour(clone);
    }

    public void FDestroyArrows()
    {
        PP_Arrow[] arrows = FindObjectsOfType<PP_Arrow>();
        for(int i=0; i<arrows.Length; i++){
            Destroy(arrows[i].gameObject);
        }
    }

    private void SetArrowMaterialColour(GameObject arrow)
    {
        Renderer[] renderers = arrow.GetComponentsInChildren<Renderer>();

        Color col = Color.red;
        if(cPPMan.mStreakBonus == 2){
            col = Color.blue;
        }else if(cPPMan.mStreakBonus == 3){
            col = Color.green;
        }else if(cPPMan.mStreakBonus > 3){
            col = Color.yellow;
        }
    
        for(int i=0; i<renderers.Length; i++){
            renderers[i].material.SetColor("_Color", col);
        }
    }
}
