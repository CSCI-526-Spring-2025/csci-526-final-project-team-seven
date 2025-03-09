using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView_03 : EnemyView
{
    
    private bool movingRight = true; 
    private float leftLimit;
    private float rightLimit;
    private Camera mainCamera;
    public float destroyY=-10f;
    
    private int hitCount = 0; // 记录被击中的次数
    public GameObject enemyView02Prefab; // EnemyView_02 预制体

    private void Start()
    {
        //hardcode!
        leftLimit = transform.position.x - 1.2f;
        rightLimit = transform.position.x + 1.2f;
        mainCamera = Camera.main;
    }
    
    protected override void Approching()
    {
       transform.Translate(Vector2.right * enemyData.Speed * Time.deltaTime * (movingRight ? 1 : -1));

        if (movingRight && transform.position.x >= rightLimit)
        {
            Flip();
        }
        else if (!movingRight && transform.position.x <= leftLimit)
        {
            Flip();
        }
        if (mainCamera != null && transform.position.y < mainCamera.transform.position.y +destroyY){
            Destroy(gameObject);
        }
    }

    protected override Vector3 GetSpawnPosition()
    {
        // var g = LevelGenerator.Instance.grounds[Random.Range(0, LevelGenerator.Instance.grounds.Count)];
        // return g.transform.position + new Vector3(0, 0.25f, 0);

        if (LevelGenerator.Instance.grounds == null || LevelGenerator.Instance.grounds.Count == 0)
        {
            Debug.LogError("❌ LevelGenerator.Instance.grounds 为空，无法生成敌人！");
            return Vector3.zero;
        }

        var g = LevelGenerator.Instance.grounds[Random.Range(0, LevelGenerator.Instance.grounds.Count)];
        Debug.Log("✅ EnemyView_03 生成在：" + g.transform.position);
        return g.transform.position + new Vector3(0, 0.5f, 0);
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void SplitIntoSmallerEnemies()
    {

        Vector3 parentPosition = transform.position; // 🔥 先存储当前位置

        for (int i = 0; i < 3; i++)
        {
            //Vector3 spawnOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            GameObject smallEnemy = Instantiate(enemyView02Prefab, parentPosition, Quaternion.identity);
            Debug.Log("✅ 正在生成小敌人：" + enemyView02Prefab.name);
            smallEnemy.GetComponent<EnemyView>().Init("Enemy_02");
            Debug.Log("✅ 生成小敌人：" + smallEnemy.name + " 在 " + smallEnemy.transform.position);
        }
        Destroy(gameObject);
    }

    public override void TakeHit()
    {
        hitCount++;
        Debug.Log("🔥 EnemyView_03 被击中，当前 hitCount = " + hitCount);
        if (hitCount >= 3)
        {
            SplitIntoSmallerEnemies();
        }
    }

    // protected override void OnCollisionEnter2D(Collision2D collision)
    // {

    //     Debug.Log("⚡ EnemyView_03 碰撞到：" + collision.gameObject.name);

    //     if (collision.gameObject.CompareTag("Bullet")) // 子弹击中
    //     {
    //         Debug.Log("💥 受到子弹攻击！");
    //         TakeHit();
    //         Destroy(collision.gameObject); // 销毁子弹
    //     }
    // }
}
