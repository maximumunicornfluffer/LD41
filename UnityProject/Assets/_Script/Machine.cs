using System.Collections.Generic;
using DefaultNamespace.UI;
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

        public override IEType Type
        {
            get { return IEType.Machine; }
        }

        private HashSet<StuffType> m_stuffs = new HashSet<StuffType>();

        private FillBar m_progressBar;
        private MachineStates m_state = MachineStates.IDLE;
        private float m_processingStartTime;
        private float m_processingEndTime;

        private Animator m_animator;

        //Sfx

        public AudioClip _audioClip;
        public bool shouldLoopAudio = false;

        private AudioSource _audioSource;
        //private AudioClip catchClip;

		private AudioClip _earthAudioClip;
		private AudioClip _cookedAudioClip;

        
        protected override void Awake()
        {
            base.Awake();
            m_animator = GetComponent<Animator>();
            m_progressBar = GetComponentInChildren<FillBar>();
            State = m_startState;
            UpdateDisplay();
        }
			
        private void OnEnable()
        {
            if (m_progressBar)
            {
                m_progressBar.SetWarning(false);
                SetProgress(-1.0f);
            }
        }
		void Start() {
			_audioSource = gameObject.AddComponent<AudioSource>();
			_audioSource.volume = 1.0f;

			//catchClip = GameManager.Instance.m_audioResources.catchSound;

			_earthAudioClip = GameManager.Instance.m_audioResources.earthSound;
			_cookedAudioClip = GameManager.Instance.m_audioResources.cookedSound;
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
            if (state == MachineStates.PROCESSING)
            {
                GameManager.Instance.ScoreManager.Add(5);
                StartProcessingSound();
            }
            else if (state == MachineStates.FULL)
            {
                StopProcessingSound();
            }
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
                SetStuffInHand(neededStuff);
                return;
            }

            SetStuffInHand(StuffType.None);
        }

        private void SetStuffInHand(StuffType t)
        {
            if (!_stuffInHandDisplayer)
                return;
            
            _stuffInHandDisplayer.SetStuffInHand(t);
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
				if (stuff == StuffType.Cinder) {
					PlayEarthSound();
				}
            }

            if (m_stuffs.Count > 0 && !HasAllIngredients())
                State = MachineStates.HALF_FILLED;

            if (State < MachineStates.PROCESSING && HasAllIngredients())
            {
                SetProgress(0);
                State = MachineStates.PROCESSING;
                m_processingStartTime = Time.realtimeSinceStartup;
                m_processingEndTime = m_processingStartTime + processingDuration;
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

        private void SetProgress(float value)
        {
            if (m_progressBar)
            {
                m_progressBar.gameObject.SetActive(value >= 0);
                m_progressBar.SetValue(value);
            }
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
            UpdateHighlight();
        }

        public override void UpdateLoop()
        {
            if (State == MachineStates.PROCESSING)
            {
                CheckProcessing();
            }
        }

        private void CheckProcessing()
        {
            if (State == MachineStates.PROCESSING)
            {
                var now = Time.realtimeSinceStartup;
                var percentage = (now - m_processingStartTime) / processingDuration;
                SetProgress(percentage);
            }

            if (Mathf.Approximately(processingDuration, 0) || Time.realtimeSinceStartup > m_processingEndTime)
            {
                m_stuffs.Clear();
                State = MachineStates.FULL;
                SetProgress(-1.0f);
            }
        }
			
		private void PlayEarthSound() {
			_audioSource.clip = _earthAudioClip;
			_audioSource.loop = false;
			_audioSource.Play();
		}

		private void PlayCookedSound() {
			_audioSource.clip = _cookedAudioClip;
			_audioSource.loop = false;
			_audioSource.Play();
		}

		private void StartProcessingSound() {
			_audioSource.clip = _audioClip;
			_audioSource.loop = shouldLoopAudio;
			_audioSource.Play();
		}

		private void StopProcessingSound() {
			if (shouldLoopAudio) {
				_audioSource.Stop();
				if (OutputType == StuffType.Fries || OutputType == StuffType.Cinder) { 
					PlayCookedSound();
				}
			}
		}
    }
}