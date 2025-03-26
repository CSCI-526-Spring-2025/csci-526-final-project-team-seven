using Async;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlotManager : MonoBehaviour
{
    public static readonly int ROW = 4;
    public static readonly int COL = 10;
    [HideInInspector]
    public SlotView[,] slotViews = new SlotView[ROW, COL];
    public GameObject slotPrefab;
    public Transform slotContainer;
    public Transform draggingTransform;
    public static SlotManager Instance { get; private set; }

    private Sequence sequence;
    private void Awake()
    {
        Instance = this;
    }
    public void Init()
    {
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                slotViews[i, j] = Instantiate(slotPrefab, slotContainer).GetComponent<SlotView>();
            }
        }
        UpdateAndRunSequence();
    }
    public SlotView CheckDrag(CardView cardView)
    {
        var cardPosition = cardView.rectTransform.position;
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                if (slotViews[i, j].content == null && slotViews[i, j].CheckInside(cardPosition)) 
                {
                    return slotViews[i, j];
                }
            }
        }
        return null;
    }
    public void UpdateAndRunSequence()
    {
        sequence.Kill();
        sequence = DOTween.Sequence();

        for (int i = 0; i < ROW; i++)
            for (int j = 0; j < COL; j++)
                slotViews[i, j].damageMultiplier = 1.0f;
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                var content = slotViews[i, j].content;
                if (content != null && content.cardSpriteData.Type == "Functional") 
                {
                    if (content.cardSpriteData.ID == "Card_DamageUp")
                    {
                        for (int k = i - 1; k <= i + 1; k++)
                        {
                            for (int l = j - 1; l <= j + 1; l++) 
                            {
                                if (k < 0 || l < 0 || k >= ROW || l >= COL)
                                    continue;
                                slotViews[k, l].damageMultiplier *= 2f;
                            }
                        }
                    }
                }
            }
        }


        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                var slot = slotViews[i, j];
                if (slot.content != null)
                {
                    var content = slot.content;
                    if (content.cardSpriteData.Type == "Bullet")
                    {
                        sequence.AppendCallback(() => BulletManager.Instance.GenerateBullet(content.cardRankData, slot.damageMultiplier));
                    }
                }
                sequence.AppendInterval(0.05f);
            }
        }
        sequence.SetLoops(-1);
    }

    public string GetAllSlotCardData()
    {
        string formattedData = "All Cards in Slots: ";

        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                var slot = slotViews[i, j];
                if (slot.content != null)
                {
                    formattedData += $"[{slot.content.cardRankData.CardID}], ";
                }
            }
        }

        if (formattedData == "All Cards in Slots: ")
        {
            return "No cards found in any slot.";
        }

        return formattedData.TrimEnd(',', ' '); 
    }
    private void OnDestroy()
    {
        sequence.Kill();
    }

}
