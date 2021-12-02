using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public static class WorldMessage
    {
        public static bool ProcessRemoveItem(ItemBlock itemBlock,int amount)
        {
            return Game.World.Player.Inventory.TryRemoveItem(itemBlock, amount);
        }
    }
}
