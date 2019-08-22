using UnityEngine;

[CreateAssetMenu(fileName="DT+Player", menuName="DT/Player")]
public class DT_Player : ScriptableObject
{
    public float                _ThrowChargeTime = 2f;
    public float                _ThrowSpd = 25f;
    public float                _ShiftChargeSlow = 3f;      // how much slower they charge the throw power with shift
    public float                _MoveSpd = 5f;
    public float                _AccRate = 50f;
}
