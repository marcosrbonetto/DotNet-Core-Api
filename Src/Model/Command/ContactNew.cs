using System;

namespace ApiDebts.Src.Model.Command
{
    public class ContactNew
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserCode { get; set; }
    }
}