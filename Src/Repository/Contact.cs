using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace ApiDebts.Src.Repository
{
    public class Contact : Base<Model.Entity.Contact>
    {

        public Contact(DAO.DebtsContext context, Model.LoggedUser usuarioLogeado) : base(context, usuarioLogeado)
        {
        }

        public List<int> GetIdsByUserId(int userId)
        {
            return Context.Set<Model.Entity.Contact>()
                .Where(x =>
                    x.UserOwnerId == userId &&
                    !x.DeleteDate.HasValue
                )
                .Select(x => x.Id)
                .ToList();
        }


        public Model.Entity.Contact Insert(int userId, Model.Command.ContactNew command)
        {
            string name = null;
            string description = null;
            int? contactUserId = null;
            if (command.UserCode != null && command.UserCode.Trim() != "")
            {
                var user = Context.Set<Model.Entity.User>()
                    .Where(x =>
                        !x.DeleteDate.HasValue &&
                        x.Code.ToLower() == command.UserCode.Trim().ToLower()
                    )
                    .SingleOrDefault();
                if (user == null)
                {
                    Error("User not found");
                }

                var existingContact = Context.Set<Model.Entity.Contact>()
                    .Where(x =>
                        !x.DeleteDate.HasValue &&
                        x.UserOwnerId == userId &&
                        x.UserId == user.Id
                    )
                    .SingleOrDefault();
                if (existingContact != null)
                {
                    return existingContact;
                }

                contactUserId = user.Id;
            }
            else
            {
                if (command.Name == null || command.Name.Trim() == "")
                {
                    Error("The name is required");
                }

                var existingContact = Context.Set<Model.Entity.Contact>()
                   .Where(x =>
                       !x.DeleteDate.HasValue &&
                       x.UserOwnerId == userId &&
                       x.Name.Trim().ToLower() == command.Name.Trim().ToLower()
                   )
                   .SingleOrDefault();
                if (existingContact != null)
                {
                    Error($"Already exists a contact with the name {command.Name}");
                }

                name = command.Name.Trim();
                if (command.Description != null && command.Description.Trim() != "")
                {
                    description = command.Description.Trim();
                }
            }

            if (command.Name == null || command.Name.Trim() == "")
            {
                Error("The name is required");
            }

            return base.Insert(new Model.Entity.Contact()
            {
                Name = name,
                Description = description,
                UserId = contactUserId,
                UserOwnerId = userId,
                LastUseDate = DateTime.Now,
            });
        }

        public Model.Entity.Contact UpdateName(int userId, int contactId, string name)
        {
            if (!Exists<Model.Entity.User>(userId))
            {
                Error("User not found");
            }

            var contact = GetById(contactId);
            if (contact == null)
            {
                Error("Contact not found");
            }

            if (contact.UserId.HasValue)
            {
                Error("The contact is linked");
            }

            if (name == null || name.Trim() == "")
            {
                Error("The name is required");
            }

            if (Context.Set<Model.Entity.Contact>()
                .Any(x =>
                    !x.DeleteDate.HasValue &&
                    x.UserOwnerId == userId &&
                    x.Name.ToLower().Trim() == name.ToLower().Trim() &&
                    x.Id != contact.Id
                )
            )
            {
                Error($"Already exists a contact with the name {name}");
            }

            contact.Name = name.Trim();
            return base.Update(contact);
        }


    }

}