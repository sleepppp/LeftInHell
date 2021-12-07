
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Project.UI
{
    public class ItemTileContainerUI : UIBase, IItemContainerUI
    {
        [Header("ItemTileContainerUI")]
        [SerializeField] GameObject m_slotPrefab;
        [SerializeField] ScrollRect m_scrollRect;
        [SerializeField] GridLayoutGroup m_gridLayoutGroup;

        ItemTileContainer m_itemTileContainer;

        readonly Dictionary<Puid, ItemTileSlotUI> m_slotUIContainer = new Dictionary<Puid, ItemTileSlotUI>();
        readonly Dictionary<Puid, InventoryItemUI> m_itemContainer = new Dictionary<Puid,InventoryItemUI>();

        public IItemContainer ItemContainer => m_itemTileContainer;

        void Start()
        {
            Game.UIManager.DragAndDropSystem.RegisterContainer(this);
        }

        void OnDestroy()
        {
            Game.UIManager.DragAndDropSystem.UnRegisterContainer(this);
        }

        public void Init(ItemTileContainer itemContainer)
        {
            m_itemTileContainer = itemContainer;
            List<IItemSlot> slotList = m_itemTileContainer.Slots;
            foreach(var slot in slotList)
            {
                GameObject newObject = Instantiate(m_slotPrefab, m_scrollRect.content);
                ItemTileSlotUI ui = newObject.GetComponent<ItemTileSlotUI>();
                ui.Init(slot);
                m_slotUIContainer.Add(slot.Puid,ui);
            }

            //force update layout
            m_gridLayoutGroup.SetLayoutHorizontal();
            m_gridLayoutGroup.SetLayoutVertical();
            Canvas.ForceUpdateCanvases();

            Refresh();
        }

        public void Refresh()
        {
            List<IItem> itemList = m_itemTileContainer.Items;
            foreach(var item in itemList)
            {
                InstantiateItemUI((ui) => 
                {
                    ui.Init(item);
                    ui.BindToSlot(GetSlotUI(item.OwnerSlot.Puid));
                });
            }
        }

        void InstantiateItemUI(Action<InventoryItemUI> callback)
        {
            foreach(var item in m_itemContainer)
            {
                if(item.Value.gameObject.activeSelf == false)
                {
                    item.Value.gameObject.SetActive(true);
                    callback?.Invoke(item.Value);
                    return;
                }
            }

            InventoryItemUI.CreateUI((ui)=> 
            {
                m_itemContainer.Add(ui.Item.Puid, ui);
                callback?.Invoke(ui);
            });
        }

        ItemTileSlotUI GetSlotUI(Puid slotPuid)
        {
            return m_slotUIContainer.GetValue(slotPuid);
        }

        public void OnEventStartDrag(InventoryItemUI inventoryItemUI)
        {
            if(m_itemContainer.ContainsKey(inventoryItemUI.Item.Puid))
            {
                m_itemContainer.Remove(inventoryItemUI.Item.Puid);
            }
        }
    }
}