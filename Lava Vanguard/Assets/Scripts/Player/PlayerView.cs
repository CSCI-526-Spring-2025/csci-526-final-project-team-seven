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
        playerData.health = playerData.healthLimit;
        playerData.currentHealthLimit = playerData.healthLimit;
        rb = GetComponent<Rigidbody2D>();
        UIGameManager.Instance.UpdateHp();
        UIGameManager.Instance.UpdateCoin();
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
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.name == "wave_platform")
        {
            isGrounded = true;

            // The player jumps on the wave platform
            if (collision.gameObject.name == "wave_platform")
            {
                // should be safe
                if (collision.GetContact(0).normal.y > 0.5f && !LevelManager.Instance.BreakTime)
                {
                    Debug.Log("Player on the wave platform");
                    LevelManager.Instance.BreakTime = true;
                    LevelManager.Instance.BeforeNextWave();
                    CameraController.Instance.UpdateDistance(LevelGenerator.Instance.WavePlatform.transform);
                }
            }
            // else if (LevelGenerator.Instance.WavePlatform && LevelManager.Instance.BreakTime)
            // {
            //     // Once the player jumps on the first ground, start next wave
            //     var p = LevelGenerator.Instance.WavePlatform;
            //     foreach (var contact in collision.contacts)
            //     {
            //         if (contact.normal.y > 0.5f) {
            //             CameraController.Instance.ResumeCamera();
            //             LevelManager.Instance.NextWave();
            //             LevelManager.Instance.BreakTime = false;
            //             break;
            //         }
            //     }
            // }
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
        HitEffect();
        invincibleTempTime = playerData.invincibleTime;
        playerData.health -= damage;
        if (playerData.health <= 0)
        {
            playerData.health = 0;
            PlayerManager.Instance.KillPlayer();
        }
        UIGameManager.Instance.UpdateHp();
    }
    public void ResetHealthLimit()
    {
        playerData.currentHealthLimit = playerData.healthLimit;
        UIGameManager.Instance.UpdateHp();
    }
    public void ResetHealth()
    {
        if (playerData.health > playerData.currentHealthLimit)
        {
            playerData.health= playerData.currentHealthLimit;
        }
        UIGameManager.Instance.UpdateHp();
    }
    public void HealthUp()
    {
        playerData.currentHealthLimit += playerData.healthUpValue;
        UIGameManager.Instance.UpdateHp();
    }
    public void RestoreHealth()
    {
        playerData.health=playerData.currentHealthLimit;
        UIGameManager.Instance.UpdateHp();
    }
    public void UpdateInvincible()
    {
        if (invincibleTempTime > 0)
        {
            invincibleTempTime -= Time.deltaTime;
        }
    }

    public void HitEffect()
    {
        StartCoroutine(ChangeColorTemporarily(Color.red, 0.05f)); // Change to desired color and duration
        //cameraController.CameraShake(0.2f, 0.8f, 1f);
        CameraController.Instance.CameraShake(0.25f, 0.3f, 10);
    }

    private IEnumerator ChangeColorTemporarily(Color color, float duration)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = color;
            yield return new WaitForSeconds(duration);
            spriteRenderer.color = originalColor;
        }
    }
}
