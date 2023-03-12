using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FundooNotesApllication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager manager;
        private readonly IConfiguration _config;
        public UserController(IUserManager manager, IConfiguration config)
        {
            this.manager = manager;
            _config = config;
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
        private string GenerateToken(LoginModel model)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
