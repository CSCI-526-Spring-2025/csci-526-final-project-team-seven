using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class EnemyView : MonoBehaviour
{
    public EnemyData enemyData;
    // Minimum spawn distance between enemy and player
    protected float SpawnDistance = 1.2f;
    public GameObject deathEffect;
    public virtual void Init(string ID)
    {
        enemyData = GameDataManager.EnemyData[ID];
        transform.position = GetSpawnPosition();
    }

    protected abstract void Approching();
    protected abstract Vector3 GetSpawnPosition();

    // How enemy been hit
    public virtual void TakeHit(int bulletAttack)
    {
        enemyData.Health -= bulletAttack;
        if (enemyData.Health <= 0)
        {
            enemyData.Health = 0;
            StartCoroutine(DeathEffect());
            PlayerManager.Instance.playerView.playerData.coin += enemyData.Coin;
            UIGameManager.Instance.UpdateCoin();
            Destroy(gameObject);
        }
        else
        {
            HitEffect();
        }
    }

    // What will happen when enemy encountered player
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.GetHurt(enemyData.Attack);
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


