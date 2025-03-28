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
    public TextMeshProUGUI healthText;

    public GameObject exclamationPrebab;
    private GameObject currentExclamation;
    public Vector3 exclamationPosition = new Vector3(8, 0, 0);
    float exclamationFlashTime = 3f;
    float exclamationFlashInterval = 0.3f;
    
    private bool startAttack = false;

    [Header("Boss Attack Settings")]
    public GameObject bulletPrefab;
    private float bulletInterval = 0.1f;
    private int bulletCount = 36;
    public override void Init(string ID)
    {
        base.Init(ID);
        if (healthBar != null)
        {
            healthBar.maxValue = enemyData.MaxHealth;
            healthBar.value = enemyData.Health;
            healthBar.gameObject.SetActive(false);
        }
        if (healthText != null)
        {
            healthText.text = "Boss";
            healthText.gameObject.SetActive(false);
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

        StartCoroutine(ShowHealthBar());

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

        StartCoroutine(AttackCycle());
    }

    private IEnumerator AttackCycle()
    {
        while (true)
        {
            Debug.Log("StartCycle");
            //Start bullet attack
            yield return StartCoroutine(BulletAttackRoutine());

            //yield return StartCoroutine(RectMovementRoutine(moveDuration));
        }
    }

    private IEnumerator BulletAttackRoutine()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            float angleDeg = (360f / bulletCount) * i;
            float angleRad = angleDeg * Mathf.Deg2Rad;

            Quaternion bulletRotation = Quaternion.Euler(0, 0, angleDeg - 90f);
            Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);

            var bullet = Instantiate(bulletPrefab, transform).GetComponent<EnemyView_Boss_01_Bullet>();
            bullet.Init("Enemy_Boss_01_Bullet", transform.position,direction,bulletRotation);

            yield return new WaitForSeconds(bulletInterval);
        }
    }

    private IEnumerator ShowHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
        }
        if (healthText != null)
        {
            healthText.gameObject.SetActive(true);
        }

        float elapsed = 0f;

        healthBar.value = 0;

        while (elapsed < exclamationFlashTime)
        {
            float t = elapsed / exclamationFlashTime;
            healthBar.value =t* healthBar.maxValue;
            elapsed += Time.deltaTime;
            yield return null;
        }
        healthBar.value = healthBar.maxValue;
    }

    public override bool TakeHit(int bulletAttack)
    {
        if(!startAttack) { return false; }
        enemyData.Health -= bulletAttack;
        Debug.Log("Been hit");
        healthBar.value = enemyData.Health;
        if (enemyData.Health <= 0)
        {
            enemyData.Health = 0;
            StartCoroutine(DeathEffect());
            PlayerManager.Instance.playerView.playerData.coin += enemyData.Coin;
            UIGameManager.Instance.UpdateCoin();
            Destroy(gameObject);
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(false);
            }
            if (healthText != null)
            {
                healthText.gameObject.SetActive(false);
            }
            return true;
        }
        else
        {
            HitEffect();
            return false;
        }
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
