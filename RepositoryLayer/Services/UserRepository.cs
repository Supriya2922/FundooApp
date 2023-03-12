using Microsoft.Extensions.Configuration;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using RepositoryLayer.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.CodeDom.Compiler;

namespace RepositoryLayer.Services
{
    public class UserRepository:IUserRepository
    {
        private readonly FunContext context;
        private readonly IConfiguration _config;
        public UserRepository(FunContext context, IConfiguration config)
        {
            this.context = context;
            this._config = config;
        }
     
        public UserEntity UserRegister(UserRegistrationModel model)
        {
            try
            {
                UserEntity entity = new UserEntity();
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Email = model.Email;
                entity.Password = EncrytPassword(model.Password);
                var check = context.User.Add(entity);
                context.SaveChanges();
                if (check != null)
                {
                    return entity;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex) { 
            Console.WriteLine(ex.Message);
                return null;
            }

           
        }

        public string EncrytPassword(string password)
        {
            var plainPassword = Encoding.UTF8.GetBytes(password);
            var encodePassword=Convert.ToBase64String(plainPassword);
            return encodePassword;

        }
        public string Login(LoginModel model)
        {
            var checkDetails=context.User.FirstOrDefault(v=>v.Email==model.Email && v.Password== EncrytPassword(model.Password));

            if (checkDetails != null)
            {
                return Generate(checkDetails.Email,checkDetails.UserId);
            }
            else
            {
                return null;
            }
        }
       private string Generate(string email,long userid)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
           new Claim(ClaimTypes.Email, email),
           new Claim("Id",userid.ToString())
       };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

           
        }
    }

}
