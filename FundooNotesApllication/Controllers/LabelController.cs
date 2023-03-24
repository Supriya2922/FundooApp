using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ModelLayer;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooNotesApllication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelManager manager;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<NotesController> _logger;
        public LabelController(ILabelManager manager, IDistributedCache distributedCache, ILogger<NotesController> logger)
        {
           this._logger = logger;
            this.manager = manager;
            this.distributedCache = distributedCache;
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
                    _logger.LogInformation("Label added successfully");
                    return Ok(new ResponseModel<LabelEntity> { Status=true ,Message="Label added successfully",Data= labeladded});
                }
                else
                {
                    _logger.LogInformation("Label could not be added");
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Label could not be added" });
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Error", ex);
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
                if (label)
                {
                    _logger.LogInformation("Label deleted successfully");
                    return Ok(new ResponseModel<bool> { Status = true, Message = "Label deleted successfully", Data = label });
                }
                else
                {
                    _logger.LogInformation("Label could not be deleted");
                    return BadRequest(new ResponseModel<bool> { Status = false, Message = "Label could not be deleted",Data=label });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
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
                    _logger.LogInformation("Label updated");
                    return Ok(new ResponseModel<LabelEntity> { Status = true, Message = "Label updated successfully", Data = label });
                }
                else
                {
                    _logger.LogInformation("Label could not be updated");
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Label could not be added" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
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
                var cacheKey = $"Labels{noteid}";
                string serializedLabelList;
                var labels = new List<LabelEntity>();
                var LabelList = distributedCache.Get(cacheKey);
                if (LabelList != null)
                {
                    serializedLabelList = Encoding.UTF8.GetString(LabelList);
                    labels = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelList);
                }
                else
                {
                    labels = manager.GetLabels(userid, noteid);
                    serializedLabelList = JsonConvert.SerializeObject(labels);
                    LabelList = Encoding.UTF8.GetBytes(serializedLabelList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    distributedCache.Set(cacheKey, LabelList, options);
                }
                
                if (labels != null)
                {
                    _logger.LogInformation("Labels retrieved");
                    return Ok(new ResponseModel<IEnumerable<LabelEntity>> { Status = true, Message = "Label retrieved successfully", Data = labels });
                }
                else
                {
                    _logger.LogInformation("Empty labels");
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Empty labels" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
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
                var cacheKey = $"Labels{userid}";
                string serializedLabelList;
                var labels = new List<LabelEntity>();
                var LabelList = distributedCache.Get(cacheKey);
                if (LabelList != null)
                {
                    serializedLabelList = Encoding.UTF8.GetString(LabelList);
                    labels = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelList);
                }
                else
                {
                    labels = manager.GetAllLabelsForUser(userid);
                    serializedLabelList = JsonConvert.SerializeObject(labels);
                    LabelList = Encoding.UTF8.GetBytes(serializedLabelList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    distributedCache.Set(cacheKey, LabelList, options);
                }
               
                if (labels != null)
                {
                    return Ok(new ResponseModel<List<LabelEntity>> { Status = true, Message = "Label retrieved successfully", Data = labels });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Empty labels" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error", ex);
                throw;
            }
        }
    }
}
