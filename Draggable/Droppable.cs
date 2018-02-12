using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class Droppable : MonoBehaviour,
                                  IDropHandler,
                                  IPointerEnterHandler,
                                  IPointerExitHandler {
    
	public class DragDropEvent : UnityEvent<Draggable, Droppable> { }
    public DragDropEvent OnHoverStart = new DragDropEvent();
    public DragDropEvent OnHoverEnd = new DragDropEvent();
    public DragDropEvent OnDropped = new DragDropEvent();

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = Draggable.FindDragging(eventData.pointerId);
        if (dragged != null &&
            dragged.IsHoveringOver == this) {
            dragged.DropOn(this);
            OnDropped.Invoke(dragged, this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var dragged = Draggable.FindDragging(eventData.pointerId);
		if (dragged != null &&
			CanDrop(dragged) && 
			dragged.TryHover(this))
		{
            OnHoverStart.Invoke(dragged, this);
		}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		var dragged = Draggable.FindDragging(eventData.pointerId);
        if (dragged != null && dragged.IsHoveringOver == this) {
            dragged.EndHover(this);
            OnHoverEnd.Invoke(dragged, this);
        }
    }

    protected abstract bool CanDrop(Draggable dragged);
}
