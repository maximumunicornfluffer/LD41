using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class IEManager
    {
        public List<IE> m_ies = new List<IE>();

        public void Register(IE ie)
        {
            m_ies.Add(ie);
        }

        public void Unregister(IE ie)
        {
            m_ies.Remove(ie);
        }
        
        public IEnumerable<IE> IES { get { return m_ies;}}

        public void UpdateLoop()
        {
            foreach (var ie in m_ies)
            {
                ie.UpdateLoop();
            }
        }
    }
}