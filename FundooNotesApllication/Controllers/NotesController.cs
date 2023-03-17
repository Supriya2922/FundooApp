using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Claims;

namespace FundooNotesApllication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesManager manager;
        public NotesController(INotesManager manager)
        {
            this.manager = manager;
        }
        [Authorize]
        [HttpPost("CreateNote")]
        public ActionResult CreateNote(CreateNotesModel model)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var creatnote=manager.createNote(model,userid);
                if(creatnote != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status=true,Message="Note created successfully", Data = creatnote });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Note could not be created ",  });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("UpdateNote/{id}")]
        public ActionResult UpdateNote(UpdateNoteModel model,long id)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var note = manager.updateNote(model,id, userid);
                if (note != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note updated successfully", Data = note });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Note could not be updated "});
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
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var note = manager.deleteNote(id, userid);
                if (note != null)
                {
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
                var note = manager.getNoteById(id, userid);
                if (note != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note found", Data = note });
                }
                else
                {
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
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var notes = manager.getAllNotes(userid);
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
        public ActionResult addBackgorundColor(long notesid,string color)
        {
            try
            {

                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var note=manager.addBackgroundColor(notesid,userid,color);
                if (note != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = $"Color changed to {color}", Data = note });
                }
                else
                {
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
                        return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note Archived", Data = note });
                    }
                    else
                    {
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
                       return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note Pinned", Data = note });
                    else
                        return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note Unpinned", Data = note });
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
    }
}
