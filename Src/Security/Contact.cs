using System.Linq;

namespace ApiDebts.Src.Security
{
    public class Contact : ApiDebts.Src.Scurity.Base
    {
        public Contact(DAO.DebtsContext context) : base(context)
        {
        }


        private bool IsFromUser(int userId, int contactId)
        {
            return Context.Set<Model.Entity.Contact>()
                            .Any(x =>
                                x.Id == contactId &&
                                x.UserOwnerId == userId &&
                                !x.DeleteDate.HasValue
                            );
        }

        public bool UserCanRead(int userId, int contactId)
        {
            return IsFromUser(userId, contactId);
        }


        public bool UserCanEdit(int userId, int contactId)
        {
            return IsFromUser(userId, contactId);
        }

        public bool UserCanDelete(int userId, int contactId)
        {
            return IsFromUser(userId, contactId);
        }
    }
}