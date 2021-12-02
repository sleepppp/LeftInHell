using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Project
{
    //StartCoroutine �Ҷ����� �ǹ� ���� GC���̴°� �Ⱦ� ���Ǽ��� ���� �����ϴ��� �̷� ������� �ڷ�ƾ�� ������ �����
    public class UpdateManager : MonoBehaviourSingleton<UpdateManager>
    {
        public struct Handler : IEquatable<Handler>
        {
            public static Handler Null = new Handler(0);

            public int ID { get; private set; }

            public Handler(int id)
            {
                ID = id;
            }

            public bool Equals(Handler other) => ID == other.ID;
        }

        private readonly List<IEnumerator> _updateList = new List<IEnumerator>();
        private readonly List<Handler> _handleList = new List<Handler>();

#if UNITY_EDITOR
        [SerializeField]int _updateCount;
#endif

        private void Update()
        {
            for(int i =0; i < _updateList.Count; ++i)
            {
                if(_updateList[i].MoveNext() == false)
                {
                    _updateList.RemoveAt(i);
                    _handleList.RemoveAt(i);
                    i--;
                }
            }
#if UNITY_EDITOR
            _updateCount = _updateList.Count;
#endif
        }

        public static Handler Register(IEnumerator routine)
        {
            Handler handle = new Handler(routine.GetHashCode());
            Instance._updateList.Add(routine);
            Instance._handleList.Add(handle);
            return handle;
        }

        public static void UnRegister(Handler handle)
        {
            for(int i =0; i < Instance._updateList.Count; ++i)
            {
                if(Instance._handleList[i].Equals(handle))
                {
                    Instance._updateList.RemoveAt(i);
                    Instance._handleList.RemoveAt(i);
                    return;
                }
            }
        }

        public static bool IsValid(Handler handle)
        {
            return Instance._handleList.Contains(handle);
        }
    }
}
