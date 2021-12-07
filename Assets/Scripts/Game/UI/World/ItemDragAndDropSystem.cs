using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class ItemDragAndDropSystem : IRefresh
    {
        readonly List<IItemContainerUI> m_containerUIList;

        public ItemDragAndDropSystem()
        {
            m_containerUIList = new List<IItemContainerUI>();
        }

        public void RegisterContainer(IItemContainerUI containerUI)
        {
            m_containerUIList.AddUnique(containerUI);
        }
        public void UnRegisterContainer(IItemContainerUI containerUI)
        {
            m_containerUIList.Remove(containerUI);
        }

        public void StartDrag(InventoryItemUI inventoryItemUI)
        {
            foreach(var container in m_containerUIList)
            {
                container.OnEventStartDrag(inventoryItemUI);
            }

            GameObject.Destroy(inventoryItemUI);
        }

        public void Refresh()
        {
            foreach(var container in m_containerUIList)
            {
                container.Refresh();
            }
        }
    }
}
