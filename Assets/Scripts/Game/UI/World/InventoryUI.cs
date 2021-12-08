using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class InventoryUI : ManagedUIBase, IRefresh
    {
        [SerializeField] ItemTileContainerUI m_bagUI;
        [SerializeField] ItemTileContainerUI m_testBagUI;
        public void Init()
        {
            m_bagUI.Init(Game.World.Player.Inventory.BagContainer);
            m_testBagUI.Init(Game.World.Player.Inventory.TestContainer);
        }

        public void Refresh()
        {
            m_bagUI.Refresh();
            m_testBagUI.Refresh();
        }
    }
}
