using System.Collections.Generic;
namespace Project
{
    public interface IItemContainer
    {
        Puid Puid { get; }
        List<IItemSlot> Slots { get; }
        List<IItem> Items { get; }
        ItemSlotBase GetSlot(Puid puid);
        Item GetItem(Puid puid);
        bool CanAddItem(int itemID, int amount);
        bool CanEquipItem(Puid slotPUID, int itemID, int amount);
        bool AddItem(int itemID, int amount);
        bool EquipItem(Puid slotPUID, int itemID, int amount);
        bool DisarmItem(Puid itemPUID);
    }
}