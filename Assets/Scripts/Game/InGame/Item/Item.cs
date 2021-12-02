using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    using Project.GameData;
    public class Item : IClone<Item>
    {
        public static Item CreateItem(ItemRecord item)
        {
            return new Item(item);
        }

        public ItemRecord ItemRecord { get; private set; }
        public ItemTypeRecord ItemTypeRecord { get; private set; }
        public ItemType ItemType { get; private set; }

        public string Name { get { return DataTableManager.Texts.GetRecord(ItemRecord.DescID).Text; } }

        public Item(ItemRecord itemRecord)
        {
            ItemRecord = itemRecord;
            ItemTypeRecord = DataTableManager.ItemTypeTable.GetRecord(ItemRecord.Type);
            ItemType = (ItemType)itemRecord.Type;
        }
        
        public T Cast<T>() where T : Item { return this as T; }

        public Item Clone()
        {
            return new Item(ItemRecord);
        }
    }
}