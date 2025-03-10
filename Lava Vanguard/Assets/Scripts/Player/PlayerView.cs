using Async;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private PlayerData playerData;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTempTime = 0.0f;
    private int health;
    private int exp = 0;
    private int currentLevelExp = 2;
    private int currentLevel = 1;
    private float invincibleTempTime = 0.0f;

    [HideInInspector]
    public float speedMultiplier = 1.0f;
    //GoD! JUst temP CoDe


    public void Init()
    {
        playerData = PlayerData.DefaultData;
        invincibleTempTime = 0.0f;
        health = playerData.maxHealth;
        currentLevel = 1;
        exp = 0;
        currentLevelExp = 2;
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveLeft()
    {
        rb.velocity = new Vector2(-playerData.speed * speedMultiplier, rb.velocity.y);
        if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void MoveRight()
    {
        rb.velocity = new Vector2(playerData.speed * speedMultiplier, rb.velocity.y);
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    public void MoveStop()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void JumpStart()
    {
        if (isGrounded)
        {
            isJumping = true;
            jumpTempTime = playerData.jumpAirTime;
            rb.velocity = new Vector2(rb.velocity.x, playerData.jumpForce);
            isGrounded = false;
        }
    }

    public void JumpMaintain()
    {
        if (isJumping && jumpTempTime > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, playerData.jumpAirForce);
            jumpTempTime -= Time.deltaTime;
        }
    }

    public void JumpStop()
    {
        isJumping = false;
    }
    public void Shoot()
    {
        if (!Async.SequenceManager.Instance) return;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    public void UpdateHealth(int damage, bool mustKilled = false)
    {
        if (mustKilled)
        {
            health = 0;
            PlayerManager.Instance.KillPlayer();
            return;
        }
        if (invincibleTempTime > 0)
        {
            return;
        }
        invincibleTempTime = playerData.invincibleTime;
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            PlayerManager.Instance.KillPlayer();
        }
    }

    public void HealthUp()
    {
        playerData.maxHealth += 2;
        health = playerData.maxHealth;
    }
    public void UpdateInvincible()
    {
        if (invincibleTempTime > 0)
        {
            invincibleTempTime -= Time.deltaTime;
        }
    }

    public float GetHealthPercent()
    {
        return 1.0f * health / playerData.maxHealth;
    }

    public float GetMaxHealth()
    {
        return playerData.maxHealth;
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    public void UpdateExp(int exp)
    {
        this.exp += exp;
        while (this.exp >= currentLevelExp)
        {
            this.exp -= currentLevelExp;
            currentLevelExp += 1;
            currentLevel += 1;
            CardSelectorManager.Instance.StartSelection();
        }
    }

    public float GetEXPPercent()
    {
        return 1.0f * exp / currentLevelExp;
    }

    public int GetLevel()
    {
        return currentLevel;
    }
}
