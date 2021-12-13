using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    using Project.UI;
    public class ItemBox : Item
    {
        readonly ItemTileContainer ItemContainer;

        public ItemBox(int itemID)
            :base(itemID,1)
        {
            int width = ItemData.GetMatchRecord(itemID, ItemMatchType.ContainerWidth).Value;
            int height = ItemData.GetMatchRecord(itemID, ItemMatchType.ContainerHeight).Value;
            ItemContainer = new ItemTileContainer(width, height);
        }

        public override bool CanUse()
        {
            return true;
        }

        public override bool TryUse()
        {
            if (CanUse() == false)
                return false;

            if(Game.UIManager.GetUI<PopupContainerUI>(UIKey.PopupContainerUI) == false)
                UIManager.AsyncCreatePopupContainerUI(ItemContainer,Vector3.zero);

            return true;
        }
    }
}
