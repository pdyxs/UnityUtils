﻿using System.Collections;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public interface IDroppableDroppedHandler
{
    void OnDropped(Draggable draggable);
}

public interface IDroppableHoverHandler
{
    void OnHoverStart(Draggable draggable);
    void OnHoverEnd(Draggable draggable);
}

public interface ICanBeDroppedSpecifier
{
    bool CanDrop(Draggable dragged);
}

public class Droppable : 
    MonoBehaviour,
    IDropHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private IDroppableDroppedHandler[] dropHandlers;
    private IDroppableHoverHandler[] hoverHandlers;

    private ICanBeDroppedSpecifier canBeDroppedSpecifier;

    private void Start()
    {
        dropHandlers = GetComponents<IDroppableDroppedHandler>();
        hoverHandlers = GetComponents<IDroppableHoverHandler>();
        canBeDroppedSpecifier = GetComponent<ICanBeDroppedSpecifier>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = Draggable.FindDragging(eventData.pointerId);
        if (dragged != null &&
            dragged.IsHoveringOver == this) {
            dragged.DropOn(this);
            foreach (var handler in dropHandlers)
            {
                handler.OnDropped(dragged);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var dragged = Draggable.FindDragging(eventData.pointerId);
		if (dragged != null &&
			CanDrop(dragged) && 
			dragged.TryHover(this))
		{
		    foreach (var handler in hoverHandlers)
		    {
		        handler.OnHoverStart(dragged);
		    }
		}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		var dragged = Draggable.FindDragging(eventData.pointerId);
        if (dragged != null && dragged.IsHoveringOver == this) {
            dragged.EndHover(this);
            foreach (var handler in hoverHandlers)
            {
                handler.OnHoverEnd(dragged);
            }
        }
    }

    public bool CanDrop(Draggable dragged)
    {
        if (canBeDroppedSpecifier != null)
        {
            return canBeDroppedSpecifier.CanDrop(dragged);
        }

        return true;
    }
}
