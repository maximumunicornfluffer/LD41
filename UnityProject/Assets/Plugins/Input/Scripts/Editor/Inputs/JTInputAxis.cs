using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.GroupActions
{
  [System.Serializable]
  public class JTInputAxis
  {
    public string m_Name;
    public string descriptiveName;
    public string descriptiveNegativeName;
    public string negativeButton;
    public string positiveButton;
    public string altNegativeButton;
    public string altPositiveButton;
    public float gravity;
    public float dead;
    public float sensitivity;
    public bool snap;
    public bool invert;
    public string type;
    public string axis;
    public string joyNum;

    public SerializedProperty ToInput(SerializedProperty input)
    {
      var enumerator = input.Copy().GetEnumerator();

      while (enumerator.MoveNext())
      {
        var serializedProperty = enumerator.Current as SerializedProperty;

        if (serializedProperty != null)
        {
          if (serializedProperty.name == "m_Name")
          {
            serializedProperty.stringValue = m_Name;
          }
          if (serializedProperty.name == "descriptiveName")
          {
            serializedProperty.stringValue = descriptiveName;
          }
          if (serializedProperty.name == "descriptiveNegativeName")
          {
            serializedProperty.stringValue = descriptiveNegativeName;
          }
          if (serializedProperty.name == "negativeButton")
          {
            serializedProperty.stringValue = negativeButton;
          }
          if (serializedProperty.name == "positiveButton")
          {
            serializedProperty.stringValue = positiveButton;
          }
          if (serializedProperty.name == "altNegativeButton")
          {
            serializedProperty.stringValue = altNegativeButton;
          }
          if (serializedProperty.name == "altPositiveButton")
          {
            serializedProperty.stringValue = altPositiveButton;
          }

          if (serializedProperty.name == "gravity")
          {
            serializedProperty.floatValue = gravity;
          }
          if (serializedProperty.name == "dead")
          {
            serializedProperty.floatValue = dead;
          }
          if (serializedProperty.name == "sensitivity")
          {
            serializedProperty.floatValue = sensitivity;
          }

          if (serializedProperty.name == "snap")
          {
            serializedProperty.boolValue = snap;
          }
          if (serializedProperty.name == "invert")
          {
            serializedProperty.boolValue = invert;
          }

          if (serializedProperty.name == "type")
          {
            serializedProperty.enumValueIndex = IndexOfElement(serializedProperty.enumNames, type);
          }
          if (serializedProperty.name == "axis")
          {
            serializedProperty.enumValueIndex = IndexOfElement(serializedProperty.enumNames, axis);
          }
          if (serializedProperty.name == "joyNum")
          {
            serializedProperty.enumValueIndex = IndexOfElement(serializedProperty.enumNames, joyNum);
          }
        }
      }

      return input;
    }

    private int IndexOfElement(string[] values, string element)
    {
      for (int i = 0; i < values.Length; i++)
      {
        if (values[i] == element)
          return i;
      }

      return 0;
    }

    public static JTInputAxis InputToJTInput(SerializedProperty input)
    {
      var jtia = new JTInputAxis();

      var enumerator = input.Copy().GetEnumerator();

      while (enumerator.MoveNext())
      {
        var serializedProperty = enumerator.Current as SerializedProperty;

        if (serializedProperty != null)
        {
          if (serializedProperty.name == "m_Name")
          {
            jtia.m_Name = serializedProperty.stringValue;
          }
          if (serializedProperty.name == "descriptiveName")
          {
            jtia.descriptiveName = serializedProperty.stringValue;
          }
          if (serializedProperty.name == "descriptiveNegativeName")
          {
            jtia.descriptiveNegativeName = serializedProperty.stringValue;
          }
          if (serializedProperty.name == "negativeButton")
          {
            jtia.negativeButton = serializedProperty.stringValue;
          }
          if (serializedProperty.name == "positiveButton")
          {
            jtia.positiveButton = serializedProperty.stringValue;
          }
          if (serializedProperty.name == "altNegativeButton")
          {
            jtia.altNegativeButton = serializedProperty.stringValue;
          }
          if (serializedProperty.name == "altPositiveButton")
          {
            jtia.altPositiveButton = serializedProperty.stringValue;
          }

          if (serializedProperty.name == "gravity")
          {
            jtia.gravity = serializedProperty.floatValue;
          }
          if (serializedProperty.name == "dead")
          {
            jtia.dead = serializedProperty.floatValue;
          }
          if (serializedProperty.name == "sensitivity")
          {
            jtia.sensitivity = serializedProperty.floatValue;
          }

          if (serializedProperty.name == "snap")
          {
            jtia.snap = serializedProperty.boolValue;
          }
          if (serializedProperty.name == "invert")
          {
            jtia.invert = serializedProperty.boolValue;
          }

          if (serializedProperty.name == "type")
          {
            jtia.type = serializedProperty.enumNames[serializedProperty.enumValueIndex];
          }
          if (serializedProperty.name == "axis")
          {
            jtia.axis = serializedProperty.enumNames[serializedProperty.enumValueIndex];
          }
          if (serializedProperty.name == "joyNum")
          {
            jtia.joyNum = serializedProperty.enumNames[serializedProperty.enumValueIndex];
          }
        }
      }
      return jtia;
    }
  }
}