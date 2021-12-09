using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    using Project.GameData;

    public class PlayerInventory : PObject, IItemContainer
    {
        readonly ItemTileContainer m_itemTileContainer;
        readonly ItemTileContainer m_testContainer;

        public ItemTileContainer BagContainer { get { return m_itemTileContainer; } }
        public ItemTileContainer TestContainer { get { return m_testContainer; } }
        public List<IItemSlot> Slots => m_itemTileContainer.Slots;
        public List<IItem> Items => m_itemTileContainer.Items;

        public PlayerInventory()
        {
            ConstConfig config = DataTableManager.ConstConfig;
            m_itemTileContainer = new ItemTileContainer(config.StartBagWidth, config.StartBagHeight);
            {
                m_itemTileContainer.AddItem(1, 100);
                m_itemTileContainer.AddItem(5, 100);
            }
            m_testContainer = new ItemTileContainer(6,15);
            {
                m_testContainer.AddItem(1, 33);
            }

        }

        public bool AddItem(int itemID, int amount)
        {
            return m_itemTileContainer.AddItem(itemID, amount);
        }

        public bool CanAddItem(int itemID, int amount)
        {
            return m_itemTileContainer.AddItem(itemID, amount);
        }

        public bool CanEquipItem(Puid slotPUID, int itemID, int amount)
        {
            return m_itemTileContainer.CanEquipItem(slotPUID, itemID, amount);
        }

        public bool DisarmItem(Puid itemPUID)
        {
            return m_itemTileContainer.DisarmItem(itemPUID);
        }

        public bool EquipItem(Puid slotPUID, int itemID, int amount)
        {
            return m_itemTileContainer.EquipItem(slotPUID, itemID, amount);
        }

        public Item GetItem(Puid puid)
        {
            return m_itemTileContainer.GetItem(puid);
        }

        public ItemSlotBase GetSlot(Puid puid)
        {
            return m_itemTileContainer.GetSlot(puid);
        }
    }
}
