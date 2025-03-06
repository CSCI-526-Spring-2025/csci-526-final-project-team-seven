using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Async
{

    public class InventoryView : MonoBehaviour
    {
        //Thinking: no card views? Yes! We do!
        //public InventoryData inventoryData;
        public RectTransform inventoryRectTransform;
        public GridLayoutGroup gridLayoutGroup;

        public Transform cardContainer;
        public GameObject[] cardPrefabs;

        public List<CardView> cardViews = new List<CardView>();
        public void Init()
        {
            //Init position and size
            inventoryRectTransform.anchoredPosition = GameDataManager.InventoryConfig.Center;
            inventoryRectTransform.sizeDelta = GameDataManager.InventoryConfig.Size;

            gridLayoutGroup.cellSize = Vector2.one * GameDataManager.CardConfig.CardSize;

            var inventoryData = GameDataManager.InventoryData;
            for (int i = 0; i < inventoryData.CardDatas.Count; i++)
            {
                var data = inventoryData.CardDatas[i];
                if (data.CardID != "Card_Empty")
                {
                    var cardView = Instantiate(cardPrefabs[0], cardContainer).GetComponent<CardView>();
                    cardView.Init(null, GameDataManager.CardData[data.CardID], data);
                    cardView.GetComponent<CardDrag>().Init(GameDataManager.CardData[data.CardID].Draggable);
                    cardViews.Add(cardView);
                }
            }
        }
        public void RemoveCardView(CardView cardView)
        {
            cardViews.Remove(cardView);
        }
        public void AddCardView(CardView cardView)
        {
            cardView.transform.SetParent(inventoryRectTransform);
            cardViews.Add(cardView);
        }
        public void AddCardView(CardRankData data)
        {
            if(data.CardID=="Card_01"){
                var cardView = Instantiate(cardPrefabs[0], cardContainer).GetComponent<CardView>();
                cardView.Init(null, GameDataManager.CardData[data.CardID], data);
                cardView.GetComponent<CardDrag>().Init(GameDataManager.CardData[data.CardID].Draggable);
                cardViews.Add(cardView);
            }
            else if(data.CardID=="Card_Async2"){
                var cardView = Instantiate(cardPrefabs[1], cardContainer).GetComponent<CardView>();
                cardView.Init(null, GameDataManager.CardData[data.CardID], data);
                cardView.GetComponent<CardDrag>().Init(GameDataManager.CardData[data.CardID].Draggable);
                cardViews.Add(cardView);
            }
        }
    }
}