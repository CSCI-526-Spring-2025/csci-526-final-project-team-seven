using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Async;
using UnityEngine.UI;
using System.Linq;
public class CardSelectorPanel : UIPanel
{ 
    public int optionNumber = 3;

    public Transform cardSelectorContainer;
    public GameObject cardSeletorPrefab;

    private List<CardSeletorView> cardSeletorViews = new List<CardSeletorView>();
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < optionNumber; i++)
        {
            cardSeletorViews.Add(Instantiate(cardSeletorPrefab, cardSelectorContainer).GetComponent<CardSeletorView>());
        }
    }
    public override void Show()
    {
        base.Show();
        var collectableCardsList = GameDataManager.CardData
            .Where(kv => kv.Value.Collectable)
            .Select(kv => kv.Value)
            .ToList();
        for (int i = 0; i < optionNumber; i++)
        {
            var data = collectableCardsList[Random.Range(0, collectableCardsList.Count)];
            collectableCardsList.Remove(data);
            var rankData = new CardRankData(data);
            if (rankData.CardID == "Card_Async")
            {
                rankData.LinkedSequenceID = "Not_Ready";
                rankData.Level = Random.Range(3, 6);
            }
            cardSeletorViews[i].Init(data, () => SelectCard(rankData));
        }
    }

    private void SelectCard(CardRankData data)
    {
        AsyncManager.Instance.GainCard(data);
        Hide();
    }
}
