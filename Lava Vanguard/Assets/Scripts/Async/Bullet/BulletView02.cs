using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// 散射
public class BulletView02 : BulletView
{
    protected float splitDistance = 1f;
    protected bool isSplited = false;
    protected float splitAngle = 30f;
    protected int splitCount = 2;
    protected int splitAttack = 2;

    protected override void SetupBullet(int level)
    {
        lifeDistance = 15.0f;
        detectionRange = 10.0f;
        speed = 10f;

        attack = 2+level;
        splitAttack = level;
        splitCount = 1 + level;
        if (!isSplited)
        {
            SetFireDirection();
            ApplyInitialRotation();
        }
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
        for(int i = 0; i < splitCount; i++)
        {
            GameObject bulletObject = Instantiate(gameObject, transform.position, Quaternion.identity, container);
            BulletView02 bullet = bulletObject.GetComponent<BulletView02>();
            bullet.Init(level);
            bullet.isSplited = true;
            bullet.attack = splitAttack;
            bullet.transform.localScale *= 0.5f;
            float angle = splitAngle*(i - (splitCount - 1) / 2f);
            bullet.fireDirection= Quaternion.Euler(0, 0, angle) * fireDirection;
        }
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
        {
            return;
        }
        if (other.CompareTag("Enemy"))
        {
            //Debug.Log("Split Hit: "+attack);
            hasHit = true;
            Async.BulletManager.Instance.bulletHit2++;
            EnemyView enemy = other.GetComponent<EnemyView>();
            if (enemy != null)
            {
                enemy.TakeHit(attack);
            }
            Destroy(gameObject);
        }
    }
}
