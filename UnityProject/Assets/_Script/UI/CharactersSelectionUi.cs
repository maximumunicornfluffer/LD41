using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.PlayerManagement;
using States;
using Ui.Game;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class CharactersSelectionUi : MonoBehaviour
    {
        [SerializeField] private CharacterSelectionUI[] _selections;
        [SerializeField] private GameObject _startText;

        private List<PlayerInput> inputs = new List<PlayerInput>();
        private bool _readyToStart = false;
        private bool m_started = false;
        private Animator _animator;
        
        private ChoiceInformation m_information;
		public AudioResources _audioResources;
		public AudioSource _audioSource;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
            _startText.SetActive(false);

            InputsManager.Instance.OnNewPlayer += OnNewPlayer;
            InputsManager.Instance.OnPlayerLeave += OnPlayerLeave;

			_audioSource.volume = 1.0f;
			_audioSource.loop = false;

            foreach (var selection in _selections)
            {
                selection.OnPlayerCanceled += OnPlayerCanceled;
            }
        }

        public void SetInformation(ChoiceInformation info)
        {
            m_information = info;
        }
        
        private void OnPlayerCanceled(PlayerInput input)
        {
			
            if (m_started) return;
			_audioSource.PlayOneShot(_audioResources.cancelSound);
            InputsManager.Instance.RequestRemovePlayerByIndex(input, input.InputIndex);
			Debug.Log("player canceled");
        }

        private void OnPlayerLeave(PlayerInput input)
        {
            inputs.Remove(input);

        }

        private void OnNewPlayer(PlayerInput input)
        {
			_audioSource.PlayOneShot(_audioResources.confirmSound);
            inputs.Add(input);
            var characterSelection = _selections.First(ui => ui.State == CharacterSelectionState.Empty);
            characterSelection.SetPlayerInput(input);
        }

        void OnExitAnimEnded()
        {
            FSM.Instance.GotoState<TutoState>();
        }
        
        void OnDestroy()
        {
            InputsManager.Instance.OnNewPlayer -= OnNewPlayer;
            InputsManager.Instance.OnPlayerLeave -= OnPlayerLeave;
        }

        private void Update()
        {
            if (m_started) return;

            var validInputIndexes = PlayerInputUtils.GetValidInputIndexes();
            foreach (var index in validInputIndexes)
            {
                if (!PlayerInputUtils.GetButtonDown(PlayerInput._A, index)) continue;
                if (inputs.Exists(input => input.InputIndex == index)) return;
                InputsManager.Instance.RequestCreatePlayerByIndex(index);
            }

            SetReady(inputs.Count != 0 && _selections.All(ui => ui.State != CharacterSelectionState.Choosing));

            if (_readyToStart)
            {
                foreach (var input in inputs)
                {
                    if (!PlayerInputUtils.GetButtonDown(PlayerInput._START, input.InputIndex)) continue;
                    _audioSource.PlayOneShot(_audioResources.confirmSound);
                    m_started = true;
                    _animator.Play("Close");
                    break;
                }
            }
        }


        private void SetReady(bool ready)
        {
            if (_readyToStart == ready) return;
            _readyToStart = ready;

            _startText.SetActive(_readyToStart);
        }
    }
}