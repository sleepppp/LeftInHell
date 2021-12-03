using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.GameData;

namespace Project.UI
{
    public class Test : MonoBehaviour
    {
        public void OpenInventory()
        {
            UIManager.AsyncCreateInventoryUI();
        }
    }
}