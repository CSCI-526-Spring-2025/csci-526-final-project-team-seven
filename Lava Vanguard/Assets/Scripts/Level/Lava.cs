using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public float cameraDistance=5f;
    private Camera mainCamera;
    private float deathDelay = 3f;

    void Start()
    {
        mainCamera = Camera.main;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            Debug.Log("Player is dead");
            PlayerManager.Instance.GetHurt(5);
        }else if (other.CompareTag("Enemy")){
            Debug.Log("Enemy is dead in lava");
            Destroy(other.gameObject, deathDelay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, mainCamera.transform.position.y - cameraDistance, 0);
    }
}
