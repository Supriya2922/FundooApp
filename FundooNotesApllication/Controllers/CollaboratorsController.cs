using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FundooNotesApllication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorsController : ControllerBase
    {
        private readonly ICollabManager manager;
        public CollaboratorsController(ICollabManager manager)
        {
            this.manager = manager;
        }
        [Authorize]
        [HttpPost]
        public ActionResult createCollab(AddCollabModel model)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var collab = manager.AddCollab(userid, model);
                if(collab != null)
                {
                    return Ok(new ResponseModel<CollaboratorEntity> { Status=true,Message="Collab added successfully",Data= collab});
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Collab was not added" });
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        [Authorize]
        [HttpGet]
        public ActionResult getAllCollab(long noteid)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var collab=manager.GetAllCollab(userid,noteid);
                if (collab != null)
                {
                    return Ok(new ResponseModel<IEnumerable<CollaboratorEntity>> { Status = true, Message = "Collab added successfully", Data = collab });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Collab was not added" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult deleteCollab(long noteid,long collabid)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var collab = manager.DeleteColab(userid, noteid, collabid);
                if (collab != null)
                {
                    return Ok(new ResponseModel<bool> { Status = true, Message = "Collab deleted successfully", Data = collab });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Status = false, Message = "Collab was not deleted" });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
