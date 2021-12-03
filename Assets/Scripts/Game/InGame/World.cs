using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    using Project.UI;
    public partial class World
    {
        public Player Player { get; private set; }

        public World()
        {
            Player = new Player();
        }
    }
}
