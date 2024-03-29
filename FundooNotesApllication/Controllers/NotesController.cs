﻿using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ModelLayer;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace FundooNotesApllication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesManager manager;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<NotesController> _logger;
        public NotesController(INotesManager manager, IDistributedCache distributedCache, ILogger<NotesController> logger)
        {
            _logger = logger;
            this.manager = manager;
            this.distributedCache = distributedCache;
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateNote(CreateNotesModel model)
        {
            try
            {
                _logger.LogInformation("called Add Notes API");
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var creatnote = manager.createNote(model, userid);
                if (creatnote != null)
                {
                    _logger.LogInformation("Note added");
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note created successfully", Data = creatnote });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Note could not be created " });
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error", e);
                throw;
            }
        }
        [Authorize]
        [HttpPut("UpdateNote/{id}")]
        public ActionResult UpdateNote(UpdateNoteModel model, long id)
        {
            try
            {
                _logger.LogInformation("Note Update API called");
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var note = manager.updateNote(model, id, userid);
                if (note != null)
                {
                    _logger.LogInformation("Note Updated");
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note updated successfully", Data = note });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Note could not be updated " });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult DeleteNote(int id)
        {
            try
            {
                _logger.LogInformation("Note Delete API called");
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var note = manager.deleteNote(id, userid);
                if (note)
                {
                    _logger.LogInformation($"Note {id} Deleted");
                    return Ok(new ResponseModel<bool> { Status = true, Message = "Note deleted successfully", Data = note });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Status = false, Message = "Note could not be deleted " });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult GetNoteById(int id)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var cacheKey = id.ToString();
                string serializedNote;
                var note = new NotesEntity();
                var CacheNote = distributedCache.Get(cacheKey);
                if (CacheNote != null)
                {
                    serializedNote = Encoding.UTF8.GetString(CacheNote);
                    note = JsonConvert.DeserializeObject<NotesEntity>(serializedNote);
                }
                else
                {
                    note = manager.getNoteById(id, userid);
                    serializedNote = JsonConvert.SerializeObject(note);
                    CacheNote = Encoding.UTF8.GetBytes(serializedNote);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    distributedCache.Set(cacheKey, CacheNote, options);
                }

                if (note != null)
                {
                    _logger.LogInformation($"Note {id} retreived");
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note found", Data = note });
                }
                else
                {
                    _logger.LogInformation($"Note {id} was not retreived as Note was not found");
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Note was not found" });
                }

            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult GetAllNotes()
        {
            try

            {
                _logger.LogInformation("GetAll Note API called");
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var cacheKey = "Notes";
                string serializedNotesList;
                var notes = new List<NotesEntity>();
                var NotesList = distributedCache.Get(cacheKey);
                if (NotesList != null)
                {
                    serializedNotesList = Encoding.UTF8.GetString(NotesList);
                    notes = JsonConvert.DeserializeObject<List<NotesEntity>>(serializedNotesList);
                }
                else
                {
                    notes = manager.getAllNotes(userid);
                    serializedNotesList = JsonConvert.SerializeObject(notes);
                    NotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    distributedCache.Set(cacheKey, NotesList, options);
                }

                if (notes != null)
                {
                    return Ok(new ResponseModel<List<NotesEntity>> { Status = true, Message = "Retreival Successful", Data = notes });
                }
                else
                {
                    return BadRequest(new ResponseModel<List<NotesEntity>> { Status = false, Message = "Retreival Unsuccessful" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("BackgroundColor/{id}")]
        public ActionResult addBackgorundColor(long id,string color)
        {
            try
            {

                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var note=manager.addBackgroundColor(id,userid,color);
                if (note != null)
                {
                    _logger.LogInformation($"Background color{color} added to Note {id}");
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = $"Color changed to {color}", Data = note });
                }
                else
                {
                    _logger.LogInformation($"Background color{color} was not added to Note {id}");
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Color could not be changed" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("Archive/{id}")]
        public ActionResult Archieve(long id)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var note=manager.Archive(id,userid);
                if (note != null)
                {
                    if(note.isArchieved)
                    {
                        _logger.LogInformation($" Note {id} was archived");
                        return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note Archived", Data = note });
                    }
                    else
                    {
                        _logger.LogInformation($" Note {id} was unarchived");
                        return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note UnArchived", Data = note });
                    }
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Unsuccessful" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("Pin/{id}")]
        public ActionResult PinNote(long id)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var note = manager.Pinned(id, userid);
                if (note != null)
                {
                    if (note.isPinned)
                    {
                        _logger.LogInformation($" Note {id} was Pinned");
                        return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note Pinned", Data = note });
                    }

                    else
                    {
                        _logger.LogInformation($" Note {id} was unpinned");
                        return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note Unpinned", Data = note });
                    }
                       
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Unsuccessful" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("Trash/{id}")]
        public ActionResult TrashNote(long id,bool deleteforever)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var note=manager.Trash(id,userid,deleteforever);
                if(note!=null) 
                { 
                    if(deleteforever)
                    {
                        return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Deleted Successfully"});
                    }
                    else if(note.trash==false)
                    {
                        return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note restored" ,Data=note});
                    }
                    else
                        return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note trashed", Data = note });
                }
                else
                     return BadRequest(new ResponseModel<string> { Status = false, Message = "Unsuccessful" });
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("Image/{id}")]
        public ActionResult ImageUpload(long id,IFormFile file)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var note = manager.ImageUpload(id, userid, file);
                if(note!=null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Image uploaded successfully", Data = note });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Image upload Unsuccessful" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet("Search/{keyword}")]
        public ActionResult NotesByKeyword(string keyword)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var notes=manager.getNotesByPhrase(userid,keyword.ToLower());
                int countOfRews = notes.Count();
                 if(notes!=null && countOfRews > 0)
                 {
                     return Ok(new ResponseModel<IEnumerable<NotesEntity>> { Status = true, Message = $"Total number of rows matching the phrase {keyword} = {countOfRews} rows", Data = notes });
                 }
                 else
                 {
                     return BadRequest(new ResponseModel<string> { Status = false, Message = "Note not found for the matching phrase" });
                 }
               
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet("Search/{keyword}/{pagesize}/{pagenum}")]
        public ActionResult NotesByKeywordPaginated(string keyword,int pagesize,int pagenum)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var notes=manager.getNotesByPhrasePagination(userid,keyword.ToLower(),pagesize,pagenum);
                int countOfRows = notes.Count();
                if(notes != null && countOfRows > 0)
                {
                    return Ok(new ResponseModel<List<NotesEntity>> { Status = true, Message = $"Total number of rows matching the phrase {keyword} = {countOfRows} rows", Data = notes });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Page Empty" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
