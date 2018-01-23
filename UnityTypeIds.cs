// Cratesmith 2017
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SceneId
#if UNITY_EDITOR
: ISerializationCallbackReceiver
#endif
{
    public string path { get { return scenePath; } }
    public string name { get { return sceneName; } }

    [SerializeField] Object editorSceneObject;
    [SerializeField] string scenePath;
    [SerializeField] string sceneName;

    public override string ToString()
    {
        return path;
    }

    public static implicit operator string(SceneId source)
    {
        return source.ToString();
    }

#if UNITY_EDITOR
    public void OnBeforeSerialize()
    {
        if (editorSceneObject != null)
        {
            var assetPath = AssetDatabase.GetAssetPath(editorSceneObject).Substring("Assets/".Length);
            scenePath = Path.GetDirectoryName(assetPath) + "/" + Path.GetFileNameWithoutExtension(assetPath);
            sceneName = editorSceneObject.name;
        }
        else
        {
            scenePath = sceneName = "";
        }
    }

    public void OnAfterDeserialize()
    {
    }

    [CustomPropertyDrawer(typeof(SceneId))]
    [CanEditMultipleObjects]
    public class Drawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var objectProp = property.FindPropertyRelative("editorSceneObject");
            EditorGUI.BeginChangeCheck();
            UnityEditor.EditorGUI.ObjectField(position, objectProp, typeof(SceneAsset), label);
            if (EditorGUI.EndChangeCheck())
            {
                var objectValue = objectProp.objectReferenceValue;
            }
        }
    }
#endif
}

[System.Serializable]
public class TagId
{
    public string value;

    public override string ToString()
    {
        return value;
    }

    public static implicit operator string(TagId source)
    {
        return source.ToString();
    }

    public static implicit operator TagId(string source)
    {
        return new TagId { value = source };
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TagId))]
    public class Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, prop);

            var valueProp = prop.FindPropertyRelative("value");

            valueProp.stringValue = EditorGUI.TagField(position, label, valueProp.stringValue);

            EditorGUI.EndProperty();
        }
    }
#endif
}

[System.Serializable]
public class LayerId
{
    public int value;

    public static implicit operator int(LayerId source)
    {
        return source.value;
    }

    public static implicit operator LayerId(int source)
    {
        return new LayerId() { value = source };
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LayerId))]
    public class LayerIdDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, prop);

            var valueProp = prop.FindPropertyRelative("value");
            valueProp.intValue = EditorGUI.LayerField(position, label, valueProp.intValue);

            EditorGUI.EndProperty();
        }
    }
#endif
}