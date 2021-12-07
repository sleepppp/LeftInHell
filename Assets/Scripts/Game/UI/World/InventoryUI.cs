using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class InventoryUI : ManagedUIBase, IRefresh
    {
        [SerializeField] ItemTileContainerUI m_bagUI;
        public void Init()
        {
            m_bagUI.Init(Game.World.Player.Inventory.BagContainer);
        }

        public void Refresh()
        {
            m_bagUI.Refresh();
        }
    }
}
