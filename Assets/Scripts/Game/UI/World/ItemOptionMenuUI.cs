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
            OpenSplitPopup,
            SplitHalf
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

            RectTransform.position = position;
            RectTransform.ReviseTransformInRect(Game.UIManager.SafeArea);
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