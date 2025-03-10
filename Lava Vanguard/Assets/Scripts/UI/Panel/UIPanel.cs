using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public Button backButton;
    [HideInInspector]
    public bool isOpen = false;
    public virtual void Init()
    {
        if (backButton != null)
            backButton.onClick.AddListener(Hide);
    }
    public virtual void Show()
    {
        isOpen = true;
        gameObject.SetActive(true);
    }
    public virtual void Hide()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
    public virtual void Switch()
    {
        isOpen = !isOpen;
        if (isOpen)
            Show();
        else
            Hide();
    }
}
