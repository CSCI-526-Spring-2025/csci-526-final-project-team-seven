using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 散射
public class BulletView02 : BulletView
{
    protected float splitDistance = 5f;
    protected bool isSplited = false;
    protected float splitAngle = 30f;

    protected override void SetupBullet()
    {
        lifeDistance = 15.0f;
        detectionRange = 10.0f;
        speed = 10f;
        attack = 3;
        if (!isSplited)
        {
            FindClosestEnemy();
            ApplyInitialRotation();
        }
    }

    private void FindClosestEnemy()
    {
        // Get all enemies
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        // Find the clothest enemy
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy") && enemy.gameObject.activeInHierarchy)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                Rigidbody2D targetRb = enemy.GetComponent<Rigidbody2D>();
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }
        }
        // If have enemy then fire that direction
        if (closestEnemy != null)
        {
            fireDirection = ((Vector2)closestEnemy.position - (Vector2)transform.position).normalized;
            hasTarget = true;
        }
        else
        {
            // no enemy then go right
            fireDirection = Vector3.right;
            hasTarget = false;
        }
        //Debug.Log("Final Fire Direction: " + fireDirection);

    }
    // Rotate the bullet
    private void ApplyInitialRotation()
    {
        // Debug.Log("Fire Direction: " + fireDirection);
        if (hasTarget)
        {
            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    protected override void Update()
    {
        base.Update();
        // If not splited and distance>splitDistance then split
        if (!isSplited && Vector3.Distance(startPosition, transform.position) >= splitDistance)
        {
            SplitBullet();
        }
    }
    protected override void MoveBullet()
    {
        transform.position += (Vector3)fireDirection * speed * Time.deltaTime;
    }

    // Split the bullet in to different direction and change the size and attack.
    private void SplitBullet()
    {
        Transform container = transform.parent;
        float[] angles = { 0, splitAngle, -splitAngle };
        for(int i = 0; i < 3; i++)
        {
            GameObject bulletObject = Instantiate(gameObject, transform.position, Quaternion.identity, container);
            BulletView02 bullet = bulletObject.GetComponent<BulletView02>();
            bullet.MakeSmall();
            bullet.fireDirection= Quaternion.Euler(0, 0, angles[i]) * fireDirection;
            bullet.speed = speed;
        }
        Destroy(gameObject);
    }

    private void MakeSmall()
    {
        if (isSplited) return; 
        isSplited = true;
        transform.localScale *= 0.5f;
        attack = attack * 2 / 3;
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
                enemy.TakeHit(attack * damageMultiplier);
            }
            Destroy(gameObject);
        }
    }
}
