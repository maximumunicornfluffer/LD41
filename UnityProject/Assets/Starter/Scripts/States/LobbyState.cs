using System.Collections.Generic;
using System.Runtime.InteropServices;
using Plugins.Utils.FSM;

namespace States
{
  public class LobbyState :AbstractState
  {

    public override string[] GetStaticSceneName()
    {
      return new  string[]{"Menu"};
    }

    public override void OnEnter() 
    {
      base.OnEnter();
    }

    public override void OnLeave()
    {
      base.OnLeave();
    }
  }
}