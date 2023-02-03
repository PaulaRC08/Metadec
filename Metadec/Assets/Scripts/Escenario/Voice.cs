using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Voice.PUN;


public class Voice : MonoBehaviour
{
    public Recorder photonVoiceControler;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)){
            photonVoiceControler.GetComponent<Recorder>().TransmitEnabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.U))
        {
            photonVoiceControler.GetComponent<Recorder>().TransmitEnabled = false;
        }
    }
}
