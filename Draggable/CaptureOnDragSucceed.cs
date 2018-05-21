using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureOnDragSucceed : 
	MonoBehaviour,
	IDroppableDroppedHandler
{
	public TransformReference captor;
	
	public void OnDropped(Draggable draggable)
	{
		var imoving = draggable.GetComponent<IMovingProvider>();
		var obj = draggable.transform;
		if (imoving != null && imoving.MovingObject != null)
		{
			obj = imoving.MovingObject.transform;
		}
		
		var layout = obj.GetComponent<LayoutElement>();
		if (layout != null)
		{
			layout.ignoreLayout = false;
		}
		
		obj.SetParent(captor.Get(this));
	}
}
