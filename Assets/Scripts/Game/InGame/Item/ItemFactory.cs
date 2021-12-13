
namespace Project
{
    public partial class Item
    {
        //todo reflected
        public static Item CreateItem(int itemID, int amount)
        {
            switch(itemID)
            {
                case 2:
                    return new ItemBox(itemID);
                default:
                    return new Item(itemID, amount);
            }
        }
    }
}