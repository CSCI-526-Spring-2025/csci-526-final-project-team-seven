using Async;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance { get; private set; }
    public GameObject[] tutorialGameObjects;
    public bool tutorial = true;
    //public TMP_Text tutorialText;
    public Canvas tutorialCanvas;
    public GameObject basicUI;
    private int cnt = 0;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (!tutorial)
        {
            basicUI.SetActive(true);
            cnt = 6;
            PlatformGenerator.Instance.StartGenerating();
            LevelManager.Instance.BeforeNextWave();
            LevelManager.Instance.NextWave();
            UIGameManager.Instance.SetCanOpen<WeaponPanel>(true);
            EndTutorial();
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
        if (cnt == 1 && PlayerManager.Instance.playerView.transform.position.y > -1 && PlayerManager.Instance.playerView.isGrounded)  
        {
            cnt++;
            SetTutorialGameObject();
            UIGameManager.Instance.SetCanOpen<WeaponPanel>(true);
            //PlatformGenerator.Instance.GenerateOneLayer(new bool[] { false, true, false });
            EnemyManager.Instance.GenerateSpecificEnemy(0, Vector3.zero);
        }
        if (cnt == 2 && UIGameManager.Instance.GetOpen<WeaponPanel>()) 
        {
            cnt++;
            SetTutorialGameObject();
            tutorialCanvas.sortingOrder = 3;
            //tutorialText.text = "Here you could DRAG cards to customize your weapon. Try dragging one card onto your weapon!";
            basicUI.SetActive(true);
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
            PlatformGenerator.Instance.StartGenerating();
            Lava.Instance.SetCameraDistance(5);
            CameraController.Instance.StartMove();
            EnemyManager.Instance.StartSpawn();
            LevelManager.Instance.BeforeNextWave();
            LevelManager.Instance.NextWave();
            
        }
        if (cnt == 5)
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
            tutorialGameObjects[i].SetActive(i == cnt);
        }
    }
}
