using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Async
{
    public class BulletManager : MonoBehaviour
    {
        public static BulletManager Instance {  get; private set; }
        public Transform bulletContainer;
        public GameObject[] bulletPrefabs;
        private void Awake()
        {
            Instance = this;
        }

        // Generate bullet by sequence manager
        public void GenerateBullet(CardRankData cardRankData)
        {
            switch (cardRankData.CardID)
            {
                case "Card_Bullet01":
                    GenerateBullet01();
                    break;
                case "Card_Bullet02":
                    GenerateBullet02();
                    break;
                case "Card_Bullet03":
                    GenerateBullet03();
                    break;
                default:
                    Debug.LogWarning("Unrecognized type: " + cardRankData.CardID);
                    break;
            }
        }
        // Generate Bullet_01
        private void GenerateBullet01()
        {
            Vector3 spawnPos = PlayerManager.Instance.playerView.transform.position;
            Instantiate(bulletPrefabs[0], spawnPos, Quaternion.identity, bulletContainer);
        }
        // Generate Bullet_02
        private void GenerateBullet02()
        {
            Vector3 spawnPos = PlayerManager.Instance.playerView.transform.position;
            Instantiate(bulletPrefabs[1], spawnPos, Quaternion.identity, bulletContainer);
        }
        // Generate Bullet_03
        private void GenerateBullet03()
        {
            Vector3 spawnPos = PlayerManager.Instance.playerView.transform.position;
            Instantiate(bulletPrefabs[2], spawnPos, Quaternion.identity, bulletContainer);
        }
    
    }
}
