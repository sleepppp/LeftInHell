using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Project
{
    public interface IObjectPool<T> : ICollection<T>
    {
        T Pop();
        void Return(T t);
        void Create(int count);
        T Create();
        bool CanPop(T t);
    }

    public interface IGameObjectPool<T> : IObjectPool<T>
    {
        void SetPrefab(GameObject gameObject);
        void SetParent(Transform transform);
    }

    public class GameObjectPool<T> : IGameObjectPool<T> where T : Component
    {
        readonly List<T> m_objectList;
        GameObject m_prefab;
        Transform m_parentTransform;
        bool m_isCreateAndSleep;
        bool m_isAutoAwake;

        public int Count => m_objectList.Count;
        public bool IsReadOnly => false;
        public bool isCreateAndSleep { get { return m_isCreateAndSleep; } set { m_isCreateAndSleep = value; } }
        public bool IsAutoAwake { get { return m_isAutoAwake; } set { m_isAutoAwake = value; } }

        public GameObjectPool(GameObject prefab)
        {
            SetPrefab(prefab);
            m_objectList = new List<T>();
        }

        public GameObjectPool(GameObject prefab, Transform parent, int count, bool isCreateAndSleep = true,bool isAutoAwake = true)
            :this(prefab)
        {
            SetParent(parent);
            m_isCreateAndSleep = isCreateAndSleep;
            m_isAutoAwake = isAutoAwake;

            Create(count);
        }

        public void Add(T item)
        {
            m_objectList.Add(item);
        }

        public bool CanPop(T t)
        {
            return t.gameObject.activeSelf == false;
        }

        public void Clear()
        {
            m_objectList.Clear();
        }

        public bool Contains(T item)
        {
            return m_objectList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_objectList.CopyTo(array, arrayIndex);
        }

        public void Create(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                Create();
            }
        }

        public T Create()
        {
            GameObject newObject = null;
            if (m_parentTransform == null)
                newObject = GameObject.Instantiate(m_prefab);
            else
                newObject = GameObject.Instantiate(m_prefab, m_parentTransform);

            if (isCreateAndSleep)
                newObject.SetActive(false);

            T result = newObject.GetComponent<T>();

            Add(result);

            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_objectList.GetEnumerator();
        }

        public T Pop()
        {
            foreach(var item in m_objectList)
            {
                if(item.gameObject.activeSelf == false)
                {
                    if(m_isAutoAwake)
                        item.gameObject.SetActive(true);
                    return item;
                }
            }

            T newComponent = Create();
            if (m_isAutoAwake)
                newComponent.gameObject.SetActive(true);
            return newComponent;
        }

        public bool Remove(T item)
        {
            return m_objectList.Remove(item);
        }

        public void SetParent(Transform transform)
        {
            if(m_parentTransform != transform)
            {
                foreach(var item in m_objectList)
                {
                    item.transform.SetParent(transform);
                }
            }

            m_parentTransform = transform;
        }

        public void SetPrefab(GameObject gameObject)
        {
            m_prefab = gameObject;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_objectList.GetEnumerator();
        }

        public void Return(T t)
        {
            if (Contains(t) == false)
                return;

            t.gameObject.SetActive(false);
        }
    }
}