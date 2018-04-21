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

        private void Awake()
        {
            m_state = MachineStates.IDLE;
            m_spriteRenderer.color = Color.white;
        }

        public StuffType Activate(StuffType stuff)
        {
            if (InputTypes.Count == 0 || InputTypes.Contains(stuff))
            {
                m_stuffs.Add(stuff);
            }

            if (m_state == MachineStates.IDLE && HasAllIngredients())
            {
                m_state = MachineStates.PROCESSING;
                m_processingEndTime = Time.realtimeSinceStartup + processingDuration;
                m_spriteRenderer.color = Color.yellow;
            }

            if (m_state == MachineStates.PROCESSING)
                CheckProcessing();

            if (m_state == MachineStates.FULL)
            {
                m_state = MachineStates.IDLE;
                m_spriteRenderer.color = Color.white;
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
            if (highlight)
                m_spriteRenderer.color = Color.red;
            else
                m_spriteRenderer.color = Color.white;
        }

        public override void UpdateLoop()
        {
            if (m_state == MachineStates.PROCESSING)
                CheckProcessing();
        }

        private void CheckProcessing()
        {
            if (Mathf.Approximately(processingDuration, 0) || Time.realtimeSinceStartup > m_processingEndTime)
            {
                m_stuffs.Clear();
                m_state = MachineStates.FULL;
                m_spriteRenderer.color = Color.green;
            }
        }
    }
}