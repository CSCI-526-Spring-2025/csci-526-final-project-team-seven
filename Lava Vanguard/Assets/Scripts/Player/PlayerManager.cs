using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public PlayerView playerView;
    public static PlayerManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
        //sessionID = DateTime.Now.Ticks;//generate unique id for forms
        //startTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
    }

    void Start()
    {
        playerView.Init();
    }

    void Update()
    {
        Move();
        Jump();
        UpdateInvincible();
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            playerView.MoveLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerView.MoveRight();
        }
        else
        {
            playerView.MoveStop();
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            playerView.JumpStart();
        }
        else if (Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            playerView.JumpMaintain();
        }
        else if (Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space))
        {
            playerView.JumpStop();
        }
    }
    

    public void GetHurt(int damage,bool mustKilled=false)
    {
        playerView.UpdateHealth(damage,mustKilled);
    } 

    void UpdateInvincible()
    {
        playerView.UpdateInvincible();
    }

    public void KillPlayer()
    {
        Time.timeScale = 0f;
        Debug.Log("kill player initiated");
        FindObjectOfType<SendToGoogle>().RecordEndTime();

        UIGameManager.Instance.Open<DeathPanel>();
    }
}

