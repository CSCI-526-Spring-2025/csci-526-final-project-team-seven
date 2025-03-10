using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    
    public GameObject player;
    public float cameraSpeedY = 0.3f;
    public float cameraFollowDistance = 5.0f;

    private Camera mainCamera;
    private bool moving = false;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        mainCamera = Camera.main;
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
            Vector3 targetCameraPosition = mainCamera.transform.position;

            targetCameraPosition.y += cameraSpeedY * Time.deltaTime;
            if (player.transform.position.x > targetCameraPosition.x + cameraFollowDistance)
            {
                targetCameraPosition.x = player.transform.position.x - cameraFollowDistance;
            }
            else if (player.transform.position.x < targetCameraPosition.x - cameraFollowDistance)
            {
                targetCameraPosition.x = player.transform.position.x + cameraFollowDistance;
            }
            mainCamera.transform.position = targetCameraPosition;
        }

    }
}
