using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public PlayerView playerView;
    public GameObject deathMenuPanel;
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
    
    public void GainEXP(int exp)
    {
        Debug.Log("Gain EXP"+exp);
        playerView.UpdateExp(exp);
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
#if UNITY_EDITOR
        FindObjectOfType<SendToGoogle>().RecordEndTime();
#endif
#if UNITY_WEBGL
        FindObjectOfType<SendToGoogle>().RecordEndTime();
#endif
        deathMenuPanel.SetActive(true);
    }
    /*
        When the player leaves the ground, isGrounded is set to false
        so that the player can't jump in the air.
        */
    // void OnCollisionExit2D(Collision2D collision){
    //     if(collision.gameObject.CompareTag("Ground")){
    //         isGrounded=false;
    //     }
    // }
}

