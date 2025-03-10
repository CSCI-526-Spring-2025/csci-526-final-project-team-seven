using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathPanel : UIPanel
{
    public Button restartButton;
    public Button exitButton;
    public override void Init()
    {
        base.Init();
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        exitButton.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
    }
}
