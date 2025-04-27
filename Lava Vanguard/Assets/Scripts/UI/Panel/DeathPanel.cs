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

    private int revive = 0;
    private string[] rankingTitle = { "1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th", "9th", "10th" };

    private int noReviveWaveData = -1;
    private int noReviveKilledData = -1;
    private string baseUrlRealtimeDatabase = "https://csci526teamsevenranking-default-rtdb.firebaseio.com/";
   
    //firestore
    private string projectId = "csci526teamsevenranking";
    private string apiKey = "";
    private string baseUrlFireStore = "https://firestore.googleapis.com/v1/projects/";
    private string dbPath = "/databases/(default)/documents:runQuery";
    private string writePath = "/databases/(default)/documents/";

    private string noReviveParts;
    private string withReviveParts;
    private int noReviveRank = 0;
    private int withReviveRank = 0;

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
        noReviveSortByWaveButton.onClick.AddListener(noReviveSortByWave);
        noReviveSortByKilledButton.onClick.AddListener(noReviveSortByKilled);
        // Setup with revive
        withReviveHeadButton.onClick.AddListener(withReviveSetUp);
        withReviveSortByWaveButton.onClick.AddListener(withReviveSortByWave);
        withReviveSortByKilledButton.onClick.AddListener(withReviveSortByKilled);
        withReviveSortByReviveButton.onClick.AddListener(withReviveSortByRevive);
    }

    public override void Open()
    {
        CameraZoomAndMove.Instance.ZoomAndMove(() =>
        {
            base.Open();
            noReviveSetUp();
        }); ;
        UIGameManager.Instance.SetFocus(true);
//        reviveButton.interactable = true;
        if (Tutorial.Instance.cnt < 15 || LevelManager.Instance.wave < 1)
            reviveButton.interactable = false;
        else
            reviveButton.interactable = true;
        nameInputField.interactable = true;
        submitScoreButton.interactable = true;
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
        if (revive == 0)
        {
            noReviveWaveData = LevelManager.Instance.wave;
            noReviveKilledData = EnemyManager.Instance.enemyKilled;
        }
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
        if (nameInputField.text.Length == 0)
        {
            return;
        }
        reviveButton.interactable = false;
        submitScoreButton.interactable = false;
        nameInputField.interactable = false;
        StartCoroutine(postScore());
    }

    private IEnumerator postScore()
    {
        if (revive == 0)
        {
            noReviveWaveData = LevelManager.Instance.wave;
            noReviveKilledData = EnemyManager.Instance.enemyKilled;
        }
        string url = baseUrlFireStore + projectId + writePath + "noRevive";
        string requestJson = @"
    {
      ""fields"": {
        ""name"":   { ""stringValue"": """ + nameInputField.text + @""" },
        ""wave"":   { ""integerValue"": """ + noReviveWaveData + @""" },
        ""killed"": { ""integerValue"": """ + noReviveKilledData + @""" }
      }
    }";
        byte[] body = System.Text.Encoding.UTF8.GetBytes(requestJson);
        using var uwr = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(body),
            downloadHandler = new DownloadHandlerBuffer()
        };
        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
            Debug.LogError("Submit noRevive Fail: " + uwr.error);
        else
            Debug.Log("Submit noRevive Success, response:\n" + uwr.downloadHandler.text);

        if (revive > 0)
        {
            url = baseUrlFireStore + projectId + writePath + "withRevive";
            requestJson = @"
        {
          ""fields"": {
            ""name"":   { ""stringValue"": """ + nameInputField.text + @""" },
            ""wave"":   { ""integerValue"": """ + LevelManager.Instance.wave + @""" },
            ""killed"": { ""integerValue"": """ + EnemyManager.Instance.enemyKilled + @""" },
            ""revive"": { ""integerValue"": """ + revive + @""" }
          }
        }";
            body = System.Text.Encoding.UTF8.GetBytes(requestJson);
            using var uwr2 = new UnityWebRequest(url, "POST")
            {
                uploadHandler = new UploadHandlerRaw(body),
                downloadHandler = new DownloadHandlerBuffer()
            };
            uwr2.SetRequestHeader("Content-Type", "application/json");
            yield return uwr2.SendWebRequest();

            if (uwr2.result != UnityWebRequest.Result.Success)
                Debug.LogError("Submit withRevive Fail: " + uwr2.error);
            else
                Debug.Log("Submit withRevive Success, response:\n" + uwr2.downloadHandler.text);
        }
    }

    private void noReviveSetUp()
    {
        noReviveSubPanel.SetActive(true);
        withReviveSubPanel.SetActive(false);
        noReviveHeadButton.image.color = ColorCenter.RankingPanelColors["HeadButtonActive"];
        withReviveHeadButton.image.color = ColorCenter.RankingPanelColors["HeadButtonInactive"];
        for(int i = 0; i < 10; i++)
        {
            noReviveRows[i].Set(rankingTitle[i], "--", "--", "--");
        }
        userNoReviveRow.Set("--/--", "You", "--", "--");
        StartCoroutine(getScore("noRevive","wave",noReviveWaveData));
    }

    private void withReviveSetUp()
    {
        noReviveSubPanel.SetActive(false);
        withReviveSubPanel.SetActive(true);
        noReviveHeadButton.image.color = ColorCenter.RankingPanelColors["HeadButtonInactive"];
        withReviveHeadButton.image.color = ColorCenter.RankingPanelColors["HeadButtonActive"];
        for(int i = 0; i < 10; i++)
        {
            withReviveRows[i].Set(rankingTitle[i], "--", "--", "--", "--");
        }
        userWithReviveRow.Set("--/--", "You", "--", "--", "--");
        StartCoroutine(getScore("withRevive","wave",LevelManager.Instance.wave));
    }
    private void noReviveSortByWave()
    {
        StartCoroutine(getScore("noRevive", "wave", noReviveWaveData));
    }
    private void noReviveSortByKilled()
    {
        StartCoroutine(getScore("noRevive", "killed", noReviveWaveData));
    }
    private void withReviveSortByWave()
    {
        StartCoroutine(getScore("withRevive", "wave", LevelManager.Instance.wave));
    }
    private void withReviveSortByKilled()
    {
        StartCoroutine(getScore("withRevive", "killed", LevelManager.Instance.wave));
    }
    private void withReviveSortByRevive()
    {
        StartCoroutine(getScore("withRevive", "revive", LevelManager.Instance.wave));
    }
    private IEnumerator getScore(string collectionId, string sortByField, int greaterThanValue)
    {
        string url = baseUrlFireStore + projectId + dbPath;

        // Get descending by wave
        string queryJson = "{ \"structuredQuery\": {"
            + "\"from\":[{\"collectionId\":\"" + collectionId + "\"}],"
            + "\"orderBy\":[{"
                + "\"field\":{\"fieldPath\":\"" + sortByField + "\"},"
                + "\"direction\":\"DESCENDING\""
            + "}],"
            + "\"limit\":10"
            + "} }";

        byte[] body = System.Text.Encoding.UTF8.GetBytes(queryJson);
        using var uwr = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(body),
            downloadHandler = new DownloadHandlerBuffer()
        };
        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Get Score Fail: " + uwr.error);
            yield break;
        }

        var parts = uwr.downloadHandler.text;
        Debug.Log(parts);
        if (collectionId == "noRevive")
        {
            noReviveParts = parts;
            parseNoReviveParts();
        }
        else 
        {
            withReviveParts = parts;
            parseWithReviveParts();
        }
    }

    private void parseNoReviveParts()
    {
        string wrapped = "{\"entries\":" + noReviveParts + "}";
        var list = JsonUtility.FromJson<FirestoreList>(wrapped);
        for (int i = 0; i < noReviveRows.Length; i++)
        {
            if (i < list.entries.Length)
            {
                var entry = list.entries[i];
                string name = entry.document.fields.name.stringValue;
                int wave = int.Parse(entry.document.fields.wave.integerValue);
                int killed = int.Parse(entry.document.fields.killed.integerValue);
                noReviveRows[i].Set(rankingTitle[i], name, wave.ToString(), killed.ToString());
            }
            else
            {
                noReviveRows[i].Set(rankingTitle[i], "--", "--", "--");
            }
        }
    }

    private void parseWithReviveParts()
    {
        string wrapped = "{\"entries\":" + withReviveParts + "}";
        var list = JsonUtility.FromJson<FirestoreList>(wrapped);
        for (int i = 0; i < withReviveRows.Length; i++)
        {
            if (i < list.entries.Length)
            {
                var entry = list.entries[i];
                string name = entry.document.fields.name.stringValue;
                int wave = int.Parse(entry.document.fields.wave.integerValue);
                int killed = int.Parse(entry.document.fields.killed.integerValue);
                int revive=int.Parse(entry.document.fields.revive.integerValue);
                withReviveRows[i].Set(rankingTitle[i], name, wave.ToString(), killed.ToString(),revive.ToString());
            }
            else
            {
                withReviveRows[i].Set(rankingTitle[i], "--", "--", "--", "--");
            }
        }
    }

}
