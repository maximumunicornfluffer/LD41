using System.Collections.Generic;
using DefaultNamespace.UI;
using States;
using Ui.Game;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
  public class Lobby : MonoBehaviour
  {
    [SerializeField] public string[] _racesNames;

    [SerializeField] private CharactersSelectionUi m_characterSelection;
    
    private void Start()
    {
      InputsManager.Instance.AutoUpdate = true;
      
      List<ChoiceInformation> choices= new List<ChoiceInformation>();
      foreach (var raceName in _racesNames)
      {
        var listener = new UnityEvent();
        listener.AddListener(()=> FSM.Instance.GotoState<GameState>(new List<string>(){raceName}));
        choices.Add(new ChoiceInformation(raceName,listener ));
      }
      
      m_characterSelection.SetInformation(choices[0]);
    }
  }
}