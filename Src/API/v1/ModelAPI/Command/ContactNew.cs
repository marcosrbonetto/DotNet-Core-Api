namespace ApiDebts.Src.API.v1.ModelAPI.Command
{
    public class ContactNew
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserCode { get; set; }

        public Model.Command.ContactNew ToModel()
        {
            return new Model.Command.ContactNew()
            {
                Name = Name,
                Description = Description,
                UserCode = UserCode,
            };
        }
    }
}