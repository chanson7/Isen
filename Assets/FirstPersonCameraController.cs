using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using ECM.Controllers;

public class FirstPersonCameraController : MonoBehaviour
{

    CinemachineVirtualCamera vcam;
    BaseCharacterController cc;
    Rigidbody rb;


    void Awake()
    {
        cc = GetComponentInParent<BaseCharacterController>();
        rb = GetComponentInParent<Rigidbody>();
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    private void LateUpdate()
    {
        if (cc.isGrounded)
        {
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = rb.velocity.magnitude;
            vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = rb.velocity.magnitude;
        }
    }


}
