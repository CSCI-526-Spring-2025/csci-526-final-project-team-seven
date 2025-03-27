using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView_Boss_01 : EnemyView
{
    public float entranceDuration = 2f;
    public Vector3 startPosition = new Vector3(12, 0, 0);
    public Vector3 endPosition = new Vector3(8, 0, 0);

    public Slider healthBar;
    public GameObject highlightPrebab;

    private bool startAttack = false;

    public override void Init(string ID)
    {
        base.Init(ID);
        if (healthBar != null)
        {
            healthBar.maxValue = enemyData.MaxHealth;
            healthBar.value = enemyData.Health;
        }
        StartCoroutine(BossEntrance());
    }
    private void Start()
    {
    }
    protected override void Approching()
    {
        transform.position = Camera.main.transform.position + endPosition;
    }
    protected override Vector3 GetSpawnPosition()
    {
        Camera camera = Camera.main;
        return camera.transform.position+startPosition;
    }
    private IEnumerator BossEntrance()
    {
        yield return null;
        // 1. 显示红色感叹号提示
        //GameObject exclamation = null;
        //if (highlightPrebab != null)
        //{
        //    exclamation = Instantiate(exclamationPrefab, transform.position, Quaternion.identity, transform);
        //}
        //yield return new WaitForSeconds(1f);
        //if (exclamation != null)
        //{
        //    Destroy(exclamation);
        //}
        //// 2. 进场动画：Boss从当前位置移动到目标位置
        //Vector3 startPos = transform.position;
        //float elapsed = 0f;
        //while (elapsed < entranceDuration)
        //{
        //    transform.position = Vector3.Lerp(startPos, targetPosition, elapsed / entranceDuration);
        //    elapsed += Time.deltaTime;
        //    yield return null;
        //}
        //transform.position = targetPosition;
        //// 3. 进场完成后开始攻击
        //isAttacking = true;
        //StartCoroutine(BulletAttack());
        //StartCoroutine(MinionSummon());
    }

}
