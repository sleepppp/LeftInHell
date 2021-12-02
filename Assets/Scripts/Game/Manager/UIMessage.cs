using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public static class UIMessage
    {
        public static void OnNotifyItemRemove()
        {
            Game.UIManager.GetUI<InventoryUI>(UIKey.InventoryUI)?.Refresh();
        }
    }
}