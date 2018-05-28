using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecifiedCanvasGroupMoveHandler : 
	CanvasGroupMover,
	IMoveableHandler
{

	public override CanvasGroup draggedCanvasGroup
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

	public void OnMoveBegin(Vector2 position)
	{
		StartMove(position);
	}

	public void OnMoveContinue(Vector2 position)
	{
		ContinueMove(position);
	}

	public void OnMoveEnd(Vector2 position)
	{
		EndMove(position);
	}
}
