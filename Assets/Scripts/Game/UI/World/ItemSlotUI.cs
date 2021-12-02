using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
namespace Project.UI
{
    public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public const float SlotUIWidth = 50f;
        public const float SlotUIHeight = 50f;

        public event Action<ItemSlotUI> EventMouseEnter;
        public event Action<ItemSlotUI> EventMouseExit;

        [SerializeField] Image _backgroundImage;
        Color _originColor;

        ItemSlot _itemSlot;

        public Color OriginalColor { get { return _originColor; } }

        public void Init(ItemSlot slot)
        {
            _itemSlot = slot;
            _originColor = _backgroundImage.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //SetBackgroundColor(new Color(0.7f, 0.7f, 0.7f, _originColor.a));
            EventMouseEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //ResetBackgroundColor();
            EventMouseExit?.Invoke(this);
        }

        public void SetBackgroundColor(Color color)
        {
            _backgroundImage.color = color;
        }

        public void ResetBackgroundColor()
        {
            SetBackgroundColor(_originColor);
        }

        public bool IsEmpty()
        {
            return _itemSlot.IsEmpty();
        }
    }
}
