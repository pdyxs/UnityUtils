using UnityEngine;

public abstract class ScriptableVariable : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline] public string DeveloperDescription = "";
#endif
}

public interface IGettable<out T>
{
    T Value { get; }
}

public abstract class ScriptableVariable<T> : 
    ScriptableVariable,
    IGettable<T>
{
    [SerializeField] 
    private T val;

    public T Value
    {
        get { return val; }
        protected set { val = value; }
    }
    
    public void Set(T value)
    {
        Value = value;
    }

    public void Set(IGettable<T> value)
    {
        Value = value.Value;
    }

    public static implicit operator T(ScriptableVariable<T> var)
    {
        return var.Value;
    }
} 
