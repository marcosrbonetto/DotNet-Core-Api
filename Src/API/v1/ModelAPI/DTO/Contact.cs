using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiDebts.Src.API.v1.ModelAPI.DTO
{
    public class Contact : _Base
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public bool Linked { get; set; }
    }
}