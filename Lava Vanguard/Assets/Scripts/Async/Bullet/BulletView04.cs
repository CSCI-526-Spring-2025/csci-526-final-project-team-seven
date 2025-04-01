using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BulletView04 : BulletView
{
    private float initialSurviveTime;
    private float surviveTime;
    private PolygonCollider2D hexCollider;
    protected override void SetupBullet(int level)
    {
        var sr = GetComponent<SpriteRenderer>();
        Color c = sr.color;
        c.a = 0.5f;
        sr.color = c;
        attack = 1;

        initialSurviveTime = surviveTime = 0.5f;
        hexCollider=GetComponent<PolygonCollider2D>();

        float scaleFactor = 3f + level*0.5f;
        transform.localScale = Vector3.one * scaleFactor;
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
            Async.BulletManager.Instance.bulletHit4++;
            EnemyView enemy = other.GetComponent<EnemyView>();
            if (enemy != null)
            {
                enemy.TakeHit(attack);
            }
        }
    }
}
