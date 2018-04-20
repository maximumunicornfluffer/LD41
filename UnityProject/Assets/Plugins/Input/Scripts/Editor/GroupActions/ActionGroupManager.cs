using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.GroupActions
{
  public class ActionGroupManager
  {
    private Dictionary<string, IGroupAction> _groupActions = new Dictionary<string, IGroupAction>();
    private string _selectedGroupAction = "";

    public void Init()
    {
      if (_groupActions.Count == 0 || _selectedGroupAction =="")
      {
        _groupActions.Clear();
        _selectedGroupAction = "";


        var type = typeof(IGroupAction);
        var types = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(s => s.GetTypes())
          .Where(p => type.IsAssignableFrom(p));

        foreach (var t in types)
        {
          if (!t.IsAbstract)
          {
            var action = Activator.CreateInstance(t) as IGroupAction;
            if (action != null)
            {
              _groupActions.Add(action.Name, action);
              if (_selectedGroupAction == "")
                _selectedGroupAction = action.Name;
            }
          }
        }
      }
    }

    public bool OnGUI(HashSet<int> selectedIndexes,  SerializedProperty axisArray)
    {
      var modified = false;

      EditorGUILayout.BeginVertical("box");

      EditorGUILayout.BeginHorizontal();

      var actions = _groupActions.Keys.ToArray();
      var index = Array.FindIndex(actions, w => w.Equals(_selectedGroupAction));
      var newIndex = EditorGUILayout.Popup("Group Actions :", index, actions);
      _selectedGroupAction = actions[newIndex];



      EditorGUILayout.EndHorizontal();
      _groupActions[_selectedGroupAction].OnGUI();

      if (axisArray!=null&& GUILayout.Button("Action"))
      {
        modified|= _groupActions[_selectedGroupAction].Run(selectedIndexes,axisArray);
      }
      EditorGUILayout.EndVertical();

      return modified;
    }
  }
}