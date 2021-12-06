using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Project.UI
{
    public class ItemDragUI : UIBase
    {
        public static void CreateUI(Action<ItemDragUI> callback)
        {
            string path = "Assets/Resource/Prefab/UI/ItemDragUI.prefab";
            AssetManager.LoadAssetAsync<GameObject>(path, (prefab) =>
            {
                GameObject newObject = GameObject.Instantiate(prefab);
                callback?.Invoke(newObject.GetComponent<ItemDragUI>());
            });
        }

        [SerializeField] ItemInfoBaseUI m_itemInfoBaseUI;

        ItemSlotBaseUI m_prevSlotUI;
        InventoryItem m_prevItem;

        public void Init(InventoryItem inventoryItem, ItemSlotBaseUI prevSlotUI)
        {
            m_prevSlotUI = prevSlotUI;
            m_prevItem = inventoryItem;
            m_itemInfoBaseUI.Init(m_prevItem.Data);

            RectTransform.SetParent(Game.UIManager.MainCanvas.transform, true);
            RectTransform.sizeDelta = InventoryItemUI.CalcUISize(m_prevItem.Data);
        }

        private void Update()
        {
            if(Input.GetMouseButtonUp(0))
            {
                ItemSlotBaseUI slotUI = Game.UIManager.Raycast<ItemSlotBaseUI>(Input.mousePosition);
                if(slotUI && slotUI.IsPossibleDrop(m_prevItem))
                {
                    slotUI.TryDrop(m_prevItem);
                }
                else //undo
                {
                    m_prevSlotUI.TryDrop(m_prevItem);
                }

                Destroy(gameObject);
            }
            else
            {
                RectTransform.position = Input.mousePosition;
                RectTransform.ReviseTransformInRect(Game.UIManager.SafeArea);

                ItemSlotBaseUI slotUI = Game.UIManager.Raycast<ItemSlotBaseUI>(Input.mousePosition);
                if(slotUI != null && slotUI.IsPossibleDrop(m_prevItem))
                {
                    m_itemInfoBaseUI.SetBackgroundColor(new Color(0f, 0.8f, 0f, m_itemInfoBaseUI.BackgroundColor.a));
                }
                else
                {
                    m_itemInfoBaseUI.SetBackgroundColor(new Color(0.8f, 0f, 0f, m_itemInfoBaseUI.BackgroundColor.a));
                }
            }
        }
    }
}