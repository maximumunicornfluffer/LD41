using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Assets.Scripts.Editor.GroupActions
{
  public class DeleteGroupAction : IGroupAction
  {
    private static readonly string NAME = "Delete";

    public string Name
    {
      get { return NAME; }
    }

    public bool Run(HashSet<int> indexes,  SerializedProperty axisArray)
    {
      if (EditorUtility.DisplayDialog("Delete Inputs ", "Do you really want to delete all selected?", "yes",
        "no"))
      {
        var l = indexes.ToList();
        l.Sort((x,y) => y.CompareTo(x));

        foreach (var index in l)
        {
          axisArray.DeleteArrayElementAtIndex(index);
        }
        return true;
      }

      return false;

    }

    public void OnGUI()
    {

    }
  }
}