using Async;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardSelectorView : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;
    public Button selectButton;
    public CardView cardView;
    public TMP_Text cost;

    private CardSpriteData data;
    private CardRankData rankData;

    private bool sold = false;
    public void Init(CardSpriteData data, CardRankData rankData)
    {
        this.title.text = data.Title;
        this.description.text = data.Description;

        this.cardView.Init(null, data, new CardRankData());

        this.data = data;
        this.rankData = rankData;

        this.sold = false;
        UpdateSelectButton();
    }
    public void UpdateSelectButton()
    {
        if (sold)
            return;
        this.cost.text = data.Cost + "$";
        bool affordable = PlayerManager.Instance.playerView.playerData.coin >= data.Cost;
        this.cost.color = affordable ? ColorCenter.SelectorPanelColors["Green"] : ColorCenter.SelectorPanelColors["Red"];
        this.selectButton.interactable = affordable;
        this.selectButton.onClick.RemoveAllListeners();
        this.selectButton.onClick.AddListener(() =>
        {
            AsyncManager.Instance.GainCard(rankData);
            selectButton.interactable = false;
            cost.text = "Sold";
            sold = true;
            PlayerManager.Instance.playerView.playerData.coin -= data.Cost;
            UIGameManager.Instance.UpdateCoin();
            UIGameManager.Instance.GetPanel<CardSelectorPanel>().UpdateSelectButton();
        });
    }
}
