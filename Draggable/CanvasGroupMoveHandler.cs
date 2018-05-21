using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGroupMoveHandler : 
	MonoBehaviour,
	IMoveableHandler,
	IMovingProvider
{
	[DefaultOwnerObject]
	public CanvasGroup draggedCanvasGroup;

	public RectTransform draggedRectTransform
	{
		get
		{
			return draggedCanvasGroup.transform as RectTransform;
		}
	}

	[DefaultOwnerObject] public Transform draggingParent;

	private Vector2 dragDelta;

	public void OnMoveBegin(Vector2 position)
	{
		if (draggedCanvasGroup != null)
		{
			draggedCanvasGroup.blocksRaycasts = false;
			var layout = draggedCanvasGroup.GetComponent<LayoutElement>();
			if (layout != null)
			{
				layout.ignoreLayout = true;
			}
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

	public GameObject MovingObject => draggedCanvasGroup.gameObject;
}
