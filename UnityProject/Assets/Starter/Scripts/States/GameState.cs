using JetBrains.Annotations;
using Plugins.Utils.FSM;

namespace States
{
  public class GameState : AbstractState
  {
    
    public override string[] GetStaticSceneName()
    {
      return new  string[]{"GlobalRace"};
    }
  }
}