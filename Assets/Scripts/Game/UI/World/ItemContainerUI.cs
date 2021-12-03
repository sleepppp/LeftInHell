using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Project.UI
{
    public class ItemContainerUI : UIBase
    {
        public const float ScrollbarWidth = 20f;

        [Header("ItemContainerUI")]
        [SerializeField] int _horizontalViewCount;
        [SerializeField] ScrollRect _scrollRect;
        [SerializeField] GridLayoutGroup _gridLayoutGroup;
        [SerializeField] GameObject _slotPrefab;
        [SerializeField] GameObject _itemBlockPrefab;
        ItemContainer _itemContainer;

        ItemSlotUI[,] _slotUIList;
        List<ItemBlockUI> _blockUIList = new List<ItemBlockUI>();

        private void OnDestroy()
        {
            ItemBlockDragAndDropSystem.UnRegisterItemContainerUI(this);
        }

        public void Init(ItemContainer itemContainer, int horizontalViewCount = 0)
        {
            _itemContainer = itemContainer;

            if (horizontalViewCount != 0)
                _horizontalViewCount = horizontalViewCount;

            SetUISize();
            InitSlotUI();
            ForceUpdateLayout();
            InitItemBlockUI();
            ItemBlockDragAndDropSystem.RegisterItemContainerUI(this);
        }

        void SetUISize()
        {
            _gridLayoutGroup.cellSize = new Vector2(ItemSlotUI.CellSize, ItemSlotUI.CellSize);
            _gridLayoutGroup.spacing = new Vector2(ItemSlotUI.Spacing, ItemSlotUI.Spacing);

            int widthCount = _itemContainer.Width;
            int heightCount = _itemContainer.Height;

            float windowSizeX = widthCount * ItemSlotUI.CellSize + (widthCount - 1) * ItemSlotUI.Spacing + ScrollbarWidth;
            float windowSizeY = _horizontalViewCount * ItemSlotUI.CellSize + (_horizontalViewCount - 1) * ItemSlotUI.Spacing;

            RectTransform.sizeDelta = new Vector2(windowSizeX, windowSizeY);
        }

        void InitSlotUI()
        {
            _slotUIList = new ItemSlotUI[_itemContainer.Height, _itemContainer.Width];
           _itemContainer.ForeachAllSlot((slot) => 
            {
                GameObject newObject = Instantiate(_slotPrefab, _scrollRect.content);
                ItemSlotUI slotUI = newObject.GetComponent<ItemSlotUI>();
                slotUI.Init(slot);

                _slotUIList[slot.IndexY, slot.IndexX] = slotUI;
            });
        }

        void InitItemBlockUI()
        {
            List<ItemBlock> blockList = _itemContainer.BlockList;
            foreach(var itemBlock in blockList)
            {
                ItemBlockUI blockUI = InstantiateItemBlockUI();
                ItemSlotUI startSlotUI = _slotUIList[itemBlock.LeftTopSlot.IndexY, itemBlock.LeftTopSlot.IndexX];
                blockUI.Init(itemBlock, startSlotUI);
                blockUI.EventPointerEnter += OnEventPointerEnterBlock;
                blockUI.EventPoinetExit += OnEventPointerExitBlock;
                blockUI.EventOnClick += OnClickBlock;
                blockUI.EventBeginDrag += OnEventBeginDrag;
                blockUI.EventDrag += OnEventDrag;
                blockUI.EventEndDrag += OnEventEndDrag;
            }
        }

        void ForceUpdateLayout()
        {
            _gridLayoutGroup.SetLayoutHorizontal();
            _gridLayoutGroup.SetLayoutVertical();
            Canvas.ForceUpdateCanvases();
        }

        ItemBlockUI InstantiateItemBlockUI()
        {
            foreach(var block in _blockUIList)
            {
                if(block.gameObject.activeSelf == false)
                {
                    return block;
                }
            }

            GameObject newObject = Instantiate(_itemBlockPrefab, _scrollRect.content);
            ItemBlockUI blockUI = newObject.GetComponent<ItemBlockUI>();
            _blockUIList.Add(blockUI);
            return blockUI;
        }

        void OnEventPointerEnterBlock(ItemBlockUI blockUI)
        {
            if(ItemBlockDragAndDropSystem.MouseState == MouseState.Free && 
                ItemBlockDragAndDropSystem.IsDragObject(blockUI) == false)
            {
                blockUI.SetBackgroundColor(new Color(0.7f, 0.7f, 0.7f, blockUI.OriginBackgroundColor.a));
            }
        }

        void OnEventPointerExitBlock(ItemBlockUI blockUI)
        {
            if (ItemBlockDragAndDropSystem.IsDragObject(blockUI) == false)
            {
                blockUI.ResetBackgroundColor();
            }
        }

        void OnClickBlock(ItemBlockUI blockUI)
        {
            if(ItemBlockDragAndDropSystem.MouseState == MouseState.Free)
            {
                Item item = blockUI.ItemBlock.Item;
                if(item.ItemType == ItemType.ItemBox)
                {
                    //todo open ItemBoxContainerUI
                }
                else
                {
                    //todo open ItemInformationUI
                }
            }
        }

        void OnEventBeginDrag(ItemBlockUI blockUI, PointerEventData eventData)
        {
            ItemBlockDragAndDropSystem.StartDrag(blockUI, eventData);
        }

        void OnEventDrag(ItemBlockUI blockUI, PointerEventData eventData)
        {
            ItemBlockDragAndDropSystem.Drag(eventData);
        }
        void OnEventEndDrag(ItemBlockUI blockUI, PointerEventData eventData)
        {
            ItemBlockDragAndDropSystem.EndDrag(eventData);
        }
    }
}