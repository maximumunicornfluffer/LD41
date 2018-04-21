using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class CharacterLogic : MonoBehaviour
    {
        public StuffType _stuffInHands = StuffType.None;

        private readonly List<IE> iesInVicinity = new List<IE>();

        public void Register(IE ie)
        {
            Debug.Log("Register IE");
            if (iesInVicinity.Contains(ie))
                return;
            
            iesInVicinity.Add(ie);
        }
        
        public void Unregister(IE ie)
        {
            Debug.Log("Unregister IE");
            iesInVicinity.Remove(ie);
        }
        
        public void Act()
        {
            Debug.LogFormat("Act ! stuff in hands :{0}", _stuffInHands);
            Debug.LogFormat("ies count: {0}", iesInVicinity.Count);
            
            bool found = false;
            if (_stuffInHands == StuffType.None)
            {
                // Find resource
                foreach (var ie in iesInVicinity.Where(e => e.Type == IEType.Resource))
                {
                    var stuff = ie as Stuff;
                    PickupStuff(stuff);
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                // Find compatible machine
                foreach (var ie in iesInVicinity.Where(e => e.Type == IEType.Machine))
                {
                    var machine = ie as Machine;
                    if (machine.InputTypes.Count == 0 || machine.InputTypes.Contains(_stuffInHands))
                    {
                        ActivateMachine(machine);
                        found = true;
                        break;
                    }
                }
            }
        }

        
        
        private void PickupStuff(Stuff stuff)
        {
            Debug.LogFormat("Pickup {0}", _stuffInHands);
            _stuffInHands = stuff.m_type;
            stuff.PickUp();
        }

        public void ActivateMachine(Machine machine)
        {
            Debug.LogFormat("Activate Machine {0}", _stuffInHands);
            _stuffInHands = machine.Activate(_stuffInHands);
        }
    }
}