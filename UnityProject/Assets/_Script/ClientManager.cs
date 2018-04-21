using System.Collections.ObjectModel;
using UnityEngine;

namespace DefaultNamespace
{
    public class ClientManager: MonoBehaviour
    {
        [SerializeField]
        private Client _clientPrefab;
        private Collection<Client> _clients;

        void Start()
        {
            _clients = new Collection<Client>();

            var client = Instantiate(_clientPrefab);

            _clients.Add(client);
        }

        public void UpdateLoop()
        {
            foreach (var client in _clients)
            {
                client.UpdateLoop();
            }
        }
    }
}