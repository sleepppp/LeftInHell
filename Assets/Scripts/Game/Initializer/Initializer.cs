using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
namespace Project
{
    using Project.UI;
    using Project.GameData;
    //정상 적인 접속이 아닌 테스트용 접속등으로 초기화를 진행해야 할 경우, 이런 식으로 Initializer 구현해서 처리
    public class Initializer : MonoBehaviour
    {
        public Game Game;
        public Canvas MainCanvas;
        public EventSystem EventSystem;
        public GraphicRaycaster Raycaster;
        public string LoaSceneName;

        private IEnumerator Start()
        {
            DataTableManager.Init();
            DataTableManager.Load();

            yield return new WaitUntil(() => { return DataTableManager.IsCompleteLoad(); });

            if (Game == null)
                Game = Game.Instance;
            if (MainCanvas == null)
                MainCanvas = FindObjectOfType<Canvas>();
            if (EventSystem == null)
                EventSystem = FindObjectOfType<EventSystem>();

            Game.Instance.Init();
            Game.UIManager.MainCanvas = MainCanvas;
            Game.UIManager.Raycaster = Raycaster;
            Game.UIManager.EventSystem = EventSystem;
            DontDestroyOnLoad(Game.Instance);
            DontDestroyOnLoad(Game.UIManager.MainCanvas.gameObject);
            DontDestroyOnLoad(Game.UIManager.EventSystem.gameObject);
            DontDestroyOnLoad(Camera.main.gameObject);
            Destroy(gameObject);
            SceneManager.LoadScene(LoaSceneName);
            Game.Instance.OnLoadWorldScene();
        }
    }
}