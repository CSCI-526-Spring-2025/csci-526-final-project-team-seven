using System.Collections;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public int wave = -1;
    public int time;
    public static readonly int[] TotalTime = new int[] { 15, 15, 25, 30, 45, 60 };
    public TMP_Text timeText;
    public TMP_Text waveText;
    public bool BreakTime = false;
    private Coroutine countdownCoroutine;
    private void Awake()
    {
        Instance = this;
    }
    public void NextWave()
    {
        var p = LevelGenerator.Instance.WavePlatform;
        if (LevelGenerator.Instance.WavePlatform)
        {
            LevelGenerator.Instance.WavePlatform.name = "";
            LevelGenerator.Instance.WavePlatform = null;
        }
        BreakTime = false;
        // wave++;
        // UpdateWave();
        ResetTimer();
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(CountdownTimer());
        EnemyManager.Instance.StartSpawn();
    }

    private void UpdateWave()
    {
        waveText.text = "Wave " + (wave + 1);
    }

    private void ResetTimer()
    {
        time = TotalTime[Mathf.Min(wave, TotalTime.Length - 1)];
    }

    private IEnumerator CountdownTimer()
    {
        bool flag = false;
        while (time > 0)
        {
            timeText.text = "Time: " + time;
            yield return new WaitForSeconds(1f);
            time--;
            if (time < 7 && !flag) // .... to make sure the long platform appear when time is up
            {
                LevelGenerator.Instance.GenWavePlatform = true;
                flag = true;
            }
        }
        timeText.text = "Time's Up!";
        UIGameManager.Instance.Open<CardSelectorPanel>();
    }

    public void BeforeNextWave()
    {
        // kill all enemies, then fast forward
        Debug.Log("before next wave");
        EnemyManager.Instance.stopSpawn();
        EnemyManager.Instance.killAll();
        wave++;
        UpdateWave();
    }
}
