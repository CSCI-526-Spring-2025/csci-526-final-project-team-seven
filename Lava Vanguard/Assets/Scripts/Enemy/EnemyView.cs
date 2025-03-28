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
    public SpriteRenderer spriteRenderer;
    public virtual void Init(string ID)
    {
        enemyData = GameDataManager.EnemyData[ID];
        transform.position = GetSpawnPosition();
    }

    protected abstract void Approching();
    protected abstract Vector3 GetSpawnPosition();

    // How enemy been hit
    public virtual bool TakeHit(int bulletAttack)
    {
        enemyData.Health -= bulletAttack;
        if (enemyData.Health <= 0)
        {
            enemyData.Health = 0;
            StartCoroutine(DeathEffect());
            PlayerManager.Instance.playerView.playerData.coin += enemyData.Coin;
            UIGameManager.Instance.UpdateCoin();
            Destroy(gameObject);
            return true;
        }
        else
        {
            HitEffect();
            return false;
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
            PlayerManager.Instance.GetHurt(enemyData.Attack);
        }
    }

    private void Update()
    {
        Approching();
    }

    protected IEnumerator DeathEffect()
    {
        var e = Instantiate(deathEffect, transform.position, Quaternion.identity);
        //Debug.Log("Destroy");
        Destroy(e, 1.0f);
        yield return new WaitForSeconds(0.5f);
    }

    protected virtual void HitEffect()
    {
        StartCoroutine(ChangeColorTemporarily(Color.red, 0.05f)); // Change to desired color and duration
    }

    protected virtual IEnumerator ChangeColorTemporarily(Color color, float duration)
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = originalColor;
        }
    }
}


