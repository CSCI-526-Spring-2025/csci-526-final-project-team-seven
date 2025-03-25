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
    private Coroutine countdownCoroutine;
    private void Awake()
    {
        Instance = this;
    }
    public void NextWave()
    {
        wave++;
        UpdateWave();
        ResetTimer();
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(CountdownTimer());
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
        while (time > 0)
        {
            timeText.text = "Time: " + time;
            yield return new WaitForSeconds(1f);
            time--;
        }
        timeText.text = "Time's Up!";
        UIGameManager.Instance.Show<CardSelectorPanel>();
    }
}
