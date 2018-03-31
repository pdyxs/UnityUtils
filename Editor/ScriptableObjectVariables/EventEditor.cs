// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent), true)]
public class EventEditor : Editor
{
    private static GameObject caller = null;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;
        
        GameEvent e = target as GameEvent;

        caller = (GameObject)EditorGUILayout.ObjectField(caller, typeof(GameObject), true);
        if (caller != null)
        {
            if (GUILayout.Button("Raise Event"))
                e.Raise(caller);
        }
        else
        {
            if (GUILayout.Button("Raise All"))
                e.RaiseAll();
        }
    }
}