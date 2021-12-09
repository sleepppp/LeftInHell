
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Project.UI
{
    public class ItemTileContainerUI : UIBase, IItemContainerUI
    {
        const float c_scrollBarWidth = 20f;

        [Header("ItemTileContainerUI")]
        [SerializeField] GameObject m_slotPrefab;
        [SerializeField] ScrollRect m_scrollRect;
        [SerializeField] GridLayoutGroup m_gridLayoutGroup;
        [SerializeField] int m_maxViewVerticalCount = 8;

        ItemTileContainer m_itemTileContainer;

        readonly Dictionary<Puid, ItemTileSlotUI> m_slotUIContainer = new Dictionary<Puid, ItemTileSlotUI>();
        readonly List<InventoryItemUI> m_itemContainer = new List<InventoryItemUI>();

        public IItemContainer ItemContainer => m_itemTileContainer;

        void Awake()
        {
            Game.UIManager.GetUI<DragAndDropSystem>(UIKey.DragAndDropSystem).RegisterContainerUI(this);
        }

        void OnDestroy()
        {
            Game.UIManager.GetUI<DragAndDropSystem>(UIKey.DragAndDropSystem).UnRegisterContainerUI(this);
        }

        public void Init(ItemTileContainer itemContainer)
        {
            float width = itemContainer.Width * 50f + c_scrollBarWidth;
            int heightCount = itemContainer.Height < m_maxViewVerticalCount ? itemContainer.Height : m_maxViewVerticalCount;
            float height = heightCount * 50f;
            RectTransform.sizeDelta = new Vector2(width, height);

            m_itemTileContainer = itemContainer;
            List<IItemSlot> slotList = m_itemTileContainer.Slots;
            foreach(var slot in slotList)
            {
                GameObject newObject = Instantiate(m_slotPrefab, m_scrollRect.content);
                ItemTileSlotUI ui = newObject.GetComponent<ItemTileSlotUI>();
                ui.Init(slot);
                m_slotUIContainer.Add(slot.Puid,ui);
            }

            //force update layout
            m_gridLayoutGroup.SetLayoutHorizontal();
            m_gridLayoutGroup.SetLayoutVertical();
            Canvas.ForceUpdateCanvases();

            Refresh();
        }

        public void Refresh()
        {
            foreach (var item in m_itemContainer)
                item.gameObject.SetActive(false);

            List<IItem> itemList = m_itemTileContainer.Items;
            foreach(var item in itemList)
            {
                InstantiateItemUI((ui) => 
                {
                    ui.Init(item);
                    ui.BindToSlot(GetSlotUI(item.OwnerSlot.Puid));
                });
            }
        }

        void InstantiateItemUI(Action<InventoryItemUI> callback)
        {
            foreach(var item in m_itemContainer)
            {
                if(item.gameObject.activeSelf == false)
                {
                    item.gameObject.SetActive(true);
                    callback?.Invoke(item);
                    return;
                }
            }

            InventoryItemUI.CreateUI((ui)=> 
            {
                m_itemContainer.Add(ui);
                callback?.Invoke(ui);
            });
        }

        ItemTileSlotUI GetSlotUI(Puid slotPuid)
        {
            return m_slotUIContainer.GetValue(slotPuid);
        }
    }
}