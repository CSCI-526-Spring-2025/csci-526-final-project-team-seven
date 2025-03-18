using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Async
{

    public class SequenceView : MonoBehaviour
    {
        //Thinking: No card views? Yes, we do.
        //public SequenceData sequenceData;
        public RectTransform selfRectTransform;
        public RectTransform backgroundRectTransform;
        public List<SlotView> slots;

        public Transform slotContainer;
        public GameObject cardPrefab;
        public GameObject slotPrefab;

        [HideInInspector]
        public float damageMultiplier = 1.0f;// Damage Multiplier of this thread

        public void Init(Vector2 localAnchorPosition, SequenceData sequenceData, string sequenceID)
        {
     
        }
        public void RemoveCardView(CardView cardView)
        {
            if (cardView.slot != null)
                cardView.slot.content = null;
            cardView.slot = null;
        }


    }
}