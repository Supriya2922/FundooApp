using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ICollabRepository
    {
        public CollaboratorEntity AddCollab(long userid, AddCollabModel model);
        public bool DeleteColab(long userid, long noteid, long colabId);
        public List<CollaboratorEntity> GetAllCollab(long userid, long noteid);
    }
}
