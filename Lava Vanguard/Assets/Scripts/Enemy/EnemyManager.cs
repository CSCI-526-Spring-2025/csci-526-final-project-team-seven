using DG.Tweening;
using UnityEngine;
using Math = System.Math;
using Random = UnityEngine.Random;
using System.Collections;
using Async;

public class EnemyManager : MonoBehaviour
{
    
    public static EnemyManager Instance { get; private set; }
    public Transform enemyContainer;
    //[HideInInspector]
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public GameObject bossRef;

    [Header("Enemy Settings")]
    private float minSpawnInterval = 0.18f;
    private float maxSpawnInterval = 2f;
    private float maxLevel = 20f;
    private Coroutine spawnCoroutine;
    public bool bossSpawned = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        
    }

    public EnemyView GenerateRandomEnemy(int level)
    {
        int suffix = Random.Range(0, enemyPrefabs.Length);
        var enemyView = Instantiate(enemyPrefabs[suffix],enemyContainer).GetComponent<EnemyView>();
        enemyView.Init("Enemy_0" + (suffix + 1),level);
        return enemyView;
    }

    public EnemyView GenerateSpecificEnemy(int suffix,int level)
    {
        var enemyView = Instantiate(enemyPrefabs[suffix], enemyContainer).GetComponent<EnemyView>();
        enemyView.Init("Enemy_0" + (suffix + 1),level);
        return enemyView;
    }
    public EnemyView GenerateSpecificEnemy(int suffix, Vector3 position)
    {
        var enemyView = Instantiate(enemyPrefabs[suffix], enemyContainer).GetComponent<EnemyView>();
        enemyView.Init("Enemy_0" + (suffix + 1), position);
        return enemyView;
    }
    private void Start()
    {
        if (!Tutorial.Instance.tutorial)
            StartSpawn();
    }

    public void StartSpawn()
    {   
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(1f);

        if (!bossSpawned && LevelManager.Instance.WaveHasBoss())
        {
            bossSpawned = true;
            SpawnBoss();
        }

        while (true)
        {
            GenerateRandomEnemy(Mathf.Min(LevelManager.Instance.wave/2+1,9));
            float waitTime = CalculateSpawnInterval();
            yield return new WaitForSeconds(waitTime);
        }
    }

    private float CalculateSpawnInterval()
    {
        int currentlevel = LevelManager.Instance.wave;
        float safeCardCount = Mathf.Clamp(currentlevel, 1f, maxLevel);
        float minSpawnCount = 1f / maxSpawnInterval;
        float maxSpawnCount = 1f / minSpawnInterval;

        float calculatedInterval = minSpawnCount + (maxSpawnCount - minSpawnCount) * (safeCardCount - 1f) / (maxLevel - 1f);
        return 1f/calculatedInterval;
    }

    private void SpawnBoss()
    {
        var boss = Instantiate(bossPrefab, enemyContainer);
        var bossView = boss.GetComponent<EnemyView>();
        bossView.Init("Enemy_Boss_01",1);
        bossRef = boss;
    }

    public void StopSpawn()
    {
        if (spawnCoroutine != null)
        {
            //Debug.Log("stop");
            StopCoroutine(spawnCoroutine);
        }
    }

    public void killAll()
    {
        foreach (Transform enemy in enemyContainer)
        {
            enemy.GetComponent<EnemyView>().ForceKill();
        }
    }
}
