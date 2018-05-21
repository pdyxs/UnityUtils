using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClonedCanvasGroupMoveHandler : 
	MonoBehaviour,
	IMoveableHandler,
	IMovingProvider
{
	public CanvasGroup draggablePrefab;

	public TransformReference draggingParent;

	private CanvasGroup draggable;
	private CanvasGroup dragging;

	public RectTransform draggingRectTransform
	{
		get
		{
			return dragging.transform as RectTransform;
		}
	}

	private Vector2 dragDelta;

	private void Start()
	{
		draggable = draggablePrefab.Spawn(transform);
	}

	public void OnMoveBegin(Vector2 position)
	{
		if (draggable != null)
		{
			dragging = draggablePrefab.Spawn(draggingParent.Get(this));
			dragging.blocksRaycasts = false;
			var layout = dragging.GetComponent<LayoutElement>();
			if (layout != null)
			{
				layout.ignoreLayout = true;
			}
			dragDelta = draggingRectTransform.position.to2D() - position;
		}
	}

	public void OnMoveContinue(Vector2 position)
	{
		if (dragging != null)
		{
			draggingRectTransform.position = (position + dragDelta).to3D();
		}
	}

	public void OnMoveEnd(Vector2 position)
	{
		if (dragging != null)
		{
			dragging.blocksRaycasts = true;
		}
	}
	
	public GameObject MovingObject => dragging.gameObject;
}
