using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class LabelManager
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
        public bool RemoveLabel(long userid, long noteid, long labelid)
        {
            return repository.RemoveLabel(userid, noteid, labelid);
        }
        public LabelEntity UpdateLabel(long userid, UpdateLabelModel model)
        {
            return repository.UpdateLabel(userid, model);
        }
        public IEnumerable<LabelEntity> GetLabels(long userid, long noteid)
        {
            return repository.GetLabels(userid, noteid);
        }
        public IEnumerable<LabelEntity> GetAllLabelsForUser(long userid)
        {
            return repository.GetAllLabelsForUser(userid);
        }
    }
}
