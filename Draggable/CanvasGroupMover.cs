using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class CanvasGroupMover : 
	MonoBehaviour
{
	public TransformReference draggingParent;
	
	public abstract CanvasGroup draggedCanvasGroup { get; }
	
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
	private Vector2 dragDelta;

	public float deltaZeroTime = 0.5f;

	public LayoutElement layout
	{
		get
		{
			if (_layout == null)
			{
				_layout = GetComponent<LayoutElement>();
			}

			return _layout;
		}
	}

	private LayoutElement _layout;
	private bool wasLayoutIgnored = false;

	protected void StartMove(Vector2 position)
	{
		if (draggedCanvasGroup != null)
		{
			if (layout != null)
			{
				wasLayoutIgnored = layout.ignoreLayout;
				layout.ignoreLayout = true;
			}

			draggedCanvasGroup.transform.SetParent(draggingParent.Get(this), true);
			draggedCanvasGroup.blocksRaycasts = false;
			dragDelta = draggedRectTransform.position.to2D() - position;

			DOTween.To(
				() => dragDelta, (v) => dragDelta = v, 
				Vector2.zero, deltaZeroTime
			);
		}
	}

	protected void ContinueMove(Vector2 position)
	{
		if (draggedCanvasGroup != null)
			draggedRectTransform.position = (position + dragDelta).to3D();
	}

	protected void EndMove(Vector2 position)
	{
		if (draggedCanvasGroup != null)
		{
			draggedCanvasGroup.blocksRaycasts = true;
			if (layout != null)
			{
				layout.ignoreLayout = wasLayoutIgnored;
			}
		}
	}
}
