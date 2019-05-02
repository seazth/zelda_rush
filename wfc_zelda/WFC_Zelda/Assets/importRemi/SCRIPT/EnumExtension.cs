using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/*
public class EnumFlagsAttribute : PropertyAttribute
{
    public EnumFlagsAttribute() { }
}

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        _property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
    }
}
public static class EnumExtension
{
    public static int[] getEnumValues<T>(int myenum)
    {
        List<int> res = new List<int>();
        for (int i = 0; i < System.Enum.GetValues(typeof(T)).Length; i++)
        {
            int layer = 1 << i;
            if ((myenum & layer) != 0) res.Add(i);
        }
        return res.ToArray();
    }
}
*/