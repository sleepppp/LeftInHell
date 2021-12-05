
namespace Project
{
    public class ItemSlotTile : IItemSlot
    {
        private readonly int m_indexX;
        private readonly int m_indexY;
        private readonly IItemContainer m_owner;
        private InventoryItem m_inventoryItem;

        public int IndexX => m_indexX;
        public int IndexY => m_indexY;

        public IItemContainer Owner => m_owner;
        public InventoryItem InventoryItem => m_inventoryItem;

        public ItemSlotTile(IItemContainer owner,int indexX, int indexY)
        {
            m_owner = owner;
            m_indexX = indexX;
            m_indexY = indexY;
        }

        public bool IsEmpty()
        {
            return InventoryItem != null;
        }

        public ItemExeption IsPossiblyDisarmItem(InventoryItem inventoryItem)
        {
            if (InventoryItem != inventoryItem)
                return ItemExeption.Failed_DiffrentItem;
            return ItemExeption.Succeeded;
                
        }

        public ItemExeption IsPossiblyEquipItem(InventoryItem inventoryItem)
        {
            if (IsEmpty())
                return ItemExeption.Succeeded;
            return ItemExeption.Failed_AlreadyEquipItem;
        }

        public ItemExeption TryDisarmItem(InventoryItem inventoryItem)
        {
            ItemExeption result = IsPossiblyDisarmItem(inventoryItem);
            if (result != ItemExeption.Succeeded)
                return result;

            m_inventoryItem = null;

            return ItemExeption.Succeeded;
        }

        public ItemExeption TryEquipItem(InventoryItem inventoryItem)
        {
            ItemExeption result = IsPossiblyEquipItem(inventoryItem);
            if (result != ItemExeption.Succeeded)
                return result;
            m_inventoryItem = inventoryItem;

            return ItemExeption.Succeeded;
        }
    }
}
