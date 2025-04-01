using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public int wave = -1;
    public int time;
    public static readonly int[] TotalTime = new int[] { 20, 20, 25, 30, 45, 60 };
    public TMP_Text timeText;
    public TMP_Text waveText;

    [HideInInspector] public bool waveEnded = false;
    private void Awake()
    {
        Instance = this;
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
        EnemyManager.Instance.StopSpawn();
        EnemyManager.Instance.killAll();
        waveEnded = true;
        CameraController.Instance.StopCamera();
        //UIGameManager.Instance.Open<CardSelectorPanel>();
    }

    public bool WaveHasBoss()
    {
        return wave >= 1;
    }
    public void NextWave()
    {
        wave++;
        waveText.text = "Wave " + (wave + 1);
        PlayerManager.Instance.canControl = true;
        ResetTimer();

        EnemyManager.Instance.StartSpawn();
        StartCoroutine("CountdownTimer");
    }
    private void Update()
    {
        if (waveEnded && PlayerManager.Instance.playerView.isGround) 
        {
            waveEnded = false;
            PlayerManager.Instance.canControl = false;
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1f);
            sequence.AppendCallback(() =>
            UIGameManager.Instance.Open<CardSelectorPanel>());
        }
    }
}
