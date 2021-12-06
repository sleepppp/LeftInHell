using System;
using System.Collections.Generic;

namespace Project
{
    using Project.GameData;
    //������ Ÿ�ϸ� ���·� �����ϴ� ���
    //Ÿ�ϸ� ���Կ��� �� ������ ������� ���� ��û�� �ϴ°�� �ش� ���Ժ��� �κ��丮 �������� ũ�⸸ŭ �˻��ؼ� ���������ϴٸ�
    //�ش�Ǵ� Ÿ�ϸ� ���Ե鿡 �������� �����Ѵ�
    public class ItemTilingContainer : IItemContainer
    {
        private readonly ItemTileSlot[,] m_itemSlots;
        private readonly int m_width;
        private readonly int m_height;
        private readonly IInventory m_ownerInventory;
        private readonly List<InventoryItem> m_items = new List<InventoryItem>();

        public int Width => m_width;
        public int Height => m_height;
        public IInventory Owner => m_ownerInventory;

        public ItemTilingContainer(IInventory inventory,int width, int height)
        {
            m_ownerInventory = inventory;  
            m_width = width;
            m_height = height;
            m_itemSlots = new ItemTileSlot[height, width];
            for(int y =0;y < height; ++y)
            {
                for(int x=0;x < width; ++x)
                {
                    m_itemSlots[y, x] = new ItemTileSlot(this, x, y);
                }
            }
        }

        public List<IGetItemData> GetAllItemData()
        {
            List<IGetItemData> result = new List<IGetItemData>(m_items.Count);
            foreach(var item in m_items)
            {
                result.Add(item.Data);
            }
            return result;
        }

        public List<InventoryItem> GetAllItem()
        {
            return m_items;
        }

        public ItemExeption IsPossiblyEquipItem(IItemSlot iItemSlot, InventoryItem inventoryItem)
        {
            if (iItemSlot.Owner != this)   //���� �����ϴ� ������ �ƴҰ��
                return ItemExeption.Failed_WrongOwner;
            //if (inventoryItem.OwnerSlot != null)
            //    return ItemExeption.Faield_AlreadyHasOwner;
            ItemTileSlot tileSlot = iItemSlot as ItemTileSlot;  //ĳ���� �̽� �������
            if (tileSlot == null)
                return ItemExeption.Failed_Casting;
            ItemExeption result = tileSlot.IsPossiblyEquipItem(inventoryItem); //Ÿ�� ��ü �˻�
            if (result != ItemExeption.Succeeded)
                return result;
            if (IsEmptyArea(tileSlot.IndexX, tileSlot.IndexY, inventoryItem.Width, inventoryItem.Height) == false)  //���� �˻�
                return ItemExeption.Failed_AlreadyEquipItem;

            return ItemExeption.Succeeded;
        }

        //�ڵ����� �������� ������� ã�� �� �� �ִ��� ���θ� üũ�ϴ� ����ε� InventoryItem Ŭ������ �������� �����͸� ������ 
        //üũ�� �� �־� �̷��� IGetItemData�� �ް� ó����(Ex. World���� �������� �������� �õ��� ���)
        public ItemExeption IsPossiblyAddItem(IGetItemData itemData)    
        {
            //�̹� �κ��丮 ���ο� ������ return
            if(m_items.Find((item) => { return item.Data == itemData; }) != null)
            {
                return ItemExeption.Failed_AlreadyEquipItem;
            }

            if (itemData.IsStackable)
            {
                //�켱 ���� ���Կ� ���� �� �ִ¸�ŭ �׾ƺ���
                int remainAmount = itemData.Amount;
                foreach(var item in m_items)
                {
                    if(item.Data.GetItemRecord().ID == itemData.GetItemRecord().ID)
                    {
                        int canAddedAmount = item.Data.MaxAmount - item.Data.Amount;
                        remainAmount -= canAddedAmount;
                        if(remainAmount <= 0 )
                        {
                            return ItemExeption.Succeeded;
                        }
                    }
                }
                //������� �Դٴ� �� ������ ���Ҵٴ� �� ���� ������ ��� �����ּ� �߰� �� �� �ִ��� üũ
                int newItemCount = remainAmount / itemData.MaxAmount;
                if (remainAmount % itemData.MaxAmount != 0)
                    newItemCount++;
                if (IsPossiblePushItem(itemData.Width, itemData.Height, newItemCount) == false)
                    return ItemExeption.Failed;
            }
            else
            {
                if (IsPossiblePushItem(itemData.Width, itemData.Height, 1) == false)
                    return ItemExeption.Failed_ContainerIsFull;
            }

            return ItemExeption.Succeeded;
        }

        public ItemExeption TryAddItem(IGetItemData addData)
        {
            ItemExeption result = IsPossiblyAddItem(addData);
            if (result != ItemExeption.Succeeded)
                return result;
            if(addData.IsStackable)
            {
                int remainAmount = addData.Amount;
                foreach(var item in m_items)
                {
                    if(item.Data.GetItemRecord().ID == addData.GetItemRecord().ID)
                    {
                        int canAddItemCount = item.Data.MaxAmount - item.Data.Amount;
                        int addCount = remainAmount >= canAddItemCount ? canAddItemCount : remainAmount;
                        item.AddAmount(addCount);
                        remainAmount -= addCount;
                        if(remainAmount <= 0)
                        {
                            return ItemExeption.Succeeded;
                        }
                    }
                }

                if(remainAmount > 0)
                {
                    //���� InventoryItem ����
                    int newItemCount = remainAmount / addData.MaxAmount;
                    if (remainAmount % addData.MaxAmount != 0)
                        newItemCount++;
                    for(int i =0; i < newItemCount; ++i)
                    {
                        int addCount = remainAmount >= addData.MaxAmount ? addData.MaxAmount : remainAmount;
                        result = TryPushItem(addData.GetItemRecord().ID, addCount);
                        if (result != ItemExeption.Succeeded)
                            return result;
                        remainAmount -= addCount;
                    }

                    if(remainAmount > 0 )
                    {
                        throw new Exception("??????What the fuck");
                    }
                }
            }
            else
            {
                if(addData.Amount > 1)
                {
                    throw new Exception("���� ���� �ȵǾ����Ƿ� ������ �߰� ��û�� ������ ȣ���ؼ� ó���Ͻÿ�");
                }
                return TryPushItem(addData.GetItemRecord().ID, 1);
            }

            return ItemExeption.Succeeded;
        }

        public ItemExeption IsPossiblyDisarmItem(InventoryItem inventoryItem)
        {
            if (inventoryItem.OwnerSlot == null)
                return ItemExeption.Failed_NullOwner;
            if (inventoryItem.OwnerSlot.Owner != this)
                return ItemExeption.Failed_WrongOwner;

            return inventoryItem.OwnerSlot.IsPossiblyDisarmItem(inventoryItem);
        }

        public ItemExeption TryEquipItem(IItemSlot iItemSlot, InventoryItem inventoryItem)
        {
            ItemExeption result = IsPossiblyEquipItem(iItemSlot, inventoryItem);
            if (result != ItemExeption.Succeeded)
                return result;

            ItemTileSlot startSlot = iItemSlot as ItemTileSlot;

            inventoryItem.OnBindSlot(startSlot);

            Foreach(startSlot.IndexX, startSlot.IndexY, inventoryItem.Width, inventoryItem.Height, (slot) =>
            {
                ItemExeption exeption = slot.TryEquipItem(inventoryItem);
                if (exeption != ItemExeption.Succeeded)
                {
                    throw new Exception("�������� �ش� ���Կ� ������ �� �����ϴ�");//���⼭ ������ ��ġ������
                }
            });

            m_items.Add(inventoryItem);

            return ItemExeption.Succeeded;
        }

        public ItemExeption TryDisarmItem(InventoryItem inventoryItem)
        {
            ItemExeption result = IsPossiblyDisarmItem(inventoryItem);
            if (result != ItemExeption.Succeeded)
                return result;

            ItemTileSlot tileSlot = inventoryItem.OwnerSlot as ItemTileSlot;

            Foreach(tileSlot.IndexX, tileSlot.IndexY, inventoryItem.Width, inventoryItem.Height, (slot) => 
            {
                ItemAssert.Assert(slot.TryDisarmItem(inventoryItem));
            });

            inventoryItem.OnDisarmSlot();

            m_items.Remove(inventoryItem);

            return ItemExeption.Succeeded;
        }

        public bool IsOutofRange(int indexX, int indexY)
        {
            if (indexX < 0 || indexX >= Width) return true;
            if (indexY < 0 || indexY >= Height) return true;
            return false;
        }

        public void Foreach(int startX, int startY, int width, int height, Action<ItemTileSlot> query)
        {
            ItemTileSlot tempSlot = null;
            for (int y = startY; y < startY + height; ++y)
            {
                for (int x = startX; x < startX + width; ++x)
                {
                    tempSlot = GetSlot(x, y);
                    if (tempSlot != null)
                    {
                        query?.Invoke(tempSlot);
                    }
                }
            }
        }


        ItemExeption TryPushItem(int itemID, int amount)
        {
            ItemRecord itemRecord = DataTableManager.ItemTable.GetRecord(itemID);
            ItemTypeRecord itemTypeRecord = DataTableManager.ItemTypeTable.GetRecord(itemRecord.Type);
            ForeachWithBreak(0, 0, Width, Height, (slot) => 
            {
                if(IsEmptyArea(slot.IndexX,slot.IndexY,itemRecord.Width,itemRecord.Height))
                {
                    InventoryItem newItem = new InventoryItem(itemID, amount);
                    TryEquipItem(slot, newItem);
                    return true;
                }
                return false;
            });
            return ItemExeption.Succeeded;
        }

        ItemTileSlot GetSlot(int indexX, int indexY)
        {
            if (IsOutofRange(indexX, indexY))
                return null;
            return m_itemSlots[indexY, indexX];
        }

        bool IsPossiblePushItem(int width, int height, int count)
        {
            // prepare check ~
            int remainCount = count;
            int maxIndexX = this.Width - 1;
            int maxIndexY = this.Height - 1;
            bool[,] arrIsEmpty = new bool[this.Height, this.Width];
            Foreach(0, 0, this.Width, this.Height, (slot) => 
            {
                arrIsEmpty[slot.IndexY,slot.IndexX] = slot.IsEmpty();
            });
            // start check ~
            ForeachWithBreak(0, 0, this.Width, this.Height, (slot) => 
            {
                if(arrIsEmpty[slot.IndexY,slot.IndexX] == true && 
                    maxIndexX - slot.IndexX >= width && 
                    maxIndexY - slot.IndexY >= height)
                {
                    bool isPossible = true;
                    for(int y= slot.IndexY; y < slot.IndexY + height; ++y)
                    {
                        for(int x= slot.IndexX; x < slot.IndexX + width; ++x)
                        {
                            if(arrIsEmpty[y,x] == false)
                            {
                                isPossible = false;
                                break;
                            }
                        }
                        if (isPossible == false)
                            break;
                    }

                    if (isPossible)
                    {
                        remainCount--;
                        if(remainCount <= 0)
                        {
                            return true;
                        }

                        for (int y = slot.IndexY; y < slot.IndexY + height; ++y)
                        {
                            for (int x = slot.IndexX; x < slot.IndexX + width; ++x)
                            {
                                arrIsEmpty[y, x] = false;
                            }
                        }
                    }
                }

                return false;
            });

            return remainCount <= 0; 
        }

        bool IsEmptyArea(int startX, int startY, int width, int height)
        {
            bool result = true;
            ForeachWithBreak(startX, startY, width, height, (slot) => 
            {
                if(slot.IsEmpty() == false)
                {
                    result = false;
                    return true;
                }
                return false;
            });
            return result;
        }

        void ForeachWithBreak(int startX, int startY, int width, int height, Func<ItemTileSlot, bool> query)
        {
            ItemTileSlot tempSlot = null;
            for (int y = startY; y < startY + height; ++y)
            {
                for (int x = startX; x < startX + width; ++x)
                {
                    tempSlot = GetSlot(x, y);
                    if (tempSlot != null)
                    {
                        if (query?.Invoke(tempSlot) == true)
                            return;
                    }
                }
            }
        }
    }
}
