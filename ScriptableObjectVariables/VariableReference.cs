public abstract class VariableReference
{
    public bool UseConstant = true;
    
    public VariableReference()
    {}
}

[System.Serializable]
public abstract class VariableReference<T, TVar> : 
    VariableReference
    where TVar : ScriptableVariable<T>
{
    public T ConstantValue;
    public TVar Variable;

    public VariableReference()
    {}
    
    public VariableReference(T val)
    {
        UseConstant = true;
        ConstantValue = val;
    }

    public T Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator T(VariableReference<T, TVar> reference)
    {
        return reference.Value;
    }
}
