using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : UIPanel
{
    public Button buySlotButton;
    public override void Init()
    {
        base.Init();
        canOpen = false;
        buySlotButton.onClick.AddListener(BuySlot);
    }
    public override void Close()
    {
        base.Close();
        Tooltip.Instance.HideTooltip();
    }
    public void BuySlot()
    {
        int currentTotalGird = SlotManager.Instance.currentTotalGrid;
        int price = 40 + (currentTotalGird - 2) * 20;
        if (PlayerManager.Instance.playerView.GetCoin() >= price)
        {
            PlayerManager.Instance.playerView.GainCoin(-price);
            UIGameManager.Instance.UpdateCoin();
            SlotManager.Instance.AddSlot();
        }
        if (SlotManager.Instance.currentTotalGrid == SlotManager.TOTAL_GRID)
            buySlotButton.gameObject.SetActive(false);
    }
}
