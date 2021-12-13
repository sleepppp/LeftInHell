using UnityEngine;
using UnityEngine.EventSystems;
using System;
namespace Project.UI
{
    using GameData;
    public class InventoryItemUI : UIBase, IRefresh ,IBeginDragHandler
    {
        static string s_path = "Assets/Resource/Prefab/UI/InventoryItem.prefab";
        public static void CreateUI(Action<InventoryItemUI> callback)
        {
            AssetManager.Instantiate<InventoryItemUI>(s_path, callback);
        }

        public static void GetPrefab(Action<GameObject> callback)
        {
            AssetManager.LoadAssetAsync<GameObject>(s_path, callback);
        }

        public static Vector2 GetItemSize(ItemRecord itemRecord)
        {
            float slotSize = 50f;
            float slotSpacing = 0f;
            Vector2 result = new Vector2()
            {
                x = slotSize * itemRecord.Width + slotSpacing * itemRecord.Width,
                y = slotSize * itemRecord.Height + slotSpacing * itemRecord.Height
            };
            return result;
        }

        public static Vector2 GetItemSize(int itemID)
        {
            return GetItemSize(DataTableManager.ItemTable.GetRecord(itemID));
        }

        [SerializeField] CommonItemUI m_commonItemUI;
        [SerializeField] MouseHoverChecker m_mouseHoverChecker;
        IItem m_item;

        public IItem Item { get { return m_item; } }

        public void Init(IItem item)
        {
            m_item = item;
            m_commonItemUI.Init(item.ItemRecord.ID, item.Amount);
            m_mouseHoverChecker.OriginColor = m_commonItemUI.OriginColor;
            m_mouseHoverChecker.ChangeColor = new Color(0.8f, 0.8f, 0.8f, m_commonItemUI.OriginBackgroundAlpha);
        }

        public void BindToSlot(ItemSlotBaseUI slotUI)
        {
            if(slotUI is ItemTileSlotUI)
            {
                RectTransform.SetParent(slotUI.transform.parent, true);
                RectTransform.position = slotUI.transform.position;
                RectTransform.sizeDelta = InventoryItemUI.GetItemSize(m_item.ItemRecord);
            }
            else //equipSlot
            {
                RectTransform.SetParent(slotUI.RectTransform, true);
                RectTransform.position = slotUI.RectTransform.position;
                RectTransform.sizeDelta = slotUI.RectTransform.sizeDelta;
            }
        }

        public void OnClick()
        {
            if(Input.GetMouseButtonUp(1))
            {
                UIManager.AsynCreateItemOptionMenuUI(m_item, Input.mousePosition);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                Game.World.Player.Inventory.UseItem(m_item.Puid);
            }
        }

        public void Refresh()
        {
            m_commonItemUI.Init(m_item.ItemRecord.ID, m_item.Amount);
        }

        public void OnEnterDragObject(HandleDrag handle)
        {
            Item item = Item as Item;
            if(item.CanMerge(handle.ItemID,handle.Amount))
            {
                m_commonItemUI.SetBackgroundColor(new Color(0f, 1f, 0f, m_commonItemUI.OriginBackgroundAlpha));
            }
            else
            {
                m_commonItemUI.SetBackgroundColor(new Color(1f, 0f, 0f, m_commonItemUI.OriginBackgroundAlpha));
            }
        }

        public void OnExitDragObject(HandleDrag handle)
        {
            m_commonItemUI.SetBackgroundColor(m_commonItemUI.OriginColor);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Game.UIManager.GetUI<DragAndDropSystem>(UIKey.DragAndDropSystem).RequestDrag(this);
        }
    }
}
