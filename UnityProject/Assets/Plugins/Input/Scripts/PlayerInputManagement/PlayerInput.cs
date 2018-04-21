using System;
using System.Collections.Generic;
using Plugins.Utils.CustomEventMethods;
using UnityEngine;

namespace Assets.Scripts.PlayerManagement
{
  public class PlayerInput : IPreUpdate
  {
    public const string _START = "Start";
    public const string _SELECT = "Select";
    public const string _A = "A";
    public const string _B = "B";
    public const string _X = "X";
    public const string _Y = "Y";
    public const string _LB = "LB";
    public const string _RB = "RB";
    public const string _LS = "LS";
    public const string _RS = "RS";

    public const string _H_1 = "H_1";
    public const string _V_1 = "V_1";
    public const string _H_4 = "H_4";
    public const string _V_4 = "V_4";

    private int _inputIndex;
    private PadUsedType _padUsedType = PadUsedType.SINGLE;


    public static float SENSIBILITY = 0.25f;
    

    public PlayerInput(int inputIndex, PadUsedType type)
    {
      _inputIndex = inputIndex;
      _padUsedType = type;
    }

    public int InputIndex
    {
      get { return _inputIndex; }
    }

    public PadUsedType PadUsedType
    {
      get { return _padUsedType; }
      set { _padUsedType = value; }
    }

    public string Name{get { return PlayerInputUtils.NameByIndexAndPadUsedType(_inputIndex, _padUsedType); }}

    
    private HashSet<string> _wasActiveAxis = new HashSet<string>();
    private HashSet<string> _activeAxis = new HashSet<string>();

    public void CustomPreUpdate()
    {
      _wasActiveAxis = _activeAxis;
      _activeAxis = new HashSet<string>();
      if (GetAxisAsButton(_H_1, true))
        _activeAxis.Add(_H_1 + "_D");
      if(GetAxisAsButton(_V_1, false))
        _activeAxis.Add(_V_1 + "_D");
      if(GetAxisAsButton(_H_1, false))
        _activeAxis.Add(_H_1+"_U");
      if(GetAxisAsButton(_V_1, true))
        _activeAxis.Add(_V_1+"_U");

    }
    
    public delegate bool GetInputMethode();
    public Dictionary<string, GetInputMethode> _externalButtonDown = new Dictionary<string, GetInputMethode>();
    

    public void AddButtonDownTrigger(string inputName, GetInputMethode getInputMethod)
    {
      _externalButtonDown.Add(inputName,getInputMethod);
    }
    
    public Dictionary<string, GetInputMethode> _externalButton = new Dictionary<string, GetInputMethode>();
    
    public void AddButtonTrigger(string inputName, GetInputMethode getInputMethod)
    {
      _externalButton.Add(inputName,getInputMethod);
    }

    public void ResetExternalTrigger()
    {
      _externalButton.Clear();
      _externalButtonDown.Clear();
    }

    // AXIS

    // DOWN
    public bool GetButtonDown(String buttonName)
    {
      if (_externalButtonDown.ContainsKey(buttonName))
        return _externalButtonDown[buttonName]() || Input.GetButtonDown(PlayerInputUtils.GetButtonNameFor(buttonName, _inputIndex));
      
      return Input.GetButtonDown(PlayerInputUtils.GetButtonNameFor(buttonName, _inputIndex));
    }

    public bool StartDown()
    {
      return GetButtonDown(_START);
    }

    public bool SelectDown()
    {
      return GetButtonDown(_SELECT);
    }
    public bool ADown()
    {
      if (_padUsedType == PadUsedType.DUAL_RIGHT || _padUsedType == PadUsedType.SINGLE)
        return GetButtonDown(_A);
      else
        return GetAxisAsButton(_H_4,true);
    }
    public bool BDown()
    {
      return GetButtonDown(_B);
    }
    public bool XDown()
    {
      return GetButtonDown(_X);
    }
    public bool YDown()
    {
      return GetButtonDown(_Y);
    }
    public bool LBDown()
    {
      return GetButtonDown(_LB);
    }
    public bool RBDown()
    {
      return GetButtonDown(_RB);
    }
    public bool LSDown()
    {
      return GetButtonDown(_LS);
    }
    public bool RSDown()
    {
      return GetButtonDown(_RS);
    }

    // STATE

    public float H1()
    {
      return Input.GetAxis(PlayerInputUtils.GetButtonNameFor(_H_1, _inputIndex));
    }

    public float V1()
    {
      return -Input.GetAxis(PlayerInputUtils.GetButtonNameFor(_V_1, _inputIndex));
    }
    
    public bool H1UpButton()
    {
      return GetAxisAsButton(_H_1);
    }

    public bool V1UpButton()
    {
      return GetAxisAsButton(_V_1, true);
    }
    
    public bool H1DownButton()
    {
      return GetAxisAsButton(_H_1, true);
    }

    public bool V1DownButton()
    {
      return GetAxisAsButton(_V_1);
    }
    
    public bool H1UpButtonDown()
    {
      return _activeAxis.Contains(_H_1+"_U") && !_wasActiveAxis.Contains(_H_1+"_U");
    }

    public bool V1UpButtonDown()
    {     
      return _activeAxis.Contains(_V_1+"_U") && !_wasActiveAxis.Contains(_V_1+"_U");
    }
    
    public bool H1DownButtonDown()
    {
      if (_externalButtonDown.ContainsKey(_H_1+"_D"))
        return _externalButtonDown[_H_1+"_D"]() || (_activeAxis.Contains(_H_1+"_D") && !_wasActiveAxis.Contains(_H_1+"_D"));
      
      return _activeAxis.Contains(_H_1+"_D") && !_wasActiveAxis.Contains(_H_1+"_D");
    }

    public bool V1DownButtonDown()
    {
      if (_externalButtonDown.ContainsKey(_V_1 + "_D"))
        return _externalButtonDown[_V_1 + "_D"]() ||
               (_activeAxis.Contains(_V_1 + "_D") && !_wasActiveAxis.Contains(_V_1 + "_D"));
      return _activeAxis.Contains(_V_1+"_D") && !_wasActiveAxis.Contains(_V_1+"_D");
    }

    private bool GetAxisAsButton(String buttonName, bool neg = false)
    {
      if(!neg)
        return Input.GetAxis(PlayerInputUtils.GetButtonNameFor(buttonName, _inputIndex)) > SENSIBILITY;
      else
        return Input.GetAxis(PlayerInputUtils.GetButtonNameFor(buttonName, _inputIndex)) < -SENSIBILITY;
    }

    public bool GetButton(String buttonName)
    {
      if (_externalButton.ContainsKey(buttonName))
        return _externalButton[buttonName]() || Input.GetButton(PlayerInputUtils.GetButtonNameFor(buttonName, _inputIndex));
      
      return Input.GetButton(PlayerInputUtils.GetButtonNameFor(buttonName, _inputIndex));
    }

    public bool Start()
    {
      return GetButton(_START);
    }

    public bool Select()
    {
      return GetButton(_SELECT);
    }
    public bool A()
    {
      if (_padUsedType == PadUsedType.DUAL_RIGHT || _padUsedType == PadUsedType.SINGLE)
        return GetButton(_A);
      else
        return GetAxisAsButton(_H_4,true);
    }
    public bool B()
    {
      if (_padUsedType == PadUsedType.DUAL_RIGHT || _padUsedType == PadUsedType.SINGLE)
        return GetButton(_B);
      else
        return GetAxisAsButton(_V_4,true);
    }
    public bool X()
    {
      if (_padUsedType == PadUsedType.DUAL_RIGHT || _padUsedType == PadUsedType.SINGLE)
        return GetButton(_X);
      else
        return GetAxisAsButton(_V_4, false);
    }
    public bool Y()
    {
      if (_padUsedType == PadUsedType.DUAL_RIGHT || _padUsedType == PadUsedType.SINGLE)
        return GetButton(_Y);
      else
        return GetAxisAsButton(_H_4,false);
    }
    public bool LB()
    {
      return GetButton(_LB);
    }
    public bool RB()
    {
      return GetButton(_RB);
    }
    public bool LS()
    {
      return GetButton(_LS);
    }
    public bool RS()
    {
      return GetButton(_RS);
    }

    #if UNITY_EDITOR
    public string DebugInputString()
    {
      return "" +
             (A() ? " A" : "") +
             (B() ? " B" : "") +
             (X() ? " X" : "") +
             (Y() ? " Y" : "");
    }
    #endif


  }
}