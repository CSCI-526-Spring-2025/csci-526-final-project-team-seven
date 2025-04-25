using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Async;
using System;
using UnityEngine.Networking;

public class DeathPanel : UIPanel
{
    //public TMP_Text survivalTime;
    public Button reviveButton;
    public Button menuButton;
    public Button exitButton;

    [Header("Submit score")]
    public Button submitScoreButton;
    public TMP_InputField nameInputField;
    public TMP_Text currentWaveText;
    public TMP_Text currentKilledText;
    public TMP_Text currentReviveText;

    [Header("No Revive")]
    public GameObject noReviveSubPanel;
    public Button noReviveHeadButton;
    public Button noReviveSortByWaveButton;
    public Button noReviveSortByKilledButton;
    public RankingNoReviveRow[] noReviveRows;
    public RankingNoReviveRow userNoReviveRow;

    [Header("With Revive")]
    public GameObject withReviveSubPanel;
    public Button withReviveHeadButton;
    public Button withReviveSortByWaveButton;
    public Button withReviveSortByKilledButton;
    public Button withReviveSortByReviveButton;
    public RankingWithReviveRow[] withReviveRows;
    public RankingWithReviveRow userWithReviveRow;

    [HideInInspector] public int revive = 0;
    private string[] rankingTitle = { "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th", "10th" };

    private string baseUrl = "https://csci526teamsevenranking-default-rtdb.firebaseio.com/";
    public override void Init()
    {
        base.Init();
        menuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        reviveButton.onClick.AddListener(Revive);
        exitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }); ;

        
        // Setup submit score
        nameInputField.onValidateInput = (text, index, ch) => (char.IsLetterOrDigit(ch) && text.Length < 9) ? ch : '\0';
        submitScoreButton.onClick.AddListener(submitScore);

        // Setup no revive
        noReviveHeadButton.onClick.AddListener(noReviveSetUp);

        // Setup with revive
        withReviveHeadButton.onClick.AddListener(withReviveSetUp);
    }


    public override void Open()
    {
        CameraZoomAndMove.Instance.ZoomAndMove(() =>
        {
            base.Open();
            noReviveSetUp();
        }); ;
        UIGameManager.Instance.SetFocus(true);
        if (Tutorial.Instance.cnt < 13 || LevelManager.Instance.wave < 1)
            reviveButton.gameObject.SetActive(false);
        else 
            reviveButton.gameObject.SetActive(true);
        //TODO: survivalTime
        currentWaveText.text="Wave: "+LevelManager.Instance.wave;
        currentKilledText.text = "Killed: "+EnemyManager.Instance.enemyKilled;
        currentReviveText.text = "Revive: " + revive;
    }
    public override void Close()
    {
        base.Close();
        CameraZoomAndMove.Instance.ResetCamera();
        Tooltip.Instance.HideTooltip();
        UIGameManager.Instance.SetFocus(false);
    }
    public void Revive()
    {
        revive++;
        isOpen = false;
        gameObject.SetActive(false);
        blocker.SetActive(false);
        Tooltip.Instance.HideTooltip();
        UIGameManager.Instance.SetFocus(false);

        PlatformGenerator.Instance.canGenerate = false;
        EnemyManager.Instance.StopSpawn();
        EnemyManager.Instance.killAll();
        EnemyManager.Instance.KillBoss();
        
        CameraZoomAndMove.Instance.ResetCameraInstantly();
        Vector3 position = PlatformGenerator.Instance.GetRevivePosition();
       
        CameraController.Instance.StopCamera();
        CameraController.Instance.SetCameraY(position.y + 4.5f);
        PlayerManager.Instance.playerView.SetPosition(position + new Vector3(0, 0.5f, 0));

        LevelManager.Instance.StopTimer();

        LevelManager.Instance.wave = GameDataManager.SavedLevelData.Wave;
        LevelManager.Instance.waveText.text = "Wave " + (GameDataManager.SavedLevelData.Wave + 1);

        PlayerManager.Instance.playerView.playerData.health = GameDataManager.SavedLevelData.Health;
        PlayerManager.Instance.playerView.playerData.coin = GameDataManager.SavedLevelData.Coin;

        UIGameManager.Instance.UpdateHp();
        UIGameManager.Instance.UpdateCoin();

        SlotManager.Instance.Init(true);
        InventoryManager.Instance.Init(true);

        UIGameManager.Instance.Open<CardSelectorPanel>();
    }

    private void submitScore()
    {
        StartCoroutine(postScore());
    }

    private IEnumerator postScore()
    {
        string url = baseUrl + "rankingNoRevive.json";
        string json = "{"
            + "\"name\":\"" + nameInputField.text + "\","
            + "\"wave\":" + +LevelManager.Instance.wave + ","
            + "\"killed\":" + EnemyManager.Instance.enemyKilled
            + "}";
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);
        using var uwr = new UnityWebRequest(url, "POST");
        uwr.uploadHandler = new UploadHandlerRaw(body);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Submission failed" + uwr.error);
        }
        else
        {
            Debug.Log("Submission success" + uwr.downloadHandler.text);
        }
    }

    private void noReviveSetUp()
    {
        noReviveSubPanel.SetActive(true);
        withReviveSubPanel.SetActive(false);
        noReviveHeadButton.image.color = ColorCenter.RankingPanelColors["HeadButtonActive"];
        withReviveHeadButton.image.color = ColorCenter.RankingPanelColors["HeadButtonInactive"];
        StartCoroutine(getScore());
    }

    private IEnumerator getScore()
    {
        string url = baseUrl + "rankingNoRevive.json";

        using var uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Get Score Fail " + uwr.error);
            yield break;
        }

        string json = uwr.downloadHandler.text;
        Debug.Log("Original JSON is:\n" + json);
    }

    private void withReviveSetUp()
    {
        noReviveSubPanel.SetActive(false);
        withReviveSubPanel.SetActive(true);
        noReviveHeadButton.image.color = ColorCenter.RankingPanelColors["HeadButtonInactive"];
        withReviveHeadButton.image.color = ColorCenter.RankingPanelColors["HeadButtonActive"];
    }
}
