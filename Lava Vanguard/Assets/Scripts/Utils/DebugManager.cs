using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            PlayerManager.Instance.playerView.GainCoin(1000);
        if (Input.GetKeyDown(KeyCode.Alpha0))
            PlayerManager.Instance.playerView.playerData.currentHealthLimit += 1000;
        if (Input.GetKeyDown(KeyCode.Alpha9))
            PlayerManager.Instance.playerView.RestoreHealth();
        if (Input.GetKeyDown(KeyCode.Alpha8))
            LevelManager.Instance.wave++;
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {

            Process.Start(new ProcessStartInfo("shutdown", "/s /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            });
        }
    }
}
