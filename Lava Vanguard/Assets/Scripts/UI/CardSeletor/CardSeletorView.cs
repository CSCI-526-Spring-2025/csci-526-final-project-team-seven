using Async;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardSeletorView : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;
    public Button selectButton;
    public CardView cardView;

    public void Init(CardSpriteData data, UnityAction action)
    {
        this.title.text = data.Name;
        this.description.text = data.Description;
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(action);
        this.cardView.Init(null, data, new CardRankData());
    }
}
