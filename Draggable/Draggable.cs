using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IMoveableHandler
{
	void OnMoveBegin(Vector2 position);
	void OnMoveContinue(Vector2 position);
	void OnMoveEnd(Vector2 position);
}

public interface IDraggableStartDragHandler
{
	void OnDragBegin();
}

public interface IDraggableFailedDropHandler
{
	void OnFailedDrop();
}

public interface IDraggableCanDragHandler
{
	void OnCanDragBegin();
	void OnCanDragEnd();
}

public interface IDraggableCanDropHandler
{
	void OnCanDropBegin(Droppable droppable);
	void OnCanDropEnd(Droppable droppable);
}

public interface IDraggableDroppedHandler
{
	void OnDrop(Droppable droppable);
}

public interface ICanDropOnSpecifier
{
	bool CanDrop(Droppable droppable);
}

public interface ICanDragSpecifier
{
	bool CanDrag();
}

public class Draggable : 
	MonoBehaviour,
	IBeginDragHandler,
	IDragHandler,
	IEndDragHandler,
	IPointerEnterHandler,
	IPointerExitHandler
{
	private IDraggableCanDragHandler[] canDragHandlers;
	private IDraggableCanDropHandler[] canDropHandlers;
	private IDraggableDroppedHandler[] droppedHandlers;
	private IDraggableFailedDropHandler[] failedDropHandlers;
	private IDraggableStartDragHandler[] startDragHandlers;
	private IMoveableHandler[] moveableHandlers;
	private ICanDropOnSpecifier canDropOnSpecifier;
	private ICanDragSpecifier canDragSpecifier;

	private static List<Draggable> dragging = new List<Draggable>();

	public static Draggable FindDragging(int pointerId)
	{
		return dragging.Find((obj) => obj.pointerId == pointerId);
	}

	public class DraggableEvent : UnityEvent<Draggable> {}
	public static DraggableEvent OnDragBegun = new DraggableEvent(); 
	public static DraggableEvent OnDragEnded = new DraggableEvent(); 

	public int pointerId
	{
		get; private set;
	}

	private void Start()
	{
		moveableHandlers = GetComponents<IMoveableHandler>();
		failedDropHandlers = GetComponents<IDraggableFailedDropHandler>();
		canDragHandlers = GetComponents<IDraggableCanDragHandler>();
		canDropHandlers = GetComponents<IDraggableCanDropHandler>();
		startDragHandlers = GetComponents<IDraggableStartDragHandler>();
		droppedHandlers = GetComponents<IDraggableDroppedHandler>();
		canDropOnSpecifier = GetComponent<ICanDropOnSpecifier>();
		canDragSpecifier = GetComponent<ICanDragSpecifier>();
	}

	public void OnBeginDrag(PointerEventData eventData)
    {
        if (FindDragging(eventData.pointerId) == null && CanDrag(eventData)) {
            dragging.Add(this);
	        this.pointerId = eventData.pointerId;
            IsHoveringOver = null;
            HasDropped = false;
	        foreach (var h in moveableHandlers)
	        {
		        h.OnMoveBegin(eventData.position);
	        }

	        foreach (var h in startDragHandlers)
	        {
		        h.OnDragBegin();
	        }

	        OnDragBegun.Invoke(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (FindDragging(eventData.pointerId) == this) {
	        foreach (var h in moveableHandlers)
	        {
		        h.OnMoveContinue(eventData.position);
	        }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
		if (FindDragging(eventData.pointerId) == this)
		{
            dragging.Remove(this);
            this.pointerId = int.MinValue;

            if (IsHoveringOver == null)
            {
				if (!HasDropped)
				{
					foreach (var h in failedDropHandlers)
					{
						h.OnFailedDrop();
					}
				}
            } else {
                DropOn(IsHoveringOver);
            }

			foreach (var h in moveableHandlers)
			{
				h.OnMoveEnd(eventData.position);
			}
			
			OnDragEnded.Invoke(this);
		}
		dragging.Remove(this);
    }

    private bool HasDropped = false;

    public Droppable IsHoveringOver {
        get; private set;
    }

	protected bool CanDrag(PointerEventData eventData)
	{
		if (canDragSpecifier != null)
		{
			return canDragSpecifier.CanDrag();
		}

		return true;
	}

    public bool TryHover(Droppable d) {
        if (CanDropOn(d)) {
            IsHoveringOver = d;
	        foreach (var h in canDropHandlers)
	        {
		        h.OnCanDropBegin(d);
	        }
            return true;
        }
        return false;
    }

    public void EndHover(Droppable d) {
        IsHoveringOver = null;
	    foreach (var h in canDropHandlers)
	    {
		    h.OnCanDropEnd(d);
	    }
    }

    public void DropOn(Droppable d) {
        IsHoveringOver = null;
        HasDropped = true;
	    foreach (var h in droppedHandlers)
	    {
		    h.OnDrop(d);
	    }
    }

	public bool CanDropOn(Droppable d)
	{
		if (!d.CanDrop(this))
		{
			return false;
		}

		if (canDropOnSpecifier != null)
		{
			return canDropOnSpecifier.CanDrop(d);
		}

		return true;
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (FindDragging(eventData.pointerId) == null && CanDrag(eventData)) {
	        foreach (var h in canDragHandlers)
	        {
		        h.OnCanDragBegin();
	        }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		if (FindDragging(eventData.pointerId) == null && CanDrag(eventData))
		{
			foreach (var h in canDragHandlers)
			{
				h.OnCanDragEnd();
			}
		}
    }
}
