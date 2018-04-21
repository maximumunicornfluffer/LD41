using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Editor.GroupActions
{
  public class ChangeJoystickGroupAction : IGroupAction
  {
    private int _joystickIndex = 0;

    public string Name { get { return "Change Joystick"; } }

    public bool Run(HashSet<int> indexes, SerializedProperty axisArray)
    {
      foreach (var index in indexes)
      {
        var property = axisArray.GetArrayElementAtIndex(index);

        var positiveButton = property.FindPropertyRelative("positiveButton").stringValue;
        var match = Regex.Match(positiveButton, "(joystick )([0-9]{0,2} |)(button [0-9]{1,2})");
        if (match.Success)
        {
          Debug.Log(match.Groups[0].Value+"|"+match.Groups[1].Value+"|"+match.Groups[2].Value);

          property.FindPropertyRelative("positiveButton").stringValue = match.Groups[1].Value + (_joystickIndex==0?"":_joystickIndex+" ") +
                                                                        match.Groups[3].Value;
        }


        property.FindPropertyRelative("joyNum").enumValueIndex = _joystickIndex;
      }

      return true;
    }

    public void OnGUI()
    {
      _joystickIndex = EditorGUILayout.IntField("joystick Index", _joystickIndex);
    }
  }
}