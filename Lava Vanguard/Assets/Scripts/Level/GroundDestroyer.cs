using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDestroyer : MonoBehaviour
{
    private Camera mainCamera;
    public float destroyY=-10f;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera != null && transform.position.y < mainCamera.transform.position.y +destroyY){
            LevelGenerator.Instance.grounds.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
