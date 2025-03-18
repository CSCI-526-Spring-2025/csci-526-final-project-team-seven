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
        {
            for (int j = 0; j < COL; j++)
            {
                if (slotViews[i, j].content != null)
                {
                    var content = slotViews[i, j].content;
                    if (content.cardSpriteData.Type == "Bullet")
                    {
                        sequence.AppendCallback(() => BulletManager.Instance.GenerateBullet(content.cardRankData, 1));
                    }
                }
                sequence.AppendInterval(0.05f);
            }
        }
        sequence.SetLoops(-1);
    }
}
