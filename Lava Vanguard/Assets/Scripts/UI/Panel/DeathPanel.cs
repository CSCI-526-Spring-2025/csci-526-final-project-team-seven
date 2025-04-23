using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DeathPanel : UIPanel
{
    //public TMP_Text survivalTime;
    public Button reviveButton;
    public Button menuButton;
    public Button exitButton;
    public override void Init()
    {
        base.Init();
        menuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        reviveButton.onClick.AddListener(Revive);
        exitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }); ;
    }


    public override void Open()
    {
        CameraZoomAndMove.Instance.ZoomAndMove(base.Open);
        UIGameManager.Instance.SetFocus(true);
        if (Tutorial.Instance.cnt < 13 || LevelManager.Instance.wave < 1)
            reviveButton.gameObject.SetActive(false);
        else 
            reviveButton.gameObject.SetActive(true);
        //TODO: survivalTime
    }
    public override void Close()
    {
        base.Close();
        CameraZoomAndMove.Instance.ResetCamera();
        Tooltip.Instance.HideTooltip();
        UIGameManager.Instance.SetFocus(false);
    }
    public void Revive()
    {
        CameraZoomAndMove.Instance.ResetCameraInstantly();
        Vector3 position = PlatformGenerator.Instance.Revive();
        CameraController.Instance.SetCameraY(position.y + 4.5f);
        PlayerManager.Instance.playerView.SetPosition(position + new Vector3(0, 0.5f, 0));
    }
}
