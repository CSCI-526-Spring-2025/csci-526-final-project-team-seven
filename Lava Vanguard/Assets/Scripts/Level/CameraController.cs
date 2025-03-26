using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    public GameObject player;
    public float cameraSpeedY = 0.3f;
    public float cameraFollowDistance = 5.0f;

    public CinemachineVirtualCamera virtualCamera;
    private bool moving = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (!Tutorial.Instance.tutorial)
            StartMove();
    }

    public void StartMove()
    {
        moving = true;
    }

    private void LateUpdate()
    {
        if (moving)
        {
            Transform cameraTransform = virtualCamera.transform;
            Vector3 targetPosition = cameraTransform.position;

            // 让相机沿着 Y 轴平滑上移
            targetPosition.y += cameraSpeedY * Time.deltaTime;

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
    }
}
