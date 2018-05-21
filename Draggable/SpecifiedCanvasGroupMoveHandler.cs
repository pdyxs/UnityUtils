using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecifiedCanvasGroupMoveHandler : 
	MonoBehaviour,
	IMoveableHandler
{
	public TransformReference draggingParent;

	private Vector2 dragDelta;

	public CanvasGroup draggedCanvasGroup
	{
		get
		{
			if (_draggedCanvasGroup == null)
			{
				var imoveable = GetComponent<IMovingProvider>();
				if (imoveable != null && imoveable.MovingObject != null)
				{
					_draggedCanvasGroup = imoveable.MovingObject.GetComponent<CanvasGroup>();
				}
			}

			return _draggedCanvasGroup;
		}
	}

	private CanvasGroup _draggedCanvasGroup;
	
	public RectTransform draggedRectTransform
	{
		get
		{
			if (draggedCanvasGroup == null)
			{
				return null;
			}
			return draggedCanvasGroup.transform as RectTransform;
		}
	}

	public void OnMoveBegin(Vector2 position)
	{
		if (draggedCanvasGroup != null)
		{
			var layout = draggedCanvasGroup.GetComponent<LayoutElement>();
			if (layout != null)
			{
				layout.ignoreLayout = true;
			}
			draggedCanvasGroup.transform.SetParent(draggingParent.Get(this), true);
			draggedCanvasGroup.blocksRaycasts = false;
			dragDelta = draggedRectTransform.position.to2D() - position;
		}
	}

	public void OnMoveContinue(Vector2 position)
	{
		if (draggedCanvasGroup != null)
		{
			draggedRectTransform.position = (position + dragDelta).to3D();
		}
	}

	public void OnMoveEnd(Vector2 position)
	{
		if (draggedCanvasGroup != null)
		{
			draggedCanvasGroup.blocksRaycasts = true;
		}
	}
}
