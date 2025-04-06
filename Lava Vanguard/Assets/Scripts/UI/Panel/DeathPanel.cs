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
        base.Open();
        survivalTime.text = "Survival Wave: " + LevelManager.Instance.wave;
    }
}
