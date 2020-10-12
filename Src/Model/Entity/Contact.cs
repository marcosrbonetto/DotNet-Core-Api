using System;

namespace ApiDebts.Src.Model.Entity
{
    public class Contact : Base
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public int UserOwnerId { get; set; }
        public User UserOwner { get; set; }
        public DateTime? LastUseDate { get; set; }
    }
}