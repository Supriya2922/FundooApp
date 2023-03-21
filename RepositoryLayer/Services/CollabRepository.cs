using Microsoft.Extensions.Configuration;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace RepositoryLayer.Services
{
    public class CollabRepository: ICollabRepository
    {
        private readonly FunContext context;
       
        public CollabRepository(FunContext context)
        {
            this.context = context;
            
        }

        public CollaboratorEntity AddCollab(long userid,AddCollabModel model)
        {
            try
            {
                CollaboratorEntity collab = new CollaboratorEntity();
                collab.CollabEmail = model.email;
                collab.UserId= userid;
                collab.NotesId = model.NoteId;
                var check=context.Collaborators.Add(collab);
                context.SaveChanges();
                if (check != null)
                {
                    return collab;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool DeleteColab(long userid,long noteid,long colabId)
        {
            try
            {
                var collab=context.Collaborators.FirstOrDefault(x=>x.UserId==userid && x.NotesId==noteid && x.CollabId==colabId);
                if(collab!=null)
                {
                    context.Collaborators.Remove(collab);
                    context.SaveChanges(true);
                    return true;
                }
                else
                { return false; }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CollaboratorEntity> GetAllCollab(long userid, long noteid)
        {
            try
            {
                var collab = context.Collaborators.Where(x => x.UserId == userid && x.NotesId == noteid).ToList();
                if (collab != null)
                    return collab;
                else
                    return null;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
