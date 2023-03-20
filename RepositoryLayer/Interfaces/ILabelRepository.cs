using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ILabelRepository
    {
        public LabelEntity AddLabel(AddLabelModel model, long userid);
        public bool RemoveLabel(long userid, long noteid, long labelid);
        public LabelEntity UpdateLabel(long userid, UpdateLabelModel model);
        public IEnumerable<LabelEntity> GetLabels(long userid, long noteid);
        public IEnumerable<LabelEntity> GetAllLabelsForUser(long userid);
    }
}
