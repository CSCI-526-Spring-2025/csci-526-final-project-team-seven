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
    public Transform platformContainer;
    public GameObject Trunk1;

    public override void Init()
    {
        base.Init();
        tutorialButton.onClick.AddListener(() =>
        {
            Tutorial.Instance.tutorial = true;
            PlatformGenerator.Instance.Init();
            UIGameManager.Instance.SetCanOpen<PausePanel>(true);

            float y = -28;
            float t = 5;
            if (LevelManager.Instance.skipCredit)
            {
                envTransform.Find("Trunk1").gameObject.SetActive(false);
                platformContainer.transform.localPosition = new Vector3(0, 14.25f);
                y = -14;
                t = 1;
            }
            envTransform.DOMoveY(y, t).onComplete += () =>
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

            float y = -28;
            float t = 5;
            if (LevelManager.Instance.skipCredit)
            {
                envTransform.Find("Trunk1").gameObject.SetActive(false);
                platformContainer.transform.localPosition = new Vector3(0, 14.25f);
                y = -14;
                t = 1;
            }
            envTransform.DOMoveY(y, t).onComplete += () =>
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
