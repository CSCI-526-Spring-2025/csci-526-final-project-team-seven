using Async;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance { get; private set; }
    public bool tutorial = true;
    public TMP_Text tutorialText;
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
            cnt = 8;
            tutorialText.text = "";
            LevelManager.Instance.NextWave();
            UIGameManager.Instance.SetCanOpen<WeaponPanel>(true);
        }

    }
    private void Update()
    {
        if (cnt == 0 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            LevelGenerator.Instance.StartGenerate();
            tutorialText.text = "Good. Now press W/Space/K to jump.";
            cnt++;
        }
        if (cnt == 1 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Space)))
        {
            tutorialText.text = "Press E to open your weapon editor.";
            UIGameManager.Instance.SetCanOpen<WeaponPanel>(true);
            cnt++;
        }
        if (cnt == 2 && UIGameManager.Instance.GetOpen<WeaponPanel>()) 
        {
            tutorialCanvas.sortingOrder = 3;
            tutorialText.text = "Here you could DRAG cards to customize your weapon. Try dragging one card onto your weapon!";
            basicUI.SetActive(true);
            UIGameManager.Instance.SetCanClose<WeaponPanel>(false);
            cnt++;
        }
        if (cnt == 3 && InventoryManager.Instance.inventoryView.cardViews.Count < 4)
        {
            tutorialText.text = "WOW! You succseefully added one module! Different modules may have their connections. You could try adding more modules or press E to exit weapon panel directly.";
            UIGameManager.Instance.SetCanClose<WeaponPanel>(true);
            cnt++;
        }
        if (cnt == 4 && !UIGameManager.Instance.GetOpen<WeaponPanel>())
        {
            tutorialCanvas.sortingOrder = 1;
            tutorialText.text = "Survive as long as possible!\nThe bullet will aim automatically.";
            EnemyManager.Instance.StartSpawn();
            LevelManager.Instance.NextWave();
            cnt++;
        }
        if (cnt == 5 && UIGameManager.Instance.GetOpen<CardSelectorPanel>()) 
        {
            tutorialText.text = "Survive as long as possible!";
            cnt++;
        }
        if (cnt == 6 && !UIGameManager.Instance.GetOpen<CardSelectorPanel>())
        {
            tutorialText.text = "Now avoid lava, kill enemies, keep going up and stay alive!";
            CameraController.Instance.StartMove();
            LevelGenerator.Instance.lava.SetCameraDistance(5);
            cnt++;
        }
        if (cnt == 7)
        {
            Invoke("EndTutorial", 4f);
            cnt++;
        }
    }
    private void EndTutorial()
    {
        tutorialText.text = "";
    }
}
