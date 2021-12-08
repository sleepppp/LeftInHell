
namespace Project
{
    public partial class Item
    {
        //todo reflected
        public static Item CreateItem(int itemID, int amount)
        {
            return new Item(itemID, amount);
        }
    }
}