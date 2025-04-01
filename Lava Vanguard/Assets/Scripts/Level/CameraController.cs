using UnityEngine;
using Cinemachine;
using TMPro;
using System;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    public static event Action OnCameraUpdated;

    public GameObject player;
    private float currentSpeedY;
    public float cameraSpeedY = 0.3f;
    public float cameraFollowDistance = 5.0f;
    private float remainingDistance = -1f;
    public float initialSpeed = 30f;
    private float totalDistance;
    public AnimationCurve speedCurve;
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noiseProfile;

    private bool moving = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentSpeedY = cameraSpeedY;
        if (!Tutorial.Instance.tutorial)
            StartMove();
        noiseProfile = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void StartMove()
    {
        moving = true;
    }

    private void LateUpdate()
    {
        Transform cameraTransform = virtualCamera.transform;//Lot of GC!!!
        Vector3 targetPosition = cameraTransform.position;
        if (moving)
        {
            if (remainingDistance > 0)
            {
                float speed = initialSpeed * speedCurve.Evaluate(remainingDistance / totalDistance);
                targetPosition.y += speed * Time.deltaTime;
                remainingDistance -= speed * Time.deltaTime;
            }
            else
            {
                targetPosition.y += currentSpeedY * Time.deltaTime;
            }
        }
        //Virtual Camera 
        cameraTransform.position = targetPosition;
        OnCameraUpdated?.Invoke();
    }

    /// <summary>
    /// Triggers a camera shake effect.
    /// </summary>
    /// <param name="duration">Duration of the shake effect.</param>
    /// <param name="amplitude">Amplitude of the shake (intensity).</param>
    /// <param name="frequency">Frequency of the shake (speed of oscillation).</param>
    public void CameraShake(float duration = 0.25f, float amplitude = 1f, float frequency = 1f)
    {
        if (noiseProfile != null)
        {
            noiseProfile.m_AmplitudeGain = amplitude;
            noiseProfile.m_FrequencyGain = frequency;
            Invoke(nameof(StopShaking), duration);
        }
    }

    /// <summary>
    /// Stops the camera shake effect.
    /// </summary>
    private void StopShaking()
    {
        if (noiseProfile != null)
        {
            noiseProfile.m_AmplitudeGain = 0f;
            noiseProfile.m_FrequencyGain = 0f;
        }
    }

    public void StopCamera()
    {
        currentSpeedY = 0;
    }

    public void ResumeCamera()
    {
        currentSpeedY = cameraSpeedY;
    }

    public void UpdateDistance(Transform transform)
    {
        remainingDistance = transform.position.y - (virtualCamera.transform.position.y - 3.5f);
        totalDistance = remainingDistance;
    }

    public bool CameraStopped()
    {
        return currentSpeedY == 0f;
    }
}
