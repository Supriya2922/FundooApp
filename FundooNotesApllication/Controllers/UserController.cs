using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
            try
            {
                var checkReg = manager.UserRegister(model);
                if (checkReg != null)
                {
                    return Ok(new ResponseModel<UserEntity> { Status = true, Message = "Register successful", Data = checkReg });
                }
                else
                {
                    return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Register Unsuccessfull", Data = checkReg });
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
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
                    
                    return Ok(new ResponseModel<string> { Status = true, Message = "Login successful", Data = checkLogin });

                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Login unsuccessful", Data = checkLogin });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
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
                    
                    return Ok(new ResponseModel<string> { Status = true, Message = "Reset link sent successfully" });
                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Reset link not sent" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
       
        [HttpPost("resetPassword")]
        public ActionResult resetPassword(ResetPasswordModel model)
        {
            var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
            var reset=manager.resetPassword(model,email);
            if (reset != null)
            {
               return Ok( new ResponseModel<bool> { Status = true, Message = "Password reset successful", Data = reset });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { Status = false, Message = "Sorry!Could not reset password" });
            }
        }
     

    }

}
