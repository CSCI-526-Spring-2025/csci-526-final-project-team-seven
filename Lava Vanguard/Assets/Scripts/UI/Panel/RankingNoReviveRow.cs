using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine;

public class RankingNoReviveRow : MonoBehaviour
{
    [SerializeField] TMP_Text textRank;
    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text textWave;
    [SerializeField] TMP_Text textKilled;

    public void Set(int rank,string name,int wave, int killed)
    {
        textRank.text = rank.ToString();
        textName.text = name;
        textWave.text = textWave.ToString();
        textKilled.text = textKilled.ToString();
    }
}
