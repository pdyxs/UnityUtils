// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject, IList<T>
{
    public List<T> Items = new List<T>();

    public void Add(T thing)
    {
        if (!Items.Contains(thing))
            Items.Add(thing);
    }

    public void Clear()
    {
        Items.Clear();
    }

    public bool Contains(T item)
    {
        return Items.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Items.CopyTo(array, arrayIndex);
    }

    public int Count
    {
        get { return Items.Count;  }
    }
    public bool IsReadOnly { get { return false; } }

    public bool Remove(T thing)
    {
        if (Items.Contains(thing))
            return Items.Remove(thing);
        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return (Items as IEnumerable<T>).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return Items.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        Items.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        Items.RemoveAt(index);
    }

    public T this[int index]
    {
        get { return Items[index]; }
        set { Items[index] = value; }
    }
}