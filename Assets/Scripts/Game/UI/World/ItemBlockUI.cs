using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
namespace Project.UI
{
    public class ItemBlockUI : UIBase, IPointerEnterHandler,IPointerExitHandler
    {
        public event Action<ItemBlockUI> EventClickItemBlock;
        public event Action<ItemBlockUI> EventMounseEnter;
        public event Action<ItemBlockUI> EventMouseExit;

        [SerializeField] ItemUIBase _itemUIBase;
        [SerializeField] Image _whiteDimmedImage;
        public ItemBlock ItemBlock { get; private set; }

        public void Init(ItemBlock itemBlock,ItemSlotUI slotUI)
        {
            ItemBlock = itemBlock;

            float width = ItemSlotUI.SlotUIWidth * itemBlock.Width;
            float height = ItemSlotUI.SlotUIHeight * itemBlock.Height;
            //this.RectTransform.SetParent(slotUI.transform);
            this.RectTransform.pivot = new Vector2(0f, 1f);
            this.RectTransform.sizeDelta = new Vector2(width, height);
            this.RectTransform.position = slotUI.transform.position;

            ShowWhiteDimmed(false);
            Refresh();
        }

        public void OnClickItemBlock()
        {
            EventClickItemBlock?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EventMounseEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EventMouseExit?.Invoke(this);
        }

        public void ShowWhiteDimmed(bool isShow)
        {
            _whiteDimmedImage.gameObject.SetActive(isShow);
        }

        public void Refresh()
        {
            if(ItemBlock.IsValid)
            {
                _itemUIBase.Init(ItemBlock.Item, ItemBlock.Amount);
            }
        }
    }
}