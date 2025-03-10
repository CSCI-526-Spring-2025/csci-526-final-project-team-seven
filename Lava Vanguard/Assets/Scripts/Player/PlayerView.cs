using Async;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [HideInInspector]
    public PlayerData playerData;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTempTime = 0.0f;

    
    private float invincibleTempTime = 0.0f;
    [HideInInspector]
    public float speedMultiplier = 1.0f;
    //GoD! JUst temP CoDe


    public void Init()
    {
        playerData = PlayerData.DefaultData;
        invincibleTempTime = 0.0f;
        playerData.health = playerData.maxHealth;
        playerData.currentLevel = 1;
        playerData.exp = 0;
        playerData.currentLevelExp = 2;
        rb = GetComponent<Rigidbody2D>();
        UIGameManager.Instance.UpdateHp();
        UIGameManager.Instance.UpdateExp();
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
            playerData.health = 0;
            PlayerManager.Instance.KillPlayer();
            return;
        }
        if (invincibleTempTime > 0)
        {
            return;
        }
        invincibleTempTime = playerData.invincibleTime;
        playerData.health -= damage;
        if (playerData.health <= 0)
        {
            playerData.health = 0;
            PlayerManager.Instance.KillPlayer();
        }
        UIGameManager.Instance.UpdateHp();
    }

    public void HealthUp()
    {
        playerData.maxHealth += 2;
        playerData.health = playerData.maxHealth;
    }
    public void UpdateInvincible()
    {
        if (invincibleTempTime > 0)
        {
            invincibleTempTime -= Time.deltaTime;
        }
    }

    public void UpdateExp(int exp)
    {
        playerData.exp += exp;
        while (playerData.exp >= playerData.currentLevelExp) 
        {
            playerData.exp -= playerData.currentLevelExp;
            playerData.currentLevelExp += 1;
            playerData.currentLevel += 1;

            UIGameManager.Instance.Show<CardSelectorPanel>();
        }
        UIGameManager.Instance.UpdateExp();
    }
}
