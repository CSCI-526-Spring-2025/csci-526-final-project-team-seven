using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFollower : MonoBehaviour
{

    private float yOffset;
    void Start()
    {
        yOffset = transform.position.y - CameraController.Instance.virtualCamera.transform.position.y;
    }

    private void OnEnable()
    {
        CameraController.OnCameraUpdated += UpdateWallPosition;
    }

    private void OnDisable()
    {
        CameraController.OnCameraUpdated -= UpdateWallPosition;
    }
    // Update is called once per frame
    void UpdateWallPosition()
    {
        transform.position = new Vector3(transform.position.x, CameraController.Instance.virtualCamera.transform.position.y + yOffset, 0);
    }
}
