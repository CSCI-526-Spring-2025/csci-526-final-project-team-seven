using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance { get; private set; }
    private int cnt = 0;
    private bool enableDebug = true;
    private void Awake()
    {
        Instance = this;
    }
    public int getCnt()
    {
        return cnt;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerManager.Instance.playerView.GainCoin(1000);
            cnt++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlayerManager.Instance.playerView.playerData.currentHealthLimit += 1000;
            UIGameManager.Instance.UpdateHp();
            cnt++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            PlayerManager.Instance.playerView.RestoreHealth();
            UIGameManager.Instance.UpdateHp();
            cnt++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            LevelManager.Instance.wave++;
            cnt++;
        }
        if (Input.GetKeyDown(KeyCode.H)) 
            enableDebug = !enableDebug;
        if (cnt == 3 && !enableDebug) 
        {
            UIGameManager.Instance.Open<CheatPanel>();
            Time.timeScale = 0;
        }
    }
}
