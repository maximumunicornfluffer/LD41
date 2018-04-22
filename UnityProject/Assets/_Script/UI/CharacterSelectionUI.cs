using Assets.Scripts.PlayerManagement;
using UnityEngine;

namespace DefaultNamespace.UI
{
  public class CharacterSelectionUI : MonoBehaviour
  {
    [SerializeField]
    private Animator _animator;

    private PlayerInput _input;
    private int _index;

    public void SetPlayerInput(PlayerInput input)
    {
      _input = input;
    }

    private void Update()
    {
      if (_input == null)
        return;

      if (_input.H1UpButtonDown())
        IncIndex();
      
      if (_input.H1DownButtonDown())
        DecIndex();
    }

    private void IncIndex()
    {
      _index++;
      if (_index > 3)
        _index = 0;

      Apply();
    }
    
    private void DecIndex()
    {
      _index--;
      if (_index < 0)
        _index = 3;
      
      Apply();
    }

    private void Apply()
    {
      _animator.SetInteger("Index",_index);
      PlayersManager.Instance.GetPlayer(_input).CharacterSelectionIndex=_index;
    }

  }
}