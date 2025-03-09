using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using　Async;
using UnityEngine.UI;
using System.Linq;
public class CardSelectorManager : MonoBehaviour
{
    public static CardSelectorManager Instance { get; private set; }
    public int optionNumber = 3;
    public GameObject cardSelectorPanel;

    public Transform cardSelectorContainer;
    public GameObject cardSeletorPrefab;

    private List<CardSeletorView> cardSeletorViews = new List<CardSeletorView>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < optionNumber; i++) 
        {
            cardSeletorViews.Add(Instantiate(cardSeletorPrefab, cardSelectorContainer).GetComponent<CardSeletorView>());
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            StartSelection();
        }
    }
    public void StartSelection(){
        Time.timeScale = 0;
        cardSelectorPanel.SetActive(true);
        var collectableCardsList = GameDataManager.CardData
            .Where(kv => kv.Value.Collectable)
            .Select(kv => kv.Value)
            .ToList();
        for (int i = 0; i < optionNumber; i++)
        {
            var data = collectableCardsList[Random.Range(0, collectableCardsList.Count)];
            collectableCardsList.Remove(data);
            var rankData = new CardRankData(data);
            cardSeletorViews[i].Init(data, () => SelectCard(rankData));
        }
    }

    private void SelectCard(CardRankData data){
        AsyncManager.Instance.GainCard(data);
        cardSelectorPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
