using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BulletView04 : BulletView
{
    private float initialSurviveTime;
    private float surviveTime;
    private PolygonCollider2D hexCollider;
    protected override void SetupBullet()
    {
        attack = 1;
        initialSurviveTime = surviveTime = 0.5f;
        hexCollider=GetComponent<PolygonCollider2D>();
    }
    protected override void MoveBullet()
    {
        hasHit = false;
        surviveTime -= Time.deltaTime;
        if(surviveTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
        {
            return;
        }
        if (other.CompareTag("Enemy"))
        {
            EnemyView enemy = other.GetComponent<EnemyView>();
            if (enemy != null)
            {
                int roundedDamage = Mathf.RoundToInt(attack * damageMultiplier);
                enemy.TakeHit(roundedDamage);
            }
        }
    }
}
