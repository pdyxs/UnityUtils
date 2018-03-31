// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
//TODO release gameobject references on destroy
[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly Dictionary<GameObject, List<GameEventListener>> eventListeners = 
        new Dictionary<GameObject, List<GameEventListener>>();

    public void RaiseAll()
    {
        foreach (var obj in eventListeners.Keys)
        {
            Raise(obj);
        }
    }
    
    public void Raise(MonoBehaviour caller)
    {
        Raise(caller.gameObject);
    }

    public void Raise(GameObject caller)
    {
        if (eventListeners.ContainsKey(caller))
        {
            for (int i = eventListeners[caller].Count - 1; i >= 0; i--)
            {
                eventListeners[caller][i].OnEventRaised();
            }
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!eventListeners.ContainsKey(listener.callFilter))
        {
            eventListeners.Add(listener.callFilter, new List<GameEventListener>());
        }
        if (!eventListeners[listener.callFilter].Contains(listener))
            eventListeners[listener.callFilter].Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (eventListeners.ContainsKey(listener.callFilter))
        {
            if (eventListeners[listener.callFilter].Contains(listener))
                eventListeners[listener.callFilter].Remove(listener);            
        }
        
    }
}