using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class SendToGoogle : MonoBehaviour
{
    [SerializeField] private string URL;

    public long sessionID = 0;
    public int expLevel = 0;
    public string startTime = "";
    public string endTime = "";
    public float maxRange = 0;
    public float finalHealth = 0;
    public string sequenceData = "";
    public int isEditor = 0;

    void Start()
    {
        sessionID = DateTime.Now.Ticks; 
        startTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

    }

    public void RecordEndTime()
    {
        endTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        Debug.Log("record time initiated");
        Send();
    }

    private void OnApplicationQuit()
    {
        if (string.IsNullOrEmpty(endTime)) // if shut down directly 
        {
            RecordEndTime();
        }
    }

    public void Send()
    {
        expLevel = -1234;
        finalHealth = PlayerManager.Instance.playerView?.GetHP() ?? 10;
        if (Application.isEditor)
        {
            isEditor = 1;
        }
        



        sequenceData = SlotManager.Instance != null
            ? SlotManager.Instance.GetAllSlotCardData()
            : "No Slot Data Available";

        StartCoroutine(Post(sessionID.ToString(), startTime, endTime, expLevel.ToString(), finalHealth.ToString(), sequenceData,isEditor.ToString()));
    }


    private IEnumerator Post(string sessionID, string startTime, string endTime, string expLevel, string finalHealth, string sequenceData,string isEditor)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1156616989", sessionID);
        form.AddField("entry.1095354465", startTime);
        form.AddField("entry.844017086", endTime);
        form.AddField("entry.1140619881", expLevel);
        form.AddField("entry.842781336", finalHealth);
        form.AddField("entry.307482289", sequenceData);
        form.AddField("entry.2097804719", isEditor);

        using (UnityWebRequest www = UnityWebRequest.Post("https://docs.google.com/forms/u/0/d/1tiA8a2FBAE7rsP9ABsVroo_ZxPtPlfyXlwR89PRpQC8/formResponse", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Data Sent Successfully!");
            }
        }
    }
}








