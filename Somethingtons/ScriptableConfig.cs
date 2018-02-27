using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class ScriptableConfig<T> : ScriptableObject where T : ScriptableConfig<T>
{
    private static T _config;
    public static T Get()
    {
#if !UNITY_EDITOR
        if (_config == null) 
#else
        if (!Application.isPlaying || _config == null)
#endif
        {
            _config = Resources.Load(AssetName) as T;
            if (_config != null) {
                _config.Initialise();
            }
        }
        return _config;
    }

    protected virtual void Initialise() {
        
    }

    private static string AssetName {
        get {
            System.Reflection.MemberInfo info = typeof(T);
            var attrs = info.GetCustomAttributes(typeof(SaveLocation), false);
            if (attrs.Length > 0) {
                return (attrs[0] as SaveLocation).Name;
            }
            return typeof(T).Name + " Configuration";
        }
    }

    private static string AssetFolder
    {
        get
        {
            System.Reflection.MemberInfo info = typeof(T);
            var attrs = info.GetCustomAttributes(typeof(SaveLocation), false);
            if (attrs.Length > 0)
            {
                var ret = (attrs[0] as SaveLocation).Folder;
                if (ret.Length > 0) {
                    return ret;
                }
            }
            return "Assets/Resources";
        }
    }

#if UNITY_EDITOR
    protected static void create()
    {
        T config = Get();
        if (config == null)
        {
            config = ScriptableObject.CreateInstance<T>();
            CheckAndCreateFolder(AssetFolder);
            AssetDatabase.CreateAsset(config, AssetFolder + "/" + AssetName + ".asset");
            AssetDatabase.SaveAssets();
        }

        Selection.activeObject = config;
        EditorUtility.FocusProjectWindow();
    }

    private static void CheckAndCreateFolder(string folder) {
        var path = folder.Split('/');
        string currentPath = "";
        foreach (string p in path) {
            if (p.Length == 0) {
                continue;
            }
            string newPath = ((currentPath.Length > 0) ? currentPath + "/" : "") + p;
            if (!AssetDatabase.IsValidFolder(newPath))
            {
                AssetDatabase.CreateFolder(currentPath, p);
            }
            currentPath = newPath;
        }
    }
#endif

}

[System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
public class SaveLocation : System.Attribute {
    public string Name {
        get; private set;
    }
    public string Folder {
        get; private set;
    }

    public SaveLocation(string name) {
        Name = name;
    }
    public SaveLocation(string folder, string name)
    {
        Name = name;
        Folder = folder;
    }
}
