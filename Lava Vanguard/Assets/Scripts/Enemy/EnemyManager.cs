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

    [Header("Enemy Settings")]
    private float minSpawnInterval = 0.18f;
    private float maxSpawnInterval = 2f;
    private float maxLevel = 20f;
    private Coroutine spawnCoroutine;
    private bool bossSpawned = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!bossSpawned && PlayerManager.Instance.playerView.playerData.currentLevel >= 2)
        {
            bossSpawned = true;
            spawnBoss();
        }
    }

    public EnemyView GenerateRandomEnemy()
    {
        int suffix = Random.Range(0, enemyPrefabs.Length);
        var enemyView = Instantiate(enemyPrefabs[suffix],enemyContainer).GetComponent<EnemyView>();
        enemyView.Init("Enemy_0" + (suffix + 1));
        return enemyView;
    }

    public EnemyView GenerateSpecificEnemy(int suffix)
    {
        var enemyView = Instantiate(enemyPrefabs[suffix], enemyContainer).GetComponent<EnemyView>();
        enemyView.Init("Enemy_0" + (suffix + 1));
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

        while (true)
        {
            GenerateRandomEnemy();
            float waitTime = CalculateSpawnInterval();
            yield return new WaitForSeconds(waitTime);
        }
    }

    private float CalculateSpawnInterval()
    {
        int currentlevel = PlayerManager.Instance.playerView.playerData.currentLevel;
        float safeCardCount = Mathf.Clamp(currentlevel, 1f, maxLevel);
        float minSpawnCount = 1f / maxSpawnInterval;
        float maxSpawnCount = 1f / minSpawnInterval;

        float calculatedInterval = minSpawnCount + (maxSpawnCount - minSpawnCount) * (safeCardCount - 1f) / (maxLevel - 1f);
        //Debug.Log("currentlevel " + currentlevel + " --- "+calculatedInterval+" -- "+minSpawnCount+" -- "+maxSpawnCount);
        return 1f/calculatedInterval;
    }

    private void spawnBoss()
    {
        var bossView= Instantiate(bossPrefab, enemyContainer).GetComponent<EnemyView>();
        bossView.Init("Enemy_Boss_01");
    }
}
