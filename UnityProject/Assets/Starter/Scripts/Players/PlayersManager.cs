using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.PlayerManagement;
using UnityEngine;

namespace DefaultNamespace
{
  public class PlayersManager : MonoBehaviour
  {
    #region Singleton

    private static PlayersManager s_instance;

    public static PlayersManager Instance
    {

      get
      {
        return s_instance;
      }
    }

    #endregion
    
    public List<Color> _availableColors = new List<Color>(){Color.red,Color.green,Color.yellow, Color.blue};
    private Dictionary<PlayerInput,Player> _players = new Dictionary<PlayerInput, Player>();
    
    
    void Start()
    {
      s_instance = this;
      InputsManager.Instance.Reset();
      InputsManager.Instance.OnNewPlayer += OnNewPlayer;
      InputsManager.Instance.OnPlayerLeave += OnPlayerLeave;
    }

    public List< Player> Players
    {
      get { return _players.Values.ToList(); }
    }

    private void OnPlayerLeave(PlayerInput input)
    {
      if (_players.ContainsKey(input))
      {
        var p = _players[input];
        _players.Remove(input);
        _availableColors.Add(p.Color);
      }
    }

    private void OnNewPlayer(PlayerInput input)
    {
      if (_availableColors.Count > 0)
      {
        _players.Add(input, new Player(_availableColors[0], input));
        _availableColors.RemoveAt(0);
      }
    }
  }
}