
namespace Project
{
    //아이템을 보관하는 슬롯 형태는 크게
    //1. 장착슬롯
    //2. 인벤토리 슬롯
    //두 가지가 존재함
    //기능
    //1. 아이템 보관 및 해지
    public interface IItemSlot
    {
        bool IsEmpty();
        ItemExeption IsPossiblyEquipItem(InventoryItem inventoryItem);
        ItemExeption IsPossiblyDisarmItem(InventoryItem inventoryItem);
        ItemExeption TryEquipItem(InventoryItem inventoryItem);
        ItemExeption TryDisarmItem(InventoryItem inventoryItem);
        IItemContainer Owner { get; }
        InventoryItem InventoryItem { get; }
    }
}
