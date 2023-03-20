using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace RepositoryLayer.Services
{
    public class LabelRepository
    {
        private readonly FunContext context;

        public LabelRepository(FunContext context)
        {
            this.context = context;

        }
        public LabelEntity AddLabel(AddLabelModel model,long userid)
        {
            try
            {
                LabelEntity label = new LabelEntity();
                label.UserId= userid;
                label.NotesId = model.NoteId;
                label.LabelName = model.LabelName;
                var check=context.Labels.Add(label);
                context.SaveChanges();
                if(check!= null)
                {
                    return label;
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

        public bool RemoveLabel(long userid,long noteid,long labelid) {
            try
            {
                var label = context.Labels.FirstOrDefault(x => x.LabelId == labelid && x.UserId == userid && x.NotesId == noteid);
                if (label != null)
                {
                    context.Labels.Remove(label);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
