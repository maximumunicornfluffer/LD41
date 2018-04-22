using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.UI
{
  public class CharactersSelectionUi : MonoBehaviour
  {
    [SerializeField] private CharacterSelectionUI[] _selections;

    private Dictionary<int,CharacterSelectionUI> _selByIndex = new Dictionary<int, CharacterSelectionUI>();
    private List<CharacterSelectionUI> _selectionsList;

    private void Awake()
    {
      _selectionsList = new List<CharacterSelectionUI>(_selections);
    }

    private void Update()
    {
      foreach (var playerInput in InputsManager.Instance.GetPlayerInputs())
      {
        if (!_selByIndex.ContainsKey(playerInput.InputIndex))
        {
          var ui = _selectionsList[0];
          _selectionsList.RemoveAt(0);
          
          ui.SetPlayerInput(playerInput);
          ui.gameObject.SetActive(true);
          _selByIndex.Add(playerInput.InputIndex, ui);
        }
      }
    }
  }
}