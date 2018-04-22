using System.Collections.Generic;
using Assets.Scripts.PlayerManagement;
using UnityEngine;

namespace DefaultNamespace
{
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

        private ClientManager _clientManager;
        private ScoreManager _scoreManager;
        private IEManager _ieManager = new IEManager();

        private LevelData _data;

        void Awake()
        {
            s_instance = this;
        }

        void Start()
        {
            _data = FindObjectOfType<LevelData>();

            _clientManager = gameObject.GetComponent<ClientManager>();
            _scoreManager = gameObject.GetComponent<ScoreManager>();

            InitializePlayers();

//      InputsManager.Instance.AutoUpdate = false;

//      _state = RaceStateEnum.Intro;
//      _countDownUI.OnCountDownFinished += () => _state = RaceStateEnum.Run;
//      _countDownUI.Reset(3);
//
//      _openRestartOrLeaveButton.onClick.AddListener(() =>
//        _restartOrLeaveUI.gameObject.SetActive(!_restartOrLeaveUI.gameObject.activeSelf));
        }

        private void InitializePlayers()
        {
            int i = 0;
            var characters = new List<Character>();

            if (PlayersManager.Instance.Players.Count == 0)
                InputsManager.Instance.ForceCreateMainPlayer();

            foreach (var p in PlayersManager.Instance.Players)
            {
                //_data._StartPoints
                //var ship = PrefabUtility.InstantiatePrefab(_shipPrefab);

                var character = Instantiate(_characterPrefab[i]);

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

                characters.Add(character);

                i++;
            }
        }

        private void Update()
        {
            _clientManager.UpdateLoop();

            _ieManager.UpdateLoop();
        }

        public Stuff InstantiateStuff(StuffType type)
        {
            return m_stuffDictionary.Instantiate(type, _data.IEContainer);
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