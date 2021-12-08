using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
namespace Project.UI
{
    public enum UIKey : int
    {
        InventoryUI = 1,
        ItemOptionMenuUI,
        DragAndDropSystem,
    }

    public partial class UIManager
    {
        public Canvas MainCanvas;
        public EventSystem EventSystem;
        public GraphicRaycaster Raycaster;

        readonly Dictionary<UIKey, ManagedUIBase> _uiContainer = new Dictionary<UIKey, ManagedUIBase>();

        public Rect SafeArea { get { return Screen.safeArea; } }

        public void CreateUI<T>(string path,UIKey uiKey, Action<T> callback) where T : ManagedUIBase
        {
            AssetManager.LoadAssetAsync<GameObject>(path, (prefab) => 
            {
                GameObject newObject = GameObject.Instantiate(prefab, MainCanvas.transform);

                ManagedUIBase uiBase = newObject.GetComponent<T>();
                T ui = uiBase as T;
                ui.UIKey = uiKey;
                AddUI(uiKey, ui);

                callback?.Invoke(ui);
            });
        }

        public void RemoveUI(UIKey uiKey)
        {
            ManagedUIBase ui = _uiContainer.GetValue(uiKey);
            if(ui != null)
            {
                _uiContainer.Remove(uiKey);
                GameObject.Destroy(ui.gameObject);
            }
        }

        public T GetUI<T>(UIKey key) where T : ManagedUIBase
        {
            ManagedUIBase result = null;
            _uiContainer.TryGetValue(key, out result);
            return result as T;
        }

        public T Raycast<T>(Vector2 screenPoint) where T :UIBase
        {
            T output = null;

            List<RaycastResult> resultList = RaycastGetList(screenPoint);

            foreach (var result in resultList)
            {
                output = result.gameObject?.GetComponent<T>();
                if (output != null)
                {
                    return output;
                }
            }

            return null;
        }

        public List<RaycastResult> RaycastGetList(Vector2 screenPoint)
        {
            //todo GC Optimaze
            PointerEventData eventData = new PointerEventData(EventSystem);
            eventData.position = screenPoint;
            List<RaycastResult> resultList = new List<RaycastResult>();
            Raycaster.Raycast(eventData, resultList);
            return resultList;
        }

        void AddUI(UIKey key, ManagedUIBase uiBase)
        {
            _uiContainer.Add(key, uiBase);
        }

    }
}
