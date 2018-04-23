using System.Collections;
using System.Collections.Generic;
using DefaultNamespace.UI;
using UnityEngine;

using States;

namespace DefaultNamespace
{
    public enum GameStateEnum
    {
        Starting,
        Running,
        Ending,
    }
    
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        private static GameManager s_instance;

        public static GameManager Instance
        {
            get { return s_instance; }
        }

        #endregion

        [SerializeField] private Character[] _characterPrefab;

        [SerializeField] private StuffDictionary m_stuffDictionary;
		[SerializeField] public AudioResources m_audioResources;
		[SerializeField] public GameLogicExit m_gameManagerExit;

        private ClientManager _clientManager;
        private ScoreManager _scoreManager;
		private TimeManager _timeManager;
        
        private IEManager _ieManager = new IEManager();

        private LevelData _data;

        private GameStateEnum m_state = GameStateEnum.Starting;
        
        void Awake()
        {
            s_instance = this;
        }

        void Start()
        {
            _data = FindObjectOfType<LevelData>();

            _clientManager = gameObject.GetComponent<ClientManager>();
            _scoreManager = gameObject.GetComponent<ScoreManager>();
            _timeManager = gameObject.GetComponent<TimeManager>();

            m_gameManagerExit.OnAnimCloseEnded += OnAnimCloseEnded;

            InitializePlayers();

//      InputsManager.Instance.AutoUpdate = false;

//      _state = RaceStateEnum.Intro;
//      _countDownUI.OnCountDownFinished += () => _state = RaceStateEnum.Run;
//      _countDownUI.Reset(3);
//
//      _openRestartOrLeaveButton.onClick.AddListener(() =>
//        _restartOrLeaveUI.gameObject.SetActive(!_restartOrLeaveUI.gameObject.activeSelf));

            m_state = GameStateEnum.Running;
        }


        private List<Character> _characters;
        
        private void InitializePlayers()
        {
            int i = 0;
            _characters = new List<Character>();

            if (PlayersManager.Instance.Players.Count == 0)
                InputsManager.Instance.ForceCreateMainPlayer();

            foreach (var p in PlayersManager.Instance.Players)
            {
                //_data._StartPoints
                //var ship = PrefabUtility.InstantiatePrefab(_shipPrefab);

                var character = Instantiate(_characterPrefab[p.CharacterSelectionIndex]);

                character.transform.SetParent(transform);
                character.transform.SetParent(null);


//        HandlerManager.Instance.CreateHandler(character,PatternEnum.Default);

                character._input = p.Input;
//        if (i == 0 && _mobileControllerUi!=null)
//        {
//          p.Input.ResetExternalTrigger();
//          p.Input.AddButtonDownTrigger(PlayerInput._A,()=>_mobileControllerUi.ConsumeIfDirectionActive(MobileControllerUI.Direction.UP));
//          p.Input.AddButtonDownTrigger(PlayerInput._V_1+"_D",()=>_mobileControllerUi.ConsumeIfDirectionActive(MobileControllerUI.Direction.DOWN));
//          p.Input.AddButtonTrigger(PlayerInput._LB,()=>_mobileControllerUi.IsDirectionActive(MobileControllerUI.Direction.RIGHT));
//          p.Input.AddButtonTrigger(PlayerInput._A,()=>Input.GetMouseButton(0));
//        }
//        controller.Color = p.Color;

                character.transform.position = _data.StartPoints[i].position;

                _characters.Add(character);

                i++;
            }
        }

        private void Update()
        {
            if (m_state == GameStateEnum.Running)
            {
                _clientManager.UpdateLoop();
                _ieManager.UpdateLoop();
                
                if (_timeManager.IsGameOver())
                {
                    StartCoroutine(EndGameCoroutine());
                }
            }

  
        }

        private IEnumerator EndGameCoroutine()
        {
            m_state = GameStateEnum.Ending;

            foreach (var character in _characters)
                character._input = null;

            // TODO Play ending sound + Stop music
            
            yield return new WaitForSeconds(2.0f);
            
            m_gameManagerExit.PlayClose();
        }

        
        private void OnAnimCloseEnded()
        {
            _clientManager.RemoveAllCients();
            FSM.Instance.GotoState<GameOverState>();
            gameObject.AddComponent<PlayersManager>();
        }

		public void ResetScore() 
		{
			if (_timeManager != null) {
				
				_scoreManager.Reset();
			}
		}

		public string GetScoreText() 
		{
			return _scoreManager.GetScoreText();
		}


		public Stuff InstantiateStuff(StuffType type, StuffSubType subtype)
        {
            return m_stuffDictionary.Instantiate(type, subtype, _data.IEContainer);
        }

        public IEManager IEManager
        {
            get { return _ieManager; }
        }

        public ClientManager ClientManager
        {
            get { return _clientManager; }
        }

        public ScoreManager ScoreManager => _scoreManager;
        public LevelData LevelData => _data;

        public StuffDictionary StuffDictionary
        {
            get { return m_stuffDictionary; }
        }
    }
}