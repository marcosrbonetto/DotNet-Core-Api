using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiDebts.Src.API.v1.ModelAPI.DTO
{
    public class User : _Base
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }

        public User(Model.Entity.User entity)
        {
            if (entity == null) return;
            Id = entity.Id;
            Name = entity.Name;
            Email = entity.Email;
            Picture = entity.Picture;
        }

        public static List<User> ToList(List<Model.Entity.User> entities)
        {
            return entities.Select(x => new User(x)).ToList();
        }

    }
}