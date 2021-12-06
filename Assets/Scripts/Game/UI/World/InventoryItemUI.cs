using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
namespace Project.UI
{
    using Project.GameData;
    public class InventoryItemUI : UIBase,IPointerDownHandler
    {
        public static void CreateUI(Action<InventoryItemUI> callback)
        {
            string path = "Assets/Resource/Prefab/UI/Item.prefab";
            AssetManager.LoadAssetAsync<GameObject>(path, (prefab) => 
            {
                GameObject newObject = GameObject.Instantiate(prefab);
                callback?.Invoke(newObject.GetComponent<InventoryItemUI>());
            });
        }

        public static Vector2 CalcUISize(IGetItemData data)
        {
            float width = data.Width * c_itemSlotSize + (data.Width) * c_itemSlotSpace;
            float height = data.Height * c_itemSlotSize + (data.Height) * c_itemSlotSpace;
            return new Vector2(width, height);
        }

        //todo ¼öÁ¤
        const float c_itemSlotSize = 50f;
        const float c_itemSlotSpace = 1f;

        [SerializeField] ItemInfoBaseUI m_itemInfoBaseUI;
        [SerializeField] MouseHoverChecker m_mouseHoverChecker;
        public ItemSlotBaseUI SlotUI { get; private set; }
        InventoryItem m_inventoryItem;

        Vector2 m_mouseOffset;

        public InventoryItem Item { get { return m_inventoryItem; } }

        public void Init(InventoryItem itemData, ItemSlotBaseUI slotUI = null)
        {
            m_inventoryItem = itemData;
            m_itemInfoBaseUI.Init(m_inventoryItem.Data);
            m_mouseHoverChecker.OriginColor = m_itemInfoBaseUI.BackgroundColor;
            m_mouseHoverChecker.ChangeColor = new Color(0.8f, 0.8f, 0.8f, m_itemInfoBaseUI.BackgroundColor.a);
            BindSlot(slotUI);
        }

        public void BindSlot(ItemSlotBaseUI slotUI)   
        {
            SlotUI = slotUI;
            if(SlotUI is ItemTileSlotUI)
            {
                float width = m_inventoryItem.Width * c_itemSlotSize + (m_inventoryItem.Width) * c_itemSlotSpace;
                float height = m_inventoryItem.Height * c_itemSlotSize + (m_inventoryItem.Height) * c_itemSlotSpace;
                RectTransform.SetParent(slotUI.transform.parent, true);
                RectTransform.position = slotUI.transform.position;
                RectTransform.sizeDelta = new Vector2(width, height);
            }
            else
            {
                //todo equipment Slot
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }

            ItemDragUI.CreateUI((ui) =>
            {
                m_inventoryItem.OwnerSlot.Owner.TryDisarmItem(m_inventoryItem);
                ui.Init(m_inventoryItem, SlotUI);

                Destroy(this.gameObject);
            });
            Debug.Log("OnPointerDown");
        }

        public void Refresh()
        {
            m_itemInfoBaseUI.Init(m_inventoryItem.Data);
        }
    }
}
