using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Async
{

    public class SequenceView : MonoBehaviour
    {
        //Thinking: No card views? Yes, we do.
        //public SequenceData sequenceData;
        public RectTransform selfRectTransform;
        public RectTransform backgroundRectTransform;
        public List<SlotView> slots;

        public Transform slotContainer;
        public GameObject cardPrefab;
        public GameObject slotPrefab;

        [HideInInspector]
        public float damageMultiplier = 1.0f;// Damage Multiplier of this thread

        public void Init(Vector2 localAnchorPosition, SequenceData sequenceData, string sequenceID)
        {
            //this.sequenceData = sequenceData;

            //Init position
            backgroundRectTransform.anchoredPosition = localAnchorPosition;

            //Init size
            selfRectTransform.sizeDelta = new Vector2(GameDataManager.CardConfig.MainLength, GameDataManager.CardConfig.MainHeight);
            backgroundRectTransform.sizeDelta = new Vector2(backgroundRectTransform.sizeDelta.x, GameDataManager.CardConfig.MainHeight);

            for (int i = 0; i < sequenceData.CardDatas.Count; i++)
            {
                var slot = Instantiate(slotPrefab, slotContainer).GetComponent<SlotView>();
                slots.Add(slot);
                slot.Init(this);
                var data = sequenceData.CardDatas[i];
                if (data.CardID != "Card_Empty")
                {
                    CardView cardView = Instantiate(cardPrefab, slot.transform).GetComponent<CardView>();
                    cardView.Init(slot, GameDataManager.CardData[data.CardID], data);
                    cardView.GetComponent<CardDrag>().Init(GameDataManager.CardData[data.CardID].Draggable);
                    slot.Init(this, cardView);
                    if (data.CardID == "Card_Async2")
                    {
                        cardView.UpdateThreadID(sequenceID);
                    }
                }

                if (data.LinkedSequenceID != null)
                {
                    SequenceManager.Instance.InitSequence(localAnchorPosition + new Vector2(i * GameDataManager.CardConfig.CardSize, 0), data.LinkedSequenceID);
                }
            }
        }
        public void RemoveCardView(CardView cardView)
        {
            if (cardView.slot != null)
                cardView.slot.content = null;
            cardView.slot = null;
        }

        public void AddCardView(CardView cardView, SlotView slotView)
        {
            if (cardView.slot != null) 
                cardView.slot.sequenceView.RemoveCardView(cardView);
            cardView.slot = slotView;
            slotView.content = cardView;
            cardView.transform.SetParent(slotView.transform);
            cardView.rectTransform.anchoredPosition = Vector2.zero;
        }

    }
}