using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ObjectReference), true)]
public class ObjectReferenceDrawer : PropertyDrawer {
	
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUIUtility.singleLineHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var referenceProperty = property.FindPropertyRelative("reference");
		var objProperty = property.FindPropertyRelative("obj");
		if (referenceProperty.enumValueIndex == (int) ObjectReference.Reference.Other)
		{
			EditorGUI.PropertyField(
				new Rect(position.x, position.y, position.width/2, position.height), 
				referenceProperty, label
			);
			EditorGUI.PropertyField(
				new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height),
				objProperty, new GUIContent("")
			);
		}
		else
		{
			EditorGUI.PropertyField(
				new Rect(position.x, position.y, position.width, position.height), 
				referenceProperty, label
			);
		}
	}
}
