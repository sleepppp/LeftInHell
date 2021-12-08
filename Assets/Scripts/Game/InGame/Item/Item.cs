using Project.GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public partial class Item : PObject,  IItem, IItemHandle ,IClone<Item>
    {
        readonly ItemRecord m_itemRecord;
        readonly ItemTypeRecord m_itemTypeRecord;
        int m_amount;
        IItemSlot m_ownerSlot;

        public override Puid Puid => m_puid;

        public ItemRecord ItemRecord => m_itemRecord;

        public ItemTypeRecord ItemTypeRecord => m_itemTypeRecord;

        public int Amount => m_amount;

        public IItemSlot OwnerSlot { get { return m_ownerSlot; } }

        public Item(int itemID, int amount)
        {
            m_itemRecord = DataTableManager.ItemTable.GetRecord(itemID);
            m_itemTypeRecord = DataTableManager.ItemTypeTable.GetRecord(m_itemRecord.Type);
            m_amount = amount;
        }

        public Item(Item clone)
            :base(clone)
        {
            m_itemRecord = clone.ItemRecord;
            m_itemTypeRecord = clone.ItemTypeRecord;
            m_amount = clone.Amount;
        }

        public bool AddAmount(int amount)
        {
            if(m_amount + amount > ItemRecord.MaxStackAmount)
            {
                return false;
            }

            m_amount += amount;

            return true;
        }

        public bool RemoveAmount(int amount)
        {
            if(m_amount - amount < 1)
            {
                return false;
            }

            m_amount -= amount;

            return true;
        }

        public Item Clone()
        {
            return new Item(this);
        }

        public bool BindToSlot(IItemSlot slot)
        {
            m_ownerSlot = slot;
            return true;
        }

        public virtual bool CanMerge(int itemID, int amount)
        {
            if(itemID == ItemRecord.ID && 
                ItemTypeRecord.IsStackable == true && 
                ItemRecord.MaxStackAmount - Amount >= amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool TryMerge(int itemID, int amount)
        {
            if (CanMerge(itemID, amount) == false)
                return false;

            return AddAmount(amount);
        }
    }
}
