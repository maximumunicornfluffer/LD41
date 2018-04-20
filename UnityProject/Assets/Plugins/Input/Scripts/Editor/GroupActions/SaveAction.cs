using System.Collections.Generic;
using UnityEditor;

namespace Assets.Scripts.Editor.GroupActions
{
  public class SaveAction : IGroupAction
  {
    public string Name {get { return "Save"; } }
    
    public bool Run(HashSet<int> indexes, SerializedProperty axisArray)
    {
      foreach (var index in indexes)
      {
        var property = axisArray.GetArrayElementAtIndex(index);


        JoystickTool._savedInputs.AddInput(property);

      }

      AssetDatabase.SaveAssets();
      return true;
    }

    public void OnGUI()
    {
    }
  }
}