using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DefaultNamespace
{
    public class Client : MonoBehaviour
    {
        public Stuff _want;

        public float _attemptMax = 10f;

        public float _arriveTimestamp;

        public ClientState _state;
        public float _time = 0f;

        public Vector3 _waitPosition;
        public Vector3 _deadPosition;
        public Vector3 _currentPosition;
        public Vector3 _startPosition;
        private LevelData _data;
        private ScoreManager _scoreManager;
        
        void Awake()
        {
            _data = FindObjectOfType<LevelData>();
            InitializeLocation();
            _arriveTimestamp = Time.realtimeSinceStartup;
            _scoreManager = FindObjectOfType<ScoreManager>();
            _state = ClientState.Arrive;
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
                    if (GetAttemptTime() > _attemptMax)
                    {
                        _state = ClientState.QuitAngry;
                    }

                    break;
                case ClientState.IsServed:
                    _state = ClientState.QuitHappy;
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

            if (_currentPosition == _waitPosition)
            {
                _state = ClientState.Wait;
                _time = 0;
                _startPosition = _currentPosition;
            }

            if (_currentPosition == _deadPosition)
            {
                _state = ClientState.ImDead;
            }
        }

        private float GetAttemptTime()
        {
            return Time.realtimeSinceStartup - _arriveTimestamp;
        }

        public bool GiveStuff(Stuff inputStuff)
        {
            if (_want.GetType() == inputStuff.GetType())
            {
                _state = ClientState.IsServed;
                _scoreManager.Add(10);
                return true;
            }

            return false;
        }
    }
}