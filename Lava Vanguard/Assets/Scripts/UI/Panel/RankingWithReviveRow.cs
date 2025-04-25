using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine;

public class RankingWithReviveRow : MonoBehaviour
{
    [SerializeField] TMP_Text textRank;
    [SerializeField] TMP_Text textName;
    [SerializeField] TMP_Text textWave;
    [SerializeField] TMP_Text textKilled;
    [SerializeField] TMP_Text textRevive;

    public void Set(int rank, string name, int wave, int killed,int revive)
    {
        textRank.text = rank.ToString();
        textName.text = name;
        textWave.text = wave.ToString();
        textKilled.text = killed.ToString();
        textRevive.text = revive.ToString();
    }
}
