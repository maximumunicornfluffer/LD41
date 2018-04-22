using UnityEngine;

namespace DefaultNamespace.UI
{
  public class ActivePlayersUI : MonoBehaviour
  {
    [SerializeField] private GameObject[] _players;
    private Animator _animator;


    void Awake()
    {
      _animator = GetComponent<Animator>();
    }

    private void Update()
    {
      if (InputsManager.Instance != null && !InputsManager.Instance.MainPlayerInput.IsPresent)
      {
        InputsManager.Instance.ForceCreateMainPlayer();
      }

      var show  = false;
      foreach (var playerInput in InputsManager.Instance.GetPlayerInputs())
      {
        if (playerInput.Start())
          show |= true;
      }
      
      _animator.SetBool("show", show);
      
      for (int i = 0 ;i < _players.Length; i++)
      {
        if (InputsManager.Instance.ActiveIndex.Contains(i + 1))
        {
          _players[i].SetActive(true);         
        }
        else
          _players[i].SetActive(false);
      }
    }
  }
}