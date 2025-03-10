using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{

    public PlayerData(float speed, float jumpForce, float jumpAirTime, float jumpAirForce, int maxHealth, float invincibleTime)
    {
        this.speed = speed;
        this.jumpForce = jumpForce;
        this.jumpAirTime = jumpAirTime;
        this.jumpAirForce = jumpAirForce;
        this.maxHealth = maxHealth;
        this.invincibleTime = invincibleTime;
    }
    public static PlayerData DefaultData = new PlayerData(
        speed: 5f,
        jumpForce: 5f,
        jumpAirTime: 0.3f,
        jumpAirForce: 5f,
        maxHealth: 10,
        invincibleTime: 1f
    );
    public int health;
    public int exp;
    public int currentLevelExp = 2;
    public int currentLevel = 1;

    public float speed;
    public float jumpForce;
    public float jumpAirTime;
    public float jumpAirForce;
    public int maxHealth;
    public float invincibleTime;
    
}
