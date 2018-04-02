using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupMoveHandler : 
	MonoBehaviour,
	IMoveableHandler
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
