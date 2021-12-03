using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Singleton<T> where T : class,new()
    {
        static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    CreateInstance();
                return instance;
            }
        }

        public static void ReleaseInstance()
        {
            instance = null;
        }

        public static void CreateInstance()
        {
            if (instance == null)
                instance = new T();
        }
    }
}