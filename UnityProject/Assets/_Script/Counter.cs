namespace DefaultNamespace
{
    public class Counter : Machine
    {
        protected override void OnStateChanged(MachineStates state)
        {
            if (InputTypes.Count == 0)
                return;
            if (state == MachineStates.FULL)
                GameManager.Instance.ClientManager.GiveStuff(InputTypes[0]);
        }
    }
}