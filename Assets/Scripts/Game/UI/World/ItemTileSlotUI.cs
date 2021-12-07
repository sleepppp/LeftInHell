using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    using Project.GameData;
    public class ItemTileSlotUI : ItemSlotBaseUI
    {
        public ItemTileSlot ItemTileSlot { get { return Slot as ItemTileSlot; } }

        public static Vector2 GetItemSize(ItemRecord itemRecord)
        {
            float slotSize = 50f;
            float slotSpacing = 1f;
            Vector2 result = new Vector2()
            {
                x = slotSize * itemRecord.Width + slotSpacing * itemRecord.Width,
                y = slotSize * itemRecord.Height + slotSpacing * itemRecord.Height
            };
            return result;
        }
    }
}
