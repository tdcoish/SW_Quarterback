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
        IO_Settings.FLOAD_SETTINGS();

        mSLDMoveSpd.value = IO_Settings.mSet.lPlayerData.mMoveSpd;
        mSLDAccRate.value = IO_Settings.mSet.lPlayerData.mAccRate;
        mSLDMaxThrowPow.value = IO_Settings.mSet.lPlayerData.mThrowSpd;
        mSLDLookSensitivity.value = IO_Settings.mSet.lLookSensitity;
        mSLDMovementPenalty.value = IO_Settings.mSet.lMovementPenalty;
        mSLDLookPenalty.value = IO_Settings.mSet.lLookPenalty;
        mSLDInaccuracyBias.value = IO_Settings.mSet.lInaccuracyBias;
        mSLDMasterVolume.value = IO_Settings.mSet.lMasterVolume;

    }  
    
    public void OnMovementSpeedChanged()
    {
        IO_Settings.mSet.lPlayerData.mMoveSpd = mSLDMoveSpd.value;
        mTXTMoveSpd.text = "Movement Speed: " + (int)IO_Settings.mSet.lPlayerData.mMoveSpd;
    }

    public void OnAccelerationRateChanged()
    {
        IO_Settings.mSet.lPlayerData.mAccRate = mSLDAccRate.value;
        mTXTAccRate.text = "Acceleration Rate: " + (int)IO_Settings.mSet.lPlayerData.mAccRate;
    }

    public void OnMaxThrowPowChanged()
    {
        IO_Settings.mSet.lPlayerData.mThrowSpd = mSLDMaxThrowPow.value;
        mTXTMaxThrowPow.text = "Throw Power: " + (int)IO_Settings.mSet.lPlayerData.mThrowSpd;
    }

    public void OnLookSensitivityChanged()
    {
        IO_Settings.mSet.lLookSensitity = mSLDLookSensitivity.value;
        mTXTLookSensitivity.text = "Look Sensitivity: " + Math.Round(IO_Settings.mSet.lLookSensitity, 2);
    }

    public void OnMovementPenaltyChanged()
    {
        IO_Settings.mSet.lMovementPenalty = mSLDMovementPenalty.value;
        mTXTMovementPenalty.text = "Movement Penalty: " + (int)IO_Settings.mSet.lMovementPenalty;
    }

    public void OnLookPenaltyChanged()
    {
        IO_Settings.mSet.lLookPenalty = mSLDLookPenalty.value;
        mTXTLookPenalty.text = "Look Penalty: " + (int)IO_Settings.mSet.lLookPenalty;
    }

    public void OnInaccuracyBiasChanged()
    {
        IO_Settings.mSet.lInaccuracyBias = mSLDInaccuracyBias.value;
        mTXTInaccuracyBias.text = "Inaccuracy Bias: " + (int)IO_Settings.mSet.lInaccuracyBias;
    }

    public void OnMasterVolumeChanged()
    {
        IO_Settings.mSet.lMasterVolume = mSLDMasterVolume.value;
        mTXTMasterVolume.text = "Master Volume: " + Math.Round(IO_Settings.mSet.lMasterVolume, 2);
        mAudioMixer.SetFloat("MASTER_VOLUME", IO_Settings.mSet.lMasterVolume);
    }

    public void BT_SaveNewSettings()
    {
        IO_Settings.FWRITE_SETTINGS();
    }

}
