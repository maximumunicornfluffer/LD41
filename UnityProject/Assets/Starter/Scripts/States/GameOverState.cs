using System.Collections.Generic;
using System.Runtime.InteropServices;
using Plugins.Utils.FSM;

namespace States
{
	public class GameOverState :AbstractState
	{

		public override string[] GetStaticSceneName()
		{
			return new  string[]{"Game Over"};
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