using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface ICanSelectSpecifier
{
	bool CanSelect();
}

public interface IOnSelectHandler
{
	void OnSelect();
}

public interface IOnDeselectHandler
{
	void OnDeselect();
}


public class Selectable : 
	MonoBehaviour,
	IPointerClickHandler
{
	public static Selectable selected;

	private ICanSelectSpecifier canSelectSpecifier;
	private IOnSelectHandler[] selectHandlers;
	private IOnDeselectHandler[] deselectHandlers;
	
	public class SelectableEvent : UnityEvent<Selectable> {}

	public static SelectableEvent OnSelected = new SelectableEvent();

	private void Start()
	{
		canSelectSpecifier = GetComponent<ICanSelectSpecifier>();
		selectHandlers = GetComponents<IOnSelectHandler>();
		deselectHandlers = GetComponents<IOnDeselectHandler>();
		
		Draggable.OnDragBegun.AddListener(OnDragBegun);
	}

	private void OnDragBegun(Draggable draggable)
	{
		if (selected == this)
		{
			OnDeselect();
			selected = null;
		}
	}

	private void OnDeselect()
	{
		foreach (var h in deselectHandlers)
		{
			h.OnDeselect();
		}
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		if (canSelectSpecifier != null && !canSelectSpecifier.CanSelect())
		{
			return;
		}
		
		if (selected != null)
		{
			selected.OnDeselect();
		}

		if (selected != this)
		{
			selected = this;
		
			foreach (var h in selectHandlers)
			{
				h.OnSelect();
			}
		}
		else
		{
			selected = null;
		}

		OnSelected.Invoke(selected);
	}
}
