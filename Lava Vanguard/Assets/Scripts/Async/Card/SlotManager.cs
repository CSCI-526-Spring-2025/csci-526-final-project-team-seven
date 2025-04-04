using Async;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlotManager : MonoBehaviour
{
    public static readonly int ROW = 4;
    public static readonly int COL = 1;
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
            {
                if (slotViews[i, j].content != null)
                {
                    slotViews[i, j].content.cardRankData.Level = 1;
                }
            }
        PlayerManager.Instance.playerView.ResetSpeed();
        int currentHP = PlayerManager.Instance.playerView.GetHP();
        PlayerManager.Instance.playerView.ResetHealthLimit();
        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                var content = slotViews[i, j].content;
                if (content != null && content.cardSpriteData.Type == "Functional")
                {
                    switch (content.cardSpriteData.ID) 
                    {
                        case "Card_LevelUp":
                            for (int k = i - 1; k <= i + 1; k++)
                            {
                                for (int l = j - 1; l <= j + 1; l++)
                                {
                                    if ((k==i&&l==j) || k < 0 || l < 0 || k >= ROW || l >= COL || slotViews[k, l].content == null)
                                        continue;
                                    slotViews[k, l].content.cardRankData.Level++;
                                }
                            }
                            break;
                        case "Card_SpeedUp":
                            PlayerManager.Instance.playerView.SpeedUp();
                            break;
                        case "Card_HealthUp":
                            PlayerManager.Instance.playerView.HealthUp(currentHP);
                            break;
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
                        content.content.color =ColorCenter.CardColors["Bullet"+content.cardRankData.Level];
                        sequence.AppendCallback(() => BulletManager.Instance.GenerateBullet(content));
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
