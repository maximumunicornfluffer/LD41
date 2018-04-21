using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.GroupActions
{
  public class RenameGroupAction : IGroupAction
  {
    private bool _fullRename = true;
    private bool _active;

    private string[] _actions = new[] {"full", "partial"};
    private int _actionIndex = 0;
    private string _newName = "";
    private string _old = "";
    private string _new = "";

    public string Name { get { return "Rename"; } }
    public bool Run(HashSet<int> indexes, SerializedProperty axisArray)
    {
      foreach (var index in indexes)
      {
        var property = axisArray.GetArrayElementAtIndex(index);

        switch (_actionIndex)
        {
          case 0:// full
            property.FindPropertyRelative("m_Name").stringValue = _newName;
            break;
          case 1:// partial
            var name = property.FindPropertyRelative("m_Name").stringValue;
            name = name.Replace(_old, _new);
            Debug.Log(name+" "+_old+" "+_new);
            property.FindPropertyRelative("m_Name").stringValue = name;
            break;
        }
      }


      return true;
    }

    public void OnGUI()
    {
      EditorGUILayout.BeginHorizontal();
      _actionIndex = EditorGUILayout.Popup(_actionIndex, _actions);
      switch (_actionIndex)
      {
          case 0:// full
          _newName = EditorGUILayout.TextField("new name", _newName);
          break;
          case 1:// partial
          _old = EditorGUILayout.TextField("from",_old);
          _new = EditorGUILayout.TextField("to", _new);
          break;
      }
      EditorGUILayout.EndHorizontal();
    }
  }
}