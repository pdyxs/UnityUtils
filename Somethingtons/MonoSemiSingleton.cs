using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSemiSingleton<T> : MonoBehaviour where T : MonoSemiSingleton<T> {

    public static T instance {
        get; private set;
    }

	// Use this for initialization
	protected virtual void Awake () {
        if (instance != null) {
            Debug.LogWarning("There's already a " + typeof(T).Name + ". Destroying it now.");
            instance.DestroyThis();
        }
        instance = this as T;
	}

    protected virtual void DestroyThis() {
        Destroy(this);
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }

    public static bool isInitialized {
        get {
            return instance != null;
        }
    }
}
