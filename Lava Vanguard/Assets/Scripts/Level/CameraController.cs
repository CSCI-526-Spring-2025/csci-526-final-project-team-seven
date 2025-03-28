using UnityEngine;
using Cinemachine;
using TMPro;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    public GameObject player;
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
            // 让相机沿着 Y 轴平滑上移
            targetPosition.y += cameraSpeedY * Time.deltaTime;
        }
        // 根据玩家位置调整 X 轴的偏移
        if (player.transform.position.x > targetPosition.x + cameraFollowDistance)
        {
            targetPosition.x = player.transform.position.x - cameraFollowDistance;
        }
        else if (player.transform.position.x < targetPosition.x - cameraFollowDistance)
        {
            targetPosition.x = player.transform.position.x + cameraFollowDistance;
        }
        // 直接修改 Virtual Camera 的位置
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

}
