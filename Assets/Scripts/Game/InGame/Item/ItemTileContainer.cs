using System.Collections.Generic;
using System;
namespace Project
{
    using Project.GameData;
    public class ItemTileContainer : PObject, IItemContainer
    {
        readonly Dictionary<Puid, Item> m_itemContainer;
        readonly Dictionary<Puid, ItemTileSlot> m_slotContainer;
        readonly ItemTileSlot[,] m_arrSlots;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public List<IItemSlot> Slots
        {
            get
            {
                List<IItemSlot> result = new List<IItemSlot>();
                foreach (var slot in m_slotContainer)
                    result.Add(slot.Value);
                return result;
            }
        }

        public List<IItem> Items
        {
            get
            {
                List<IItem> result = new List<IItem>();
                foreach (var item in m_itemContainer)
                    result.Add(item.Value);
                return result;
            }
        }

        public ItemTileContainer(int width, int height)
        {
            Width = width;
            Height = height;
            m_itemContainer = new Dictionary<Puid, Item>();
            m_slotContainer = new Dictionary<Puid, ItemTileSlot>();
            m_arrSlots = new ItemTileSlot[Height, Width];
            for(int y=0;y < Height; ++y)
            {
                for(int x=0;x < Width; ++x)
                {
                    m_arrSlots[y, x] = new ItemTileSlot(this, x, y);
                    m_slotContainer.Add(m_arrSlots[y,x].Puid,m_arrSlots[y, x]);
                }
            }
        }

        public bool CanAddItem(int itemID, int amount)
        {
            ItemRecord itemRecord = DataTableManager.ItemTable.GetRecord(itemID);
            ItemTypeRecord typeRecord = DataTableManager.ItemTypeTable.GetRecord(itemRecord.Type);
            if(typeRecord.IsStackable)
            {
                int remainAmout = amount;
                m_itemContainer.Foreach((item) => 
                {
                    if(item.ItemRecord.ID == itemRecord.ID && 
                        item.ItemRecord.MaxStackAmount - item.Amount > 0)
                    {
                        int leftAmount = item.ItemRecord.MaxStackAmount - item.Amount;
                        int addAmount = leftAmount > remainAmout ? remainAmout : leftAmount;
                        remainAmout -= addAmount;
                    }
                });
                if (remainAmout <= 0)
                    return true;

                int newItemCount = remainAmout / itemRecord.MaxStackAmount;
                if (remainAmout % itemRecord.MaxStackAmount != 0)
                    newItemCount++;

                return CanPushItem(itemID, newItemCount);
            }
            else
            {
                int newItemCount = amount;
                return CanPushItem(itemID, newItemCount);
            }
        }

        public bool CanEquipItem(Puid slotPUID, int itemID, int amount)
        {
            ItemTileSlot startSlot = m_slotContainer.GetValue(slotPUID);
            if(startSlot == null)
            {
                return false;
            }

            //can merge
            if(startSlot.Item != null && startSlot.Item.ItemRecord.ID == itemID)
            {
               if(startSlot.Item.ItemTypeRecord.IsStackable &&  
                    startSlot.Item.ItemRecord.MaxStackAmount - startSlot.Item.Amount >= amount)
                {
                    return true;
                }
            }

            ItemRecord itemRecord = DataTableManager.ItemTable.GetRecord(itemID);

            if(IsEmptyArea(startSlot.IndexX,startSlot.IndexY, itemRecord.Width, itemRecord.Height) == false)
            {
                return false;
            }

            return true;
        }

        public bool AddItem(int itemID, int amount)
        {
            if (CanAddItem(itemID, amount) == false)
                return false;
            ItemRecord itemRecord = DataTableManager.ItemTable.GetRecord(itemID);
            ItemTypeRecord typeRecord = DataTableManager.ItemTypeTable.GetRecord(itemRecord.Type);
            if(typeRecord.IsStackable)
            {
                int remainAmount = amount;
                m_itemContainer.Foreach((item) => 
                {
                    if (item.ItemRecord.ID == itemRecord.ID && 
                        typeRecord.IsStackable == true &&
                       item.ItemRecord.MaxStackAmount - item.Amount > 0)
                    {
                        int leftAmount = item.ItemRecord.MaxStackAmount - item.Amount;
                        int addAmount = leftAmount > remainAmount ? remainAmount : leftAmount;

                        EquipItem(item.OwnerSlot.Puid, itemID, addAmount);

                        remainAmount -= addAmount;
                    }
                });

                if(remainAmount > 0)
                {
                    int newItemCount = remainAmount / itemRecord.MaxStackAmount;
                    if (remainAmount % itemRecord.MaxStackAmount != 0)
                        newItemCount++;

                    for(int i =0; i < newItemCount; ++i)
                    {
                        int addAmount = itemRecord.MaxStackAmount < remainAmount ? itemRecord.MaxStackAmount : remainAmount;
                        Foreach(0, 0, Width, Height, (slot) => 
                        {
                            if(CanEquipItem(slot.Puid,itemID,addAmount))
                            {
                                EquipItem(slot.Puid, itemID, addAmount);
                                remainAmount -= addAmount;
                                return true;
                            }
                            return false;
                        });
                    }

                    if(remainAmount > 0)
                    {
                        throw new Exception("아이템을 성공적으로 추가하지 못함");
                    }
                }
            }
            else
            {
                int newItemCount = amount;
                int remainCount = newItemCount;
                for (int i = 0; i < newItemCount; ++i)
                {
                    int addAmount = 1;
                    Foreach(0, 0, Width, Height, (slot) =>
                    {
                        if (CanEquipItem(slot.Puid, itemID, addAmount))
                        {
                            EquipItem(slot.Puid, itemID, addAmount);
                            remainCount--;
                            return true;
                        }
                        return false;
                    });
                }

                if(remainCount > 0)
                {
                    throw new Exception("아이템을 성공적으로 추가하지 못함");
                }
            }

            return true;
        }

        public bool DisarmItem(Puid itemPUID)
        {
            Item item = m_itemContainer.GetValue(itemPUID);
            if (item == null)
                return false;

            ItemTileSlot startSlot = item.OwnerSlot as ItemTileSlot;
            Foreach(startSlot.IndexX, startSlot.IndexY, item.ItemRecord.Width, item.ItemRecord.Height, (slot) => 
            {
                slot.SetItem(null);
            });
            item.BindToSlot(null);

            m_itemContainer.Remove(itemPUID);

            return true;
        }

        public bool EquipItem(Puid slotPUID, int itemID, int amount)
        {
            if (CanEquipItem(slotPUID, itemID, amount) == false)
                return false;

            ItemTileSlot slot = m_slotContainer.GetValue(slotPUID);
            if(slot.Item != null)
            {
                if(slot.Item.ItemRecord.ID == itemID &&
                    slot.Item.ItemRecord.MaxStackAmount - slot.Item.Amount >= amount)
                {
                    Item item = GetItem(slot.Item.Puid);
                    item.AddAmount(amount);
                }
            }
            else
            {
                ItemRecord itemRecord = DataTableManager.ItemTable.GetRecord(itemID);
                if(IsEmptyArea(slot.IndexX,slot.IndexY,itemRecord.Width,itemRecord.Height))
                {
                    Item item = new Item(itemID, amount);
                    item.BindToSlot(slot);
                    Foreach(slot.IndexX, slot.IndexY, itemRecord.Width, itemRecord.Height, (slot) => 
                    {
                        slot.SetItem(item);
                    });

                    m_itemContainer.Add(item.Puid, item);
                }
            }

            return true;
        }

        public Item GetItem(Puid puid)
        {
            return m_itemContainer.GetValue(puid);
        }

        public ItemSlotBase GetSlot(Puid puid)
        {
            return m_slotContainer.GetValue(puid);
        }

        void Foreach(int indexX, int indexY, int width, int height,Func<ItemTileSlot,bool> query)
        {
            for (int y = indexY; y <indexY +  height; ++y)
            {
                for (int x = indexX; x <indexX +  width; ++x)
                {
                    ItemTileSlot slot = GetSlot(x, y);
                    if (slot != null)
                    {
                        if (query?.Invoke(slot) == true)
                            return;
                    }
                }
            }
        }

        void Foreach(int indexX, int indexY, int width, int height, Action<ItemTileSlot> query)
        {
            for (int y = indexY; y <indexY +  height; ++y)
            {
                for (int x = indexX; x <indexX +  width; ++x)
                {
                    ItemTileSlot slot = GetSlot(x, y);
                    if (slot != null)
                    {
                        query?.Invoke(slot);
                    }
                }
            }
        }

        bool IsEmptyArea(int indexX, int indexY,int width, int height)
        {
            bool result = true;
            Foreach(indexX, indexY, width, height, (slot) => 
            {
                if(slot.IsEmpty == false)
                {
                    result = false;
                    return true;
                }
                return false;
            });
            return result;
        }

        bool IsOutOfRange(int indexX, int indexY)
        {
            if (indexX < 0 || indexX >= Width) return true;
            if (indexY < 0 || indexY >= Height) return true;
            return false;
        }
        ItemTileSlot GetSlot(int indexX, int indexY)
        {
            if (IsOutOfRange(indexX, indexY))
                return null;
            return m_arrSlots[indexY, indexX];
        }
        bool CanPushItem(int itemID,int itemCount)
        {
            ItemRecord itemRecord = DataTableManager.ItemTable.GetRecord(itemID);
            bool[,] isEmptyList = new bool[Height, Width];
            m_slotContainer.Foreach((slot) => { isEmptyList[slot.IndexY, slot.IndexX] = slot.IsEmpty; });
            int maxIndexX = Width - 1;
            int maxIndexY = Height - 1;
            int remainCount = itemCount;
            bool result = false;
            Foreach(0, 0, Width, Height, (slot) => 
            {
                if(isEmptyList[slot.IndexY,slot.IndexX] == true && 
                    maxIndexX - slot.IndexX >= itemRecord.Width && 
                    maxIndexY - slot.IndexY >= itemRecord.Height)
                {
                    bool isAllEmpty = true;
                    for (int y = slot.IndexY; y < slot.IndexY + itemRecord.Height; ++y)
                    {
                        for(int x= slot.IndexX; x < slot.IndexX + itemRecord.Width; ++x)
                        {
                            if (isEmptyList[y, x] == false)
                            {
                                isAllEmpty = false;
                                break;
                            }
                        }
                        if (isAllEmpty == false)
                            break;
                    }

                    if(isAllEmpty)
                    {
                        remainCount--;
                        if (remainCount <= 0)
                        {
                            result = true;
                            return true;
                        }
                        else
                        {
                            for (int y = slot.IndexY; y < slot.IndexY + itemRecord.Height; ++y)
                            {
                                for (int x = slot.IndexX; x < slot.IndexX + itemRecord.Width; ++x)
                                {
                                    isEmptyList[y, x] = false;
                                }
                            }
                        }
                    }
                }
                return false;
            });

            return result;
        }
    }
}
