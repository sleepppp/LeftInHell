using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    //todo BaseSlotUI ¸¸µé±â
    public class ItemTileSlotUI : ItemSlotBaseUI
    {
        public override bool IsPossibleDrop(InventoryItem inventoryItem)
        {
            bool result = ItemAssert.Assert(Slot.Owner.IsPossiblyEquipItem(Slot,inventoryItem),false);
            if(result == false)
            {
                result = ItemAssert.Assert(Slot.InventoryItem.Handle.IsPossiblyAddAmount(inventoryItem.Data.Amount),false);
            }
            return result;
        }

        public override bool TryDrop(InventoryItem inventoryItem)
        {
            bool result = ItemAssert.Assert(Slot.Owner.TryEquipItem(Slot, inventoryItem),false);
            if(result == false)
            {
                result = ItemAssert.Assert(Slot.InventoryItem.Handle.TryAddAmount(inventoryItem.Data.Amount));
            }
            else
            {
                InventoryItemUI.CreateUI((ui) =>
                {
                    ui.Init(inventoryItem, this);
                });
            }

            if(result)
            {
                Game.UIManager.GetUI<InventoryUI>(UIKey.InventoryUI)?.Refresh();
            }

            return result;
        }
    }
}
