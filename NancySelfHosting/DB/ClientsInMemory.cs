namespace NancySelfHosting.DB

{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NancySelfHosting.Entities;

    public class ClientsInMemory
    {
        private object _lock = new object();
        private int nextId;
        private List<ClientEntity> _clients;

        public ClientsInMemory()
        {
            _clients = new List<ClientEntity>();
            nextId = _clients.Count();
        }

        public void RebuildLocalizations()
        {
            lock (_lock)
            {
                _clients = new List<ClientEntity>();
            }
        }

        public void Add(ClientEntity clientEntity)
        {
            lock (_lock)
            {
                clientEntity._id = nextId;
                 _clients.Add(clientEntity);
                nextId++;
            }
        }

        public List<ClientEntity> GetAll()
        {
            return _clients.ToList();
        }

        public ClientEntity Get(string id)
        {
            ClientEntity resu = null;
            int numId;
            if (Int32.TryParse(id, out numId))
            {
                resu = _clients.Find(x => x._id == numId);
            }
            return resu;
        }

        public ClientEntity Delete(string id)
        {
            ClientEntity resu = null;
            int numId;
            if (Int32.TryParse(id, out numId))
            {
                resu = _clients.Find(x => x._id == numId);
                lock (_lock)
                {
                    if (resu != null)
                    {
                        _clients.Remove(resu);
                    }
                }
            }
            return resu;
        }

        public void Replace(ClientEntity newClient)
        {
            int? id = newClient._id;
            _clients[_clients.FindIndex(x => x._id == id)] = newClient;
        }

    }

}

