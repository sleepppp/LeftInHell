using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class ItemTileSlotUI : MonoBehaviour
    {
        public ItemSlotTile Slot { get; private set; }

        public void Init(ItemSlotTile itemSlot)
        {
            Slot = itemSlot;
        }
    }
}
