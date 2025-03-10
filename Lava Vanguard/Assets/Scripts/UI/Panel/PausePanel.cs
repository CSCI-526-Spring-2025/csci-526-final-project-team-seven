using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : UIPanel
{
    public Button restartButton;
    public Button exitButton;
    protected override void Start()
    {
        base.Start();
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        exitButton.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
    }
}
