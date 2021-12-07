using System.Collections.Generic;
using System;
namespace Project
{
    public static class CollectionExtension
    {
        public static TValue GetValue<TKey,TValue>(this Dictionary<TKey,TValue> dic, TKey key) where TValue : class
        {
            TValue result = null;
            dic.TryGetValue(key, out result);
            return result;
        }

        public static void AddUnique<TValue>(this List<TValue> list, TValue value)
        {
            if (list.Contains(value) == false)
            {
                list.Add(value);
            }
        }

        public static void Foreach<TKey,TValue>(this Dictionary<TKey,TValue> dic,Action<TValue> query)
        {
            foreach(var item in dic)
            {
                query.Invoke(item.Value);
            }
        }
    }
}
