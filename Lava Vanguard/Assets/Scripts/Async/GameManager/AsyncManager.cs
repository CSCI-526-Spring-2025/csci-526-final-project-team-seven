using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Async
{
    //Just for temp testing
    public class AsyncManager : MonoBehaviour
    {
        public static AsyncManager Instance { get; private set; }
        [HideInInspector]
        public string cardSelection = "";
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            SlotManager.Instance.Init();
            InventoryManager.Instance.Init();
        }
        public void GainCard(CardRankData data)
        {
            RecordCardSelection(data);
            if (data.CardID == "Card_RestoreHealth")
            {
                PlayerManager.Instance.playerView.RestoreHealth();
                return;
            }
            InventoryManager.Instance.inventoryView.AddCardView(data);
        }

        public void RecordCardSelection(CardRankData data)
        {
            int wave = LevelManager.Instance.wave;
            cardSelection += $"Wave {wave + 1}:  {data.CardID} \n";
        }

    }
}