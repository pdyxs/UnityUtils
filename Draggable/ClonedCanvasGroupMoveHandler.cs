using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClonedCanvasGroupMoveHandler : 
	CanvasGroupMover,
	IMoveableHandler,
	IMovingProvider
{
	public CanvasGroup draggablePrefab;

	private CanvasGroup draggable;
	private CanvasGroup dragging;

	private void Start()
	{
		draggable = draggablePrefab.Spawn(transform);
	}

	public void OnMoveBegin(Vector2 position)
	{
		if (draggable != null)
		{
			dragging = draggablePrefab.Spawn(draggingParent.Get(this));
			dragging.transform.position = transform.position;
			StartMove(position);
		}
	}

	public void OnMoveContinue(Vector2 position)
	{
		ContinueMove(position);
	}

	public void OnMoveEnd(Vector2 position)
	{
		EndMove(position);
	}
	
	public GameObject MovingObject => dragging.gameObject;
	public override CanvasGroup draggedCanvasGroup => dragging;
}
