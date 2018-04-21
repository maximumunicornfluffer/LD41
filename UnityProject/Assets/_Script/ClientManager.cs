using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace DefaultNamespace
{
    public class ClientManager: MonoBehaviour
    {
        [SerializeField]
        private Client _clientPrefab;
        private Collection<Client> _clients;
        private float _lastClientPop;

        void Start()
        {
            _clients = new Collection<Client>();

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

        public void GiveStuff(StuffType inputStuff)
        {
            foreach (var client in _clients)
            {
                if (client.GiveStuff(inputStuff)) ;
                {
                    break;
                }
            }
        }
    }
}