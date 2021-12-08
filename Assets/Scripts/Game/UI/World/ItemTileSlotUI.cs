using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    using Project.GameData;
    public class ItemTileSlotUI : ItemSlotBaseUI
    {
        public ItemTileSlot ItemTileSlot { get { return Slot as ItemTileSlot; } }
    }
}
