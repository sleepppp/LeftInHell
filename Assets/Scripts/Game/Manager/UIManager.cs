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
    }

    public enum UISortingLayer : int
    {
        DefaultDown = -1,
        Default = 0,
        DefaultUp = 1
    }

    public partial class UIManager
    {
        public Canvas MainCanvas;
        public EventSystem EventSystem;

        readonly Dictionary<UIKey, UIBase> _uiContainer = new Dictionary<UIKey, UIBase>();

        public Rect SafeArea { get { return Screen.safeArea; } }

        public void CreateUI<T>(string path,UIKey uiKey, Action<T> callback) where T : UIBase
        {
            AssetManager.LoadAssetAsync<GameObject>(path, (prefab) => 
            {
                GameObject newObject = GameObject.Instantiate(prefab, MainCanvas.transform);

                UIBase uiBase = newObject.GetComponent<T>();
                T ui = uiBase as T;

                AddUI(uiKey, ui);

                callback?.Invoke(ui);
            });
        }

        public T GetUI<T>(UIKey key) where T : UIBase
        {
            UIBase result = null;
            _uiContainer.TryGetValue(key, out result);
            return result as T;
        }

        void AddUI(UIKey key, UIBase uiBase)
        {
            _uiContainer.Add(key, uiBase);
        }

        void RemoveUI(UIKey key)
        {
            _uiContainer.Remove(key);
        }
    }
}
