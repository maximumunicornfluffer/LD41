using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private StuffInHandDisplayer _stuffInHandDisplayer;
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
        private Animator _animator;

        //Sfx
        private AudioSource _audioSource;

        public void SetOrder(int order)
        {
            var sortingOrder = 100 - order;
            _renderer.sortingOrder = sortingOrder;
            if (_stuffInHandDisplayer)
                _stuffInHandDisplayer.Renderer.sortingOrder = sortingOrder;
        }

        private SpriteRenderer _renderer;
        
        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            
            _renderer.flipX = true;
            
            InitializeLocation();

            ChangeState(ClientState.Arrive);

            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = true;
            _audioSource.volume = 1.0f;
        }

        private void InitializeLocation()
        {
            _startPosition = _currentPosition = GameManager.Instance.LevelData.ClientPoints[0].localPosition;
            _waitPosition = GameManager.Instance.LevelData.ClientPoints[1].localPosition;
            _deadPosition = GameManager.Instance.LevelData.ClientPoints[2].localPosition;
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
                    /*if (GetAttemptTime() > _waitMax)
                    {
                        ChangeState(ClientState.QuitAngry);
                    }*/

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

        private MoodStates GetHappiness()
        {
            var waitTime =  Time.realtimeSinceStartup - _arriveTimestamp;
            if (waitTime < 10)
                return MoodStates.VeryFine;
            if (waitTime < 20)
                return MoodStates.Fine;
            return waitTime < 30 ? MoodStates.AndYou : MoodStates.Bad;
        }

        public bool GiveStuff(StuffType inputStuff)
        {
            if ((int) _state >= (int) ClientState.IsServed && _state != ClientState.QueueMove)
                return false;
            
            if (_want != inputStuff)
                return false;
            
            ChangeState(ClientState.IsServed);

            var mood = GetHappiness();
            switch (mood)
            {
                    case MoodStates.VeryFine:
                        GameManager.Instance.ScoreManager.Add(10);
                        break;
                    case MoodStates.Fine:
                        GameManager.Instance.ScoreManager.Add(5);
                        break;
                    case  MoodStates.AndYou:
                        GameManager.Instance.ScoreManager.Add(1);
                        break;
            }
            return true;
        }

        public void ChangeState(ClientState state)
        {
            Debug.Log("ChangeState : " + state);
            _state = state;
            _animator.SetInteger("State", (int) _state);
            switch (_state)
            {
                case ClientState.Arrive:
                    if (_stuffInHandDisplayer)
                        _stuffInHandDisplayer.SetStuffInHand(_want);
                    break;
                case ClientState.QuitAngry:
                case ClientState.QuitHappy:
                    if (_stuffInHandDisplayer)
                        _stuffInHandDisplayer.SetStuffInHand(StuffType.None);
                    break;
            }
        }

        public void OnEatAnimationStart(AudioClip audioClip)
        {
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }

        public void OnEatAnimationEnd()
        {
            _audioSource.Stop();
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

    public enum MoodStates
    {
        VeryFine,
        Fine,
        AndYou,
        Bad
    }
}
