using Microsoft.EntityFrameworkCore;
using ModelLayer;
using ModelLayer.Helper;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace RepositoryLayer.Services
{
    public class LabelRepository: ILabelRepository
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
                if (context.Labels.Any(x => x.LabelName == model.LabelName))
                    throw new AppException("LabelName already present");
                var check=context.Labels.Add(label);
               
                if(check.State == EntityState.Added)
                {
                    context.SaveChanges();
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

        public bool RemoveLabel(long userid,long labelid) {
            try
            {
                var label = context.Labels.FirstOrDefault(x => x.LabelId == labelid && x.UserId == userid );
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

        public LabelEntity UpdateLabel(long userid,UpdateLabelModel model)
        {
            try
            {
                var label = context.Labels.FirstOrDefault(x => x.UserId == userid && x.LabelId == model.labelid);
                if(label != null) 
                {
                    if (context.Labels.Any(x => x.LabelName == model.labelname)) ;
                        throw new AppException("LabelName already present");
                    label.LabelName = model.labelname;
                    context.SaveChanges();
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
        public List<LabelEntity> GetLabels(long userid,long noteid)
        {
            try
            {
                var label = context.Labels.Where(x => x.UserId == userid && x.NotesId == noteid).ToList();
                if (label.Any())
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
        public List<LabelEntity> GetAllLabelsForUser(long userid)
        {
            try
            {

                var label = context.Labels.Where(x => x.UserId == userid).ToList();
                if (label.Any())
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

        
       
    }
}
