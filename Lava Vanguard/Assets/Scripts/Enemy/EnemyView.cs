using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class EnemyView : MonoBehaviour
{
    public EnemyData enemyData;
    // Current health
    protected int health;
    // Maximum health
    protected int maxHealth;
    // How many health will player loose
    protected int attack;
    // How many exp will palyer gained when player killed this enemy
    protected int expGained;
    // Minimum spawn distance between enemy and player
    protected float SpawnDistance = 1.2f;
    public GameObject deathEffect;
    public virtual void Init(string ID)
    {
        enemyData = GameDataManager.EnemyData[ID];
        health = enemyData.Health;
        maxHealth = enemyData.MaxHealth;
        attack = enemyData.Attack;
        expGained = enemyData.ExpGained;
        transform.position = GetSpawnPosition();
    }

    protected abstract void Approching();
    protected abstract Vector3 GetSpawnPosition();
    
    // How enemy been hit
    public virtual void TakeHit(int bulletAttack)
    {
        health -= bulletAttack;
        if (health <= 0)
        {
            health = 0;
            PlayerManager.Instance.GainEXP(expGained);
            Instantiate(deathEffect, transform.position, Quaternion.identity);
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

    public virtual void OnChildTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 bossPos = transform.position;
                Vector2 playerPos = other.transform.position;

                Vector2 pushDir = (playerPos - bossPos);
                pushDir.y = 0f;
                if (pushDir == Vector2.zero)
                    pushDir = Vector2.right;
                pushDir.Normalize();

                float pushSpeed = 3f;
                playerRb.velocity = pushDir * pushSpeed;
            }
            PlayerManager.Instance.GetHurt(attack);
        }
    }

    private void Update()
    {
        Approching();
    }
}


