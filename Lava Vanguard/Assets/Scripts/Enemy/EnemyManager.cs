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

    [Header("Enemy Settings")]
    public float baseSpawnInterval = 3.0f;
    public float minSpawnInterval = 0.3f;
    public float maxSpawnInterval = 2f;
    public float difficultyCurve = 0.3f;
    private Coroutine spawnCoroutine;
    private void Awake()
    {
        Instance = this;
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
        StartSpawn();
        
        // InvokeRepeating(nameof(GenerateRandomEnemy), 1f, repeatRate);
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
        int cardCount = SequenceManager.Instance.GetCardNumber();
        int safeCardCount = Mathf.Max(cardCount, 1);
        
        float calculatedInterval = baseSpawnInterval / (Mathf.Log10(safeCardCount) * difficultyCurve);
    
        return Mathf.Clamp(calculatedInterval, minSpawnInterval, maxSpawnInterval);
    }
}
