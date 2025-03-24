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
    public GameObject exclamationPrebab;
    public Vector3 exclamationPosition = new Vector3(8, 0, 0);

    private bool startAttack = false;

    public override void Init(string ID)
    {
        base.Init(ID);
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
        StartCoroutine(BossEntrance());
    }
    private void Start()
    {
    }
    protected override void Approching()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition.z = 0;

        transform.position = cameraPosition + endPosition;
    }
    protected override Vector3 GetSpawnPosition()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition.z = 0;
        return cameraPosition + startPosition;
    }
    private IEnumerator BossEntrance()
    {
        yield return null;
        Debug.Log("Show Exclamation");
        GameObject exclamation = null;
        if (exclamationPrebab != null)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            cameraPosition.z = 0;
            exclamation = Instantiate(exclamationPrebab, cameraPosition + exclamationPosition, Quaternion.identity,transform);
        }
        yield return new WaitForSeconds(10f);
        if (exclamation != null)
        {
            Destroy(exclamation);
        }
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
