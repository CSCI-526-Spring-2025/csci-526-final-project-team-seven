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
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            //SequenceManager.Instance.Init();
            SlotManager.Instance.Init();
            InventoryManager.Instance.Init();
        }
        public void GainCard(CardRankData data)
        {
            if (data.CardID == "Card_RestoreHealth")
            {
                PlayerManager.Instance.playerView.RestoreHealth();
                return;
            }
            InventoryManager.Instance.inventoryView.AddCardView(data);
        }
    }
}