using Async;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance { get; private set; }
    public CanvasGroup[] tutorialGameObjects;
    public bool tutorial = true;
    public Canvas tutorialCanvas;
    public CanvasGroup basicUI;
    public CanvasGroup basicUI2;
    private int cnt = -1;
    private void Awake()
    {
        Instance = this;
    }
    public void Init()
    {
        if (!tutorial)
        {
            basicUI.alpha = 1;
            basicUI2.alpha = 1;
            cnt = 7;
            PlatformGenerator.Instance.StartGenerating();
            LevelManager.Instance.NextWave();
            Lava.Instance.SetCameraDistance(5);
            CameraController.Instance.StartMove();
            UIGameManager.Instance.SetCanOpen<WeaponPanel>(true);
            EndTutorial();
        }
        else
        {
            cnt++;
            SetTutorialGameObject();
        }
    }
    private void Update()
    {
        if (cnt == 0 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))) 
        { 
            cnt++;
            SetTutorialGameObject();
            PlatformGenerator.Instance.GenerateOneLayer(new bool[] { true, false, false });

        }
        if (cnt == 1 && PlayerManager.Instance.playerView.transform.position.y > -1 && PlayerManager.Instance.playerView.isGround)  
        {
            cnt++;
            SetTutorialGameObject();
            UIGameManager.Instance.SetCanOpen<WeaponPanel>(true);
            //PlatformGenerator.Instance.GenerateOneLayer(new bool[] { false, true, false });
            EnemyManager.Instance.GenerateSpecificEnemy(0, new Vector3(5, 5, 0));
        }
        if (cnt == 2 && UIGameManager.Instance.GetOpen<WeaponPanel>()) 
        {
            cnt++;
            SetTutorialGameObject();
            tutorialCanvas.sortingOrder = 3;
            
            UIGameManager.Instance.SetCanClose<WeaponPanel>(false);
        }
        if (cnt == 3 && InventoryManager.Instance.inventoryView.cardViews.Count < 1)
        {
            cnt++;
            SetTutorialGameObject();
            UIGameManager.Instance.SetCanClose<WeaponPanel>(true);
            
        }
        if (cnt == 4 && !UIGameManager.Instance.GetOpen<WeaponPanel>())
        {
            cnt++;
            SetTutorialGameObject();
            tutorialCanvas.sortingOrder = 1;
        }
        if (cnt == 5 && EnemyManager.Instance.enemyViews.Count == 0) 
        {
            cnt++;
            SetTutorialGameObject();

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1f);
            sequence.AppendCallback(PlatformGenerator.Instance.GenerateOneLayer);
            sequence.AppendInterval(1f);
            sequence.AppendCallback(PlatformGenerator.Instance.GenerateOneLayer);
            sequence.AppendInterval(1f);
            sequence.AppendCallback(() =>
            {
                basicUI.DOFade(1, 0.5f);
                basicUI2.DOFade(1, 0.5f);
            });
            sequence.AppendInterval(1f);
            sequence.AppendCallback(() =>
            {
                PlatformGenerator.Instance.StartGenerating();
                Lava.Instance.SetCameraDistance(5);
                CameraController.Instance.StartMove();
                LevelManager.Instance.NextWave();
            });
        }
        if (cnt == 6)
        {
            cnt++;
            Invoke("EndTutorial", 4f);
            
        }
    }
    private void EndTutorial()
    {
        SetTutorialGameObject();
    }
    private void SetTutorialGameObject()
    {
        for (int i = 0; i < tutorialGameObjects.Length; i++) 
        {

            tutorialGameObjects[i].DOFade(i == cnt ? 1 : 0, 0.5f).SetUpdate(true);
        }
    }
}
