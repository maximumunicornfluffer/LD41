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
        private 

        void Start()
        {
            _clients = new Collection<Client>();

            var client = Instantiate(_clientPrefab);

            _clients.Add(client);
        }

        public void UpdateLoop()
        {
            if (Time.realtimeSinceStartup > 20 && _clients.Count < 10 )
            {
                var newclient = Instantiate(_clientPrefab);
                _clients.Add(newclient);
            }
            
            foreach (var client in _clients)
            {
                client.UpdateLoop();
            }
        }
    }
}