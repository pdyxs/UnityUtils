using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
public class RecycleOnDragFail : 
	MonoBehaviour,
	IDraggableFailedDropHandler
{

	public GameObject draggingObject
	{
		get
		{
			if (_draggingObject == null)
			{
				var imp = GetComponent<IMovingProvider>();
				if (imp != null)
				{
					_draggingObject = imp.MovingObject;
				}
			}

			return _draggingObject;
		}
	}

	private GameObject _draggingObject;


	public void OnFailedDrop()
	{
		if (draggingObject != null)
		{
			draggingObject.Recycle();
			_draggingObject = null;
		}
	}
}
