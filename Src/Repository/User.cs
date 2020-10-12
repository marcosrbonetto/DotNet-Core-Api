using System;
using System.Linq;
using System.Transactions;

namespace ApiDebts.Src.Repository
{
    public class User : Base<Model.Entity.User>
    {

        public User(DAO.DebtsContext context, Model.LoggedUser usuarioLogeado) : base(context, usuarioLogeado)
        {
        }

        public Model.DTO.UserLogin Login(string tokenId)
        {
            using var transaction = new TransactionScope();

            var userFirebase = Utils.FirebaseUtils.Instance.GetData(tokenId);

            //Search if exists an user with the same email and email verified
            var user = Context.Set<Model.Entity.User>().SingleOrDefault(x =>
                x.UID == userFirebase.UID &&
                !x.DeleteDate.HasValue
            );

            // If not exists and user with the same verified email 
            // create the user and deletes all user with the email unverified
            if (user == null)
            {
                user = Insert(new Model.Entity.User()
                {
                    UID = userFirebase.UID,
                    Name = userFirebase.Name,
                    Email = userFirebase.Email,
                    Picture = userFirebase.Picture,
                });
            }
            else
            {
                //Update user picture
                if (user.Picture == null || user.Picture.Trim() == "")
                {
                    if (userFirebase.Picture != null && userFirebase.Picture.Trim() != "")
                    {
                        user.Picture = userFirebase.Picture;
                        user = Update(user);
                    }
                }
            }

            var result = GetUserLogin(user.Id);
            transaction.Complete();
            return result;
        }


        public Model.DTO.UserLogin GetUserLogin(int userId)
        {
            var user = GetById<Model.Entity.User>(userId);
            if (user == null) return null;

            var token = new Repository.UserToken(Context, LoggedUser).Generate(userId, false);
            return new Model.DTO.UserLogin(user, token);
        }

        #region Update

        public Model.Entity.User UpdateName(int id, string name)
        {
            var user = Context.Set<Model.Entity.User>().SingleOrDefault(x =>
                x.Id == id &&
                !x.DeleteDate.HasValue
            );
            if (user == null)
            {
                Error("Usuario no encontrado");
            }

            if (name == null || name.Trim() == "")
            {
                Error("El nombre es un dato requerido");
            }

            user.Name = name;
            return Update(user);
        }

        #endregion
    }
}