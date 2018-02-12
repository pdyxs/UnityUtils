using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SerialisedLong {

    public static long fromInts(int high, int low) {
		long val = high;
		val = (val << 32) + low;
        return val;
	}

	public static System.DateTime toDate(int high, int low)
	{
		return new System.DateTime(fromInts(high, low));
	}

#if UNITY_EDITOR
    public static long fromInts(SerializedProperty property)
	{
        long val = property.FindPropertyRelative("high").intValue;
		val = (val << 32) + property.FindPropertyRelative("low").intValue;
		return val;
	}

	public static System.DateTime toDate(SerializedProperty property)
	{
        return new System.DateTime(fromInts(property));
	}

    public static void FromDate(SerializedProperty p, System.DateTime date) {
        var l = date.Ticks;
        p.FindPropertyRelative("high").intValue = toHigh(l);
        p.FindPropertyRelative("low").intValue = toLow(l);
    }
#endif

    public static int toHigh(long l) {
        return (int)(l >> 32);
	}

	public static int toLow(long l)
	{
        return (int)(l & uint.MaxValue);
	}

    public System.DateTime date {
        get {
            return new System.DateTime(val);
        }
        set {
            val = value.Ticks;
        }
    }

    public long val {
        get {
            return fromInts(high, low);
        }
        set {
            low = toLow(value);
            high = toHigh(value);
        }
    }

    [SerializeField]
    private int low;

    [SerializeField]
    private int high;
}
