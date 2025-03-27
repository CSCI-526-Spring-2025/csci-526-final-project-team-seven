using UnityEngine;
using Cinemachine;
using TMPro;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    public GameObject player;
    public float currentCamSpeedY;
    public float cameraSpeedY = 0.3f;
    public float cameraFollowDistance = 5.0f;

    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noiseProfile;

    private bool moving = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentCamSpeedY = cameraSpeedY;
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
            // ��������� Y ��ƽ������
            targetPosition.y += currentCamSpeedY * Time.deltaTime;

            if (LevelGenerator.Instance.WavePlatform) {
                var p = LevelGenerator.Instance.WavePlatform;
                if (p.transform.position.y < cameraTransform.position.y - 3.5) {
                    currentCamSpeedY = 0;
                }
            }
        }
        // �������λ�õ��� X ���ƫ��
        if (player.transform.position.x > targetPosition.x + cameraFollowDistance)
        {
            targetPosition.x = player.transform.position.x - cameraFollowDistance;
        }
        else if (player.transform.position.x < targetPosition.x - cameraFollowDistance)
        {
            targetPosition.x = player.transform.position.x + cameraFollowDistance;
        }
        // ֱ���޸� Virtual Camera ��λ��
        cameraTransform.position = targetPosition;
    }
    private void Update()
    {
        // Check if the 'R' key is pressed down
        if (Input.GetKeyDown(KeyCode.R))
            CameraShake(0.25f, 0.3f, 10);
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

    public void stopCamera()
    {
        currentCamSpeedY = 0;
    }

    public void resumeCamera()
    {
        currentCamSpeedY = cameraSpeedY;
    }
}
