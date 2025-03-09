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
        // var g = LevelGenerator.Instance.grounds[Random.Range(0, LevelGenerator.Instance.grounds.Count)];
        // return g.transform.position + new Vector3(0, 0.25f, 0);

        if (LevelGenerator.Instance.grounds == null || LevelGenerator.Instance.grounds.Count == 0)
        {
            Debug.LogError("No level grounds");
            return Vector3.zero;
        }

        var g = LevelGenerator.Instance.grounds[Random.Range(0, LevelGenerator.Instance.grounds.Count)];
        Debug.Log("EnemyView_03 spawned on: " + g.transform.position);
        return g.transform.position + new Vector3(0, 0.5f, 0);
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void SplitIntoSmallerEnemies()
    {

        Vector3 parentPosition = transform.position;

        for (int i = 0; i < 3; i++)
        {
            //Vector3 spawnOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            GameObject smallEnemy = Instantiate(enemyView02Prefab, parentPosition, Quaternion.identity);
            Debug.Log("Generate sub enemy: " + enemyView02Prefab.name);
            smallEnemy.GetComponent<EnemyView>().Init("Enemy_02");
        }
        Destroy(gameObject);
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