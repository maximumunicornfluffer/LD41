using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Client : MonoBehaviour
    {
        public StuffType _want = StuffType.Fries;

        public float _waitMax = 10f;

        public float _arriveTimestamp;

        public ClientState _state;
        public float _time = 0f;

		public MusicManager _musicManager;

        public Vector3 _waitPosition;
        private Vector3 _deadPosition;
        public Vector3 _currentPosition;
        private Vector3 _startPosition;
        private LevelData _data;
        private ScoreManager _scoreManager;
        private Animator _animator;

		//Sfx
		private AudioClip EatClip;
		private AudioSource ClientAudioSource;

        public void SetOrder(int order)
        {
            GetComponent<SpriteRenderer>().sortingOrder = 100 - order;
        }

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _data = FindObjectOfType<LevelData>();
            InitializeLocation();

            _scoreManager = FindObjectOfType<ScoreManager>();
            ChangeState(ClientState.Arrive);

			gameObject.AddComponent<AudioSource>();
			AudioSource[] allAudioSources = GetComponents<AudioSource>();
			ClientAudioSource = allAudioSources[0];

			EatClip = (AudioClip)Resources.Load("crouchcrouch", typeof(AudioClip));

			ClientAudioSource.clip = EatClip;
			ClientAudioSource.loop = true;

        }

        private void InitializeLocation()
        {
            _startPosition = _currentPosition = _data.ClientPoints[0].localPosition;
            _waitPosition = _data.ClientPoints[1].localPosition;
            _deadPosition = _data.ClientPoints[2].localPosition;
            _currentPosition.x = _currentPosition.x + (Random.Range(-0.005f, 0.005f));
            transform.localPosition = _currentPosition;
        }

        public void UpdateLoop()
        {
            switch (_state)
            {
                case ClientState.Arrive:
                case ClientState.QuitAngry:
                case ClientState.QuitHappy:
                    case ClientState.QueueMove:
                    Move();
                    break;
                case ClientState.Wait:
                    if (GetAttemptTime() > _waitMax)
                    {
                        ChangeState(ClientState.QuitAngry);
                    }

                    break;
                case ClientState.IsServed:
                    ChangeState(ClientState.QuitHappy);
                    break;
            }
        }

        private void Move()
        {
            float newPosition = 0f;
            _time += Time.deltaTime;
            switch (_state)
            {
                case ClientState.Arrive:
                    newPosition = Mathf.Lerp(_startPosition.y, _waitPosition.y, _time / 5f);
                    break;
                case ClientState.QuitAngry:
                case ClientState.QuitHappy:
                    newPosition = Mathf.Lerp(_startPosition.y, _deadPosition.y, _time / 2f);
                    break;
                case ClientState.QueueMove:
                    newPosition = Mathf.Lerp(_startPosition.y, _waitPosition.y, _time / 1f);
                    break;
            }

            var newVector = new Vector3(_currentPosition.x, newPosition, _currentPosition.z);
            SetPosition(newVector);

            if (Mathf.Approximately(_currentPosition.y, _waitPosition.y))
            {
                _arriveTimestamp = Time.realtimeSinceStartup;
                ChangeState(ClientState.Wait);
                _time = 0;
                _startPosition = _currentPosition;
            }

            if (Mathf.Approximately(_currentPosition.y, _deadPosition.y))
            {
                ChangeState(ClientState.ImDead);
            }
        }

        private float GetAttemptTime()
        {
            return Time.realtimeSinceStartup - _arriveTimestamp;
        }

        public bool GiveStuff(StuffType inputStuff)
        {
            if ((int) _state >= (int) ClientState.IsServed && _state != ClientState.QueueMove)
                return false;
            if (_want.GetType() != inputStuff.GetType())
                return false;
            ChangeState(ClientState.IsServed);
            _scoreManager.Add(10);
            return true;
        }

        public void ChangeState(ClientState state)
        {
            _state = state;
            _animator.SetInteger("State", (int) _state);
        }

		public void OnEatAnimationStart()
		{
			ClientAudioSource.Play();
		}

		public void OnEatAnimationEnd()
		{
			ClientAudioSource.Stop();
		}

		public void OnDeathAnimationStart()
		{
			//Debug.Log("death animation start!!!");
			_musicManager.PlayDarkEvent();
		}

        public void OnDeathAnimationEnd()
        {
            var corpse = GameManager.Instance.InstantiateStuff(StuffType.Corpes);
            corpse.transform.position = transform.position;
            Destroy(gameObject);
        }

        private void SetPosition(Vector3 vector3)
        {
            transform.localPosition = new Vector3(vector3.x, vector3.y, vector3.z);
            _currentPosition = transform.localPosition;
        }
    }
}