using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;

    public float shakeDuration;
    public float shakeAmplitude;
    public float shakeFrequency;
    public float dampingTime;

    public float rotationShakeDuration;
    public float rotationShakeMagnitude;

    private float currentShakeTime = 0f;

    public void Awake()
    {
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    //the object have to be enable when calling camera shake
    public void TriggerShake(float duration, NoiseSettings cameraShakeProfil, float magnitude = 1, float frequency = 1)
    {
        shakeDuration = duration > 0 ? duration : shakeDuration;
        shakeAmplitude = magnitude > 0 ? magnitude : shakeAmplitude;
        shakeFrequency = frequency > 0 ? frequency : shakeFrequency;

        perlinNoise.m_NoiseProfile = cameraShakeProfil;

        perlinNoise.m_AmplitudeGain = shakeAmplitude;
        perlinNoise.m_FrequencyGain = shakeFrequency;
        currentShakeTime = shakeDuration;
       
    }

    void Update()
    {
        if (currentShakeTime > 0)
        {
            // Reduce shake time
            currentShakeTime -= Time.deltaTime;

            // After shaking is done, reset camera position
            if (currentShakeTime <= 0)
            {
                perlinNoise.m_AmplitudeGain = 0f;
                perlinNoise.m_FrequencyGain = 0f;
            }
            
        }
    }
}
