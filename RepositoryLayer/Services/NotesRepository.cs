using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace RepositoryLayer.Services
{
    public class NotesRepository : INotesRepository
    {
        private readonly FunContext context;
        private readonly Cloudinary _cloudinary;
        public NotesRepository(FunContext context, IConfiguration config)
        {
            this.context = context;
            var account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]);
            _cloudinary = new Cloudinary(account);

        }

        public NotesEntity CreateNotes(CreateNotesModel model, long id)
        {
            try
            {
                NotesEntity notes = new NotesEntity();
                notes.Title = model.Title;
                notes.Description = model.Description;

                notes.isPinned = false;
                notes.trash = false;
                notes.isArchieved = false;
                notes.UserId = id;
                notes.CreatedOn = DateTime.Now;
                notes.LastUpdatedOn = DateTime.Now;

                var check = context.Notes.Add(notes);
              
                if (check.State==EntityState.Added)
                {
                    context.SaveChanges();
                    return notes;
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

        public bool deleteNote(long noteid, long userid)
        {
            try
            {
                var checkId = context.Notes.FirstOrDefault(x => x.NotesId == noteid && x.UserId == userid);
                if (checkId != null)
                {
                    context.Notes.Remove(checkId);

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
        public NotesEntity updateNote(UpdateNoteModel model, long id, long userid)
        {
            try
            {
                var notes = context.Notes.FirstOrDefault(x => x.NotesId == id && x.UserId == userid);
                if (notes != null)
                {
                    notes.Title = model.Title;
                    notes.Description = model.Description;

                    notes.LastUpdatedOn = DateTime.Now;

                    context.SaveChanges();
                    return notes;
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
        public List<NotesEntity> getAllNotes(long userid)
        {
            try
            {
                var checkuser = context.Notes.Where(x => x.UserId == userid);
                if (checkuser.Any())
                    return checkuser.ToList();
                else
                    return null;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public NotesEntity getNoteById(long id, long userid)
        {
            try
            {
                var notes = context.Notes.FirstOrDefault(x => x.NotesId == id && x.UserId == userid);
                if (notes != null)
                {
                    return notes;
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
        public NotesEntity addBackgroundColor(long id, long userid, string color)
        {
            try
            {
                var note = context.Notes.FirstOrDefault(x => x.NotesId == id && x.UserId == userid);
                if (note != null)
                {
                    note.BackgroundColor = color;
                    note.LastUpdatedOn = DateTime.Now;
                    context.SaveChanges();
                    return note;
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

        public NotesEntity Archive(long id, long userid)
        {
            try
            {
                var note = context.Notes.FirstOrDefault(x => x.UserId == userid && x.NotesId == id);
                if (note != null)
                {
                    if (note.isArchieved)
                        note.isArchieved = false;
                    else
                        note.isArchieved = true;
                    context.SaveChanges();
                    return note;
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
        public NotesEntity Pinned(long id, long userid)
        {
            try
            {
                var note = context.Notes.FirstOrDefault(x => x.UserId == userid && x.NotesId == id);
                if (note != null)
                {
                    if (note.isPinned)
                        note.isPinned = false;
                    else
                        note.isPinned = true;
                    context.SaveChanges();
                    return note;
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
        public NotesEntity Trash(long id, long userid, bool deleteforever)
        {
            try
            {
                var note = context.Notes.FirstOrDefault(x => x.UserId == userid && x.NotesId == id);
                if (note != null)
                {
                    if (note.trash && deleteforever)
                        deleteNote(id, userid);
                    else if (note.trash == false)
                        note.trash = true;
                    else if (note.trash)
                        note.trash = false;
                    context.SaveChanges();
                    return note;
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
        public NotesEntity SetReminder(long id, long userid, DateTime reminder)
        {
            try
            {
                var note = context.Notes.FirstOrDefault(x => x.UserId == userid && x.NotesId == id);
                if (note != null)
                {
                    note.Reminder = reminder;
                    context.SaveChanges();
                    return note;
                }
                else
                { return null; }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NotesEntity ImageUpload(long id, long userid, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return null;
                var note = context.Notes.FirstOrDefault(x => x.UserId == userid && x.NotesId == id);
                if (note != null)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, stream),

                        };
                        var uploadResult = _cloudinary.Upload(uploadParams);
                        note.Image = uploadResult.SecureUrl.ToString();
                        note.LastUpdatedOn = DateTime.Now;
                        context.SaveChanges();
                        return note;
                    }
                }
                else
                { return null; }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public IEnumerable<NotesEntity> getNotesByPhrase(long userid, string phrase)
        {
            try
            {
                if (phrase == "" || phrase == null)
                {
                    return null;
                }
                var checkphrase = context.Notes.Where(x => x.UserId == userid && (x.Title.ToLower().Contains(phrase) || x.Description.ToLower().Contains(phrase))).ToList();
                if (checkphrase != null)
                {
                    return checkphrase;
                }
                else
                { return null; }
               
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<NotesEntity> getNotesByPhrasePagination(long userid, string phrase, int pagesize, int pagenumber)
        {
            try
            {
                if (phrase == "" || phrase == null)
                {
                    return null;
                }
                var skipsize = (pagenumber - 1) * pagesize;
                var checkphrase = context.Notes.Where(x => x.UserId == userid && (x.Title.ToLower().Contains(phrase) || x.Description.ToLower().Contains(phrase)))
                                               .OrderBy(x => x.NotesId)
                                               .Skip(skipsize)
                                               .Take(pagesize).ToList();
                if (checkphrase != null)
                {
                    return checkphrase;
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
               
               
           
//
        }
    }
}
