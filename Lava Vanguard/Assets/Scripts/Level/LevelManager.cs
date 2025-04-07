using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public int wave = -1;
    public int time;
    public static readonly int[] TotalTime = new int[] { 15, 20, 25, 30 };
    public TMP_Text timeText;
    public TMP_Text waveText;
    public string healthForWave;

    [HideInInspector] public bool waveEnded = false;
    [HideInInspector] public bool genLongPlatform = false;
    [HideInInspector] public bool enteredNext = false;
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
        if (WaveHasBoss() && EnemyManager.Instance.bossRef.gameObject != null)
        {
            CameraController.Instance.StopCamera();
            yield return new WaitForSeconds(1f);
            timeText.text = "Kill the Boss!";
            yield return new WaitUntil(() => { return EnemyManager.Instance.bossRef.gameObject == null; });
            timeText.text = "Boss Killed!";
        }
        EnemyManager.Instance.StopSpawn();
        EnemyManager.Instance.killAll();
        waveEnded = true;
        genLongPlatform = true;
        CameraController.Instance.SetCameraSpeed(1.5f);
        yield return new WaitUntil(() => showPanel());
        // yield return new WaitForSeconds(0.2f);

        // CameraController.Instance.StopCamera();
        UIGameManager.Instance.Open<CardSelectorPanel>();
    }

    public bool WaveHasBoss()
    {
        return wave>=3&&(wave+1)%2==0;
    }
    public void NextWave()
    {
        enteredNext = false;
        waveEnded = false;
        wave++;
        waveText.text = "Wave " + (wave + 1);
        ResetTimer();
        recordHealthForThisWave();
        EnemyManager.Instance.StartSpawn();
        StartCoroutine("CountdownTimer");
    }
    private void Update()
    {
        // if (waveEnded && PlayerManager.Instance.playerView.isGround) 
        // {
        //     waveEnded = false;
        //     Sequence sequence = DOTween.Sequence();
        //     sequence.AppendInterval(1f);
        //     sequence.AppendCallback(() =>
        //     UIGameManager.Instance.Open<CardSelectorPanel>());
        // }
    }

    public void recordHealthForThisWave()
    {
       
        int health = PlayerManager.Instance.playerView.GetHP();

        healthForWave += $"Wave {wave+1}: HP {health}\n"; 
    }

    private bool showPanel()
    {
        return waveEnded && CameraController.Instance.CameraStopped() && enteredNext;
    }
}
