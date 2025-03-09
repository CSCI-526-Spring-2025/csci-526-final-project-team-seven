using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView_02 : EnemyView
{
    private bool movingRight = true; 
    private float leftLimit;
    private float rightLimit;
    private Camera mainCamera;
    public float destroyY=-10f;

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
        var g = LevelGenerator.Instance.grounds[Random.Range(0, LevelGenerator.Instance.grounds.Count)];
        return g.transform.position + new Vector3(0, 0.25f, 0);
    }
    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    // protected override void TakeHit()
    // {
    //     Debug.Log($"{gameObject.name} 被击中，死亡");
    //     Destroy(gameObject);
    // }

    // protected override void OnCollisionEnter2D(Collision2D collision)
    // {

    //     Debug.Log($"{gameObject.name} 碰撞到了 {collision.gameObject.name}");

    //     if (collision.gameObject.CompareTag("Bullet"))
    //     {
    //         TakeHit();
    //     }
    // }
}

