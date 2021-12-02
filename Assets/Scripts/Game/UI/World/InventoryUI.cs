using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class InventoryUI : UIBase
    {
        enum MouseState : int
        {
            Error = 0,
            Free = 1
        }

        [Header("Common")]
        [SerializeField] GameObject _slotPrefab;
        [SerializeField] GameObject _blockPrefab;
        [Header("Bag")]
        [SerializeField] ScrollRect _bagScrollRect;

        ItemSlotUI[,] _bagSlotList;
        List<ItemBlockUI> _bagBlockList = new List<ItemBlockUI>();

        MouseState _mouseState;

        public void Init()
        {
            InitBag();
            ChangeMouseState(MouseState.Free);
        }

        void InitBag()
        {
            ItemBag bag = Game.World.Player.Inventory.Bag;
            if (bag == null)
                return;
            // {{ InstantiageSlot ~
            _bagSlotList = new ItemSlotUI[bag.ItemContainer.Height, bag.ItemContainer.Width];

            for(int y = 0; y < bag.ItemContainer.Height; ++y)
            {
                for(int x=0;x < bag.ItemContainer.Width; ++x)
                {
                    GameObject newObject = Instantiate(_slotPrefab, _bagScrollRect.content);
                    _bagSlotList[y, x] = newObject.GetComponent<ItemSlotUI>();
                    _bagSlotList[y, x].EventMouseEnter += OnMouseEnterSlot;
                    _bagSlotList[y, x].EventMouseExit += OnMouseExitSlot;
                    _bagSlotList[y, x].Init(bag.ItemContainer.GetItemSlot(x, y));
                }
            }
            // }} 

            _bagScrollRect.SetLayoutHorizontal();
            _bagScrollRect.SetLayoutVertical();
            Canvas.ForceUpdateCanvases();

            // {{ Instantiage Block ~
            List<ItemBlock> blockList = bag.ItemContainer.BlockList;
            foreach(var block in blockList)
            {
                GameObject newObject = Instantiate(_blockPrefab,_bagScrollRect.content);
                ItemBlockUI blockUI = newObject.GetComponent<ItemBlockUI>();
                ItemSlotUI slotUI = GetBagSlotUI(block.LeftTopSlot.IndexX, block.LeftTopSlot.IndexY);
                blockUI.EventMounseEnter += OnMouseEnterBlockUI;
                blockUI.EventMouseExit += OnMouseExitBlockUI;
                blockUI.Init(block, slotUI);
                _bagBlockList.Add(blockUI);
            }
            // }}
        }

        void ChangeMouseState(MouseState state)
        {
            if (_mouseState == state)
                return;
            _mouseState = state;
        }

        void OnMouseEnterSlot(ItemSlotUI slot)
        {
            switch(_mouseState)
            {
                case MouseState.Free:
                    if(slot.IsEmpty())
                        slot.SetBackgroundColor(new Color(0.7f, 0.7f, 0.7f, slot.OriginalColor.a));
                    break;
            }
        }

        void OnMouseExitSlot(ItemSlotUI slot)
        {
            switch (_mouseState)
            {
                case MouseState.Free:
                    slot.ResetBackgroundColor();
                    break;
            }
        }

        void OnMouseEnterBlockUI(ItemBlockUI block)
        {
            switch (_mouseState)
            {
                case MouseState.Free:
                    block.ShowWhiteDimmed(true);
                    break;
            }
        }

        void OnMouseExitBlockUI(ItemBlockUI block)
        {
            switch (_mouseState)
            {
                case MouseState.Free:
                    block.ShowWhiteDimmed(false);
                    break;
            }
        }

        ItemSlotUI GetBagSlotUI(int indexX, int indexY)
        {
            if (Game.World.Player.Inventory.Bag.ItemContainer.IsOutOfRange(indexX, indexY))
                return null;

            return _bagSlotList[indexY, indexX];
        }

        public void Refresh()
        {
            //foreach(var slot in _bagSlotList)
            //{
            //    slot.Refresh();
            //}

            foreach(var block in _bagBlockList)
            {
                block.Refresh();
                //todo if unValid destroy block
                if(block.ItemBlock.IsValid == false)
                {
                    _bagBlockList.Remove(block);
                    Destroy(block.gameObject);
                }
            }
        }
    }
}
