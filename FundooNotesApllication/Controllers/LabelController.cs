using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.Entity;
using System;

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
        [HttpDelete]
        public ActionResult deleteLabel(int noteid,int labelid) 
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var label=manager.RemoveLabel(userid,noteid,labelid);
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
       
    }
}
