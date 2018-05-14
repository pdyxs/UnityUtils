using System.Collections;
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
				var p = self.transform.parent;
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
				return ret;
			case Reference.Other:
				return obj;
		}

		return obj;
	}

	public T Get(GameObject self)
	{
		if (!initialised)
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
		Other
	}

	[SerializeField] protected Reference reference = Reference.Self;
}
