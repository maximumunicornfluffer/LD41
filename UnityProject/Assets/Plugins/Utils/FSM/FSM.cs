using System;
using System.Collections;
using System.Collections.Generic;
using Plugins.Utils.FSM;
using UnityEngine;

public class FSM : MonoBehaviour
{
	public bool _debugLog;

	#region Singleton
	private static FSM s_instance = null;

	public static FSM Instance
	{
		get { return s_instance; }
	}
	#endregion

	void Start()
	{
		s_instance = this;
		Debug.Log("FSM Start");
	}
	
	private AbstractState _currentState=null;
	private AbstractState _nextState = null;
	
	// Update is called once per frame
	void Update () {
		if (_nextState != null && _currentState !=null &&_currentState.Status == StateStatus.Running)
		{
			Debug.LogWarning(_currentState.GetType()+ " OnLeave");
			_currentState.OnLeave();
		}

		if (_currentState != null && _currentState.Status == StateStatus.LeaveEnded)
		{
			_currentState = null;
		}

		if (_currentState == null && _nextState != null)
		{
			_currentState = _nextState;
			_nextState = null;
			Debug.LogWarning(_currentState.GetType()+ " OnStart");
			_currentState.OnEnter();
		}
		
		if(_currentState!=null && _currentState.Status ==StateStatus.Running)
			_currentState.Update();
		
	}

	public T GotoState<T>(List<string> dynamicSceneNames =null) where T : AbstractState, new()
	{
		if (_currentState != null && _currentState is T)
			return (T)_currentState;
		
		var state = new T();
		state.Init(dynamicSceneNames);
		_nextState = state;

		return state;
	}

	public void RestartState()
	{
		if (_currentState != null)
		{
			var state = (AbstractState)Activator.CreateInstance(_currentState.GetType());
			state.Init(_currentState.DynamicScenesName);
			_nextState = state;
		}
	}
	
	
}
