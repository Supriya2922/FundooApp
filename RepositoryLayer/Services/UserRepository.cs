using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRepository:IUserRepository
    {
        private readonly FunContext context;
        public UserRepository(FunContext context)
        {
            this.context = context;
        }
        public UserEntity UserRegister(UserRegistrationModel model)
        {
            UserEntity entity= new UserEntity();
            entity.FirstName= model.FirstName;
            entity.LastName= model.LastName;
            entity.Email= model.Email;
            entity.Password= EncrytPassword( model.Password);
           var check= context.User.Add(entity);
            context.SaveChanges();
            if (check!= null){
                return entity;
            }
            else
            {
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
                return "Login successful";
            }
            else
            {
                return null;
            }
        }
    }

}
