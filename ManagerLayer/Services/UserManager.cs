using ManagerLayer.Interfaces;
using ModelLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace ManagerLayer.Services
{
    public class UserManager:IUserManager
    {
        private readonly IUserRepository repository;
        public UserManager(IUserRepository repository)
        {
            this.repository = repository;
        }
        public UserEntity UserRegister(UserRegistrationModel model)
        {
            return repository.UserRegister(model);
        }

        public string Login(LoginModel model)
        {
            return repository.Login(model);
        }

      


        public string forgetPassword(string email)
        {
            return repository.forgetPassword(email);
        }

      

        public bool resetPassword(ResetPasswordModel model,string email)
        {
           return repository.resetPassword(model,email);
        }
    }
}
