using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    using Project.GameData;
    public class ItemBagBase : Item
    {
        public ItemContainer ItemContainer { get; private set; }
        public ItemBagBase(ItemRecord record) : base(record)
        {
            if (record.Type != (int)ItemType.Bag &&
                record.Type != (int)ItemType.ItemBox)
            {
                throw new System.Exception("Wrong itemRecord");
            }
            Vector2Int size = ItemDataTable.GetItemContainerSize(record.ID);
            ItemContainer = new ItemContainer(size.x, size.y);
        }
        public bool CanAddItem(int itemRecordID, int amount)
        {
            return ItemContainer.CanAddItem(itemRecordID, amount);
        }

        public bool TryAddItem(int itemRecordID, int amount)
        {
            return ItemContainer.TryAddItem(itemRecordID, amount);
        }

        public bool TryRemoveItem(ItemBlock itemBlock, int amount)
        {
            return ItemContainer.TryRemoveItem(itemBlock, amount);
        }

        public int GetTotalWeight()
        {
            return ItemContainer.GetTotalWeight();
        }
    }
}
