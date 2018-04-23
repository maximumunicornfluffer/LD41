using Plugins.Utils.FSM;
using Ui.Game;

namespace States
{
	public class TutoState :AbstractState
	{
		public override string[] GetStaticSceneName()
		{
			return new  string[]{"Tuto"};
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