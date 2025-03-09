using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 狙击
public class BulletView03 : BulletView
{

    [Header("Auto-Aiming")]
    protected float maxAimDeviation = 5f;

    protected override void SetupBullet()
    {
        detectionRange = lifeDistance = 20.0f;
        speed = 30f;
        attack = 3;
        FindClosestEnemy();
        ApplyInitialRotation();
    }

    private void FindClosestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        //Debug.Log($"Found {enemies.Length} enemies in range.");

        foreach (Collider2D enemy in enemies)
        {   
            if (enemy.CompareTag("Enemy") && enemy.gameObject.activeInHierarchy)
            {
                Rigidbody2D targetRb = enemy.GetComponent<Rigidbody2D>();
                Vector3 targetVelocity = targetRb != null ? targetRb.velocity : Vector3.zero;

                Vector3 targetPosition = enemy.transform.position;
                float timeToHit = Vector3.Distance(transform.position, targetPosition) / speed;
                Vector3 predictedPosition = targetPosition + targetVelocity * timeToHit;

                float distance = Vector3.Distance(predictedPosition, transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            // Debug.Log("ClosestEnemy: " + closestEnemy.position);
            fireDirection = ((Vector2)closestEnemy.position - (Vector2)transform.position).normalized;
            hasTarget = true;
        }
        else
        {
            fireDirection = Vector2.right;
            hasTarget = false;
        }
        //Debug.Log("Final Fire Direction: " + fireDirection);

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

        if (other.CompareTag("Enemy"))
        {
            PlayerManager.Instance.GainEXP(1);

            EnemyView enemy = other.GetComponent<EnemyView>();
            if (enemy != null)
            {
                enemy.TakeHit(attack); 
            }
            Destroy(gameObject);
        }
    }
}
