using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 普通
public class BulletView05 : BulletView
{
    protected override void SetupBullet()
    {
        lifeDistance = 8.0f;
        detectionRange = 8.0f;
        speed = 15f;
        attack = 2;
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
            bool killed = false;
            if (enemy != null)
            {
                int roundedDamage = Mathf.RoundToInt(attack * damageMultiplier);
                killed = enemy.TakeHit(roundedDamage);
            }
            else
            {
                return;
            }
            if (killed)
            {
                PlayerManager.Instance.playerView.GainCoin(1);//do not hard code
                UIGameManager.Instance.UpdateCoin();
            }
            Destroy(gameObject);
        }
    }
}
