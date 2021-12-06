using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    using Project.GameData;

    //임시
    struct CreateData : IGetItemData
    {
        public ItemRecord ItemRecord;
        public ItemTypeRecord ItemTypeRecord;
        public int ItemCount;

        public int Amount => ItemCount;

        public int MaxAmount => ItemRecord.MaxStackAmount;

        public int Width => ItemRecord.Width;

        public int Height => ItemRecord.Height;

        public bool IsStackable => ItemTypeRecord.IsStackable;

        public bool IsConsumeable => ItemTypeRecord.IsConsumeable;

        public ItemRecord GetItemRecord()
        {
            return ItemRecord;
        }

        public ItemTypeRecord GetItemTypeRecord()
        {
            return ItemTypeRecord;
        }
    }

    public sealed class PlayerInventory : IInventory
    {
        readonly ItemTilingContainer m_bag;

        public ItemTilingContainer Bag { get { return m_bag; } }

        public PlayerInventory()
        {
            ConstConfig config = DataTableManager.ConstConfig;
            m_bag = new ItemTilingContainer(this, config.StartBagWidth, config.StartBagHeight);

            //test int
            for (int i = 0; i < 3; ++i)
            {
                CreateData data = new CreateData();
                data.ItemCount = 3;
                data.ItemRecord = DataTableManager.ItemTable.GetRecord(1);
                data.ItemTypeRecord = DataTableManager.ItemTypeTable.GetRecord(data.ItemRecord.Type);

                ItemAssert.Assert(m_bag.TryAddItem(data));
            }

            for(int i = 0; i< 5; ++i)
            {
                CreateData data = new CreateData();
                data.ItemCount = 5;
                data.ItemRecord = DataTableManager.ItemTable.GetRecord(5);
                data.ItemTypeRecord = DataTableManager.ItemTypeTable.GetRecord(data.ItemRecord.Type);

                ItemAssert.Assert(m_bag.TryAddItem(data));
            }
        }

        //지금은 Bag 하나만 있어서 m_bag에게만 요청하지만 후에 PlayerInventory에 여러 엘리먼트(장착 슬롯 등등)이 추가 되면 추가 작업 필요
        public List<IGetItemData> GetAllItemData()
        {
            return m_bag.GetAllItemData();
        }

        public List<InventoryItem> GetAllItem()
        {
            return m_bag.GetAllItem();
        }

        public ItemExeption IsPossiblyAddItem(IGetItemData itemData)
        {
            return m_bag.IsPossiblyAddItem(itemData);
        }

        public ItemExeption IsPossiblyDisarmItem(InventoryItem inventoryItem)
        {
            return m_bag.IsPossiblyDisarmItem(inventoryItem);
        }

        public ItemExeption IsPossiblyEquipItem(IItemSlot iItemSlot, InventoryItem inventoryItem)
        {
            return m_bag.IsPossiblyEquipItem(iItemSlot, inventoryItem);
        }

        public ItemExeption TryAddItem(IGetItemData addData)
        {
            return m_bag.TryAddItem(addData);
        }

        public ItemExeption TryDisarmItem(InventoryItem inventoryItem)
        {
            return m_bag.TryDisarmItem(inventoryItem);
        }

        public ItemExeption TryEquipItem(IItemSlot iItemSlot, InventoryItem inventoryItem)
        {
            return m_bag.TryEquipItem(iItemSlot, inventoryItem);
        }
    }
}
