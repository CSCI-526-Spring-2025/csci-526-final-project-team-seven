using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : UIPanel
{
    private int initialPrice = 5;
    private int addPrice = 5;
    public int BuySlotPrice { get => initialPrice + (SlotManager.Instance.currentTotalGrid - 2) * addPrice; }
    
    public override void Init()
    {
        base.Init();
        canOpen = false;
    }
    public override void Close()
    {
        base.Close();
        CameraZoomAndMove.Instance.ResetCamera();
        Tooltip.Instance.HideTooltip();
    }
    public override void Open()
    {
        CameraZoomAndMove.Instance.ZoomAndMove(base.Open);
    }
    public void BuySlot()
    {
        int currentTotalGird = SlotManager.Instance.currentTotalGrid;
        int price = initialPrice + (currentTotalGird - 2) * addPrice;
        if (PlayerManager.Instance.playerView.GetCoin() >= price)
        {
            PlayerManager.Instance.playerView.GainCoin(-price);
            UIGameManager.Instance.UpdateCoin();
            SlotManager.Instance.AddSlot();
        }
        if (SlotManager.Instance.currentTotalGrid == SlotManager.TOTAL_GRID)
        {
            SlotManager.Instance.HideBuySlot();
        }
    }
}
