using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace DefaultNamespace
{
    public class ClientManager : MonoBehaviour
    {
        [SerializeField] private Client _clientPrefab;
        [SerializeField] private Client _kidClientPrefab;
        [SerializeField] private Client _titineClientPrefab;
        private List<Client> _clients;
        private float _lastClientPop;
        private Vector3[] _waitPositionsPossible;
        private LevelData _data;

		public MusicManager _musicManager;

        private void GenerateWaitPoints()
        {
            _data = FindObjectOfType<LevelData>();

            _waitPositionsPossible = new Vector3[10];
            _waitPositionsPossible[0] = _data.ClientPoints[1].localPosition;

            var space = (_data.ClientPoints[0].localPosition.y - _data.ClientPoints[1].localPosition.y) / 10;

            for (int i = 1; i < 10; i++)
            {
                _waitPositionsPossible[i] = new Vector3(
                    _waitPositionsPossible[i - 1].x + (Random.Range(-0.005f, 0.005f)),
                    _waitPositionsPossible[i - 1].y + space,
                    _waitPositionsPossible[i - 1].z);
            }
        }

        void Start()
        {
            GenerateWaitPoints();
            _clients = new List<Client>();

            AddNewClient();
        }

        private void AddNewClient()
        {
            var client = Instantiate(DetermineClientType());

            for (var cpt = 0; cpt < 10; cpt++)
            {
                if (_clients.Any(c => c._waitPosition == _waitPositionsPossible[cpt]))
                    continue;
                client._waitPosition = _waitPositionsPossible[cpt];
                client.SetOrder(cpt);
                break;
            }

			client._musicManager = _musicManager;
            _clients.Add(client);
            _lastClientPop = Time.time;
        }

        private Client DetermineClientType()
        {
            var random = Random.Range(0, 151);
            if (random < 50)
                return _clientPrefab;
            if (random < 100)
                return _kidClientPrefab;
            return _titineClientPrefab;
        }

        public void UpdateLoop()
        {
            RefreshClientList();

            if ((Time.time - _lastClientPop) > 1 && _clients.Count < 10)
            {
                AddNewClient();
            }

            foreach (var client in _clients)
            {
                client.UpdateLoop();
            }
        }

        private void RefreshClientList()
        {
            _clients = _clients.Where(c => c != null).ToList();
        }

        public void GiveStuff(StuffType inputStuff)
        {
            RefreshClientList();

            foreach (var client in _clients)
            {
                if (client.GiveStuff(inputStuff))
                {
                    break;
                }
            }

            var i = 0;
            foreach (var client in _clients)
            {
                switch (client._state)
                {
                    case ClientState.IsServed:
                    case ClientState.QuitAngry:
                    case ClientState.QuitHappy:
                    case ClientState.ImDead:
                        client._waitPosition = new Vector3();
                        break;
                    case ClientState.Wait:
                        client.ChangeState(ClientState.QueueMove);
                        i = MajWaitPosition(client, i);
                        break;
                    default:
                        i = MajWaitPosition(client, i);
                        break;
                }
            }
        }

        private int MajWaitPosition(Client client, int i)
        {
            client.SetOrder(i);
            client._waitPosition = _waitPositionsPossible[i];
            i++;
            return i;
        }
    }
}