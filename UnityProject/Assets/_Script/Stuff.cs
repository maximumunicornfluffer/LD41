namespace DefaultNamespace
{
    public enum StuffType
    {
        None,
        Water,
        Potatoes,
        Fries,
        Corpes,
        Cinder,
    }

    public enum StuffSubType
    {
        None,
        Corpse1, Corpse2, Corpse3
    }

    public enum StuffState
    {
        OnTheGround,
        PickedUp,
    }
    
    public class Stuff : IE
    {
        public StuffType m_type;
        public StuffSubType m_subType = StuffSubType.None;
        
        public override IEType Type => IEType.Resource;

        private StuffState _mStuffState = StuffState.PickedUp;

        public StuffState StuffState
        {
            get { return _mStuffState; }
            set { _mStuffState = value;}
        }

        protected override void Highlight(bool highlight)
        {
            
        }

        public void PickUp()
        {
            // TODO : Despawn animation
            _mStuffState = StuffState.PickedUp;
            Destroy(gameObject);
        }

        public override void UpdateLoop()
        {
            
        }
    }
}