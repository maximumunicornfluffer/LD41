using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DefaultNamespace
{
    public class Client : MonoBehaviour
    {
        private Rigidbody2D _rb;

        public Stuff _want;

        public float _attemptMax = 10f;

        public float _arriveTimestamp;

        public ClientState _state;

        void Awake()
        {
            _arriveTimestamp = Time.realtimeSinceStartup;
            _rb = GetComponent<Rigidbody2D>();
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
                return true;
            }

            return false;
        }
    }
}