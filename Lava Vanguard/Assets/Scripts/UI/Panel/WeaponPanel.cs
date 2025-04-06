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
        if (PlayerManager.Instance.playerView.GetCoin() >= 40)
        {
            PlayerManager.Instance.playerView.GainCoin(-40);
            UIGameManager.Instance.UpdateCoin();
            SlotManager.Instance.AddSlot();
        }
    }
}
