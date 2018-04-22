using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace DefaultNamespace
{
    public class ClientManager: MonoBehaviour
    {
        [SerializeField]
        private Client _clientPrefab;
        private List<Client> _clients;
        private float _lastClientPop;

        void Start()
        {
            _clients = new List<Client>();

            AddNewClient();
        }

        private void AddNewClient()
        {
            var client = Instantiate(_clientPrefab);
            _clients.Add(client);
            _lastClientPop = Time.time;
        }
        
        public void UpdateLoop()
        {
            RefreshClientList();
            
            if ((Time.time - _lastClientPop) > 20 && _clients.Count < 10 )
            {
                AddNewClient();
            }

            foreach (var client in _clients)
            {
                {
                    client.UpdateLoop();
                }
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
        }
    }
}