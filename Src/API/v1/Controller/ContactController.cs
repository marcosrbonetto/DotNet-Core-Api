using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ApiDebts.Src.API.v1.Controllers
{
    [ApiController]
    [Route("")]

    public class ContactV1Controller : _BaseController
    {

        public ContactV1Controller(DAO.DebtsContext context) : base(context)
        {
        }

        [HttpGet]
        [Route("v1/Contact")]
        [ServiceFilter(typeof(Attributes.WithToken))]
        public List<ModelAPI.DTO.Contact> GetList(Model.Enums.ContactSort? sort, bool? sortAscending, int? page, int? pageLenght)
        {
            var user = GetLoggedUser();
            var ids = new Repository.Contact(Context, user).GetIdsByUserId(user.Id);
            return new Converter.Contact(Context).To(user.Id, ids, sort, sortAscending, page, pageLenght);
        }

        [HttpGet]
        [Route("v1/Contact/{id}")]
        [ServiceFilter(typeof(Attributes.WithToken))]
        public ModelAPI.DTO.Contact GetById(int id)
        {
            var user = GetLoggedUser();
            if (!new Security.Contact(Context).UserCanRead(user.Id, id))
            {
                ErrorSecurity();
            }

            var data = new Repository.Contact(Context, user).GetById(id);
            if (data == null) return null;
            return new Converter.Contact(Context).To(user.Id, data.Id);
        }

        [HttpPost]
        [Route("v1/Contact")]
        [ServiceFilter(typeof(Attributes.WithToken))]
        public ModelAPI.DTO.Contact Insert(ModelAPI.Command.ContactNew command)
        {
            var user = GetLoggedUser();
            var data = new Repository.Contact(Context, user).Insert(user.Id, command.ToModel());
            return new Converter.Contact(Context).To(user.Id, data.Id);
        }

        [HttpPut]
        [Route("v1/Contact/{contactId}/Name")]
        [ServiceFilter(typeof(Attributes.WithToken))]
        public ModelAPI.DTO.Contact UpdateName(int contactId, string value)
        {
            var user = GetLoggedUser();
            if (!new Security.Contact(Context).UserCanEdit(user.Id, contactId))
            {
                ErrorSecurity();
            }

            var data = new Repository.Contact(Context, user).UpdateName(user.Id, contactId, value);
            return new Converter.Contact(Context).To(user.Id, data.Id);
        }

        [HttpDelete]
        [Route("v1/Contact/{contactId}")]
        [ServiceFilter(typeof(Attributes.WithToken))]
        public void Delete(int contactId)
        {
            var user = GetLoggedUser();
            if (!new Security.Contact(Context).UserCanDelete(user.Id, contactId))
            {
                ErrorSecurity();
            }

            new Repository.Contact(Context, user).Delete(contactId);
        }
    }
}