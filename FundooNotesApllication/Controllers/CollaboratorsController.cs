using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesApllication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorsController : ControllerBase
    {
        private readonly ICollabManager manager;
        private readonly IDistributedCache distributedCache;
        private readonly IBus _bus;
        public CollaboratorsController(ICollabManager manager, IDistributedCache distributedCache, IBus bus)
        {
            this.manager = manager;
            this.distributedCache = distributedCache;
            _bus = bus;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> createCollab(AddCollabModel model)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var collab = manager.AddCollab(userid, model);
               
              
                if (collab != null)
                {
                    CollabModel collabModel = new CollabModel();
                    collabModel.NoteId = model.NoteId;
                    collabModel.email = email;
                    collabModel.collabemail = model.email;
                    if (collabModel != null)
                    {

                        Uri uri = new Uri("rabbitmq://localhost/CollabQueue");
                        var endPoint = await _bus.GetSendEndpoint(uri);
                        await endPoint.Send(collabModel);
                        return Ok(new ResponseModel<CollaboratorEntity> { Status = true, Message = "Collab added successfully and email will be sent to Collabortor", Data = collab });
                    }
                    return BadRequest();
                   
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Collab was not added-Collab email already present" });
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        [Authorize]
        [HttpGet("{noteid}")]
        public ActionResult getAllCollab(long noteid)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var cacheKey = $"Collabs{noteid}";
                string serializedCollabList;
                var collab = new List<CollaboratorEntity>();
                var collabList = distributedCache.Get(cacheKey);
                if (collabList != null)
                {
                    serializedCollabList = Encoding.UTF8.GetString(collabList);
                    collab = JsonConvert.DeserializeObject<List<CollaboratorEntity>>(serializedCollabList);
                }
                else
                {
                    collab = manager.GetAllCollab(userid, noteid);
                    serializedCollabList=JsonConvert.SerializeObject(collab);
                    collabList = Encoding.UTF8.GetBytes(serializedCollabList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    distributedCache.Set(cacheKey, collabList, options);
                }
                 
                if (collab != null)
                {
                    return Ok(new ResponseModel<List<CollaboratorEntity>> { Status = true, Message = "Collab Retrieved successfully", Data = collab });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Collab could not be retrieved" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpDelete("{collabid}")]
        public ActionResult deleteCollab(long noteid,long collabid)
        {
            try
            {
                var userid = Convert.ToInt64(User.FindFirst("Id").Value.ToString());
                var collab = manager.DeleteColab(userid, noteid, collabid);
                if (collab )
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
