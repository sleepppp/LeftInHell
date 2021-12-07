using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project
{
    public class AssetManager
    {
        public static void Init()
        {

        }

        public static void LoadAssetAsync<T>(string filePath, Action<T> callback) where T : UnityEngine.Object
        {
            Addressables.LoadAssetAsync<T>(filePath).Completed += (handle) => 
            {
                callback?.Invoke(handle.Result);
            };
        }

        public static void Instantiate<T>(string filePath, Action<T> callback, Transform parent = null) where T : MonoBehaviour
        {
            LoadAssetAsync<GameObject>(filePath, (prefab) => 
            {
                GameObject newObject = null;
                if (parent)
                    newObject = GameObject.Instantiate(prefab, parent);
                else
                    newObject = GameObject.Instantiate(prefab);

                callback?.Invoke(newObject.GetComponent<T>());
            });
        }
    }
}