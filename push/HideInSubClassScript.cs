using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;


//public class HideInSubClassAttribute : PropertyAttribute { }

//[CustomPropertyDrawer(typeof(HideInSubClassAttribute))]
//public class HideInSubClassAttributeDrawer : PropertyDrawer
//{
//    private bool Show(SerializedProperty property)
//    {
//        Type type = property.serializedObject.targetObject.GetType();
//        FieldInfo field = type.GetField(property.name);
//        Type declareType = field.DeclaringType;
//        return type == declareType;
//    }

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        if (Show(property))
//        {
//            EditorGUI.PropertyField(position, property);
//        }
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        if (Show(property))
//        {
//            return base.GetPropertyHeight(property, label);
//        }
//        else
//        {
//            return 0;
//        }
//    }
//}
