using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Project.UI
{
    public class ItemTilingContainerUI : UIBase
    {
        public const int ScrollBarWidth = 20;
        [Header("ItemTilingContainerUI")]
        [SerializeField] GameObject m_tilingSlotPrefab;
        [SerializeField] GameObject m_inventoryItemPrefab;
        [SerializeField] ScrollRect m_scrollRect;
        [SerializeField] GridLayoutGroup m_gridLayoutGroup;

        ItemTilingContainer m_itemTilingContainer;

        ItemTileSlotUI[,] m_slots;
        List<InventoryItemUI> m_itemUIList = new List<InventoryItemUI>();

        public void Init(ItemTilingContainer container)
        {
            m_itemTilingContainer = container;
            m_slots = new ItemTileSlotUI[container.Height, container.Width];
            m_itemTilingContainer.Foreach(0, 0, container.Width, container.Height, (slot) =>
            {
                InstantiateSlotUI(slot);
            });

            m_gridLayoutGroup.SetLayoutHorizontal();
            m_gridLayoutGroup.SetLayoutVertical();
            Canvas.ForceUpdateCanvases();

            List<InventoryItem> items = m_itemTilingContainer.GetAllItem();
            foreach (var item in items)
            {
                InventoryItemUI itemUI = InstantiateItemUI(item);
                ItemTileSlot slot = item.OwnerSlot as ItemTileSlot;
                itemUI.Init(item, GetSlotUI(slot.IndexX, slot.IndexY));
            }
        }

        void InstantiateSlotUI(ItemTileSlot slot)
        {
            GameObject newObject = Instantiate(m_tilingSlotPrefab, m_scrollRect.content);
            ItemTileSlotUI ui = newObject.GetComponent<ItemTileSlotUI>();
            ui.Init(slot);
            m_slots[slot.IndexY, slot.IndexX] = ui;
        }

        InventoryItemUI InstantiateItemUI(InventoryItem item)
        {
            foreach(var itemUI in m_itemUIList)
            {
                if(itemUI.gameObject.activeSelf == false)
                {
                    return itemUI;
                }
            }
            InventoryItemUI ui = Instantiate(m_inventoryItemPrefab, m_scrollRect.content).GetComponent<InventoryItemUI>();
            m_itemUIList.Add(ui);
            return ui;
        }

        ItemTileSlotUI GetSlotUI(int indexX, int indexY)
        {
            if (m_itemTilingContainer.IsOutofRange(indexX, indexY))
                return null;

            return m_slots[indexY, indexX];
        }

        public void Refresh()
        {
            foreach(var itemUI in m_itemUIList)
            {
                itemUI.Refresh();
            }
        }
    }
}