using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView_02 : EnemyView
{
    private bool movingRight = true; 
    private float leftLimit;
    private float rightLimit;
    private Camera mainCamera;
    public float destroyY=-10f;

    private void Start()
    {
        leftLimit = transform.position.x - 1.2f;
        rightLimit = transform.position.x + 1.2f;
        mainCamera = Camera.main;
    }
    protected override void Approching()
    {

        transform.Translate(Vector2.right * enemyData.Speed * Time.deltaTime * (movingRight ? 1 : -1));

        if (movingRight && transform.position.x >= rightLimit)
        {
            Flip();
        }
        else if (!movingRight && transform.position.x <= leftLimit)
        {
            Flip();
        }
        if (mainCamera != null && transform.position.y < mainCamera.transform.position.y +destroyY){
            Destroy(gameObject);
        }
    }
    // protected override Vector3 GetSpawnPosition()
    // {
    //     var playerPos = PlayerManager.Instance.playerView.transform.position;
    //     Vector3 spawnPosition;
    //     do
    //     {
    //         var g = PlatformGenerator.Instance.platforms[Random.Range(0, PlatformGenerator.Instance.platforms.Count)];

    //         // Collect all non-null platforms in the selected layer
    //         List<PlatformView> validPlatforms = new List<PlatformView>();
    //         foreach (var platform in g)
    //         {
    //             if (platform != null)
    //             {
    //                 validPlatforms.Add(platform);
    //             }
    //         }

    //         PlatformView chosenPlatform = validPlatforms[Random.Range(0, validPlatforms.Count)];
    //         spawnPosition = chosenPlatform.transform.position + new Vector3(0, 0.75f, 0);


    //     } while (Vector3.Distance(playerPos, spawnPosition) < SpawnDistance);
    //     return spawnPosition;
    // }

    protected override Vector3 GetSpawnPosition()
    {
        var playerPos = PlayerManager.Instance.playerView.transform.position;
        Vector3 spawnPosition = Vector3.zero;
        PlatformView chosenPlatform = null;

        do
        {
            // 随机选一组platform
            var g = PlatformGenerator.Instance.platforms[Random.Range(0, PlatformGenerator.Instance.platforms.Count)];
            
            // 把不为空的platform挑出来
            List<PlatformView> validPlatforms = new List<PlatformView>();
            foreach (var platform in g)
            {
                if (platform != null)
                {
                    validPlatforms.Add(platform);
                }
            }

            if (validPlatforms.Count == 0)
                continue;

            // 随机选一个platform
            chosenPlatform = validPlatforms[Random.Range(0, validPlatforms.Count)];

            // 🔥 检查这个平台上已经有多少个敌人了
            int enemyCountOnPlatform = 0;
            foreach (var enemy in EnemyManager.Instance.enemyViews)
            {
                if (enemy != null && enemy.currentPlatform == chosenPlatform)
                {
                    enemyCountOnPlatform++;
                }
            }

            // 如果超过了2个，就换平台
            if (enemyCountOnPlatform >= 2)
            {
                continue;
            }

            // 找到了合适的平台！
            spawnPosition = chosenPlatform.transform.position + new Vector3(0, 0.75f, 0);

        } while (Vector3.Distance(playerPos, spawnPosition) < SpawnDistance);

        // 👇 记得把出生的平台记录下来
        this.currentPlatform = chosenPlatform;

        return spawnPosition;
    }

    void Flip()
    {
        movingRight = !movingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}

