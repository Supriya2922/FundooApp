using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class NotesManager : INotesManager
    {
        private readonly INotesRepository repository;
        public NotesManager(INotesRepository repository)
        {
            this.repository = repository;
        }
        public NotesEntity createNote(CreateNotesModel model,long id)
        {
            return repository.CreateNotes(model,id);
        }
        public bool deleteNote(long noteid, long userid)
        {
            return repository.deleteNote(noteid, userid);
        }
        public NotesEntity updateNote(UpdateNoteModel model, long id, long userid)
        {
            return repository.updateNote(model,id, userid);
        }
        public List<NotesEntity> getAllNotes(long userid)
        {
            return repository.getAllNotes(userid);
        }
        public NotesEntity getNoteById(long id, long userid)
        {
            return repository.getNoteById(id, userid);
        }

        public NotesEntity addBackgroundColor(long id, long userid, string color)
        {
            return repository.addBackgroundColor(id, userid, color);
        }

        public NotesEntity Archive(long id, long userid)
        {
           return repository.Archive(id, userid);
        }

        public NotesEntity Pinned(long id, long userid)
        {
            return repository.Pinned(id, userid);
        }

        public NotesEntity Trash(long id, long userid, bool deleteforever)
        {
            return repository.Trash(id, userid, deleteforever);
        }

       public  NotesEntity SetReminder(long id, long userid, DateTime reminder)
        {
            return repository.SetReminder(id, userid, reminder);
        }
        public NotesEntity ImageUpload(long id, long userid, IFormFile file)
        {
            return repository.ImageUpload(id, userid, file);
        }
        public IEnumerable<NotesEntity> getNotesByPhrase(long userid,  string phrase)
        {
            return repository.getNotesByPhrase(userid, phrase);
        }
        public IEnumerable<NotesEntity> getNotesByPhrasePagination(long userid, string phrase, int pagesize, int pagenumber)
        {
            return repository.getNotesByPhrasePagination(userid,phrase,pagesize, pagenumber);
        }
    }
}
