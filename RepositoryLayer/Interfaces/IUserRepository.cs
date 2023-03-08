using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
   public interface IUserRepository
    {
        public UserEntity UserRegister(UserRegistrationModel model);
      public string Login(LoginModel model);
       
    }
}
