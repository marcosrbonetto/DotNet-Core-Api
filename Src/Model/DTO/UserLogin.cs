using System;

namespace ApiDebts.Src.Model.DTO
{
    public class UserLogin : User
    {
        public string Token { get; set; }

        public UserLogin(Entity.User entity, string token) : base(entity)
        {
            this.Token = token;
        }
    }
}