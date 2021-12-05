

namespace Project
{
    using Project.GameData;
    //실질적인 아이템 관련 데이터는 해당 클래스를 통해 처리
    public sealed class ItemData : IGetItemData, IHandleItemData, IClone<ItemData>
    {
        private readonly ItemRecord m_itemRecord;
        private readonly ItemTypeRecord m_itemTypeRecord;
        private int m_amount;

        public int Amount => m_amount;
        public int MaxAmount => m_itemRecord.MaxStackAmount;
        public int Width => m_itemRecord.Width;
        public int Height => m_itemRecord.Height;
        public bool IsStackable => m_itemTypeRecord.IsStackable;
        public bool IsConsumeable => m_itemTypeRecord.IsConsumeable;

        public ItemData(int itemID, int amount)
        {
            m_itemRecord = DataTableManager.ItemTable.GetRecord(itemID);
            m_itemTypeRecord = DataTableManager.ItemTypeTable.GetRecord(m_itemRecord.Type);
            m_amount = amount;
            if(m_amount > m_itemRecord.MaxStackAmount)
            {
                throw new System.Exception("amount is out of maxAmount");
            }
        }

        public ItemRecord GetItemRecord()
        {
            return m_itemRecord;
        }

        public ItemTypeRecord GetItemTypeRecord()
        {
            return m_itemTypeRecord;
        }
        public ItemExeption IsPossiblyAddAmount(int amount)
        {
            if (m_amount + amount > m_itemRecord.MaxStackAmount)
                return ItemExeption.Failed_OverMaxAmount;

            return ItemExeption.Succeeded;
        }

        public ItemExeption IsPossiblyRemoveAmount(int amount)
        {
            if (m_amount - amount < 0)
                return ItemExeption.Failed_OverMinAmount;
            else
                return ItemExeption.Succeeded;
        }

        public ItemExeption TryAddAmount(int amount)
        {
            ItemExeption result = IsPossiblyAddAmount(amount);
            if (result != ItemExeption.Succeeded)
                return result;

            m_amount += amount;
            return ItemExeption.Succeeded;
        }

        public ItemExeption TryRemoveAmount(int amount)
        {
            ItemExeption result = IsPossiblyRemoveAmount(amount);
            if (result != ItemExeption.Succeeded)
                return result;

            m_amount -= amount;
            return ItemExeption.Succeeded;
        }

        public ItemData Clone()
        {
            return new ItemData(this.m_itemRecord.ID, this.m_amount);
        }
    }
}