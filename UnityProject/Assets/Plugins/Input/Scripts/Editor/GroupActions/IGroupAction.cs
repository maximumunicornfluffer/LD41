using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;

namespace Assets.Scripts.Editor.GroupActions
{
  public interface IGroupAction
  {
    string Name { get; }
    bool Run(HashSet<int> indexes,  SerializedProperty axisArray);

    void OnGUI();
  }
}