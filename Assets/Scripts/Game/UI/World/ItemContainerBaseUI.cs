using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public interface IItemContainerUI : IRefresh
    {
        IItemContainer ItemContainer { get; }
        void OnEventStartDrag(InventoryItemUI inventoryItemUI);
    }
}
