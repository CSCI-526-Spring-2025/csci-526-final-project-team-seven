using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 狙击
public class BulletView03 : BulletView
{

    protected override void SetupBullet()
    {
        detectionRange = lifeDistance = 20.0f;
        speed = 30f;
        attack = 3;
        SetFireDirection();
        ApplyInitialRotation();
    }

    private void ApplyInitialRotation()
    {
        // Debug.Log("Fire Direction: " + fireDirection);
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
                int roundedDamage = Mathf.RoundToInt(attack * damageMultiplier);
                enemy.TakeHit(roundedDamage);
            }
            Destroy(gameObject);
        }
    }
}
