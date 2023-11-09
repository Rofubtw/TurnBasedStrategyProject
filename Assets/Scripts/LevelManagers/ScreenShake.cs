using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        instance = this;

        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        
    }

    public void Shake(float intensity = 1f) 
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
