using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class ItemOptionPopupUI : UIBase
    {
        [Header("ItemOptionPopup")]
        [SerializeField] Button m_openDetailButton;
        [SerializeField] Button m_splitButton;
        [SerializeField] Button m_halfSplitButton;

        InventoryItemUI m_targetItemUI;
        public void Init(InventoryItemUI taretUI,Vector2 position)
        {
            m_targetItemUI = taretUI;

            SetSplitButton(m_targetItemUI.Item.Data.IsStackable && m_targetItemUI.Item.Data.Amount > 1);

            RectTransform.position = position;
            RectTransform.ReviseTransformInRect(Game.UIManager.SafeArea);
        }

        public void OnClickDetailButton()
        {
            //todo open item detail popup
            gameObject.SetActive(false);
        }

        public void OnClickSplitButton()
        {
            //todo open input number popup
            gameObject.SetActive(false);
        }

        public void OnClickHalfSplitButton()
        {
            int removeCount = m_targetItemUI.Item.Data.Amount / 2;
            ItemAssert.Assert(m_targetItemUI.Item.Handle.TryRemoveAmount(removeCount));
            m_targetItemUI.Init(m_targetItemUI.Item, m_targetItemUI.SlotUI);

            ItemDragUI.CreateUI((ui) => 
            {
                ui.Init(new InventoryItem(m_targetItemUI.Item.Data.GetItemRecord().ID, removeCount),m_targetItemUI.SlotUI);
            });

            gameObject.SetActive(false);
        }

        void SetSplitButton(bool isActive)
        {
            if(m_splitButton != null)
            {
                m_splitButton.gameObject.SetActive(isActive);
                m_halfSplitButton.gameObject.SetActive(isActive);
            }
        }
    }
}
