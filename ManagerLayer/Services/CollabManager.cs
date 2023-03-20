using ManagerLayer.Interfaces;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class CollabManager:ICollabManager
    {
        private readonly ICollabRepository repository;
        public CollabManager(ICollabRepository repository)
        {
            this.repository = repository;
        }
        public CollaboratorEntity AddCollab(long userid, AddCollabModel model)
        {
            return repository.AddCollab(userid, model);
        }
        public bool DeleteColab(long userid, long noteid, long colabId)
        {
            return repository.DeleteColab(userid, noteid, colabId);
        }
        public IEnumerable<CollaboratorEntity> GetAllCollab(long userid, long noteid)
        {
            return repository.GetAllCollab(userid, noteid);
        }
    }
}
