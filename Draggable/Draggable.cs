using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public abstract class Draggable : MonoBehaviour,
                         IBeginDragHandler,
                         IDragHandler,
                         IEndDragHandler,
                         IPointerEnterHandler,
                         IPointerExitHandler
{
    public class DragEvent : UnityEvent<Draggable> {}
	public DragEvent OnCanDragBegin = new DragEvent();
    public DragEvent OnCanDragEnd = new DragEvent();
    public DragEvent OnDragBegun = new DragEvent();
    public DragEvent OnDragEnded = new DragEvent();

	private static List<Draggable> dragging = new List<Draggable>();

	public static Draggable FindDragging(int pointerId)
	{
		return dragging.Find((obj) => obj.pointerId == pointerId);
	}

	public abstract CanvasGroup DraggedObjectCanvasGroup
	{
        get;
	}

	public int pointerId
	{
		get; private set;
	}
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (FindDragging(eventData.pointerId) == null && CanDrag(eventData)) {
            InitialiseDrag(eventData);
            dragging.Add(this);
            DraggedObjectCanvasGroup.blocksRaycasts = false;
            this.pointerId = eventData.pointerId;
            IsHoveringOver = null;
            HasDropped = false;
            OnDragBegun.Invoke(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (FindDragging(eventData.pointerId) == this) {
            ContinueDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
		if (FindDragging(eventData.pointerId) == this)
		{
			if (DraggedObjectCanvasGroup != null)
			{
				DraggedObjectCanvasGroup.blocksRaycasts = true;
            }
            dragging.Remove(this);
            this.pointerId = int.MinValue;

            if (IsHoveringOver == null)
            {
				if (!HasDropped)
				{
					NotDropped(eventData);
				}
            } else {
                DropOn(IsHoveringOver);
            }


            OnDragEnded.Invoke(this);
		}
		dragging.Remove(this);
    }

    private bool HasDropped = false;

    public Droppable IsHoveringOver {
        get; private set;
    }

    protected abstract bool CanDrag(PointerEventData eventData);
    protected abstract void InitialiseDrag(PointerEventData eventData);
    protected abstract void ContinueDrag(PointerEventData eventData);

    public bool TryHover(Droppable d) {
        if (StartHoverOver(d)) {
            IsHoveringOver = d;
            return true;
        }
        return false;
    }

    public void EndHover(Droppable d) {
        IsHoveringOver = null;
        EndHoverOver(d);
    }

    public void DropOn(Droppable d) {
        IsHoveringOver = null;
        HasDropped = true;
        Dropped(d);
    }

    protected abstract void Dropped(Droppable d);
    protected abstract void NotDropped(PointerEventData eventData);

    public abstract bool StartHoverOver(Droppable d);
    public abstract void EndHoverOver(Droppable d);

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (FindDragging(eventData.pointerId) == null && CanDrag(eventData)) {
            OnCanDragBegin.Invoke(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		if (FindDragging(eventData.pointerId) == null && CanDrag(eventData))
		{
            OnCanDragEnd.Invoke(this);
		}
    }
}
