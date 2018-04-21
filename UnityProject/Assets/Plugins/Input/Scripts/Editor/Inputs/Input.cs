using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.GroupActions
{
  public class Inputs : ScriptableObject
  {
    [SerializeField] private List<JTInputAxis> axis;

    private void AddInput(JTInputAxis jtInput)
    {
      if (axis == null)
        axis = new List<JTInputAxis>();

      if (axis.Exists(i => i.m_Name == jtInput.m_Name))
      {
        axis.RemoveAll(i => i.m_Name == jtInput.m_Name);
      }

      axis.Add(jtInput);
      
    }

    public void AddInput(SerializedProperty input)
    {
      AddInput(JTInputAxis.InputToJTInput(input));
    }

    public SerializedProperty Extract(SerializedProperty input, int index)
    {
      return axis[index].ToInput(input);
    }

    public string[] Get()
    {
      return axis.Select(i => i.m_Name).ToArray();
    }
  }

}