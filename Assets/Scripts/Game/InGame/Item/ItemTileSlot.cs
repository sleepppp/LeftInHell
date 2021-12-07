
namespace Project
{
    public class ItemTileSlot : ItemSlotBase
    {
        public int IndexX { get; private set; }
        public int IndexY { get; private set; }

        public ItemTileSlot(IItemContainer owner,int indexX,int indexY)
            :base(owner)
        {
            IndexX = indexX;
            IndexY = indexY;
        }
    }
}
