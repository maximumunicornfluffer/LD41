using System.Collections.Generic;
using UnityEditor;

namespace Assets.Scripts.Editor.GroupActions
{
  public class DuplicateGroupAction:IGroupAction
  {
    public string Name { get { return "Duplicate"; } }
    public bool Run(HashSet<int> indexes, SerializedProperty axisArray)
    {
      foreach (var index in indexes)
      {
        axisArray.InsertArrayElementAtIndex(index);
        axisArray.MoveArrayElement(index, axisArray.arraySize - 1);
      }

      return true;
    }

    public void OnGUI()
    {

    }
  }
}