using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class ItemSlotUI : UIBase
    {
        public const float CellSize = 50f;
        public const float Spacing = 0f;

        public ItemSlot ItemSlot { get; private set; }

        public void Init(ItemSlot itemSlot)
        {
            ItemSlot = itemSlot;
        }
    }
}