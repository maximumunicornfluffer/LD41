using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PlayerManagement
{
  public class PlayerInputUtils
  {
    public static bool GetButtonDown(string buttonName, int inputIndex)
    {
      return Input.GetButtonDown(GetButtonNameFor(buttonName, inputIndex));
    }

    public static string NameByIndexAndPadUsedType(int index, PadUsedType padUsedType)
    {
      return index + "_" + padUsedType;
    }

    public static HashSet<int> GetValidInputIndexes()
    {
      var validIndex = new HashSet<int>();

      var names = Input.GetJoystickNames();
      for (int i = 1; i <= names.Length; i++)
      {
        if (names[i - 1].Length == 0)
          continue;

        validIndex.Add(i);
      }

      return validIndex;
    }

    public static void test()
    {
      
    }


    public static string GetButtonNameFor(string buttonName, int playerIndex)
    {
      return "PAD_" + playerIndex + "_" + buttonName;
    }
  }
}