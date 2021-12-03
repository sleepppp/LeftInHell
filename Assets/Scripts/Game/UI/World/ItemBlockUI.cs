using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
namespace Project.UI
{
    using Project.GameData;
    public class ItemBlockUI : UIBase, IPointerEnterHandler, IPointerExitHandler ,IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<ItemBlockUI> EventPointerEnter;
        public event Action<ItemBlockUI> EventPoinetExit;
        public event Action<ItemBlockUI> EventOnClick;
        public event Action<ItemBlockUI, PointerEventData> EventBeginDrag;
        public event Action<ItemBlockUI, PointerEventData> EventDrag;
        public event Action<ItemBlockUI, PointerEventData> EventEndDrag;

        [Header("ItemBlockUI")]
        [SerializeField] ItemUIBase _itemUIBase;
        [SerializeField] Image _backgroundImage;
        public ItemSlotUI StartSlotUI { get; private set; }
        public Color OriginBackgroundColor { get; private set; }
        public ItemBlock ItemBlock { get; private set; }

        public void Init(ItemBlock itemBlock, ItemSlotUI startSlotUI)
        {
            ItemBlock = itemBlock;
            _itemUIBase.Init(ItemBlock.Item, ItemBlock.Amount);
            StartSlotUI = startSlotUI;
            ColorRecord colorRecord = DataTableManager.ColorTable.GetRecord(ItemBlock.Item.ItemTypeRecord.ColorID);
            _backgroundImage.color = OriginBackgroundColor = colorRecord.GetColor();
            ResetTransform();
        }

        public void ResetTransform()
        {
            float width = ItemSlotUI.CellSize * ItemBlock.Width + ItemSlotUI.Spacing * (ItemBlock.Width);
            float height = ItemSlotUI.CellSize * ItemBlock.Height + ItemSlotUI.Spacing * (ItemBlock.Height);

            RectTransform.SetParent(StartSlotUI.transform.parent, true);
            RectTransform.position = StartSlotUI.transform.position;
            RectTransform.sizeDelta = new Vector2(width, height);
        }

        public void Refresh()
        {
            if(ItemBlock.IsValid)
            {
                _itemUIBase.Init(ItemBlock.Item, ItemBlock.Amount);
                ResetTransform();
            }
        }

        public void SetBackgroundColor(Color color)
        {
            _backgroundImage.color = color;
        }

        public void ResetBackgroundColor()
        {
            SetBackgroundColor(OriginBackgroundColor);
        }

        public void OnClick()
        {
            EventOnClick?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EventPointerEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EventPoinetExit?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            EventBeginDrag?.Invoke(this,eventData);

        }

        public void OnDrag(PointerEventData eventData)
        {
            EventDrag?.Invoke(this, eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EventEndDrag?.Invoke(this,eventData);

        }
    }
}