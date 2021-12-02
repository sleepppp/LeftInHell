using System;
using System.Collections.Generic;
namespace Project
{
    using Project.GameData;
    //(ItemContainer) has (ItemSlot) has (ItemBlock)

    public class ItemBlock : IClone<ItemBlock>
    {
        public Item Item { get; private set; }
        public int Amount { get; private set; }
        public ItemSlot LeftTopSlot { get; private set; }

        public int Width { get { return Item.ItemRecord.Width; } }
        public int Height { get { return Item.ItemRecord.Height; } }

        public int Weight { get { return Item.ItemRecord.Weight * Amount; } }

        public bool IsValid { get { return Item != null && Amount > 0; } }

        public ItemBlock(Item item , int amount)
        {
            Item = item;
            Amount = amount;
        }

        public bool TryAddAmount(int amount)
        {
            if (GetRemainAmount() < amount)
                return false;
            Amount += amount;
            return true;
        }

        public bool TryRemoveAmount(int amount)
        {
            if (Amount < amount)
                return false;
            Amount -= amount;
            return true;
        }

        public bool IsStackable()
        {
            return Item.ItemTypeRecord.IsStackable;
        }

        public int GetRemainAmount()
        {
            if (IsStackable() == false) return 0;
            return Item.ItemRecord.MaxStackAmount - Amount;
        }

        public void SetLeftTopSlot(ItemSlot slot)
        {
            LeftTopSlot = slot;
        }

        public ItemBlock Clone()
        {
            return new ItemBlock(Item.CreateItem(Item?.ItemRecord), Amount);
        }
    }

    public class ItemSlot : IClone<ItemSlot>
    {
        public ItemBlock Block { get; private set; }
        public int IndexX { get; private set; }
        public int IndexY { get; private set; }
        public ItemContainer ItemContainer { get; private set; }

        public ItemSlot(ItemContainer container,int indexX, int indexY)
        {
            ItemContainer = container;
            IndexX = indexX;
            IndexY = indexY;
        }

        public ItemSlot(ItemContainer container,ItemBlock block,int indexX, int indexY)
            :this(container,indexX, indexY)
        {
            Block = block;
        }

        public bool IsEmpty()
        {
            return Block == null;
        }

        public void RegisterBlock(ItemBlock block)
        {
            //if(Block != null)
            //{
            //    throw new Exception("Already has block");
            //}

            Block = block;
        }

        public void UnRegisterBlock()
        {
            Block = null;
        }

        public void SetItemContiner(ItemContainer container)
        {
            ItemContainer = container;
        }

        public ItemSlot Clone()
        {
            return new ItemSlot(ItemContainer,Block?.Clone(), IndexX, IndexY);
        }
    }

    public class ItemContainer : IClone<ItemContainer>
    {
        public ItemSlot[,] ItemSlots { get; private set; }
        public List<ItemBlock> BlockList { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public ItemContainer() 
        {
            BlockList = new List<ItemBlock>();
        }

        public ItemContainer(int width, int height) : this()
        {
            Width = width;
            Height = height;

            ItemSlots = new ItemSlot[Height, Width];
            for(int y =0; y < Height; ++y)
            {
                for(int x=0;x < Width; ++x)
                {
                    ItemSlots[y, x] = new ItemSlot(this,x, y);
                }
            }
        }

        public ItemSlot GetItemSlot(int indexX, int indexY)
        {
            if (IsOutOfRange(indexX, indexY)) return null;
            return ItemSlots[indexY, indexX];
        }

        public int GetTotalWeight()
        {
            int result = 0;
            BlockList.ForEach((block) => 
            {
                result += block.Weight;
            });
            return result;
        }

        public List<ItemBlock> GetItemBlocks(Func<ItemBlock,bool> query)
        {
            List<ItemBlock> result = new List<ItemBlock>();
            foreach(var block in BlockList)
            {
                if (query(block))
                    result.Add(block);
            }
            return result;
        }

        public bool TryRemoveItem(ItemBlock block, int amount)
        {
            if (BlockList.Contains(block) == false)
                return false;

            if (block.TryRemoveAmount(amount) == false)
                return false;

            if(block.Amount == 0) //블록 삭제
            {
                DestroyItemBlock(block);
            }

            return true;
        }

        public bool TryAddItem(int itemRecordID, int amount)
        {
            if (CanAddItem(itemRecordID, amount) == false)
                return false;
            
            ItemRecord itemRecord = DataTableManager.ItemTable.GetRecord(itemRecordID);
            ItemTypeRecord itemTypeRecord = DataTableManager.ItemTypeTable.GetRecord(itemRecord.Type);
            int remainAmount = amount;
            if(itemTypeRecord.IsStackable)
            {
                List<ItemBlock> blocks = GetItemBlocks((block) => 
                {
                    if (block.Item.ItemRecord.ID == itemRecordID)
                        return true;
                    return false;
                });

                foreach(var block in blocks)
                {
                    int addedAmount = 0;
                    if (block.GetRemainAmount() > 0)
                    {
                        addedAmount = remainAmount > block.GetRemainAmount() ? block.GetRemainAmount() : remainAmount;
                        block.TryAddAmount(addedAmount);
                        remainAmount -= addedAmount;
                    }
                }

                if(remainAmount > 0)
                {
                    int newBlockCount = remainAmount / itemRecord.MaxStackAmount;
                    if (remainAmount % itemRecord.MaxStackAmount != 0)
                        newBlockCount++;

                    for(int i =0; i < newBlockCount; ++i)
                    {
                        int newAmount = itemRecord.MaxStackAmount;
                        if (remainAmount < itemRecord.MaxStackAmount)
                            newAmount = remainAmount;
                        ItemBlock block = new ItemBlock(Item.CreateItem(itemRecord),newAmount);
                        ItemSlot slot = FindEmptyAreaAndGetStartSlot(block.Width, block.Height);
                        TryRegiterItemBlock(block, slot.IndexX, slot.IndexY);
                        remainAmount -= newAmount;
                    }

                    if(remainAmount > 0)
                    {
                        throw new Exception("Still left itemAmount");
                    }
                }
            }
            else
            {
                int newBlockCount = amount;
                for(int i =0; i < newBlockCount; ++i)
                {
                    ItemBlock block = new ItemBlock(Item.CreateItem(itemRecord), 1);
                    ItemSlot slot = FindEmptyAreaAndGetStartSlot(block.Width, block.Height);
                    TryRegiterItemBlock(block, slot.IndexX, slot.IndexY);
                }
            }

            return true;
        }

        public bool CanAddItem(int itemRecordID,int amount)
        {
            ItemRecord itemRecord = DataTableManager.ItemTable.GetRecord(itemRecordID);
            ItemTypeRecord itemTypeRecord = DataTableManager.ItemTypeTable.GetRecord(itemRecord.Type);
            if(itemTypeRecord.IsStackable)  //쌓을 수 있는 아이템이면 기존 블록에 쌓기만 하면 되는 수량인지 체크
            {
                List<ItemBlock> blocks = GetItemBlocks((block) => 
                {
                    if (block.Item.ItemRecord.ID == itemRecordID) 
                        return true;
                    return false;
                });
                int remainAmount = amount;
                foreach(var block in blocks)
                {
                    remainAmount -= block.GetRemainAmount();

                    if (remainAmount <= 0)   //기존 블록들에 쌓기만 하면 되는 수량
                    {
                        return true;
                    }
                }
            }

            //쌓기만 해서는 부족할 때 새로운 블록을 몇개 생성해야 하는지 체크
            ItemContainer clone = this.Clone();
            if(itemTypeRecord.IsStackable)  //쌓기 가능할 경우
            {
                //{{ 우선 쌓을 수 있는만큼 쌓는다
                int remainAmount = amount;
                List<ItemBlock> blocks = clone.GetItemBlocks((block) => 
                {
                    if (block.Item.ItemRecord.ID == itemRecordID) return true;
                    return false;
                });

                foreach(var block in blocks)
                {
                    int addedAmount = 0;
                    if (block.GetRemainAmount() > 0)
                    {
                        addedAmount = remainAmount > block.GetRemainAmount() ? block.GetRemainAmount() : remainAmount;
                        block.TryAddAmount(addedAmount);
                        remainAmount -= addedAmount;
                    }
                }
                // }}
                //블록 몇 개 생성해야 하는지 체크
                int newBlockCount = remainAmount / itemRecord.MaxStackAmount;
                if (remainAmount % itemRecord.MaxStackAmount != 0)
                    newBlockCount++;

                for(int i =0; i < newBlockCount; ++i)
                {
                    int newAmount = itemRecord.MaxStackAmount;
                    if (remainAmount < itemRecord.MaxStackAmount)
                        newAmount = remainAmount;
                    ItemBlock newBlock = new ItemBlock(Item.CreateItem(itemRecord), newAmount);
                    ItemSlot startSlot = clone.FindEmptyAreaAndGetStartSlot(newBlock.Width, newBlock.Height);
                    if(startSlot != null)
                    {
                        clone.TryRegiterItemBlock(newBlock, startSlot.IndexX, startSlot.IndexY);
                    }
                    else //빈 공간이 없다는 뜻
                    {
                        return false;
                    }

                    remainAmount -= newAmount;
                }

                if(remainAmount > 0)    //이론 상 여기에 까지 오면 버그, 테스트 필요
                {
                    return false;
                }
            }
            else //쌓기가 불가능한 아이템의 경우 amount만큼 블록을 생성
            {
                int newBlockCount = amount;
                for(int i =0; i < newBlockCount; ++i)
                {
                    ItemBlock newBlock = new ItemBlock(Item.CreateItem(itemRecord), 1);
                    ItemSlot startSlot = clone.FindEmptyAreaAndGetStartSlot(newBlock.Width, newBlock.Height);
                    if(startSlot != null)
                    {
                        clone.TryRegiterItemBlock(newBlock, startSlot.IndexX, startSlot.IndexY);
                    }
                    else
                    {
                        return false;   //빈 공간이 없다는 뜻
                    }
                }
            }

            return true;
        }

        public void TryRegiterItemBlock(ItemBlock block, int startX, int startY)
        {
            if (block == null) return;
            if (IsEmptyArea(startX, startY, block.Width, block.Height) == false) return;
            Foreach(startX, startY, block.Width, block.Height, (slot) => 
            {
                slot.RegisterBlock(block);
            });

            block.SetLeftTopSlot(GetItemSlot(startX, startY));

            BlockList.Add(block);
        }

        public bool IsEmptyArea(int startX, int startY,int width, int height)
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

        public ItemSlot FindEmptyAreaAndGetStartSlot(int width, int height)
        {
            ItemSlot result = null;
            ForeachWithBreak(0, 0, this.Width, this.Height, (slot) => 
            {
                if(IsEmptyArea(slot.IndexX,slot.IndexY,width,height))
                {
                    result = slot;
                    return true;
                }
                return false;
            });
            return result;
        }

        public bool IsOutOfRange(int indexX, int indexY)
        {
            if (indexX < 0 || indexX >= Width) return true;
            if (indexY < 0 || indexY >= Height) return true;
            return false;
        }

        void Foreach(int startX, int startY, int width, int height,Action<ItemSlot> query)
        {
            for(int y= startY; y < startY + height; ++y)
            {
                for(int x= startX; x < startX + width; ++x)
                {
                    ItemSlot slot = GetItemSlot(x, y);
                    if(slot != null)
                        query.Invoke(slot);
                }
            }
        }

        void ForeachWithBreak(int startX, int startY, int width, int height, Func<ItemSlot,bool> query)
        {
            for (int y = startY; y < startY + height; ++y)
            {
                for (int x = startX; x < startX + width; ++x)
                {
                    ItemSlot slot = GetItemSlot(x, y);
                    if(slot != null)
                    {
                        if (query.Invoke(slot))
                            return;
                    }
                }
            }
        }

        void DestroyItemBlock(ItemBlock block)
        {
            BlockList.Remove(block);
            Foreach(block.LeftTopSlot.IndexX, block.LeftTopSlot.IndexY, block.Width, block.Height, (slot) => 
            {
                slot.UnRegisterBlock();
            });
        }

        public ItemContainer Clone()
        {
            ItemContainer clone = new ItemContainer();
            clone.Width = this.Width;
            clone.Height = this.Height;
            clone.ItemSlots = new ItemSlot[this.Height, this.Width];
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    clone.ItemSlots[y, x] = this.ItemSlots[y, x].Clone();
                    clone.ItemSlots[y, x].SetItemContiner(clone);
                }
            }
            
            foreach(var block in BlockList)
            {
                ItemBlock cloneBlock = block.Clone();
                int leftTopIndexX = block.LeftTopSlot.IndexX;
                int leftTopIndexY = block.LeftTopSlot.IndexY;
                cloneBlock.SetLeftTopSlot(clone.ItemSlots[leftTopIndexY, leftTopIndexX]);
                clone.ItemSlots[leftTopIndexY, leftTopIndexX].UnRegisterBlock();
                clone.ItemSlots[leftTopIndexY, leftTopIndexX].RegisterBlock(cloneBlock);

                clone.BlockList.Add(cloneBlock);
            }
            return clone;
        }
    }
}
