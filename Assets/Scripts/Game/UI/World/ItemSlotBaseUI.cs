using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class ItemSlotBaseUI : UIBase
    {
        public IItemSlot Slot { get; private set; }

        public void Init(IItemSlot slot)
        {
            Slot = slot;
        }
    }
}