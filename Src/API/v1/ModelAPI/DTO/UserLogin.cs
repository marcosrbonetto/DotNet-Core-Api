using System;
namespace ApiDebts.Src.API.v1.ModelAPI.DTO
{
    public class UserLogin : User
    {
        public string Token { get; set; }

        public UserLogin(Model.DTO.UserLogin entity) : base(null)
        {
            Id = entity.Id;
            Name = entity.Name;
            Email = entity.Email;
            Picture = entity.Picture;
            Token = entity.Token;
        }
    }
}