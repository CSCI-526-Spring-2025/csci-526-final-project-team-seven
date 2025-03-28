using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView_Boss_01 : EnemyView
{
    public float entranceDuration = 4f;
    private Vector3 startPosition = new Vector3(20.5f, 0, 0);
    private Vector3 endPosition = new Vector3(0, 0, 0);

    public Slider healthBar;
    public GameObject exclamationPrebab;
    private GameObject currentExclamation;
    public Vector3 exclamationPosition = new Vector3(8, 0, 0);
    float exclamationFlashTime = 3f;
    float exclamationFlashInterval = 0.3f;
    
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
    }
    protected override Vector3 GetSpawnPosition()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        return new Vector3(startPosition.x,cameraPosition.y,0);
    }
    private IEnumerator BossEntrance()
    {
        yield return null;

        // Show exclamation mark
        if (exclamationPrebab != null)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            cameraPosition.z = 0;
            currentExclamation = Instantiate(exclamationPrebab, cameraPosition + exclamationPosition, Quaternion.identity,transform);
        }

        SpriteRenderer[] exclamationRenderers = currentExclamation.GetComponentsInChildren<SpriteRenderer>();

        // Flash excalamation mark
        float timeElapsed = 0f;
        while (timeElapsed < exclamationFlashTime)
        {
            foreach (var renderer in exclamationRenderers)
                renderer.enabled = !renderer.enabled;

            yield return new WaitForSeconds(exclamationFlashInterval);
            timeElapsed += exclamationFlashInterval;
        }
        if (currentExclamation != null)
        {
            Destroy(currentExclamation);
        }

        // Boss entrance
        timeElapsed = 0f;
        while (timeElapsed < entranceDuration)
        {
           transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / entranceDuration);
           transform.position=new Vector3(transform.position.x,Camera.main.transform.position.y,0);
           timeElapsed += Time.deltaTime;
           yield return null;
        }

        startAttack = true;
        //StartCoroutine(BulletAttack());
        //StartCoroutine(MinionSummon());
    }

    private void LateUpdate()
    {
        if (currentExclamation != null)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            cameraPosition.z = 0;
            currentExclamation.transform.position = cameraPosition + exclamationPosition;
        }
        if (startAttack)
        {
            transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y, 0) ;
        }
    }
}
