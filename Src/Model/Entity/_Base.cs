using System;

namespace ApiDebts.Src.Model.Entity
{
    public abstract class Base
    {
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }
        public int CreationUserId { get; set; }
        public User CreationUser { get; set; }

        public DateTime? ModificationDate { get; set; }
        public int? ModificationUserId { get; set; }
        public User ModificationUser { get; set; }

        public DateTime? DeleteDate { get; set; }
        public int? DeleteUserId { get; set; }
        public User DeleteUser { get; set; }
    }
}