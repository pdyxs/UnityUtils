// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Float")]
public class FloatVariable : ScriptableVariable<float>
{
    public void ApplyChange(float amount)
    {
        Value += amount;
    }

    public void ApplyChange(IGettable<float> amount)
    {
        Value += amount.Value;
    }
}