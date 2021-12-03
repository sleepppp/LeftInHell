using UnityEditor;
using UnityEngine;

namespace Project
{
    using Project.UI;
    using Project.GameData;

    public class Game : MonoBehaviourSingleton<Game>
    {
        public static UIManager UIManager { get; private set; }
        public static World World { get; private set; }

        public void Init()
        {
            if(UIManager == null)
                UIManager = new UIManager();
        }

        public void OnLoadWorldScene()
        {
            if(World == null)
                World = new World();
        }
    }
}