using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.PlayerManagement;
using Plugins.Utils;
using Plugins.Utils.CustomEventMethods;
using Plugins.Utils.Extensions;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class InputsManager : MonoBehaviour ,IPreUpdate
{

  public bool DEBUG = true;
  
  public float LEAVE_PROCEDURE_DELAY = 3;

  public Transform NotificationContainer;
  public GameObject LeaveNotificationPrefab;

  public event Action<PlayerInput> OnNewPlayer;
  public event Action<PlayerInput> OnPlayerLeave;

  private Dictionary<string,PlayerInput> _playersByPadIndex = new Dictionary<string, PlayerInput>();
  private PlayerInput _mainPlayerInput;

  private HashSet<int> _activeIndex = new HashSet<int>();

  private Dictionary<int,float> _padIndexInProcedure = new Dictionary<int, float>();

  private Dictionary<int,GameObject> _notification = new Dictionary<int, GameObject>();

  private static InputsManager m_instance;

  private bool _autoUpdate = true;

  public static InputsManager Instance
  {
    get { return m_instance; }
  }

  public bool AutoUpdate
  {
    get { return _autoUpdate; }
    set { _autoUpdate = value; }
  }
  
  public HashSet<int> ActiveIndex
  {
    get { return _activeIndex; }
								
  }

  void Awake()
  {
    if (m_instance == null)
      m_instance = this;
  }

  private void Update()
  {
    if(_autoUpdate)
      InternalUpdate();
  }

  public void CustomPreUpdate()
  {
    if (_autoUpdate)
    {
      Debug.LogError("CustomPreUpdate is called with _autoUpdate to true");
      return;
    }
    
    InternalUpdate();
  }

  private void InternalUpdate ()
	{
	  foreach (PlayerInput pI in _playersByPadIndex.Values)
	  {
	    pI.CustomPreUpdate();
	  }


	  var validInputIndexes = PlayerInputUtils.GetValidInputIndexes();
	  foreach (int i in validInputIndexes)
	  {
	    HandleStart(i);
	    CheckPlayerActions(i);
	  }

	  var indexLeft = _activeIndex.Where(ai => !validInputIndexes.Contains(ai)).ToArray();
	  foreach (var i in indexLeft)
	  {
	    LeaveAllForIndex(i);
	  }
	}

  public void ForceCreateMainPlayer()
  {
    if (_mainPlayerInput == null)
    {
      CreatePlayer(1, PadUsedType.SINGLE);
    }
  }

  public Option<PlayerInput> MainPlayerInput
  {
    get { return Option<PlayerInput>.of(_mainPlayerInput); }
  }
  
  public void Reset()
  {
    _mainPlayerInput = null;
    _activeIndex.Clear();
    _padIndexInProcedure.Clear();
    _playersByPadIndex.Clear();
  }

  private void HandleStart(int index)
  {
    if (PlayerInputUtils.GetButtonDown(PlayerInput._START,index))
    {
      if (!_activeIndex.Contains(index))
      {
        CreatePlayer(index, PadUsedType.SINGLE);
      }
      else
      {
        if (!_padIndexInProcedure.ContainsKey(index))
        {
          _padIndexInProcedure.Add(index, Time.time);
          _notification.Add(index,(GameObject)Instantiate(LeaveNotificationPrefab, NotificationContainer));
        }
      }
    }
  }

  private void CheckPlayerActions(int index)
  {
    if (_padIndexInProcedure.ContainsKey(index))
    {

      var delay = Time.time - _padIndexInProcedure[index];
      if ( delay > LEAVE_PROCEDURE_DELAY)
      {
        _padIndexInProcedure.Remove(index);
        Destroy(_notification[index]);
        _notification.Remove(index);
      }
      else
      {
        foreach (var playerInput in _playersByPadIndex.Values.ToList())
        {
          if (!_padIndexInProcedure.ContainsKey(index))
            continue;

          CheckLeave(playerInput, index);
          CheckSplit(playerInput, index);
        }
      }
    }
  }

  private void CheckSplit(PlayerInput playerInput, int index)
  {
    if (playerInput.Select() && playerInput.Start() && playerInput.X())
    {
      if (playerInput.PadUsedType == PadUsedType.SINGLE)
      {
        ChangePadUsedTypeForPlayerInput(playerInput,PadUsedType.DUAL_LEFT);
        CreatePlayer(playerInput.InputIndex, PadUsedType.DUAL_RIGHT);
      }
    }
  }

  private void CheckLeave(PlayerInput playerInput, int index)
  {
    if (
      playerInput.Start( ) &&
      playerInput.Select( ) &&
      playerInput.A( )
    )
    {
       Leave(playerInput, index);
    }
  }

  private void LeaveAllForIndex(int index)
  {
    foreach (PadUsedType type in Enum.GetValues(typeof(PadUsedType)))
    {
      var name = PlayerInputUtils.NameByIndexAndPadUsedType(index, type);
      if (_playersByPadIndex.ContainsKey(name))
      {
        Leave(_playersByPadIndex[name],index);
      }
    }
  }

  private void Leave(PlayerInput playerInput, int index)
  {

    _playersByPadIndex.Remove(playerInput.Name);
    if (playerInput.PadUsedType != PadUsedType.SINGLE)
    {
      var otherName = PlayerInputUtils.NameByIndexAndPadUsedType(index,PadUsedTypeUtils.GetOtherSide(playerInput.PadUsedType));
      var other = _playersByPadIndex[otherName];
      ChangePadUsedTypeForPlayerInput(other,PadUsedType.SINGLE);

    }
    else
    {
      _activeIndex.Remove(index);
    }


    _padIndexInProcedure.Remove(index);

    if (_notification.ContainsKey(index))
    {
      Destroy(_notification[index]);
      _notification.Remove(index);
    }

    if (_mainPlayerInput == playerInput)
    {
      if (_activeIndex.Count > 0)
        _mainPlayerInput = _playersByPadIndex.Values.First();
      else
        _mainPlayerInput = null;

    }
    
    OnPlayerLeave?.Invoke(playerInput);
  }

  private void CreatePlayer(int padIndex, PadUsedType type)
  {
    _activeIndex.Add(padIndex);

      var playerInput = new PlayerInput(padIndex, type);
      _playersByPadIndex.Add(PlayerInputUtils.NameByIndexAndPadUsedType(padIndex,type),playerInput);
      OnNewPlayer?.Invoke(playerInput);

    if (_mainPlayerInput == null)
    {
      _mainPlayerInput = playerInput;
    }
  }

  private void ChangePadUsedTypeForPlayerInput(PlayerInput pInput, PadUsedType to)
  {
    _playersByPadIndex.Remove(pInput.Name);
    pInput.PadUsedType = to;
    _playersByPadIndex.Add(pInput.Name, pInput);
  }
  
  



#if UNITY_EDITOR
  void OnGUI()
  {
    if (!DEBUG)
      return;
    
    EditorGUILayout.LabelField("Player Count",""+_playersByPadIndex.Count);
    EditorGUILayout.LabelField("Player in Remove procedure Count",""+_padIndexInProcedure.Count);
    
    EditorGUILayout.LabelField("Player main index",""+((_mainPlayerInput!=null)?_mainPlayerInput.Name:""));

    foreach (var pInput in _playersByPadIndex.Values)
    {
        EditorGUILayout.LabelField("Name "+pInput.Name + " : "+pInput.DebugInputString());
    }
  }
#endif

}
