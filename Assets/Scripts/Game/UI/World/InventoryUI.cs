using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class InventoryUI : ManagedUIBase
    {
        [SerializeField] ItemContainerUI _bagContainerUI;
        [SerializeField] ItemContainerUI _testBagContainerUI;

        public void Init()
        {
            ItemBlockDragAndDropSystem.CreateInstance();
            InitBag();
        }

        void InitBag()
        {
            _bagContainerUI.Init(Game.World.Player.Inventory.Bag.ItemContainer,10);
            _testBagContainerUI.Init(Game.World.Player.Inventory.TestBag.ItemContainer, 10);
        }

        private void OnDestroy()
        {
            ItemBlockDragAndDropSystem.ReleaseInstance();
        }

        public void Refresh()
        {

        }
    }
}
