using System.Collections.Generic;

namespace Project
{
    public interface IItemContainer
    {
        ItemExeption IsPossiblyEquipItem(IItemSlot iItemSlot, InventoryItem inventoryItem);
        ItemExeption IsPossiblyDisarmItem(InventoryItem inventoryItem);
        ItemExeption IsPossiblyAddItem(IGetItemData itemData);
        ItemExeption TryEquipItem(IItemSlot iItemSlot, InventoryItem inventoryItem);
        ItemExeption TryDisarmItem(InventoryItem inventoryItem);
        ItemExeption TryAddItem(IGetItemData addData);
        List<IGetItemData> GetAllItemData();
        List<InventoryItem> GetAllItem();

    }

    public interface IInventory : IItemContainer
    {

    }
}
