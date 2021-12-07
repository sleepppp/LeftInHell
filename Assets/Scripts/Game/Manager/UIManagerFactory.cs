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
            Game.UIManager.CreateUI<InventoryUI>(path, UIKey.InventoryUI, (ui) =>
            {
                ui.Init();
            });
        }

        public static void AsynCreateItemOptionMenuUI(IItem item,Vector3 position)
        {
            string path = "Assets/Resource/Prefab/UI/ItemOptionMenuUI.prefab";
            Game.UIManager.CreateUI<ItemOptionMenuUI>(path, UIKey.ItemOptionMenuUI, (ui) => 
            {
                ui.Init(item,position);
            });
        }
    }
}