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
        [SerializeField] ScrollRect m_scrollRect;

        ItemTilingContainer m_itemTilingContainer;

        public void Init(ItemTilingContainer container)
        {
            m_itemTilingContainer = container;

            m_itemTilingContainer.Foreach(0, 0, container.Width, container.Height, (slot) =>
            {
                InstantiateSlot(slot);
            });
        }

        void InstantiateSlot(ItemSlotTile slot)
        {
            GameObject newObject = Instantiate(m_tilingSlotPrefab, m_scrollRect.content);
            ItemTileSlotUI ui = newObject.GetComponent<ItemTileSlotUI>();
            ui.Init(slot);
        }
    }
}