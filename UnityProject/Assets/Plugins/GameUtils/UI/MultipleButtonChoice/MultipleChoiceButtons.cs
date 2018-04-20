using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ui.Game
{
  public class MultipleChoiceButtons : MonoBehaviour
  {
    [SerializeField]
    private ChoiceButton _buttonPrefab;
    
    [SerializeField]
    private ChoiceInformation[] _choiceButtons;
    
    private List<ChoiceButton> _buttons = new List<ChoiceButton>();
    
    private Dictionary<string,UnityEvent> _eventByButtonName = new Dictionary<string, UnityEvent>();
    
    private int _currentSelectedButtonIndex = 0;

    private bool _initilized = false;
    
    // Use this for initialization
    void Start ()
    {
      InternalInit();
      _initilized = true;
    }

    public void Init(List<ChoiceInformation> choiceInformations)
    {

      _choiceButtons = choiceInformations.ToArray();
      if (_initilized)
      {
        Clear();
        InternalInit();
      }
    }

    public void Clear()
    {
      _currentSelectedButtonIndex = 0;
      _eventByButtonName.Clear();
      foreach (var button in _buttons)
      {
        DestroyImmediate(button);
      }
      _buttons.Clear();
    }

    private void InternalInit()
    {
      Clear();
      if (_choiceButtons != null && _choiceButtons.Length > 0)
      {
        var idx = 0;
        foreach (ChoiceInformation info in _choiceButtons)
        {
          var button = Instantiate(_buttonPrefab, transform);
          _buttons.Add(button);
          button.Init(idx, info.Name);
          button.OnPointerEnterActon = OnButtonPointerEnter;
          button.OnPointerClickActon = OnButtonPointerClick;
          _eventByButtonName.Add(info.Name, info.Event);
          idx++;
        }

        _buttons[0].Select(true);
      }
    }

    void Update ()
    {
      CheckSelectionChange();
    }

    private void CheckSelectionChange()
    {
      if (_buttons.Count == 0)
        return;

      var mainPlayerInput = InputsManager.Instance.MainPlayerInput;
      if (mainPlayerInput.IsPresent)
      {
        var newRaceIndex = _currentSelectedButtonIndex;
        if (mainPlayerInput.Get.V1UpButtonDown() || mainPlayerInput.Get.H1UpButtonDown()||Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.UpArrow))
        {
          newRaceIndex--;
          if (newRaceIndex < 0)
            newRaceIndex = _buttons.Count - 1;
        }
        if (mainPlayerInput.Get.V1DownButtonDown() || mainPlayerInput.Get.H1DownButtonDown() ||Input.GetKeyDown(KeyCode.RightArrow)||Input.GetKeyDown(KeyCode.DownArrow))
        {
          newRaceIndex++;
          if (newRaceIndex >= _buttons.Count)
            newRaceIndex = 0;
        }

        if (_currentSelectedButtonIndex != newRaceIndex)
        {
          _buttons[_currentSelectedButtonIndex].Select(false);
          _buttons[newRaceIndex].Select(true);
          _currentSelectedButtonIndex = newRaceIndex;

        }

        if (mainPlayerInput.Get.ADown())
        {
          OnButtonPointerClick(_currentSelectedButtonIndex);
        }
      }
    }

    private void OnButtonPointerClick(int idx)
    {
      var currentName = _buttons[idx].Name;
      _eventByButtonName[currentName].Invoke();
    }
    
    private void OnButtonPointerEnter(int idx)
    {
      _buttons[_currentSelectedButtonIndex].Select(false);
      _currentSelectedButtonIndex = idx;
      _buttons[_currentSelectedButtonIndex].Select(true);
    }
  }

  [Serializable] 
  public class ChoiceInformation
  {
    [SerializeField] private string _name;
    [SerializeField] private UnityEvent _action;

    public ChoiceInformation(string name, UnityEvent action)
    {
      _name = name;
      _action = action;
    }

    public string Name
    {
      get { return _name; }
    }

    public UnityEvent Event
    {
      get { return _action; }
    }
  }
}