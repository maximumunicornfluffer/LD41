using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Assets.Scripts.Editor.GroupActions
{
  public class JoystickReorderedList
  {
    private ReorderableList _rList;

    private bool _modified;
    
    private HashSet<int> _selectedIndexes = new HashSet<int>();

    public HashSet<int> SelectedIndexes
    {
      get { return new HashSet<int>(_selectedIndexes); }
    }

    public JoystickReorderedList(SerializedObject inputManager)
    {
      _rList = new ReorderableList(inputManager, inputManager.FindProperty("m_Axes") , true, false, true, true);
      _rList.drawElementCallback = DrawElementCallback;
      _rList.onSelectCallback = OnSelectCallback;
      _rList.onRemoveCallback = OnRemoveCallback;
      _rList.onChangedCallback = OnChangedCallback;
    }

    public bool OnGUI()
    {
      _rList.DoLayoutList();
      var m = _modified;
      _modified = false;
      return m;
    }

    private void OnChangedCallback(ReorderableList list)
    {
      _selectedIndexes.Clear();
      _modified = true;
    }

    private void OnRemoveCallback(ReorderableList list)
    {
      var l = _selectedIndexes.ToList();
      l = l.OrderBy(t => t).ToList();
      for (int i = _selectedIndexes.Count-1; i >= 0; i--)
      {
        var name = list.serializedProperty.GetArrayElementAtIndex(l[i]).FindPropertyRelative("m_Name").stringValue;
        if (EditorUtility.DisplayDialog("Delete Input " + name, "Do you really want to delete " + name + "?", "yes","no"))
        {
          list.serializedProperty.DeleteArrayElementAtIndex(l[i]);
          _modified = true;
        }
      }
    
      _selectedIndexes.Clear();
    }

    private void OnSelectCallback(ReorderableList list)
    {
      _selectedIndexes.Clear();
      _selectedIndexes.Add(list.index);
    }

    private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
      var element = _rList.serializedProperty.GetArrayElementAtIndex(index);
      rect.y += 2;
      EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),element.FindPropertyRelative("m_Name").stringValue);

      if (EditorGUI.Toggle(
        new Rect(rect.width - EditorGUIUtility.singleLineHeight, rect.y, EditorGUIUtility.singleLineHeight,
          EditorGUIUtility.singleLineHeight)
        , _selectedIndexes.Contains(index)))
      {
        if (Event.current!=null && Event.current.shift)//Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
          for (int i = index; i >=0 && !_selectedIndexes.Contains(i); i--)
          {
            _selectedIndexes.Add(i);
          }
        }
        else
        {
          _selectedIndexes.Add(index);
        }
      }
      else
      {
        _selectedIndexes.Remove(index);
      }
    }
  }
}