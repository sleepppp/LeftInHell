using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{
    public void AsyncOpenInventory()
    {
        Project.UI.UIManager.AsyncCreateInventoryUI();
    }
}
