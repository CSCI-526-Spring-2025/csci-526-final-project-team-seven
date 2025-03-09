using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class EnemyView : MonoBehaviour
{
    public EnemyData enemyData;
    public void Init(string ID)
    {
        enemyData = GameDataManager.EnemyData[ID];
        transform.position = GetSpawnPosition();
    }
    protected abstract void Approching();
    protected abstract Vector3 GetSpawnPosition();
    
    // 🚀 抽象方法，让子类决定如何受击
    //protected abstract void TakeHit();

    // 🚀 抽象方法，让子类决定如何响应碰撞
    //protected abstract void OnCollisionEnter2D(Collision2D collision);
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.GetHurt(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) // 碰到子弹
        {
            Debug.Log($"{gameObject.name} 被子弹击中！");
            Destroy(gameObject); // 立即销毁自己
        }
    }
    private void Update()
    {
        Approching();
    }
}


