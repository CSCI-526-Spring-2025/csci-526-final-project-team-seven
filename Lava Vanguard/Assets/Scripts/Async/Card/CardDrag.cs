using UnityEngine;
using UnityEngine.EventSystems;
namespace Async
{

    [RequireComponent(typeof(CardView))]
    public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
    {
        private CardView cardView;
        private RectTransform rectTransform;
        private Canvas canvas;
        private Vector2 originalPosition;
        private bool draggable = false;
        private Transform originalParent;
        private Transform draggingParent;

        public enum DragType
        {
            Sequence,
            Inventory
        }
        private DragType dragStartType;
        private void Awake()
        {

        }
        private void Start()
        {
            cardView = GetComponent<CardView>();
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            draggingParent = SlotManager.Instance.draggingTransform;
            //originalParent = transform.parent;
        }
        public void Init(bool draggable)
        {
            this.draggable = draggable;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!draggable)
                return;

           
            originalParent = transform.parent;
            originalPosition = rectTransform.anchoredPosition;
            transform.SetParent(draggingParent);
            
            if (cardView.slot == null)
                dragStartType = DragType.Inventory;
            else
                dragStartType = DragType.Sequence;
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!draggable)
                return;
            
            if (canvas == null) return;

            Vector2 delta = eventData.delta / canvas.scaleFactor;
            rectTransform.anchoredPosition += delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!draggable)
                return;

            //Remove from old position
            if (dragStartType == DragType.Inventory)
            {
                InventoryManager.Instance.inventoryView.RemoveCardView(cardView);
            }
            // 1. Drag to inventory.
            if (GameDataManager.InventoryConfig.CheckInside(rectTransform.anchoredPosition))
            {
                if (cardView.slot != null) 
                    cardView.slot.RemoveCardView();
                InventoryManager.Instance.inventoryView.AddCardView(cardView);
                SlotManager.Instance.UpdateAndRunSequence();
                return;
            }
            // 2. Drag to another empty slot.
            var slot = SlotManager.Instance.CheckDrag(cardView);
            if (slot != null) 
            {
                if (cardView.slot != null)
                {
                    cardView.slot.RemoveCardView();
                }
                slot.AddCardView(cardView);
                SlotManager.Instance.UpdateAndRunSequence();
                return;
            }
            // 3. Drag to another slot. Swap.
            // 4. Drag to self.
            // 5. Drag to somewhere else.
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
           
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            originalPosition = rectTransform.anchoredPosition;
            float e = 0.001f;
            if (Vector3.Distance(originalPosition, rectTransform.anchoredPosition) < e)
            {
                Tooltip.Instance.ShowTooltip(cardView.cardSpriteData);
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.Instance.HideTooltip();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Tooltip.Instance.HideTooltip();
        }
    }
}