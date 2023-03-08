using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        public UserEntity UserRegister(UserRegistrationModel model);
        public string Login(LoginModel model);

    }
}
