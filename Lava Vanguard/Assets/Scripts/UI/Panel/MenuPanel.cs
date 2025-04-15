using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Async;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using static System.Collections.Specialized.BitVector32;
public class MenuPanel : UIPanel
{
    public Button tutorialButton;
    public Button startButton;
    public Button quitButton;
    public Transform envTransform;
    public override void Init()
    {
        base.Init();
        tutorialButton.onClick.AddListener(() =>
        {
            Tutorial.Instance.tutorial = true;
            PlatformGenerator.Instance.Init();
            //PlatformGenerator.Instance.StartGenerating();
            UIGameManager.Instance.SetCanOpen<PausePanel>(true);
            envTransform.DOMoveY(-28, 5f).onComplete += () =>
            {
                PlayerManager.Instance.Init();
                
                Tutorial.Instance.Init();
            };
           
            Close();
            EventSystem.current.SetSelectedGameObject(null);
            tutorialButton.onClick.RemoveAllListeners();
        });
        startButton.onClick.AddListener(() =>
        {
            Tutorial.Instance.tutorial = false;
            PlatformGenerator.Instance.Init();
            PlatformGenerator.Instance.StartGenerating();
            UIGameManager.Instance.SetCanOpen<PausePanel>(true);
            envTransform.DOMoveY(-28, 5f).onComplete += () =>
            {
                PlayerManager.Instance.Init();
                
                Tutorial.Instance.Init();
            };
           
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

        gameObject.SetActive(false);

    }
}
