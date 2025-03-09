using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView_03 : EnemyView
{
    
    private bool movingRight = true; 
    private float leftLimit;
    private float rightLimit;
    private Camera mainCamera;
    // Enemy will be destroyed if lower than mainCamera.y+destroyY
    public float destroyY=-10f;
    // How many small enemies will be splited
    private int splitCount=3;
    // The radius of split
    private float splitRadius = 1.0f;
    
    public GameObject enemyView02Prefab;

    private void Start()
    {
        //hardcode!
        Health = 6;
        MaxHealth = 6;
        expGained = 2;
        attack = 1;
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
        if (LevelGenerator.Instance.grounds == null || LevelGenerator.Instance.grounds.Count == 0)
        {
            return Vector3.zero;
        }

        var g = LevelGenerator.Instance.grounds[Random.Range(0, LevelGenerator.Instance.grounds.Count)];
        return g.transform.position + new Vector3(0, 0.6f, 0);
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void SplitIntoSmallerEnemies()
    {
        Vector3 parentPosition = transform.position;
        for (int i = 0; i < splitCount; i++)
        {
            float rad = i * (360f / (1f * splitCount))*Mathf.Deg2Rad;
            Vector3 offset=new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * splitRadius;
            Vector3 spawnLocation = parentPosition + offset;
            var enemyView=EnemyManager.Instance.GenerateSpecificEnemy(0);
            enemyView.transform.position = spawnLocation;
        }
    }

    public override void TakeHit(int bulletAttack)
    {
        Health -= bulletAttack;
        Debug.Log("Enemy03 been hit " + Health + " - " + bulletAttack);
        if (Health<=0)
        {
            Health = 0;
            SplitIntoSmallerEnemies();
            Destroy(gameObject);
        }
        
    }
}