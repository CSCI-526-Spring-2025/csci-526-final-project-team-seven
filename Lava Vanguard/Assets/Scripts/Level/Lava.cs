using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public float cameraDistance = 10f;
    private float deathDelay = 3f;
    private void Start()
    {
        if (!Tutorial.Instance.tutorial)
            SetCameraDistance(5, 0);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is dead");
            PlayerManager.Instance.GetHurt(5);
        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy is dead in lava");
            Destroy(other.gameObject, deathDelay);
        }
    }
    public void SetCameraDistance(float distance, float duration = 1f)
    {
        DOTween.To(() => cameraDistance, x => cameraDistance = x, distance, duration).SetEase(Ease.Linear);
    }
    private void Update()
    {
        transform.position = new Vector3(0, Camera.main.transform.position.y - cameraDistance, 0);
    }
}
