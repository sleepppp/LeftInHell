using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public class PopupContainerUI : ManagedUIBase
    {
        [SerializeField] ItemTileContainerUI m_containerUI;

        public void Init(ItemTileContainer itemContainer,Vector3 startPosition)
        {
            m_containerUI.Init(itemContainer);
        }
    }
}