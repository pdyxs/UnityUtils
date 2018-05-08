using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TransformReference : ObjectReference<Transform> {}

[System.Serializable]
public class RectTransformReference : ObjectReference<RectTransform> {}

public class ObjectReference<T> : ObjectReference
	where T : Component
{
	[SerializeField]
	private T obj;

	private bool initialised = false;

	public T Get(GameObject self)
	{
		if (!initialised)
		{
			initialised = true;
			switch (reference)
			{
				case Reference.None:
					obj = null;
					break;
				case Reference.Self:
					obj = self.GetComponent<T>();
					break;
				case Reference.Parent:
					obj = self.transform.parent.GetComponent<T>();
					break;
				case Reference.Other:
					break;
			}
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
		Other
	}

	[SerializeField] protected Reference reference = Reference.Self;
}
