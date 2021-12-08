using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class ItemOptionMenuUI : ManagedUIBase
    {
        public enum MenuItemType : int
        {
            OpenDetailInfo = 0,
            OpenSplitPopup = 1,
            SplitHalf = 2,
            Delete = 3
        }

        [SerializeField]Button[] m_buttonList;
        IItem m_targetItem;


        public void Init(IItem item,Vector3 position)
        {
            m_targetItem = item;

            //init button ~ 
            GetButton(MenuItemType.OpenDetailInfo)?.gameObject?.SetActive(true);
            GetButton(MenuItemType.OpenSplitPopup)?.gameObject?.SetActive(item.ItemTypeRecord.IsStackable && item.Amount >= 2);
            GetButton(MenuItemType.SplitHalf)?.gameObject?.SetActive(item.ItemTypeRecord.IsStackable && item.Amount >= 2);
            GetButton(MenuItemType.Delete)?.gameObject.SetActive(true);

            RectTransform.position = position;
            RectTransform.ReviseTransformInRect(Game.UIManager.SafeArea);
        }

        public void OnClickButton(int index)
        {
            switch((MenuItemType)index)
            {
                case MenuItemType.OpenSplitPopup:
                    //todo open split popup ui;
                    break;
                case MenuItemType.SplitHalf:
                    Item item = m_targetItem as Item;
                    int halfAmount = item.Amount / 2;
                    item.RemoveAmount(halfAmount);

                    Game.UIManager.GetUI<DragAndDropSystem>(UIKey.DragAndDropSystem).RequestDrag(item.ItemRecord.ID, halfAmount, null, () => 
                    {
                        item.AddAmount(halfAmount);
                    });
                    break;
                case MenuItemType.Delete:
                    //todo open confirm popup ui
                    m_targetItem.OwnerSlot.Owner.DisarmItem(m_targetItem.Puid);
                    Game.UIManager.GetUI<DragAndDropSystem>(UIKey.DragAndDropSystem).Refresh();
                    break;
            }

            Close();
        }

        Button GetButton(MenuItemType type)
        {
            int index = (int)type;
            if (index < m_buttonList.Length)
                return m_buttonList[index];
            return null;
        }
    }
}