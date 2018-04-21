using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Client : MonoBehaviour
    {
        public StuffType _want;

        public float _waitMax = 10f;

        public float _arriveTimestamp;

        private ClientState _state;
        public float _time = 0f;

        private Vector3 _waitPosition;
        private Vector3 _deadPosition;
        private Vector3 _currentPosition;
        private Vector3 _startPosition;
        private LevelData _data;
        private ScoreManager _scoreManager;
        private Animator _animator;
        
        void Awake()
        {
            _animator = GetComponent<Animator>();
            _data = FindObjectOfType<LevelData>();
            InitializeLocation();
            
            _scoreManager = FindObjectOfType<ScoreManager>();
            _state = ClientState.Arrive;
            _animator.SetInteger("State", (int)_state);
        }

        private void InitializeLocation()
        {
            _startPosition = _currentPosition = _data.ClientPoints[0].localPosition;
            _waitPosition = _data.ClientPoints[1].localPosition;
            _deadPosition = _data.ClientPoints[2].localPosition;

            transform.localPosition = _startPosition;
        }

        public void UpdateLoop()
        {
            switch (_state)
            {
                case ClientState.Arrive:
                case ClientState.QuitAngry:
                case ClientState.QuitHappy:
                    Move();
                    break;
                case ClientState.Wait:
                    if (GetAttemptTime() > _waitMax)
                    {
                        _state = ClientState.QuitAngry;
                        _animator.SetInteger("State", (int)_state);
                    }

                    break;
                case ClientState.IsServed:
                    _state = ClientState.QuitHappy;
                    _animator.SetInteger("State", (int)_state);
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
            }

            transform.localPosition = new Vector3(_currentPosition.x, newPosition, _currentPosition.z);
            _currentPosition = transform.localPosition;

            if (Mathf.Approximately(_currentPosition.y,_waitPosition.y))
            {
                _arriveTimestamp = Time.realtimeSinceStartup;
                _state = ClientState.Wait;
                _animator.SetInteger("State", (int)_state);
                _time = 0;
                _startPosition = _currentPosition;
            }

            if (Mathf.Approximately(_currentPosition.y,_deadPosition.y))
            {
                _state = ClientState.ImDead;
                _animator.SetInteger("State", (int)_state);
            }
        }

        private float GetAttemptTime()
        {
            return Time.realtimeSinceStartup - _arriveTimestamp;
        }

        public bool GiveStuff(StuffType inputStuff)
        {
            if (_want.GetType() == inputStuff.GetType())
            {
                _state = ClientState.IsServed;
                _animator.SetInteger("State", (int)_state);
                _scoreManager.Add(10);
                return true;
            }

            return false;
        }

        public void OnDeathAnimationEnd()
        {
            // instantié un prefab
            Destroy(gameObject);
        }
    }
}