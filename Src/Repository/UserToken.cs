using System;
using JWT.Algorithms;
using JWT;
using JWT.Serializers;
using System.Collections.Generic;
using System.Linq;

namespace ApiDebts.Src.Repository
{
    public class UserToken : Base<Model.Entity.UserToken>
    {

        public UserToken(DAO.DebtsContext context, Model.LoggedUser usuarioLogeado) : base(context, usuarioLogeado)
        {
        }


        public string Generate(int userId, bool withLimitDate)
        {
            var secret = ConfigurationProvider.Get().TokenSecret;

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJsonSerializer serializer = new JsonNetSerializer();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(new
            {
                idUsuario = userId,
                fechaCreacion = DateTime.Now
            }, secret);

            DateTime? limitDate = null;
            if (withLimitDate)
            {
                limitDate = DateTime.Now.AddHours(1);
            }

            Insert(new Model.Entity.UserToken()
            {
                Data = token,
                UserId = userId,
                LimitDate = limitDate,
            });
            return token;
        }


        public Model.Entity.UserToken GetByToken(string token)
        {
            return Context.Set<Model.Entity.UserToken>()
                .FirstOrDefault((x) =>
                    x.Data == token &&
                    !x.DeleteDate.HasValue
                );
        }

        public Model.Entity.User Validate(string token, bool refresh = true)
        {
            var entity = GetByToken(token);
            if (entity == null || entity.DeleteDate.HasValue)
            {
                Error("Token inválido");
            }

            if (entity.LimitDate.HasValue && DateTime.Now > entity.LimitDate.Value)
            {
                Error("Token inválido");
            }

            if (refresh == true && entity.LimitDate.HasValue)
            {
                var newLimitDate = DateTime.Now.AddHours(1);
                entity.LimitDate = newLimitDate;
                Update(entity);
            }

            var user = GetById<Model.Entity.User>(entity.UserId);
            if (user == null)
            {
                Error("Usuario no encontrado");
            }

            return user;
        }


        public void LogOut(int userId, string token)
        {
            var entity = GetByToken(token);
            if (entity == null || entity.DeleteDate.HasValue) return;
            if (entity.UserId != userId) return;
            if (entity.LimitDate.HasValue && DateTime.Now > entity.LimitDate.Value) return;
            Delete(entity);
        }


        public List<Model.Entity.UserToken> GetFromUser(int userId, int? limit = null)
        {
            var query = Context.Set<Model.Entity.UserToken>()
                .Where((x) =>
                    x.UserId == userId &&
                    !x.DeleteDate.HasValue &&
                    (
                        x.LimitDate.HasValue == false ||
                        x.LimitDate.Value > DateTime.Now
                    )
                );

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }
            query = query.OrderByDescending((x) => x.CreationDate);
            return query.ToList();
        }
    }
}