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

    public void LateUpdate()
    {
        var enemyManager = EnemyManager.Instance;
        var cam = CameraController.Instance;
        var p = LevelGenerator.Instance.WavePlatform;
        var camPosY = cam.virtualCamera.transform.position.y;
        if (WaveHasBoss() && p && p.transform.position.y < camPosY + 5f && p.transform.position.y > camPosY)
        {
            if (enemyManager.bossRef != null && !cam.CameraStopped())
            {
                cam.StopCamera();
                p.GetComponent<Collider2D>().enabled = false;
            }
            else if (enemyManager.bossSpawned && enemyManager.bossRef == null && cam.CameraStopped())
            {
                cam.ResumeCamera();
                p.GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    public void NextWave()
    {
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
        yield return new WaitUntil(() => ShouldShowPanel());
        UIGameManager.Instance.Open<CardSelectorPanel>();
    }

    public void BeforeNextWave()
    {
        // kill all enemies, then fast forward
        //Debug.Log("before next wave");
        EnemyManager.Instance.stopSpawn();
        EnemyManager.Instance.killAll();
        wave++;
        UpdateWave();
    }

    public bool WaveHasBoss()
    {
        return wave >= 5;
    }

    private bool ShouldShowPanel()
    {
        var cam = CameraController.Instance;
        var p = LevelGenerator.Instance.WavePlatform;
        return p.transform.position.y < cam.virtualCamera.transform.position.y && cam.CameraStopped();
    }
}
