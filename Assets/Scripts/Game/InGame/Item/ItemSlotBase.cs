
namespace Project
{
    public interface IItemSlot
    {
        Puid Puid { get; }
        IItem Item { get; }
        IItemContainer Owner { get; }
        bool IsEmpty { get; }
    }

    public interface IItemSlotHandle
    {
        void SetItem(IItem item);
    }

    public class ItemSlotBase : PObject, IItemSlot, IItemSlotHandle
    {
        readonly IItemContainer m_owner;
        IItem m_item;

        public IItem Item => m_item;
        public IItemContainer Owner => m_owner;
        public bool IsEmpty => m_item == null;

        public ItemSlotBase(IItemContainer owner)
        {
            m_owner = owner;
        }

        public void SetItem(IItem item)
        {
            m_item = item;
        }
    }
}