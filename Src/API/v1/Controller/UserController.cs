using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ApiDebts.Src.API.v1.Controllers
{
    [ApiController]
    [Route("")]

    public class UserV1Controller : _BaseController
    {

        public UserV1Controller(DAO.DebtsContext context) : base(context)
        {
        }

        [HttpPost]
        [Route("v1/User/LoginFirebase")]
        [ServiceFilter(typeof(Attributes.WithTokenId))]
        public ModelAPI.DTO.UserLogin LogInFirebase()
        {
            var tokenId = GetTokenId();
            var user = GetAdminUser();
            var data = new Repository.User(Context, user).Login(tokenId);
            return new ModelAPI.DTO.UserLogin(data);
        }

        [HttpPost]
        [Route("v1/User/Login")]
        [ServiceFilter(typeof(Attributes.WithToken))]
        public ModelAPI.DTO.UserLogin Login()
        {
            var user = GetLoggedUser();
            var data = new Repository.User(Context, user).GetUserLogin(user.Id);
            if (data == null) return null;
            return new ModelAPI.DTO.UserLogin(data);
        }


        [HttpGet]
        [Route("v1/User")]
        [ServiceFilter(typeof(Attributes.WithToken))]
        public ModelAPI.DTO.User GetDetail()
        {
            var user = GetLoggedUser();
            var data = new Repository.User(Context, user).GetById<Model.Entity.User>(user.Id);
            if (data == null) return null;
            return new ModelAPI.DTO.User(data);
        }

        #region Update

        [HttpPut]
        [Route("v1/User/Name")]
        [ServiceFilter(typeof(Attributes.WithToken))]
        public ModelAPI.DTO.User UpdateName(string name)
        {
            var user = GetLoggedUser();
            var data = new Repository.User(Context, user).UpdateName(user.Id, name);
            return new ModelAPI.DTO.User(data);
        }

        #endregion

        #region Tokens

        [HttpGet]
        [Route("v1/User/Token")]
        [ServiceFilter(typeof(Attributes.WithToken))]
        public void ValidateToken()
        {
            GetLoggedUser();
        }


        [HttpGet]
        [Route("v1/User/Tokens")]
        [ServiceFilter(typeof(Attributes.WithToken))]
        public List<ModelAPI.DTO.UserToken> GetTokens(int? limit = null)
        {
            var user = GetLoggedUser();
            var data = new Repository.UserToken(Context, user).GetFromUser(user.Id, limit);
            return ModelAPI.DTO.UserToken.ToList(data);
        }


        [HttpDelete]
        [Route("v1/User/Token")]
        [ServiceFilter(typeof(Attributes.WithToken))]
        public void LogOut(string token)
        {
            var user = GetLoggedUser();
            new Repository.UserToken(Context, user).LogOut(user.Id, token);
        }

        #endregion

    }
}