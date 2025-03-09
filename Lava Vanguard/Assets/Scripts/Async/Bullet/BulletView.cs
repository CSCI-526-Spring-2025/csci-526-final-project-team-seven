using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public abstract class BulletView : MonoBehaviour
{
    protected Vector2 startPosition;
    public float speed = 1.0f;
    public float lifeDistance = 30.0f;
    protected Vector2 fireDirection = Vector2.right;
    protected int attack = 1;
    protected bool hasTarget = false;
    protected float detectionRange = 10.0f;
    public LayerMask enemyLayer;
    protected bool hasHit=false;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        SetupBullet();
    }

    protected abstract void SetupBullet();

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) > lifeDistance)
        {
            Destroy(gameObject);
            return;
        }
        MoveBullet();
    }

    protected abstract void MoveBullet();

    protected abstract void OnTriggerEnter2D(Collider2D other);

}
