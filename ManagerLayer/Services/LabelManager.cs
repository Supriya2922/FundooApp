using ManagerLayer.Interfaces;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class LabelManager:ILabelManager
    {
        private readonly ILabelRepository repository;
        public LabelManager(ILabelRepository repository)
        {
            this.repository = repository;
        }

        public LabelEntity AddLabel(AddLabelModel model, long userid)
        {
            return repository.AddLabel(model, userid);
        }
        public bool RemoveLabel(long userid,  long labelid)
        {
            return repository.RemoveLabel(userid, labelid);
        }
        public LabelEntity UpdateLabel(long userid, UpdateLabelModel model)
        {
            return repository.UpdateLabel(userid, model);
        }
        public List<LabelEntity> GetLabels(long userid, long noteid)
        {
            return repository.GetLabels(userid, noteid);
        }
        public List<LabelEntity> GetAllLabelsForUser(long userid)
        {
            return repository.GetAllLabelsForUser(userid);
        }
    }
}
