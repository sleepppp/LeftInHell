using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public partial class UIManager
    {
        public static void AsyncCreateInventoryUI()
        {
            string path = "Assets/Resource/Prefab/UI/InventoryUI.prefab";
            Game.UIManager.CreateUI<InventoryUI>(path,UIKey.InventoryUI, (ui) => 
            {
                ui.Init();
            });
        }
    }
}