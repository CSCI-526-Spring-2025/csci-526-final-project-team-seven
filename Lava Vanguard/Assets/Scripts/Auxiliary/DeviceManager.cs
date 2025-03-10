using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeviceManager : MonoBehaviour
{
    public GameObject HintPC1; 
    public GameObject HintPC2;
    public GameObject Hint3;
    public GameObject HintMobile;
    public RectTransform pauseButton;
    public RectTransform weaponButton;
    public Transform expBarGroup;
    public Transform hpBarGroup;
    public Transform card1;
    public Transform card2;
    public Transform card3;
    public RectTransform quitButton;
    public RectTransform restartButton;


    bool is_mobile_test = true;
    // Start is called before the first frame update
    void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld || is_mobile_test )
        {
            Debug.Log("Running on Mobile");
            HintPC1.SetActive(false); 
            HintPC2.SetActive(false);
            Hint3.SetActive(false);
            HintMobile.SetActive(true);
            StartCoroutine(HideHintMobileAfterDelay(10f));
            pauseButton.localScale = new Vector3(2.5f, 2.5f, 1f);
            weaponButton.localScale = new Vector3(2.5f, 2.5f, 1f);
            pauseButton.anchoredPosition += new Vector2(100f, 0f);
            weaponButton.anchoredPosition += new Vector2(350f, 0f);
            expBarGroup.position -= new Vector3(0f, 100f, 0f);
            hpBarGroup.position -= new Vector3(0f, 50f, 0f);
            expBarGroup.localScale = new Vector3(2f, 2f, 1f);
            hpBarGroup.localScale = new Vector3(2f, 2f, 1f);
            card1.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            card2.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            card3.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            card1.position -= new Vector3(0f, 100f, 0f);
            card2.position -= new Vector3(0f, 100f, 0f);
            card3.position -= new Vector3(0f, 100f, 0f);
            quitButton.localScale = new Vector3(3f, 3f, 1f);
            quitButton.position -= new Vector3(0f, 100f, 0f);
            restartButton.localScale = new Vector3(3f, 3f, 1f);
        }
        else
        {
            Debug.Log("Running on PC");
            HintPC1.SetActive(true);
            HintPC2.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator HideHintMobileAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        HintMobile.SetActive(false);
        Debug.Log("HintMobile now hide");
    }

}
