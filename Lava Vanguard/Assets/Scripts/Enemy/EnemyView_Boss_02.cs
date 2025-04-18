using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView_Boss_02 : EnemyView
{
    // The relative position of the camera
    private Vector3 currentPosition = new Vector3(0, 0, 0);

    private float healthBarAppearTime = 3f;

    [Header("Exclamation Bar")]
    public GameObject exclamationPrefab;
    private GameObject currentExclamation;
    private Vector3 exclamationCurrentPosition;
    private Vector3 exclamationEntrancePosition = new Vector3(0, -4f, 0);
    float exclamationFlashTime = 3f;
    float exclamationFlashInterval = 0.3f;

    private float bulletAttackHealthPercentage = 0.5f;
    
    private bool startAttack = false;

    [Header("Boss Phase 1: Laser Attack")]
    private Vector3 entranceStartPosition = new Vector3(0, -10, 0);
    private Vector3 centerPosition = new Vector3(0, 0, 0);
    public Transform leftArmPivot;
    public Transform rightArmPivot;
    public GameObject laserPrefab;
    public GameObject windUpEffect;
    public float armRotationSpeed = 10f;
    public float armCooldown = 2f;

    [Header("Boss Phase 2")]
    public GameObject flyingGuard;

    public override void Init(string ID,int level)
    {
        this.level = level;
        originalColor = ColorCenter.CardColors["EnemyBoss02"];

        spriteRenderer.color = originalColor;
        enemyData = GameDataManager.EnemyData[ID];
        enemyData.Health = Mathf.RoundToInt(150 * Mathf.Pow(4500f / 150f, (level - 1f) / 8f));
        enemyData.MaxHealth = enemyData.Health;
        transform.position = GetSpawnPosition();

        InitHealthBar();
        
        StartCoroutine(AttackCycle());
    }

    private void InitHealthBar()
    {
        if (UIGameManager.Instance.bossHPBar != null)
        {
            UIGameManager.Instance.bossHPBar.maxValue = enemyData.MaxHealth;
            UIGameManager.Instance.bossHPBar.value = enemyData.Health;
            UIGameManager.Instance.bossHPBar.gameObject.SetActive(false);
        }
        if (UIGameManager.Instance.BossHPLabel != null)
        {
            UIGameManager.Instance.BossHPLabel.text = "Boss "+level;
            UIGameManager.Instance.BossHPLabel.gameObject.SetActive(false);
        }
    }

    private IEnumerator AttackCycle()
    {
        startAttack = false;
        SetTagRecursively(gameObject, "Untagged");
        yield return StartCoroutine(ShowHealthBar());
        yield return StartCoroutine(ShowExclamationMark(exclamationEntrancePosition));
        SetTagRecursively(gameObject, "Enemy");
        startAttack = true;
        yield return StartCoroutine(MoveFromTo(entranceStartPosition,centerPosition));
        yield return StartCoroutine(LaserAttackRoutine());
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
        currentPosition = entranceStartPosition;
        return new Vector3(entranceStartPosition.x, cameraPosition.y, 0);
    }

    private IEnumerator ShowExclamationMark(Vector3 spawnPos)
    {
        startAttack=false;

        // Show exclamation mark
        if (exclamationPrefab != null)
        {
            exclamationCurrentPosition = spawnPos;
            currentExclamation = Instantiate(exclamationPrefab);
            //currentExclamation.transform.SetParent(transform,true);
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
        startAttack=true;

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

    private IEnumerator LaserAttackRoutine()
    {
        Transform pivot;
        float timer = 0f;
        while (enemyData.Health > enemyData.MaxHealth * bulletAttackHealthPercentage)
        {
            // Move left arm
            timer = armCooldown;
            while (timer>0f)
            {
                pivot = leftArmPivot;
                Vector3 toPlayer = PlayerManager.Instance.playerView.transform.position - pivot.position;
                float currentAngle = pivot.eulerAngles.z;
                float targetAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg + 90f;
                float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, armRotationSpeed * Time.deltaTime);
                pivot.rotation = Quaternion.Euler(0, 0, newAngle);
                yield return null;
                timer -= Time.deltaTime;
            }
            if(enemyData.Health <= enemyData.MaxHealth * bulletAttackHealthPercentage) yield break;
            // Move right arm
            timer = armCooldown;
            while (timer > 0f)
            {
                pivot = rightArmPivot;
                Vector3 toPlayer = PlayerManager.Instance.playerView.transform.position - pivot.position;
                float currentAngle = pivot.eulerAngles.z;
                float targetAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg + 90f;
                float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, armRotationSpeed * Time.deltaTime);
                pivot.rotation = Quaternion.Euler(0, 0, newAngle);
                yield return null;
                timer -= Time.deltaTime;
            }
        }
    }

    private IEnumerator ShowHealthBar()
    {
        if (UIGameManager.Instance.bossHPBar != null)
        {
            UIGameManager.Instance.bossHPBar.gameObject.SetActive(true);
        }
        if (UIGameManager.Instance.BossHPLabel != null)
        {
            UIGameManager.Instance.BossHPLabel.gameObject.SetActive(true);
        }

        float elapsed = 0f;

        UIGameManager.Instance.bossHPBar.value = 0;

        while (elapsed < healthBarAppearTime)
        {
            float t = elapsed / healthBarAppearTime;
            UIGameManager.Instance.bossHPBar.value =t* UIGameManager.Instance.bossHPBar.maxValue;
            elapsed += Time.deltaTime;
            yield return null;
        }
        UIGameManager.Instance.bossHPBar.value = UIGameManager.Instance.bossHPBar.maxValue;
    }

    public override bool TakeHit(int bulletAttack)
    {
        if(!startAttack) { return false; }
        enemyData.Health -= bulletAttack;
        UIGameManager.Instance.bossHPBar.value = enemyData.Health;
        if (enemyData.Health <= 0)
        {
            enemyData.Health = 0;
            StartCoroutine(DeathEffect());
            PlayerManager.Instance.playerView.GainCoin(enemyData.Coin);
            UIGameManager.Instance.UpdateCoin();
            if (currentExclamation != null)
            {
                Destroy(currentExclamation);
            }
            Destroy(gameObject);
            if (UIGameManager.Instance.bossHPBar != null)
            {
                UIGameManager.Instance.bossHPBar.gameObject.SetActive(false);
            }
            if (UIGameManager.Instance.BossHPLabel != null)
            {
                UIGameManager.Instance.BossHPLabel.gameObject.SetActive(false);
            }
            return true;
        }
        else
        {
            HitEffect();
            return false;
        }
    }

    private void OnEnable()
    {
        CameraController.OnCameraUpdated += UpdateBossPosition;
    }

    private void OnDisable()
    {
        CameraController.OnCameraUpdated -= UpdateBossPosition;
    }

    private void UpdateBossPosition()
    {
        // Update exclamation bar location
        if (currentExclamation != null)
        {
            Vector3 tmpPosition = CameraController.Instance.virtualCamera.transform.position + exclamationCurrentPosition;
            tmpPosition.z = 0;
            currentExclamation.transform.position = tmpPosition;
        }
        
        // Update the boss position by relative position
        transform.position = new Vector3(currentPosition.x, CameraController.Instance.virtualCamera.transform.position.y + currentPosition.y, 0);
    }

    void SetTagRecursively(GameObject obj, string tagName)
    {
        obj.tag = tagName;
        foreach (Transform child in obj.transform)
        {
            SetTagRecursively(child.gameObject, tagName);
        }
    }
}
