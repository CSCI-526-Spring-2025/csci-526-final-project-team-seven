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
    public Button nextWaveButton;
    public Button refreshButton;
    
    [HideInInspector]
    public List<CardSelectorView> cardSeletorViews = new List<CardSelectorView>();
    private int refreshCount = 0;
    private TMP_Text refreshText;
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < optionNumber; i++)
        {
            cardSeletorViews.Add(Instantiate(cardSeletorPrefab, cardSelectorContainer).GetComponent<CardSelectorView>());
        }
        nextWaveButton.onClick.AddListener(NextWaveFunc);
        refreshButton.onClick.AddListener(RefreshFunc);
        refreshText = refreshButton.transform.GetChild(0).GetComponent<TMP_Text>();
    }
    public override void Open()
    {
        base.Open();
        refreshCount = 0;
        var collectableCardsList = GameDataManager.CardData
            .Where(kv => kv.Value.Collectable)
            .Select(kv => kv.Value)
            .ToList();
        for (int i = 0; i < optionNumber; i++)
        {
            var data = collectableCardsList[Random.Range(0, collectableCardsList.Count)];
            collectableCardsList.Remove(data);
            var rankData = new CardRankData(data);
            cardSeletorViews[i].Init(data, rankData);
        }
    }
    public void RefreshCard(List<string> IDs, List<int> prices)
    {
        int i = 0;
        for (; i < IDs.Count; i++) 
        {
            var data = GameDataManager.CardData[IDs[i]];
            var rankData = new CardRankData(data);
            data.Cost = prices[i];
            cardSeletorViews[i].Init(data, rankData);
        }
        for (; i < optionNumber; i++)
            cardSeletorViews[i].Reset();
    }
    public override void Close()
    {
        base.Close();
    }
    public void UpdateSelectButton()
    {
        foreach (var v in cardSeletorViews)
            v.UpdateSelectButton();
    }
    private void NextWaveFunc()
    {
        CameraController.Instance.ResumeCamera();
        LevelManager.Instance.NextWave();
        Close();
    }
    private void RefreshFunc()
    {
        int price = 3 + 3 * refreshCount;
        if (PlayerManager.Instance.playerView.GetCoin() >= price)
        {
            PlayerManager.Instance.playerView.GainCoin(-price);
            UIGameManager.Instance.UpdateCoin();
            var collectableCardsList = GameDataManager.CardData
            .Where(kv => kv.Value.Collectable)
            .Select(kv => kv.Value)
            .ToList();
            for (int i = 0; i < optionNumber; i++)
            {
                var data = collectableCardsList[Random.Range(0, collectableCardsList.Count)];
                collectableCardsList.Remove(data);
                var rankData = new CardRankData(data);
                cardSeletorViews[i].Init(data, rankData);
            }

            refreshCount++;
            refreshText.text = "Refresh" + '\n' + (3 + 3 * refreshCount) + "$";
        }
    }
}
