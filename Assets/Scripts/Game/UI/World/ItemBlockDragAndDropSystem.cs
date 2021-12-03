using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Project.UI
{
    public enum MouseState : int
    {
        Free = 0,
        Drag = 1
    }
    public class ItemBlockDragAndDropSystem : Singleton<ItemBlockDragAndDropSystem>
    {
        MouseState _mouseState;
        Vector2 _offset;
        ItemBlockUI _currentDragUI;
        readonly List<ItemContainerUI> _containerList = new List<ItemContainerUI>();

        ItemContainer _prevItemContainer;
        public static MouseState MouseState { get { return Instance._mouseState; } }

        public static void RegisterItemContainerUI(ItemContainerUI containerUI)
        {
            Instance._containerList.AddUnique(containerUI);
        }

        public static void UnRegisterItemContainerUI(ItemContainerUI containerUI)
        {
            Instance._containerList.Remove(containerUI);
        }

        public static bool StartDrag(ItemBlockUI blockUI, PointerEventData eventData)
        {
            if (Instance._mouseState != MouseState.Free)
                return false;

            ChangeMouseState(MouseState.Drag);
            Instance._currentDragUI = blockUI;
            Instance._offset = eventData.position - (Vector2)Instance._currentDragUI.transform.position;
            Instance._currentDragUI.transform.SetParent(Game.UIManager.MainCanvas.transform, true);
            Instance._prevItemContainer = Instance._currentDragUI.ItemBlock.LeftTopSlot.ItemContainer;
            Instance._prevItemContainer.UnRegisterBlock(blockUI.ItemBlock);
            TryCheckDropPossible();
            return true;
        }

        public static void Drag(PointerEventData eventData)
        {
            if (Instance._mouseState != MouseState.Drag)
                return;

            ItemSlotUI targetSlot = GetDropSlotUI(eventData.position);
            if(targetSlot != null)
            {
                Instance._currentDragUI.transform.position = targetSlot.transform.position;
            }
            else
            {
                Instance._currentDragUI.transform.position = eventData.position - Instance._offset;
            }

            TryCheckDropPossible();
        }

        public static void EndDrag(PointerEventData eventData)
        {
            if (Instance._mouseState != MouseState.Drag)
                return;

            Instance._currentDragUI.ResetBackgroundColor();
            if (IsDropPossible())
            {
                ItemBlock targetBlock = Instance._currentDragUI.ItemBlock;
                Instance._prevItemContainer.UnRegisterBlock(targetBlock);

                ItemSlotUI targetSlotUI = GetDropSlotUI(Instance._currentDragUI.RectTransform.position);
                ItemSlot targetSlot = targetSlotUI.ItemSlot;
                ItemContainer newContainer = targetSlot.ItemContainer;
                newContainer.TryRegiterItemBlock(targetBlock, targetSlot.IndexX, targetSlot.IndexY);

                Instance._currentDragUI.Init(targetBlock, targetSlotUI);

                Instance._currentDragUI.SetBackgroundColor(new Color(0.7f, 0.7f, 0.7f, Instance._currentDragUI.OriginBackgroundColor.a));
            }
            else
            {
                ItemSlot prevItemSlot = Instance._currentDragUI.StartSlotUI.ItemSlot;
                Instance._prevItemContainer.TryRegiterItemBlock(Instance._currentDragUI.ItemBlock, prevItemSlot.IndexX, prevItemSlot.IndexY);
                Instance._currentDragUI.ResetTransform();
            }
            Instance._currentDragUI = null;
            ChangeMouseState(MouseState.Free);
        }

        public static bool IsDragObject(ItemBlockUI blockUI)
        {
            return Instance._currentDragUI == blockUI;
        }

        static void TryCheckDropPossible()
        {
            Color color;
            if(IsDropPossible())
            {
                color = new Color(Color.green.r, Color.green.g, Color.green.b, Instance._currentDragUI.OriginBackgroundColor.a);
            }
            else
            {
                color = new Color(Color.red.r, Color.red.g, Color.red.b, Instance._currentDragUI.OriginBackgroundColor.a);
            }
            Instance._currentDragUI.SetBackgroundColor(color);
        }

        static bool IsDropPossible()
        {
            ItemSlotUI targetSlotUI = GetDropSlotUI(Instance._currentDragUI.RectTransform.position);
            if (targetSlotUI != null)
            {
                ItemSlot targetSlot = targetSlotUI.ItemSlot;
                ItemContainer targetContainer = targetSlot.ItemContainer;
                ItemBlock targetBlock = Instance._currentDragUI.ItemBlock;
                if (targetContainer.IsEmptyAreaAndInBound(targetSlot.IndexX, targetSlot.IndexY, targetBlock.Width,targetBlock.Height))
                {
                    return true;
                }
            }
            return false;
        }
        static ItemSlotUI GetDropSlotUI(Vector2 position)
        {
            Vector2 leftTopPosition = position;
            ItemSlotUI slotUI = Game.UIManager.Raycast<ItemSlotUI>(leftTopPosition,false);
            if (slotUI != null)
            {
                return slotUI;
            }
            return null;
        }


        static void ChangeMouseState(MouseState state)
        {
            Instance._mouseState = state;
        }
    }
}