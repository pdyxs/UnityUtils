using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListUtils {
    
	public static void AddUnique<T>(this List<T> list, T item)
	{
		if (!list.Contains(item))
		{
			list.Add(item);
		}
	}

	public static void AddUnique<T>(this List<T> list, List<T> items)
	{
        for (int i = 0; i != items.Count; ++i) {
            list.AddUnique(items[i]);
        }
	}

    public static List<T> Without<T>(this List<T> list, List<T> items) {
        return list.FindAll((item) => !items.Contains(item));
    }

    public delegate U Map<T, U>(T obj);

    public static List<T> UnionMap<T, U>(this List<T> list, List<U> items, 
                                         System.Converter<U,T> map)
    {
        return list.Union(items.ConvertAll(map)).ToList();
    }

    public static void AddRepeat<T>(this List<T> list, T item, int count) {
        for (var i = 0; i != count; ++i) {
            list.Add(item);
        }
    }

    public static List<T> PrependRepeat<T>(this List<T> list, T item, int count)
    {
        List<T> ret = new List<T>();
        ret.AddRepeat(item, count);
        ret.AddRange(list);
        return ret;
    }
}
