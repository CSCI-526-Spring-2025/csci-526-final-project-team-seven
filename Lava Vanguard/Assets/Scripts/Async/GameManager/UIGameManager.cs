using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIGameManager : MonoBehaviour
{
    public static UIGameManager Instance { get; private set; }

    //Panel buttons;
    public Button pauseButton;
    public Button weaponButton;


    //HP and Coin
    public TMP_Text coinText;
    public Image hpBarFill;
    public TMP_Text hpLabel;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        foreach (var p in UIPanels)
            p.Init();
        pauseButton.onClick.AddListener(() => Open<PausePanel>());
        weaponButton.onClick.AddListener(() => Open<WeaponPanel>());
    }
    public UIPanel[] UIPanels;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !(GetOpen<PausePanel>() || GetOpen<DeathPanel>())) 
        {
            Switch<WeaponPanel>();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Open<PausePanel>();
        }
        bool open = false;
        foreach(var p in UIPanels)
        {
            if (p.isOpen)
            {
                Time.timeScale = 0;
                open = true;
            }
        }
        if (!open)
        {
            Time.timeScale = 1;
        }
    }
    public void Open<T>() where T : UIPanel
    {
        foreach (var p in UIPanels)
        {
            if (p is T && p.canOpen) p.Open();
        }
    }
    public void Close<T>() where T : UIPanel
    {
        foreach (var p in UIPanels)
        {
            if (p is T && p.canClose) p.Close();
        }
    }
    public bool GetOpen<T>() where T : UIPanel
    {
        foreach (var p in UIPanels)
        {
            if (p is T) return p.isOpen;
        }
        return false;
    }
    public void SetCanOpen<T>(bool canOpen) where T : UIPanel
    {
        foreach (var p in UIPanels)
        {
            if (p is T) p.canOpen = canOpen;
        }
    }
    public void SetCanClose<T>(bool canClose) where T : UIPanel
    {
        foreach (var p in UIPanels)
        {
            if (p is T) p.canClose = canClose;
        }
    }
    public void Switch<T>() where T : UIPanel
    {
        foreach (var p in UIPanels)
        {
            if (p is T) p.Switch();
        }
    }
    public T GetPanel<T>() where T : UIPanel
    {
        foreach (var p in UIPanels)
        {
            if (p is T) return p as T; 
        }
        return null; 
    }

    public void UpdateHp()
    {
        int hp = PlayerManager.Instance.playerView.playerData.health;
        int maxHP = PlayerManager.Instance.playerView.playerData.currentHealthLimit;
        float percentage = 1.0f * hp / maxHP;
        hpLabel.text = "HP: " + hp + "/" + maxHP;
        hpBarFill.fillAmount = percentage;
    }
    public void UpdateCoin()
    {
        int coin = PlayerManager.Instance.playerView.playerData.coin;
        coinText.text = "Coin: " + coin;
    }
    
}