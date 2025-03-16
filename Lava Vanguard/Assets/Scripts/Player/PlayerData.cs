using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{

    public PlayerData(float speed, float jumpForce, float jumpAirTime, float jumpAirForce, int healthLimit, float invincibleTime, int healthUpValue)
    {
        this.speed = speed;
        this.jumpForce = jumpForce;
        this.jumpAirTime = jumpAirTime;
        this.jumpAirForce = jumpAirForce;
        this.healthLimit = healthLimit;
        this.invincibleTime = invincibleTime;
        this.healthUpValue = healthUpValue;
    }
    public static PlayerData DefaultData = new PlayerData(
        speed: 5f,
        jumpForce: 5f,
        jumpAirTime: 0.3f,
        jumpAirForce: 5f,
        healthLimit: 10,
        invincibleTime: 1f,
        healthUpValue:2
    );
    public int health;
    public int currentHealthLimit;
    public int exp;
    public int currentLevelExp = 2;
    public int currentLevel = 1;

    public float speed;
    public float jumpForce;
    public float jumpAirTime;
    public float jumpAirForce;
    public int healthLimit;
    public float invincibleTime;
    public int healthUpValue;
}
