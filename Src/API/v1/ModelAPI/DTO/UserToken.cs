using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiDebts.Src.API.v1.ModelAPI.DTO
{
    public class UserToken : _Base
    {
        public string Token { get; set; }
        public DateTime? LimitDate { get; set; }
        public DateTime CreationDate { get; set; }

        public UserToken(Model.Entity.UserToken entity)
        {
            Token = entity.Data;
            LimitDate = entity.LimitDate;
            CreationDate = entity.CreationDate;
        }

        public static List<UserToken> ToList(List<Model.Entity.UserToken> entities)
        {
            return entities.Select(x => new UserToken(x)).ToList();
        }

    }
}