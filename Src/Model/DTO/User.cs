using System;
namespace ApiDebts.Src.Model.DTO
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }

        public User(Entity.User entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Email = entity.Email;
            Picture = entity.Picture;
        }
    }
}