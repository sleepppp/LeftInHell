using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Player
    {
        public PlayerInventory Inventory { get; private set; }
        public Player()
        {
            Inventory = new PlayerInventory();
        }
    }
}
