using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class InventoryUI : ManagedUIBase
    {
        [SerializeField] ItemTilingContainerUI _bagUI;

        public void Init()
        {
            _bagUI.Init(Game.World.Player.Inventory.Bag);
        }
    }
}
