using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace DefaultNamespace
{
    
    public class Machine : IE
    {
        [SerializeField] private StuffInHandDisplayer _stuffInHandDisplayer;
        public List<StuffType> InputTypes;
        public StuffType OutputType = StuffType.None;
        public MachineStates m_startState = MachineStates.IDLE;
        public float processingDuration;
        
        
        public override IEType Type { get { return IEType.Machine; } }

        private HashSet<StuffType> m_stuffs = new HashSet<StuffType>();
        
        private MachineStates m_state = MachineStates.IDLE;
        private float m_processingEndTime;

        private Animator m_animator;
        
        protected override void Awake()
        {
            base.Awake();
            m_animator = GetComponent<Animator>();
            State = m_startState;
            UpdateDisplay();
        }

        public MachineStates State
        {
            get { return m_state; }
            set
            {
                if (m_state == value)
                    return;
                m_state = value;
                UpdateHighlight();
                OnStateChanged(m_state);
            }
        }

        protected virtual void OnStateChanged(MachineStates state)
        {
            UpdateDisplay();
        }
        
        private void UpdateHighlight()
        {
//            switch (m_state)
//            {
//                case MachineStates.IDLE:
//                case MachineStates.HALF_FILLED:
//                    m_spriteRenderer.color = Color.white;
//                    break;
//                case MachineStates.PROCESSING:
//                    m_spriteRenderer.color = Color.yellow;
//                    break;
//                case MachineStates.FULL:
//                    m_spriteRenderer.color = Color.green;
//                    break;
//            }
        }

        public void Update()
        {
            if (m_animator)
            {
                var state = (int) m_state;
                var animState = m_animator.GetInteger("State");
                if (state != animState)
                    m_animator.SetInteger("State", state);
            }            
        }
        
        public bool IsIdle
        {
            get { return State == MachineStates.IDLE || State == MachineStates.HALF_FILLED; }
        }
        
        private void UpdateDisplay()
        {
            if (InputTypes.Count != 0 && IsIdle && !HasAllIngredients())
            {
                var neededStuff = NeededStuff();
                _stuffInHandDisplayer.SetStuffInHand(neededStuff);
                return;
            }
            
            _stuffInHandDisplayer.SetStuffInHand(StuffType.None);
        }

        public StuffType NeededStuff()
        {
            foreach (var neededStuff in InputTypes)
            {
                if (!m_stuffs.Contains(neededStuff))
                {
                    return neededStuff;
                }
            }

            return StuffType.None;
        }

        public StuffType Activate(StuffType stuff)
        {
            if (IsIdle && InputTypes.Count != 0 && NeededStuff() == stuff)
            {
                m_stuffs.Add(stuff);
                UpdateDisplay();
            }
            
            if (m_stuffs.Count > 0 && !HasAllIngredients())
                State = MachineStates.HALF_FILLED;

            if (State < MachineStates.PROCESSING && HasAllIngredients())
            {
                State = MachineStates.PROCESSING;
                m_processingEndTime = Time.realtimeSinceStartup + processingDuration;
            }

            if (State == MachineStates.PROCESSING)
                CheckProcessing();

            if (State == MachineStates.FULL)
            {
                State = MachineStates.IDLE;
                return OutputType;
            }
            
            return StuffType.None;
        }

        private bool HasAllIngredients()
        {
            foreach (var needed in InputTypes)
            {
                if (!m_stuffs.Contains(needed))
                    return false;
            }

            return true;
        }

        protected override void Highlight(bool highlight)
        {
            m_highlighted = highlight;
            UpdateHighlight();
        }

        public override void UpdateLoop()
        {
            if (State == MachineStates.PROCESSING)
                CheckProcessing();
        }

        private void CheckProcessing()
        {
            if (Mathf.Approximately(processingDuration, 0) || Time.realtimeSinceStartup > m_processingEndTime)
            {
                m_stuffs.Clear();
                State = MachineStates.FULL;
            }
        }
    }
}