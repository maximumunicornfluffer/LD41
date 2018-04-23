using System;
using Assets.Scripts.PlayerManagement;
using UnityEngine;

namespace DefaultNamespace.UI
{
	public enum CharacterSelectionState
	{
		Choosing,
		Selected,
		Empty,
	}

	public class CharacterSelectionUI : MonoBehaviour
	{
		[SerializeField]
		private Animator _animator;

		private PlayerInput _input;
		private int _index;
    
		[SerializeField]
		private GameObject m_emptyGO;
		[SerializeField]
		private GameObject m_choosingGO;
		[SerializeField]
		private GameObject m_readyGO;
    
		private CharacterSelectionState m_state = CharacterSelectionState.Empty;

		public event Action<PlayerInput> OnPlayerCanceled;

		private AudioSource _audioSource;
		public AudioResources _audioSounds;

		void Awake ()
		{
			m_state = CharacterSelectionState.Choosing;
			State = CharacterSelectionState.Empty;

			//Sfx
			_audioSource = gameObject.AddComponent<AudioSource> ();
			_audioSource.loop = false;
			_audioSource.volume = 1.0f;
		}

		public void SetPlayerInput (PlayerInput input)
		{
			if (_input != null)
				return;
			_input = input;
			State = _input == null ? CharacterSelectionState.Empty : CharacterSelectionState.Choosing;
		}

		private void Update ()
		{
			if (_stateJustChanged)
			{
				// on passe une frame pour éviter que le bouton d'activation ne sélectionne un perso.
				_stateJustChanged = false;
				return;
			}
      
			switch (State)
			{
			case CharacterSelectionState.Empty:
          //
				break;
			case CharacterSelectionState.Choosing:
				UpdateChoosing ();
				break;
			case CharacterSelectionState.Selected:
				UpdateSelected ();
				break;
			}
		}

		private void UpdateChoosing ()
		{
			if (_input.H1UpButtonDown ())
			{
				_audioSource.PlayOneShot (_audioSounds.selectSound);
				IncIndex ();
			}
      
			if (_input.H1DownButtonDown ())
			{
				_audioSource.PlayOneShot (_audioSounds.selectSound);
				DecIndex ();
			}

			if (_input.ADown ())
			{
				_audioSource.PlayOneShot (_audioSounds.confirmSound);
				State = CharacterSelectionState.Selected;
			} else if (_input.BDown ())
			{
				_audioSource.PlayOneShot (_audioSounds.cancelSound);
				State = CharacterSelectionState.Empty;
				OnPlayerCanceled?.Invoke (_input);
				_input = null;
				_index = 0;
			}
		}

		private void UpdateSelected ()
		{
			if (_input.B ())
			{
				State = CharacterSelectionState.Choosing;
			}
		}

		private bool _stateJustChanged = false;

		public CharacterSelectionState State
		{
			get { return m_state; }
			set
			{
				if (m_state == value)
					return;
				m_state = value;
				m_choosingGO.SetActive (m_state == CharacterSelectionState.Choosing);
				m_emptyGO.SetActive (m_state == CharacterSelectionState.Empty);
				m_readyGO.SetActive (m_state == CharacterSelectionState.Selected);
				_animator.gameObject.SetActive (m_state != CharacterSelectionState.Empty);
				_stateJustChanged = true;
			}
		}

		private void IncIndex ()
		{
			_index++;
			if (_index > 3)
				_index = 0;

			Apply ();
		}

		private void DecIndex ()
		{
			_index--;
			if (_index < 0)
				_index = 3;
      
			Apply ();
		}

		private void Apply ()
		{
			_animator.SetInteger ("Index", _index);
			PlayersManager.Instance.GetPlayer (_input).CharacterSelectionIndex = _index;
		}

	}
}