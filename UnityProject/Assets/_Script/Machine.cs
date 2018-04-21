using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    
    public class Machine : IE
    {
        public List<StuffType> InputTypes;
        public StuffType OutputType = StuffType.None;
        public float processingDuration;
        
        public override IEType Type { get { return IEType.Machine; } }

        private HashSet<StuffType> m_stuffs = new HashSet<StuffType>();
        
        private MachineStates m_state = MachineStates.IDLE;
        private float m_processingEndTime;
        private bool m_highlighted = false; 
        
        protected override void Awake()
        {
            base.Awake();
            State = MachineStates.IDLE;
        }

        public virtual MachineStates State
        {
            get { return m_state; }
            set
            {
                if (m_state == value)
                    return;
                m_state = value;
                UpdateHighlight();
            }
        }

        protected virtual void OnStateChanged(MachineStates state)
        {
        }
        
        private void UpdateHighlight()
        {
            switch (m_state)
            {
                case MachineStates.IDLE:
                    if (m_highlighted)
                        m_spriteRenderer.color = Color.red;
                    else
                        m_spriteRenderer.color = Color.white;
                    break;
                case MachineStates.PROCESSING:
                    m_spriteRenderer.color = Color.yellow;
                    break;
                case MachineStates.FULL:
                    m_spriteRenderer.color = Color.green;
                    break;
            }
        }

        public StuffType Activate(StuffType stuff)
        {
            if (State == MachineStates.IDLE && (InputTypes.Count == 0 || InputTypes.Contains(stuff)))
            {
                m_stuffs.Add(stuff);
            }

            if (State == MachineStates.IDLE && HasAllIngredients())
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