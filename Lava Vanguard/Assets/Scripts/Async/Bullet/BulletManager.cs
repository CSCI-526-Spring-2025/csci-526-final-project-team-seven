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

        public void GenerateBullet(CardRankData cardRankData, float damageMultiplier)
        {
            Vector3 spawnPos = PlayerManager.Instance.playerView.transform.position;
            int index = cardRankData.CardID[^1] - '1';
            //Debug.Log("GenerateBullet - "+index+" mult: "+damageMultiplier);
            //Debug.Log(index);
            var b = Instantiate(bulletPrefabs[index], spawnPos, Quaternion.identity, bulletContainer).GetComponent<BulletView>();
            b.damageMultiplier = damageMultiplier;
        }
    }
}
