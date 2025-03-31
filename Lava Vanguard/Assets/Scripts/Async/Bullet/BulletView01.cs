using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 普通
public class BulletView01 : BulletView
{

    protected override void SetupBullet(int level)
    {
        lifeDistance = 8.0f;
        detectionRange = 8.0f;
        speed = 15f;
        attack = 2 + level;
        SetFireDirection();
        ApplyInitialRotation();
    }

    private void ApplyInitialRotation()
    {
        if (hasTarget)
        {
            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    protected override void MoveBullet()
    {
        transform.position += (Vector3)fireDirection* speed *Time.deltaTime;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
        {
            return;
        }
        if (other.CompareTag("Enemy"))
        {
            hasHit = true;
            EnemyView enemy = other.GetComponent<EnemyView>();
            if (enemy != null)
            {
                enemy.TakeHit(attack);
            }
            Destroy(gameObject);
        }
    }
}
