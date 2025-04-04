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
    [HideInInspector]
    public bool canOpen = true;
    [HideInInspector]
    public bool canClose = true;
    public virtual void Init()
    {
        if (backButton != null)
            backButton.onClick.AddListener(Close);
    }
    public virtual void Open()
    {
        isOpen = true;
        gameObject.SetActive(true);
    }
    public virtual void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
    public virtual void Switch()
    {
        if (!isOpen && canOpen)
        {
            Open();
        }
        else if (isOpen && canClose)
        {
            Close();
        }
    }
}
