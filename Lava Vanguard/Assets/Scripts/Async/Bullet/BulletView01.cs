using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView01 : BulletView
{

    [Header("Auto-Aiming")]
    protected float maxAimDeviation = 5f;   
    private float selfDetectionRange = 5.0f;
    protected override void SetupBullet()
    {
        lifeDistance = 8.0f;
        detectionRange = 8.0f;
        speed = 5f;
        attack = 3;
        FindClosestEnemy();
        ApplyInitialRotation();
    }

    private void FindClosestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, selfDetectionRange, enemyLayer);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Debug.Log($"Found {enemies.Length} enemies in range.");

        foreach (Collider2D enemy in enemies)
        {   
            if (enemy.CompareTag("Enemy") && enemy.gameObject.activeInHierarchy)
            {
                Rigidbody2D targetRb = enemy.GetComponent<Rigidbody2D>();
                Vector3 targetVelocity = targetRb != null ? targetRb.velocity : Vector3.zero;

                Vector3 targetPosition = enemy.transform.position;
                float timeToHit = Vector3.Distance(transform.position, targetPosition) / speed;
                // Vector3 predictedPosition = targetPosition + targetVelocity * timeToHit;

                float distance = Vector3.Distance(targetPosition, transform.position);
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
        Debug.Log("Final Fire Direction: " + fireDirection);

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

            Debug.Log($"🛑 Bullet 碰到了 {other.gameObject.name}");

            PlayerManager.Instance.GainEXP(1);
            //Debug.Log("Enemy is dead");

            EnemyView enemy = other.GetComponent<EnemyView>();
            if (enemy != null)
            {
                enemy.TakeHit(); // 让敌人自己处理受击
            }
            
            Destroy(gameObject);
            //Use EnemyView to destroy enemy, not by bullet
            //Destroy(other.gameObject);
        }
    }
}
