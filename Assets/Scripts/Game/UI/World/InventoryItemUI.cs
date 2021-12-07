using UnityEngine;
using UnityEngine.EventSystems;
using System;
namespace Project.UI
{
    public class InventoryItemUI : UIBase, IRefresh,IBeginDragHandler
    {
        public static void CreateUI(Action<InventoryItemUI> callback)
        {
            string path = "Assets/Resource/Prefab/UI/InventoryItem.prefab";
            AssetManager.Instantiate<InventoryItemUI>(path, callback);
        }

        [SerializeField] CommonItemUI m_commonItemUI;
        [SerializeField] MouseHoverChecker m_mouseHoverChecker;
        IItem m_item;

        public IItem Item { get { return m_item; } }

        public void Init(IItem item)
        {
            m_item = item;
            m_commonItemUI.Init(item.ItemRecord.ID, item.Amount);
            m_mouseHoverChecker.OriginColor = m_commonItemUI.OriginColor;
            m_mouseHoverChecker.ChangeColor = new Color(0.8f, 0.8f, 0.8f, m_commonItemUI.OriginBackgroundAlpha);
        }

        public void BindToSlot(ItemSlotBaseUI slotUI)
        {
            if(slotUI is ItemTileSlotUI)
            {
                RectTransform.SetParent(slotUI.transform.parent, true);
                RectTransform.position = slotUI.transform.position;
                RectTransform.sizeDelta = ItemTileSlotUI.GetItemSize(m_item.ItemRecord);
            }
            else //equipSlot
            {
                RectTransform.SetParent(slotUI.RectTransform, true);
                RectTransform.position = slotUI.RectTransform.position;
                RectTransform.sizeDelta = slotUI.RectTransform.sizeDelta;
            }
        }

        public void OnClick()
        {
            if(Input.GetMouseButtonUp(1))
            {
                UIManager.AsynCreateItemOptionMenuUI(m_item, Input.mousePosition);
            }
        }

        public void Refresh()
        {
            m_commonItemUI.Init(m_item.ItemRecord.ID, m_item.Amount);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Game.UIManager.DragAndDropSystem.StartDrag(this);
        }
    }
}
