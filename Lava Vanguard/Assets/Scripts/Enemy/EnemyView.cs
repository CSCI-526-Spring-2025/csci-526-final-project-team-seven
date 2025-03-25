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
            StartCoroutine(DeathEffect());
            Destroy(gameObject);
        } else {
            HitEffect();
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

    protected IEnumerator DeathEffect()
    {
        var e = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Debug.Log("Destroy");
        Destroy(e, 1.0f);
        yield return new WaitForSeconds(0.5f);
    }

    public void HitEffect()
    {
        StartCoroutine(ChangeColorTemporarily(Color.red, 0.05f)); // Change to desired color and duration
    }

    protected IEnumerator ChangeColorTemporarily(Color color, float duration)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = originalColor;
        }
    }
}


