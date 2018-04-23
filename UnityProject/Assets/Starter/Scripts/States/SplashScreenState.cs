using System.Collections.Generic;
using System.Runtime.InteropServices;
using Plugins.Utils.FSM;

namespace States
{
	public class SplashScreenState : AbstractState
	{

		public override string[] GetStaticSceneName()
		{
			return new  string[]{"Splash Screen"};
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