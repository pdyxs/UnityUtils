﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TransformReference : ObjectReference<Transform> {}

[System.Serializable]
public class RectTransformReference : ObjectReference<RectTransform> {}

[System.Serializable]
public class TextReference : ObjectReference<Text> {}

public class ObjectReference<T> : ObjectReference
	where T : Component
{
	[SerializeField]
	private T obj;

	private bool initialised = false;

	private Transform _myParent;
	private Transform _refParent;

	public T Find(GameObject self)
	{
		switch (reference)
		{
			case Reference.None:
				return null;
			case Reference.Self:
				return self.GetComponent<T>();
			case Reference.Parent:
				return self.transform.parent.GetComponent<T>();
			case Reference.Ancestor:
				_myParent = self.transform.parent;
				var p = _myParent;
				T ret = null;
				while (ret == null)
				{
					if (p == null)
					{
						break;
					}
					ret = p.GetComponent<T>();
					p = p.parent;
				}

				if (ret != null)
				{
					_refParent = ret.transform.parent;
				}
				return ret;
			case Reference.Child:
				_myParent = self.transform.parent;
				var r = self.GetComponentInChildren<T>();
				if (r != null)
				{
					_refParent = r.transform.parent;
				}
				return r;
			case Reference.Other:
				return obj;
		}

		return obj;
	}

	public bool Check(GameObject self)
	{
		switch (reference)
		{
			case Reference.Self:
				return obj.gameObject == self;
			case Reference.Parent:
				return obj.gameObject == self.transform.parent.gameObject;
			case Reference.Ancestor:
			case Reference.Child:
				return self.transform.parent == _myParent && obj && obj.transform.parent == _refParent;
		}

		return true;
	}

	public T Get(GameObject self)
	{
		if (!initialised || !Check(self))
		{
			initialised = true;
			obj = Find(self);
		}
		
		return obj;
	}

	public T Get(MonoBehaviour self)
	{
		return Get(self.gameObject);
	}
}

public abstract class ObjectReference
{
	public enum Reference
	{
		None,
		Self,
		Parent,
		Ancestor,
		Child,
		Other
	}

	[SerializeField] protected Reference reference = Reference.Self;
}
