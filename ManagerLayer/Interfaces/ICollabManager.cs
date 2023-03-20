using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface ICollabManager
    {
        public CollaboratorEntity AddCollab(long userid, AddCollabModel model);
        public bool DeleteColab(long userid, long noteid, long colabId);
        public IEnumerable<CollaboratorEntity> GetAllCollab(long userid, long noteid);
    }
}
