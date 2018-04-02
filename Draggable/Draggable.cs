using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IMoveableHandler
{
	void OnMoveBegin(Vector2 position);
	void OnMoveContinue(Vector2 position);
	void OnMoveEnd(Vector2 position);
}

public interface IDraggableHandler
{
	void OnCanDragBegin();
	void OnCanDragEnd();
	void OnCanDropBegin(Droppable droppable);
	void OnCanDropEnd(Droppable droppable);
	void OnDrop(Droppable droppable);
	void OnFailedDrop();
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
	private IDraggableHandler[] draggableHandlers;
	private IMoveableHandler[] moveableHandlers;
	private ICanDropOnSpecifier canDropOnSpecifier;
	private ICanDragSpecifier canDragSpecifier;

	private static List<Draggable> dragging = new List<Draggable>();

	public static Draggable FindDragging(int pointerId)
	{
		return dragging.Find((obj) => obj.pointerId == pointerId);
	}

	public int pointerId
	{
		get; private set;
	}

	private void Start()
	{
		moveableHandlers = GetComponents<IMoveableHandler>();
		draggableHandlers = GetComponents<IDraggableHandler>();
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
					foreach (var h in draggableHandlers)
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
	        foreach (var h in draggableHandlers)
	        {
		        h.OnCanDropBegin(d);
	        }
            return true;
        }
        return false;
    }

    public void EndHover(Droppable d) {
        IsHoveringOver = null;
	    foreach (var h in draggableHandlers)
	    {
		    h.OnCanDropEnd(d);
	    }
    }

    public void DropOn(Droppable d) {
        IsHoveringOver = null;
        HasDropped = true;
	    foreach (var h in draggableHandlers)
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
	        foreach (var h in draggableHandlers)
	        {
		        h.OnCanDragBegin();
	        }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		if (FindDragging(eventData.pointerId) == null && CanDrag(eventData))
		{
			foreach (var h in draggableHandlers)
			{
				h.OnCanDragEnd();
			}
		}
    }
}
