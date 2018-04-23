namespace DefaultNamespace
{
    public class Counter : Machine
    {

		private ClientManager _clientManager;

        protected override void OnStateChanged(MachineStates state)
        {
            base.OnStateChanged(state);
            if (InputTypes.Count == 0)
                return;
            if (state == MachineStates.FULL)
                GameManager.Instance.ClientManager.GiveStuff(InputTypes[0]);
        }

		public bool ClientAvailable() {
			_clientManager = GameManager.Instance.ClientManager;
			return _clientManager.ClientExists();
		}
    }
}