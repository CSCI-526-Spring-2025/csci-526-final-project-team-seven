using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class EnemyView : MonoBehaviour
{
    public EnemyData enemyData;
    // Current health
    protected int Health;
    // Maximum health
    protected int MaxHealth;
    // How many health will player loose
    protected int attack;
    // How many exp will palyer gained when player killed this enemy
    protected int expGained;
    protected float SpawnDistance = 1.2f;
    public void Init(string ID)
    {
        enemyData = GameDataManager.EnemyData[ID];
        var playerPos = PlayerManager.Instance.playerView.transform.position;
        Vector3 spawnPos;
        while (Vector3.Distance(playerPos, spawnPos = GetSpawnPosition()) < SpawnDistance);
        transform.position = spawnPos;
    }
    protected abstract void Approching();
    protected abstract Vector3 GetSpawnPosition();
    
    // How enemy been hit
    public virtual void TakeHit(int bulletAttack)
    {
        Health -= bulletAttack;
        if (Health <= 0)
        {
            Health = 0;
            Destroy(gameObject);
        }
    }

    // What will happen when enemy encountered player
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.GetHurt(attack);
        }
    }
    private void Update()
    {
        Approching();
    }
}


