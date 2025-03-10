using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Async
{
    public class CardView : MonoBehaviour
    {
        public CardSpriteData cardSpriteData;
        public CardRankData cardRankData;

        public SlotView slot;

        public RectTransform rectTransform;
        public Image background;
        public Image outline;
        public Image content;
        public void Init(SlotView slot, CardSpriteData cardSpriteData, CardRankData cardRankData)//Consider using inheritance and other data like atk.
        {
            this.slot = slot;
            this.cardSpriteData = cardSpriteData;
            this.cardRankData = cardRankData;
            //Init size
            rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = Vector2.one * GameDataManager.CardConfig.CardSize;

            //Init sprite
            background.sprite = GameDataManager.BackgroundSprite[cardSpriteData.Background];
            outline.sprite = GameDataManager.OutlineSprite[cardSpriteData.Outline];
            content.sprite = GameDataManager.ContentSprite[cardSpriteData.Content];
            content.color = ColorCenter.CardColors[cardSpriteData.Type];
        }
    }
}