using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class InventoryUI : ManagedUIBase
    {
        [SerializeField] ItemTilingContainerUI _bagUI;
        [SerializeField] ItemOptionPopupUI _optionPopupUI;
        public void Init()
        {
            _bagUI.Init(Game.World.Player.Inventory.Bag);
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(1))
            {
                InventoryItemUI pickUI = Game.UIManager.Raycast<InventoryItemUI>(Input.mousePosition);
                if(pickUI != null)
                {
                    OpenItemOptionPopup(pickUI, Input.mousePosition);
                }
            }
        }

        void OpenItemOptionPopup(InventoryItemUI targetItemUI,Vector2 position)
        {
            _optionPopupUI.gameObject.SetActive(true);
            _optionPopupUI.Init(targetItemUI, position);
        }

        public void Refresh()
        {
            _bagUI.Refresh();
            _optionPopupUI.gameObject.SetActive(false);
        }
    }
}
