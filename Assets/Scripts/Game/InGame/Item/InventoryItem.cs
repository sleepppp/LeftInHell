
namespace Project
{
    //InventoryBase에는 InventoryItem만 들어 갈 수 있습니다
    public class InventoryItem
    {
        private readonly ItemData m_itemData;
        public IItemSlot OwnerSlot { get; private set; }
        public IGetItemData Data => m_itemData;
        public int Width => m_itemData.Width;
        public int Height => m_itemData.Height;

        public InventoryItem(IItemSlot owner, int itemID, int amount)
        {
            OwnerSlot = owner;
            m_itemData = new ItemData(itemID, amount);
        }

        public void OnBindSlot(IItemSlot slot)
        {
            OwnerSlot = slot;
        }

        public void OnDisarmSlot()
        {
            OwnerSlot = null;
        }

        public ItemExeption AddAmount(int amount)
        {
            return m_itemData.TryAddAmount(amount);
        }
    }
}
