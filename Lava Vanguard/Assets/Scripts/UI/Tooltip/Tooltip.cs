using Async;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance {  get; private set; }
    public GameObject tooltip;
    public TMP_Text title;
    public TMP_Text description;
    private void Awake()
    {
        Instance = this; 
    }
    public void ShowTooltip(CardView data)
    {
        tooltip.SetActive(true);
        title.text = data.cardSpriteData.Title+" (Level: "+data.cardRankData.Level+")";
        description.text = data.cardSpriteData.Description;
        tooltip.transform.position = Input.mousePosition + new Vector3(0, -150, 0);
    }
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
