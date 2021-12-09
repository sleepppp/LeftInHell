using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Project.UI
{
    using Project.GameData;
    public class HandleDrag
    {
        public int ItemID;
        public int Amount;
        public Action SucceededNoti;
        public Action FailedNoti;
        public InventoryItemUI LastEnterItemUI;
    }

    public class DragAndDropSystem : ManagedUIBase, IRefresh
    {
        CommonItemUI m_itemUI;
        HandleDrag m_handleDrag;
        List<IItemContainerUI> m_containerUIList = new List<IItemContainerUI>();

        UpdateManager.Handler m_updateHandle = UpdateManager.Handler.Null;

        public bool IsDragState => GetHandle() != null;

        public void Init()
        {
            m_itemUI = GetComponentInChildren<CommonItemUI>();
            gameObject.SetActive(false);
        }

        void Update()
        {
            HandleDrag handle = GetHandle();
            if (handle == null)
                return;

            if(Input.GetMouseButtonUp(0))
            {
                handle.LastEnterItemUI?.OnExitDragObject(handle);
                InventoryItemUI inventoryItemUI = Game.UIManager.Raycast<InventoryItemUI>(RectTransform.position,true);
                if (inventoryItemUI)
                {
                    Item item = inventoryItemUI.Item as Item;
                    if(item.CanMerge(handle.ItemID,handle.Amount))
                    {
                        item.TryMerge(handle.ItemID, handle.Amount);
                        handle.SucceededNoti?.Invoke();
                    }
                    else
                    {
                        handle.FailedNoti?.Invoke();
                    }
                }
                else
                {
                    ItemSlotBaseUI slotUI = Game.UIManager.Raycast<ItemSlotBaseUI>(RectTransform.position, true);
                    if(slotUI != null && slotUI.Slot.Owner.CanEquipItem(slotUI.Slot.Puid,handle.ItemID,handle.Amount))
                    {
                        slotUI.Slot.Owner.EquipItem(slotUI.Slot.Puid, handle.ItemID, handle.Amount);
                        handle.SucceededNoti?.Invoke();
                    }
                    else
                    {
                        handle.FailedNoti?.Invoke();
                    }
                }
                OnFinishedDrag();
            }
            else
            {
                UpdatePosition();
                handle.LastEnterItemUI?.OnExitDragObject(handle);

                bool isPossibleDrop = false;

                InventoryItemUI inventoryItemUI = Game.UIManager.Raycast<InventoryItemUI>(RectTransform.position, true);
                if(inventoryItemUI)
                {
                    handle.LastEnterItemUI = inventoryItemUI;
                    handle.LastEnterItemUI.OnEnterDragObject(handle);

                    Item item = handle.LastEnterItemUI.Item as Item;
                    isPossibleDrop = item.CanMerge(handle.ItemID, handle.Amount);
                }
                else
                {
                    ItemSlotBaseUI slotUI = Game.UIManager.Raycast<ItemSlotBaseUI>(RectTransform.position, true);
                    if(slotUI != null)
                    {
                        isPossibleDrop = slotUI.Slot.Owner.CanEquipItem(slotUI.Slot.Puid, handle.ItemID, handle.Amount);
                    }
                }

                if (isPossibleDrop)
                    m_itemUI.SetBackgroundColor(new Color(0f, 1f, 0f, m_itemUI.OriginBackgroundAlpha));
                else
                    m_itemUI.SetBackgroundColor(new Color(1f, 0f, 0f, m_itemUI.OriginBackgroundAlpha));
            }

        }

        public void RegisterContainerUI(IItemContainerUI containerUI)
        {
            m_containerUIList.AddUnique(containerUI);
        }

        public void UnRegisterContainerUI(IItemContainerUI containerUI)
        {
            m_containerUIList.Remove(containerUI);
        }

        public void RequestDrag(InventoryItemUI itemUI)
        {
            if (IsDragState)
                return;

            IItemSlot saveSlot = itemUI.Item.OwnerSlot;
            int saveItemID = itemUI.Item.ItemRecord.ID;
            int saveAmount = itemUI.Item.Amount;

            itemUI.gameObject.SetActive(false);
            itemUI.Item.OwnerSlot.Owner.DisarmItem(itemUI.Item.Puid);

            Action succeededNoti = null;
            Action failedNoti = () =>
            {
                saveSlot.Owner.EquipItem(saveSlot.Puid, saveItemID, saveAmount);
            };

            RequestDrag(saveItemID, saveAmount, succeededNoti, failedNoti);
        }

        public void RequestDrag(int itemID, int amount, Action succeededNoti, Action failedNoti)
        {
            if (IsDragState)
                return;
            Refresh();

            CreateHandle(itemID,amount,succeededNoti,failedNoti);

            m_itemUI.Init(itemID, amount);
            gameObject.SetActive(true);
            RectTransform.sizeDelta = InventoryItemUI.GetItemSize(itemID);
            UpdatePosition();
        }

        public void Refresh()
        {
            foreach (var container in m_containerUIList)
                container.Refresh();
        }

        public override void Close()
        {
            base.Close();

            if(m_updateHandle.Equals(UpdateManager.Handler.Null) == false)
            {
                UpdateManager.UnRegister(m_updateHandle);
            }
        }

        HandleDrag GetHandle()
        {
            return m_handleDrag;
        }

        HandleDrag CreateHandle(int itemId, int amount,Action succeededNoti, Action failedNoti)
        {
            m_handleDrag = new HandleDrag()
            {
                ItemID = itemId,
                Amount = amount,
                SucceededNoti = succeededNoti,
                FailedNoti = failedNoti
            };
            m_updateHandle = UpdateManager.Register(ForceUpdateLastSibling());

            return GetHandle();
        }

        void RemoveHandle()
        {
            UpdateManager.UnRegister(m_updateHandle);

            m_handleDrag = null;
            m_updateHandle = UpdateManager.Handler.Null;
        }

        void UpdatePosition()
        {
            RectTransform.position = Input.mousePosition - ((Vector3)new Vector2(RectTransform.sizeDelta.x, -RectTransform.sizeDelta.y) * 0.5f);
            RectTransform.ReviseTransformInRect(Game.UIManager.SafeArea);
        }

        void OnFinishedDrag()
        {
            gameObject.SetActive(false);
            RemoveHandle();
            Refresh();
        }

        IEnumerator ForceUpdateLastSibling()    //always view Front
        {
            float checkTime = 0.5f;
            float currentTime = 0f;

            while(true)
            {
                currentTime = 0f;
                var count = transform.GetSiblingIndex() + 1;
                if (count != transform.parent.childCount)
                {
                    transform.SetAsLastSibling();
                }

                while (currentTime < checkTime)
                {
                    currentTime += Time.unscaledDeltaTime;
                    yield return null;
                }
            }
        }
    }
}
