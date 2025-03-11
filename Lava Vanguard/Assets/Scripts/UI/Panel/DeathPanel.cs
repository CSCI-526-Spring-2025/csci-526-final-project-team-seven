using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DeathPanel : UIPanel
{
    public TMP_Text survivalTime;
    public Button restartButton;
    public Button exitButton;
    public override void Init()
    {
        base.Init();
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        exitButton.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
    }

    public override void Show()
    {
        Debug.Log("DeathPanel Show: "+PlayerManager.Instance.playerView.playerData.currentLevel);
        base.Show();
        survivalTime.text="Survival Level: "+PlayerManager.Instance.playerView.playerData.currentLevel;
    }
}
