﻿using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface ILabelManager
    {
        public LabelEntity AddLabel(AddLabelModel model, long userid);
        public bool RemoveLabel(long userid,  long labelid);
        public LabelEntity UpdateLabel(long userid, UpdateLabelModel model);
        public List<LabelEntity> GetLabels(long userid, long noteid);
        public List<LabelEntity> GetAllLabelsForUser(long userid);
    }
}
