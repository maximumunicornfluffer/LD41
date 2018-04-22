﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class CharacterLogic : MonoBehaviour
    {
        public StuffType _stuffInHands = StuffType.None;

        [SerializeField] private StuffInHandDisplayer _stuffInHandDisplayer;

        private readonly List<IE> iesInVicinity = new List<IE>();

		//Sfx
		private AudioSource _audioSource;
		private AudioClip catchClip;
        private Animator _animator;

        void Start() {
			_audioSource = gameObject.AddComponent<AudioSource>();
			_audioSource.loop = true;
			_audioSource.volume = 1.0f;

			catchClip = GameManager.Instance.m_audioResources.catchSound;

		    _animator = GetComponent<Animator>();
		}

        private StuffType StuffInHands
        {
            get { return _stuffInHands; }
            set
            {
                if (_stuffInHands == value)
                    return;
                _stuffInHands = value;

                if (_stuffInHandDisplayer)
                    _stuffInHandDisplayer.SetStuffInHand(_stuffInHands);
                
                _animator.SetBool("isCarrying", _stuffInHands!=StuffType.None);
            }
        }

        public void Register(IE ie)
        {
            if (iesInVicinity.Contains(ie))
                return;

            iesInVicinity.Add(ie);
        }

        public void Unregister(IE ie)
        {
            iesInVicinity.Remove(ie);
        }

        public void Act()
        {
            bool found = false;
            if (StuffInHands == StuffType.None)
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

                    if (machine.IsIdle &&
                        (machine.InputTypes.Count == 0 && StuffInHands == StuffType.None || machine.NeededStuff() == StuffInHands))
                    {
                        ActivateMachine(machine);
                        found = true;
                        break;
                    }
                    else if (machine.State == MachineStates.FULL && StuffInHands == StuffType.None)
                    {
                        ActivateMachine(machine);
                        found = true;
                        break;
                    }
                }
            }

            if (!found && StuffInHands != StuffType.None)
            {
                var stuff = GameManager.Instance.InstantiateStuff(StuffInHands);
                stuff.transform.position = transform.position;
                StuffInHands = StuffType.None;
            }
        }


        private void PickupStuff(Stuff stuff)
        {
			PlayCatchSound();
            StuffInHands = stuff.m_type;
            stuff.PickUp();
        }

		private void PlayCatchSound() {
			_audioSource.clip = catchClip;
			_audioSource.loop = false;
			_audioSource.Play();
		}

        public void ActivateMachine(Machine machine)
        {
            StuffInHands = machine.Activate(_stuffInHands);
			if (StuffInHands == StuffType.Water) {

			} else if (StuffInHands != StuffType.None) {
				PlayCatchSound();
			}
        }
    }
}