using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public abstract class ItemSlotBaseUI : UIBase
    {
        public IItemSlot Slot { get; private set; }

        public void Init(IItemSlot itemSlot)
        {
            Slot = itemSlot;
        }

        public abstract bool IsPossibleDrop(InventoryItem inventoryItem);
        public abstract bool TryDrop(InventoryItem inventoryItem);
    }
}
