using Microsoft.AspNetCore.Mvc;
using System;

namespace ApiDebts.Src.API.v1.Controllers
{
    public class _BaseController : ControllerBase
    {
        public DAO.DebtsContext Context { get; }
        public _BaseController(DAO.DebtsContext context)
        {
            Context = context;
        }


        protected void Error(string message)
        {
            throw new Exception(message);
        }

        protected void ErrorSecurity()
        {
            throw new System.Exception("you do not have the necessary permission to perform this operation");
        }

        protected Model.LoggedUser GetAdminUser()
        {
            return new Model.LoggedUser()
            {
                Id = 1,
                UID = "root",
                Email = "root@debts.com",
            };
        }

        protected Model.LoggedUser GetLoggedUser()
        {
            var config = ConfigurationProvider.Get();
            var token = Request.Headers[config.Headers.Token][0];

            var usuarioAdmin = GetAdminUser();

            var user = new Repository.UserToken(Context, usuarioAdmin).Validate(token, true);
            return new Model.LoggedUser()
            {
                Id = user.Id,
                Email = user.Email,
                UID = user.UID,
            };
        }

        protected string GetTokenId()
        {
            var config = ConfigurationProvider.Get();
            return Request.Headers[config.Headers.TokenId][0];
        }

    }
}