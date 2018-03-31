using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DefaultOwnerObjectAttribute))]
public class DefaultOwnerObjectAttributeDrawer : 
	PropertyDrawer
{
	private static readonly string[] options = new string[]
	{
		"None",
		"Self",
		"Other"
	};

	private int index = -1;
	
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUIUtility.singleLineHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var mb = (MonoBehaviour) property.serializedObject.targetObject;
		var type = property.GetSerializedType();
		var go = mb.gameObject;
		if (index == -1)
		{
			index = 0;

			if (property.objectReferenceValue != null)
			{
				if ((type == typeof(GameObject) && property.objectReferenceValue == go) ||
				    (property.objectReferenceValue == mb.GetComponent(type)))
				{
					index = 1;
				}
				else
				{
					index = 2;
				}
			}
		}

		var newIndex = EditorGUI.Popup(
			new Rect(position.x, position.y, index < 2 ? position.width : position.width/2, position.height), 
			label.text, index, options
		);

		if (index != newIndex)
		{
			index = newIndex;
			switch (index)
			{
				case 0:
					property.objectReferenceValue = null;
					break;
				case 1:
					if (type == typeof(GameObject))
					{
						property.objectReferenceValue = go;
					}
					else
					{
						property.objectReferenceValue = mb.GetComponent(type);
					}
					break;
				case 2:
					if ((type == typeof(GameObject) && property.objectReferenceValue == go) ||
						(property.objectReferenceValue == mb.GetComponent(type)))
					{
						property.objectReferenceValue = null;
					}
					break;
			}
		}

		if (index == 2)
		{
			EditorGUI.PropertyField(
				new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height),
				property, new GUIContent(""));
		}

		property.serializedObject.ApplyModifiedProperties();
	}
}
