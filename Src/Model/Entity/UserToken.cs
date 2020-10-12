using System;

namespace ApiDebts.Src.Model.Entity
{
    public class UserToken : Base
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime? LimitDate { get; set; }
        public string Data { get; set; }
    }
}
