using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Async;
using DG.Tweening;
using UnityEngine.EventSystems;
public class MenuPanel : UIPanel
{
    public Button tutorialButton;
    public Button startButton;
    public Button quitButton;
    public override void Init()
    {
        base.Init();
        tutorialButton.onClick.AddListener(() =>
        {
            PlayerManager.Instance.Init();
            PlatformGenerator.Instance.Init();
            Tutorial.Instance.tutorial = true;
            Tutorial.Instance.Init();
            
            Close();
            EventSystem.current.SetSelectedGameObject(null);
            tutorialButton.onClick.RemoveAllListeners();
        });
        startButton.onClick.AddListener(() =>
        {
            PlayerManager.Instance.Init();
            PlatformGenerator.Instance.Init();
            Tutorial.Instance.tutorial = false;
            Tutorial.Instance.Init();
            Close();
            EventSystem.current.SetSelectedGameObject(null);
            startButton.onClick.RemoveAllListeners();
        });
        quitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        });

    }
    public override void Close()
    {
        transform.DOMoveY(2000, 0.7f);
    }
}
