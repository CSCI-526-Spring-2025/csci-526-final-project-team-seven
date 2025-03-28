using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView_Boss_01 : EnemyView
{
    public float entranceDuration = 4f;
    private Vector3 rightStartPosition = new Vector3(20.5f, 2f, 0);
    private Vector3 rightEndPosition = new Vector3(-20.5f, 2f, 0);
    private Vector3 leftStartPosition = new Vector3(-20.5f, -2f, 0);
    private Vector3 leftEndPosition = new Vector3(20.5f, -2f, 0);
    private Vector3 centerPosition = new Vector3(0, 0, 0);

    // The relative position of the camera
    private Vector3 currentPosition = new Vector3(0, 0, 0);

    public Slider healthBar;
    public TextMeshProUGUI healthText;

    public GameObject exclamationPrebab;
    private GameObject currentExclamation;
    public Vector3 exclamationRightPosition = new Vector3(8f, 2f, 0);
    public Vector3 exclamationLeftPosition = new Vector3(-8f, -2f, 0);
    float exclamationFlashTime = 3f;
    float exclamationFlashInterval = 0.3f;

    private float bulletAttackHealthPercentage = 0.5f;

    private bool startAttack = true;

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
        currentPosition = rightStartPosition;
        StartCoroutine(AttackCycle());
    }

    private IEnumerator AttackCycle()
    {
        yield return StartCoroutine(ShowHealthBar());
        startAttack = true;
        yield return StartCoroutine(RectMovementRoutine());
        yield return StartCoroutine(BulletAttackRoutine());
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
        return new Vector3(rightStartPosition.x,cameraPosition.y,0);
    }
    private IEnumerator RectMovementRoutine()
    {
        while (enemyData.Health > enemyData.MaxHealth * bulletAttackHealthPercentage)
        {
            // Boss came from right
            yield return StartCoroutine(ShowExclamationMark(exclamationRightPosition));
            yield return StartCoroutine(MoveFromTo(rightStartPosition, rightEndPosition));
            if(enemyData.Health <= enemyData.MaxHealth * bulletAttackHealthPercentage)
            {
                break;
            }
            // Boss came from left
            yield return StartCoroutine(ShowExclamationMark(exclamationLeftPosition));
            yield return StartCoroutine(MoveFromTo(leftStartPosition, leftEndPosition));
        }
        // Boss from currentPosition then move to center
        yield return StartCoroutine(MoveFromTo(currentPosition,centerPosition,true));
    }

    private IEnumerator ShowExclamationMark(Vector3 spawnPos)
    {
        // Show exclamation mark
        if (exclamationPrebab != null)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            cameraPosition.z = 0;
            currentExclamation = Instantiate(exclamationPrebab, cameraPosition + spawnPos, Quaternion.identity, transform);
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
        yield return null;
    }

    // Move boss from fromPos to toPos
    private IEnumerator MoveFromTo(Vector3 fromPos, Vector3 toPos,bool forceMove=false)
    {
        currentPosition = fromPos;

        while (Vector3.Distance(currentPosition, toPos) > 0.01f)
        {
            currentPosition = Vector3.MoveTowards(currentPosition, toPos, enemyData.Speed * Time.deltaTime);
            yield return null;
            if(!forceMove&&enemyData.Health <= enemyData.MaxHealth * bulletAttackHealthPercentage)
            {
                yield break;
            }
        }
        transform.position = toPos;
        yield return null;
    }

    private IEnumerator BulletAttackRoutine()
    {
        while (true)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float angleDeg = (360f / bulletCount) * i;
                float angleRad = angleDeg * Mathf.Deg2Rad;

                Quaternion bulletRotation = Quaternion.Euler(0, 0, angleDeg - 90f);
                Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);

                var bullet = Instantiate(bulletPrefab, transform).GetComponent<EnemyView_Boss_01_Bullet>();
                bullet.Init("Enemy_Boss_01_Bullet", transform.position, direction, bulletRotation);

                yield return new WaitForSeconds(bulletInterval);
            }
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
        //if (currentExclamation != null)
        //{
        //    Vector3 cameraPosition = Camera.main.transform.position;
        //    cameraPosition.z = 0;
        //    currentExclamation.transform.position = cameraPosition + exclamationPosition;
        //}

        // Update the boss position by relative position
        transform.position = new Vector3(currentPosition.x, Camera.main.transform.position.y + currentPosition.y, 0);
    }
}
