using Microsoft.AspNetCore.Http;
using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRepository
    {
        public NotesEntity CreateNotes(CreateNotesModel model,long id);
        public bool deleteNote(long noteid, long userid);
        public NotesEntity updateNote(UpdateNoteModel model, long id, long userid);
        public List<NotesEntity> getAllNotes( long userid);
        public NotesEntity getNoteById(long id, long userid);
        public NotesEntity addBackgroundColor(long id, long userid, string color);
        public NotesEntity Archive(long id, long userid);
        public NotesEntity Pinned(long id, long userid);
        public NotesEntity Trash(long id, long userid, bool deleteforever);
        public NotesEntity SetReminder(long id, long userid, DateTime reminder);
        public NotesEntity ImageUpload(long id, long userid, IFormFile file);
        public IEnumerable<NotesEntity> getNotesByPhrase(long userid, string phrase);
        public IEnumerable<NotesEntity> getNotesByPhrasePagination(long userid, string phrase, int pagesize, int pagenumber);
    }
}
