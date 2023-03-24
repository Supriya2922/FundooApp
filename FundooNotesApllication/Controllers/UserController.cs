using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Security.Claims;

namespace FundooNotesApllication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager manager;

        private readonly ILogger<NotesController> _logger;
        public UserController(IUserManager manager, ILogger<NotesController> logger)
        {
            this.manager = manager;
            _logger = logger;

        }

        [HttpPost("Register")]
        public ActionResult UserRegister(UserRegistrationModel model)
        {
            try
            {

                var checkReg = manager.UserRegister(model);
                if (checkReg != null)
                {
                    _logger.LogInformation("User Registration successfull");
                    return Ok(new ResponseModel<UserEntity> { Status = true, Message = "Register successful", Data = checkReg });
                }
                else
                {
                    _logger.LogInformation("User Registration unsuccessfull");
                    return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Register Unsuccessfull", Data = checkReg });
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Error", ex);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                var checkLogin = manager.Login(model);
                if (checkLogin != null)
                {
                    _logger.LogInformation("User Login successful");
                    return Ok(new ResponseModel<string> { Status = true, Message = "Login successful", Data = checkLogin });

                }
                else
                {
                   
                    _logger.LogInformation("User Registration unsuccessfull");
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Login unsuccessful", Data = checkLogin });
                }
            }
            catch (Exception ex)
            {


                _logger.LogError("Error", ex);
                throw;
            }
        }



        [HttpPost("ForgetPassword")]
        public ActionResult forgetPassword(string email)
        {
            try
            {
                var token = manager.forgetPassword(email);
                if (token != null)
                {
                    _logger.LogInformation("Reset link sent successfull");
                    return Ok(new ResponseModel<string> { Status = true, Message = "Reset link sent successfully" });
                }
                else
                {
                    _logger.LogInformation("Reset link was not sent ");
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Reset link not sent" });
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("Error", ex);
                throw;
            }
        }

        [Authorize]

        [HttpPost("resetPassword")]
        public ActionResult resetPassword(ResetPasswordModel model)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
            var reset = manager.resetPassword(model, email);
            if (reset != null)
            {
                _logger.LogInformation("Password reset successfull");
                return Ok(new ResponseModel<bool> { Status = true, Message = "Password reset successful", Data = reset });
            }
            else
            {
                _logger.LogInformation("Reset link sent unsuccessfull");
                return BadRequest(new ResponseModel<bool> { Status = false, Message = "Sorry!Could not reset password" });
            }
        }


    }

}
