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
    private List<CardSelectorView> cardSeletorViews = new List<CardSelectorView>();
    public override void Init()
    {
        base.Init();
        for (int i = 0; i < optionNumber; i++)
        {
            cardSeletorViews.Add(Instantiate(cardSeletorPrefab, cardSelectorContainer).GetComponent<CardSelectorView>());
        }
        nextWaveButton.onClick.AddListener(() =>
        {
            CameraController.Instance.ResumeCamera();
            LevelManager.Instance.NextWave();
            Close();
        });
    }
    public override void Open()
    {
        base.Open();
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
    public override void Close()
    {
        base.Close();
    }
    public void UpdateSelectButton()
    {
        foreach (var v in cardSeletorViews)
            v.UpdateSelectButton();
    }
}
