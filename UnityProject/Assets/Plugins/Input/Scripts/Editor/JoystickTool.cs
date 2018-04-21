using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Editor.GroupActions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UI;

public class JoystickTool : EditorWindow
{
  private ActionGroupManager _actionGroupManager;

  private Vector2 _scrollPosition;

  private SerializedObject _inputManager;

  private bool _modified;

  private JoystickReorderedList _list;

  public static Inputs _savedInputs=null;
  private int _importSelectedIndex = 0;

  [MenuItem("Lim/Joystick tool")]
  public static void GetJoystickTool()
  {
    JoystickTool w = (JoystickTool) EditorWindow.GetWindow<JoystickTool>();
    w.Show();
  }

  private void InitInputList()
  {
    
    if (_inputManager == null)
    {
      var inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
      _inputManager = new SerializedObject(inputManager);
      _modified = false;
      
      _list = new JoystickReorderedList(_inputManager);
      
      SerializedProperty axisArray = _inputManager.FindProperty("m_Axes");
    }
  }

  private void InitActionGroups()
  {
    if (_actionGroupManager == null)
    {
      _actionGroupManager = new ActionGroupManager();
      _actionGroupManager.Init();

    }
  }
  
  private void LoadSavedInput()
  {
    if (_savedInputs == null)
    {
      var assets = AssetDatabase.LoadAllAssetsAtPath("Assets/Plugins/Input/Editor/SavedInput.asset");
      if (assets.Length <= 0)
      {
        _savedInputs = ScriptableObject.CreateInstance<Inputs>();
        AssetDatabase.CreateAsset(_savedInputs, "Assets/Plugins/Input/Editor/SavedInput.asset");
        AssetDatabase.SaveAssets();
      }
      else
      {
        _savedInputs = assets[0] as Inputs;
      }
    }
  }
  

  void OnGUI()
  {
    InitInputList();
    InitActionGroups();
    LoadSavedInput();
    

    SerializedProperty axisArray = _inputManager.FindProperty("m_Axes");


    EditorGUILayout.BeginHorizontal();

//    if (_inputManager != null && GUILayout.Button("load Default inputs"))
//    {
//      AssetDatabase.CopyAsset("ProjectSettings/InputManager.asset", "ProjectSettings/InputManager.backup");
//      AssetDatabase.CopyAsset("Assets/Plugins/Input/Default/InputManager.asset~", "ProjectSettings/InputManager.asset");
//      AssetDatabase.SaveAssets();
//      _inputManager = null;
//      InitInputList();
//
//    }

    if(_savedInputs !=null)
    {
      _importSelectedIndex = EditorGUILayout.Popup(_importSelectedIndex, _savedInputs.Get());
      if (GUILayout.Button("import"))
      {
        axisArray.InsertArrayElementAtIndex(axisArray.arraySize);
        var p = axisArray.GetArrayElementAtIndex(axisArray.arraySize - 1);
        _savedInputs.Extract(p, _importSelectedIndex);
      }
    }


    if (_inputManager != null && _modified && GUILayout.Button("save"))
    {
      _inputManager.ApplyModifiedProperties();
      //AssetDatabase.CreateAsset(_inputManager.targetObject,"Assets/Plugins/Input/Default/tmp.asset");
      Debug.LogError(Application.dataPath);
      //File.Copy(,true);
      
      AssetDatabase.SaveAssets();
      _modified = false;
    }


    EditorGUILayout.EndHorizontal();

    //GroupActionMenu();


    if (_inputManager != null)
    {
      EditorGUILayout.BeginHorizontal();

      EditorGUILayout.BeginVertical(GUILayout.Width(250));
//        if (_inputManager != null && GUILayout.Button("+"))
//        {
//          axisArray.InsertArrayElementAtIndex(axisArray.arraySize);
//          _modified = true;
//        }
        DisplayInputList(axisArray);
      EditorGUILayout.EndVertical();

      EditorGUILayout.BeginVertical();
      if (_list.SelectedIndexes.Count == 1)
      {
        DisplaySelection(axisArray);
      }else if (_list.SelectedIndexes.Count > 1)
      {
        DisplayMultiSelection(axisArray);
      }

      if(_list.SelectedIndexes.Count >= 1)
        _modified|=_actionGroupManager.OnGUI(_list.SelectedIndexes,axisArray);

      EditorGUILayout.EndVertical();

      EditorGUILayout.EndHorizontal();
    }
  }

  private void DisplayInputList(SerializedProperty axisArray)
  {
    _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
   
    _modified |= _list.OnGUI();
    _inputManager.ApplyModifiedProperties();
    
//    for (int i = 0; i < axisArray.arraySize; i++)
//    {
//      var axis = axisArray.GetArrayElementAtIndex(i);
//
//      var name = axis.FindPropertyRelative("m_Name").stringValue;
//
//      EditorGUILayout.BeginHorizontal();
//      if (GUILayout.Toggle(_selectedIndexes.Contains(i), "",GUILayout.Width(20)))
//      {
//        _selectedIndexes.Add(i);
//      }
//      else
//      {
//        _selectedIndexes.Remove(i);
//      }
//
//      if (GUILayout.Button(name))
//      {
//        _selectedIndexes.Clear();
//        _selectedIndexes.Add(i);
//      }
//      if (i != 0 && GUILayout.Button("^", GUILayout.Width(20)))
//      {
//        axisArray.MoveArrayElement(i, i - 1);
//        _modified = true;
//      }
//      if (i != axisArray.arraySize - 1 && GUILayout.Button("v", GUILayout.Width(20)))
//      {
//        axisArray.MoveArrayElement(i, i + 1);
//        _modified = true;
//      }
//      if (GUILayout.Button("-", GUILayout.Width(20)))
//      {
//        if (EditorUtility.DisplayDialog("Delete Input " + name, "Do you really want to delete " + name + "?", "yes",
//          "no"))
//        {
//          axisArray.DeleteArrayElementAtIndex(i);
//          _modified = true;
//        }
//      }
//      EditorGUILayout.EndHorizontal();
//    }
    EditorGUILayout.EndScrollView();
  }

  private void DisplayMultiSelection(SerializedProperty axisArray)
  {
    EditorGUILayout.BeginVertical("box");
    foreach (var index in _list.SelectedIndexes)
    {
      var currentInput = axisArray.GetArrayElementAtIndex(index);
      EditorGUILayout.LabelField(currentInput.FindPropertyRelative("m_Name").stringValue);
    }


    EditorGUILayout.EndVertical();
  }


  private void DisplaySelection(SerializedProperty axisArray)
  {
    EditorGUILayout.BeginVertical("box");
    var first = _list.SelectedIndexes.First();
    var currentInput = first<axisArray.arraySize?axisArray.GetArrayElementAtIndex(_list.SelectedIndexes.First()):null;
    if (currentInput != null)
    {
      //Debug.Log(_currentInput.FindPropertyRelative("m_Name").stringValue);

      var enumerator = currentInput.Copy().GetEnumerator();

      while (enumerator.MoveNext())
      {
        var serializedProperty = enumerator.Current as SerializedProperty;

        if (serializedProperty != null)
        {
          if (serializedProperty.propertyType == SerializedPropertyType.String)
          {
            var newValue = EditorGUILayout.TextField(serializedProperty.name, serializedProperty.stringValue);
            if (newValue != serializedProperty.stringValue)
            {
              serializedProperty.stringValue = newValue;
              _modified = true;
            }
          }
          else if (serializedProperty.propertyType == SerializedPropertyType.Float)
          {
            var newValue = EditorGUILayout.FloatField(serializedProperty.name, serializedProperty.floatValue);
            if (newValue != serializedProperty.floatValue)
            {
              serializedProperty.floatValue = newValue;
              _modified = true;
            }
          }
          else if (serializedProperty.propertyType == SerializedPropertyType.Boolean)
          {
            var newValue = EditorGUILayout.Toggle(serializedProperty.name, serializedProperty.boolValue);
            if (newValue != serializedProperty.boolValue)
            {
              serializedProperty.boolValue = newValue;
              _modified = true;
            }
          }
          else if (serializedProperty.propertyType == SerializedPropertyType.Enum)
          {
            var newValue = EditorGUILayout.Popup(serializedProperty.name, serializedProperty.enumValueIndex,
              serializedProperty.enumNames);
            if (newValue != serializedProperty.enumValueIndex)
            {
              serializedProperty.enumValueIndex = newValue;
              _modified = true;
            }
          }
          else
          {
            Debug.Log("6" + serializedProperty.propertyType + " " + serializedProperty.name);
          }
        }
      }
    }
    EditorGUILayout.EndVertical();
  }
}