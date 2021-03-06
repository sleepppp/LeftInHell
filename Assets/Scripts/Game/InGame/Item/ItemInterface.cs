
namespace Project
{
    using Project.GameData;

    public interface IItem
    {
        Puid Puid { get; }
        ItemRecord ItemRecord { get; }
        ItemTypeRecord ItemTypeRecord { get; }
        int Amount { get; }
        IItemSlot OwnerSlot { get; }
        bool AddAmount(int amount);
        bool RemoveAmount(int amount);
        bool CanBindToSlot(IItemSlot slot);
        bool BindToSlot(IItemSlot slot);
        bool CanMerge(int itemID, int amount);
        bool TryMerge(int itemID, int amount);
        bool CanUse();
        bool TryUse();
    }

}