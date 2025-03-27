    using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance { get; private set; }
    public bool tutorial = true;
    public TMP_Text tutorialText;
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
            cnt = 7;
            tutorialText.text = "";
            LevelManager.Instance.NextWave();
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
            cnt++;
        }
        if (cnt == 2 && UIGameManager.Instance.GetOpen<WeaponPanel>()) 
        {
            tutorialText.text = "Here you could drag card to customize your weapon. Press E again to exit wepon editor.";
            basicUI.SetActive(true);
            cnt++;
        }
        if (cnt == 3 && !UIGameManager.Instance.GetOpen<WeaponPanel>())
        {
            tutorialText.text = "Survive as long as possible!\nThe bullet will aim automatically.";
            EnemyManager.Instance.StartSpawn();
            LevelManager.Instance.NextWave();
            cnt++;
        }
        if (cnt == 4 && UIGameManager.Instance.GetOpen<CardSelectorPanel>()) 
        {
            tutorialText.text = "Once you survived one wave, you got a new module. Choose one to continue.";
            cnt++;
        }
        if (cnt == 5 && !UIGameManager.Instance.GetOpen<CardSelectorPanel>())
        {
            tutorialText.text = "Now avoid lava, shoot enemies, keep going up and stay alive!";
            CameraController.Instance.StartMove();
            LevelGenerator.Instance.lava.SetCameraDistance(5);
            cnt++;
        }
        if (cnt == 6)
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
