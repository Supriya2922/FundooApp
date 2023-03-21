using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;

namespace FundooNotesApllication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelManager manager;
        public LabelController(ILabelManager manager)
        {
            this.manager = manager;
        }
        [Authorize]
        [HttpPost]
        public ActionResult addLabel(AddLabelModel model)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var labeladded=manager.AddLabel(model,userid);
                if(labeladded!=null)
                {
                    return Ok(new ResponseModel<LabelEntity> { Status=true ,Message="Label added successfully",Data= labeladded});
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Label could not be added" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpDelete("{labelid}")]
        public ActionResult deleteLabel(int labelid) 
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var label=manager.RemoveLabel(userid,labelid);
                if (label != null)
                {
                    return Ok(new ResponseModel<bool> { Status = true, Message = "Label deleted successfully", Data = label });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Status = false, Message = "Label could not be deleted",Data=label });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("{labelid}")]
        public ActionResult UpdateLabel(UpdateLabelModel model)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var label = manager.UpdateLabel(userid, model);
                if (label!= null)
                {
                    return Ok(new ResponseModel<LabelEntity> { Status = true, Message = "Label updated successfully", Data = label });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Label could not be added" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet("{noteid}")]
        public ActionResult GetAllLabels(long noteid)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var label = manager.GetLabels(userid, noteid);
                if (label != null)
                {
                    return Ok(new ResponseModel<IEnumerable<LabelEntity>> { Status = true, Message = "Label retrieved successfully", Data = label });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Empty labels" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet]
        public ActionResult GetLabelsByUser()
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var label = manager.GetAllLabelsForUser(userid);
                if (label != null)
                {
                    return Ok(new ResponseModel<IEnumerable<LabelEntity>> { Status = true, Message = "Label retrieved successfully", Data = label });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Empty labels" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
