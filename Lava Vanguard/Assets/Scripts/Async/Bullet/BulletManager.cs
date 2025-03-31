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

        public void GenerateBullet(CardView cardView)
        {
            Vector3 spawnPos = PlayerManager.Instance.playerView.transform.position;
            int index = cardView.cardRankData.CardID[^1] - '1';
            //Debug.Log("GenerateBullet - "+index+" mult: "+damageMultiplier);
            //Debug.Log(index);
            var b = Instantiate(bulletPrefabs[index], spawnPos, Quaternion.identity, bulletContainer);
            b.GetComponent<SpriteRenderer>().color = cardView.content.color;
            b.GetComponent<BulletView>().Init(cardView.cardRankData.Level);
        }
    }
}
