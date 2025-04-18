using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : UIPanel
{
    public Button buySlotButton;
    public TMP_Text costCoin;
    private int initialPrice=5;
    private int addPrice = 5;
    public override void Init()
    {
        base.Init();
        canOpen = false;
        buySlotButton.onClick.AddListener(BuySlot);
        costCoin.text = "Buy Slot\r\n" + initialPrice+"$";
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
        int price = 5 + (currentTotalGird - 2) * addPrice;
        if (PlayerManager.Instance.playerView.GetCoin() >= price)
        {
            PlayerManager.Instance.playerView.GainCoin(-price);
            UIGameManager.Instance.UpdateCoin();
            SlotManager.Instance.AddSlot();
            costCoin.text = "Buy Slot\r\n" + (price + addPrice)+"$";
        }
        if (SlotManager.Instance.currentTotalGrid == SlotManager.TOTAL_GRID)
            buySlotButton.gameObject.SetActive(false);
    }
}
