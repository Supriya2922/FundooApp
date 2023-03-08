using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using RepositoryLayer.Entity;

namespace FundooNotesApllication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager manager;
        public UserController(IUserManager manager)
        {
            this.manager = manager;
        }

        [HttpPost("Register")]
        public ActionResult UserRegister(UserRegistrationModel model)
        {
            var checkReg = manager.UserRegister(model);
            if(checkReg != null)
            {
                return Ok(new ResponseModel<UserEntity> { Status = true, Message = "Register successful", Data = checkReg });
            }
            else
            {
                return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Register Unsuccessfull", Data = checkReg });
            }
        }

        [HttpPost("Login")]
        public ActionResult Login(LoginModel model)
        {
            var checkLogin = manager.Login(model);
            if(checkLogin != null)
            {
                return Ok(new ResponseModel<string> { Status = true, Message = "Login Successfull", Data = checkLogin });

            }
            else
            {
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Login unsuccessful", Data = checkLogin });
            }
        }
    }
}
