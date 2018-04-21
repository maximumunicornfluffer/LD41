using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class CharacterLogic : MonoBehaviour
    {
        public StuffType _stuffInHands = StuffType.None;

        [SerializeField]
        private StuffInHandDisplayer _stuffInHandDisplayer;

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
                    if (machine.State == MachineStates.IDLE && (machine.InputTypes.Count == 0 || machine.InputTypes.Contains(_stuffInHands)))
                    {
                        ActivateMachine(machine);
                        found = true;
                        break;
                    } else if (machine.State == MachineStates.FULL && _stuffInHands == StuffType.None)
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
            _stuffInHands = stuff.m_type;
            _stuffInHandDisplayer.SetStuffInHand(_stuffInHands);
            stuff.PickUp();
            Debug.LogFormat("Pickup {0}", _stuffInHands);
        }

        public void ActivateMachine(Machine machine)
        {
            var stuffInHands = _stuffInHands;
            _stuffInHands = machine.Activate(_stuffInHands);
            _stuffInHandDisplayer.SetStuffInHand(_stuffInHands);
            Debug.LogFormat("Activate Machine Before:{0} After:{1}", stuffInHands, _stuffInHands);
        }
    }
}