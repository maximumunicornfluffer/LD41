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

    public enum StuffState
    {
        OnTheGround,
        PickedUp,
    }
    
    public class Stuff : IE
    {
        public StuffType m_type;
        
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
            _mStuffState = StuffState;
            Destroy(gameObject);
        }

        public override void UpdateLoop()
        {
            
        }
    }
}