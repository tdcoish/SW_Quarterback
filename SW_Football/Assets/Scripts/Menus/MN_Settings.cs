/*************************************************************************************
Lots of tweakable stuffs here.

Movement Speed
Acceleration Speed
Max Throw Power
Rotation Speed for Camera
Movement accuracy penalties
Look accuracy penalties
Inaccuracy bias x/y
Master Volume

*************************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

using System;

public class MN_Settings : MonoBehaviour
{
    public DT_Player        lPlayerData;

    public SO_Float         lLookSensitity;
    public SO_Float         lMovementPenalty;
    public SO_Float         lLookPenalty;
    public SO_Float         lInaccuracyBias;
    public SO_Float         lMasterVolume;

    public AudioMixer       mAudioMixer;

    public Slider           mSLDMoveSpd;
    public Text             mTXTMoveSpd;

    public Slider           mSLDAccRate;
    public Text             mTXTAccRate;

    public Slider           mSLDMaxThrowPow;
    public Text             mTXTMaxThrowPow;

    public Slider           mSLDLookSensitivity;
    public Text             mTXTLookSensitivity;

    public Slider           mSLDMovementPenalty;
    public Text             mTXTMovementPenalty;

    public Slider           mSLDLookPenalty;
    public Text             mTXTLookPenalty;

    public Slider           mSLDInaccuracyBias;
    public Text             mTXTInaccuracyBias;

    public Slider           mSLDMasterVolume;
    public Text             mTXTMasterVolume;

    void Start()
    {
        mSLDMoveSpd.value = lPlayerData._MoveSpd;
        mSLDAccRate.value = lPlayerData._AccRate;
        mSLDMaxThrowPow.value = lPlayerData._ThrowSpd;
        mSLDLookSensitivity.value = lLookSensitity.Val;
        mSLDMovementPenalty.value = lMovementPenalty.Val;
        mSLDLookPenalty.value = lLookPenalty.Val;
        mSLDInaccuracyBias.value = lInaccuracyBias.Val;
        mSLDMasterVolume.value = lMasterVolume.Val;
    }

    public void OnMovementSpeedChanged()
    {
        lPlayerData._MoveSpd = mSLDMoveSpd.value;
        mTXTMoveSpd.text = "Movement Speed: " + (int)lPlayerData._MoveSpd;
    }

    public void OnAccelerationRateChanged()
    {
        lPlayerData._AccRate = mSLDAccRate.value;
        mTXTAccRate.text = "Acceleration Rate: " + (int)lPlayerData._AccRate;
    }

    public void OnMaxThrowPowChanged()
    {
        lPlayerData._ThrowSpd = mSLDMaxThrowPow.value;
        mTXTMaxThrowPow.text = "Throw Power: " + (int)lPlayerData._ThrowSpd;
    }

    public void OnLookSensitivityChanged()
    {
        lLookSensitity.Val = mSLDLookSensitivity.value;
        mTXTLookSensitivity.text = "Look Sensitivity: " + Math.Round(lLookSensitity.Val, 2);
    }

    public void OnMovementPenaltyChanged()
    {
        lMovementPenalty.Val = mSLDMovementPenalty.value;
        mTXTMovementPenalty.text = "Movement Penalty: " + (int)lMovementPenalty.Val;
    }

    public void OnLookPenaltyChanged()
    {
        lLookPenalty.Val = mSLDLookPenalty.value;
        mTXTLookPenalty.text = "Look Penalty: " + (int)lLookPenalty.Val;
    }

    public void OnInaccuracyBiasChanged()
    {
        lInaccuracyBias.Val = mSLDInaccuracyBias.value;
        mTXTInaccuracyBias.text = "Inaccuracy Bias: " + (int)lInaccuracyBias.Val;
    }

    public void OnMasterVolumeChanged()
    {
        lMasterVolume.Val = mSLDMasterVolume.value;
        mTXTMasterVolume.text = "Master Volume: " + Math.Round(lMasterVolume.Val, 2);
        mAudioMixer.SetFloat("MASTER_VOLUME", lMasterVolume.Val);
    }

}
