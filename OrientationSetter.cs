using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationSetter : MonoBehaviour {

	public enum DeviceTypes
	{
		iPad
	}

	public DeviceTypes deviceType;

	public ScreenOrientation orientation;
	
	void Start() {
		SetRotation ();
	}

	public void SetRotation () {
		switch (deviceType)
		{
			case DeviceTypes.iPad:
				if (UnityEngine.iOS.Device.generation.ToString().Contains("iPad"))
					break;
				return;
		}
		Screen.orientation = orientation;
	}
}
