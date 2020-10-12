using System.Collections.Generic;
using System.Linq;
using ApiDebts.Src.DAO;

namespace ApiDebts.Src.API.v1.Converter
{
    public class Contact : Base<ModelAPI.DTO.Contact>
    {
        public Contact(DebtsContext context) : base(context)
        {
        }

        public List<ModelAPI.DTO.Contact> To(int userId, List<int> ids, Model.Enums.ContactSort? sort, bool? sortAscending, int? page, int? pageLenght)
        {
            if (!sort.HasValue)
            {
                sort = Model.Enums.ContactSort.Name;
            }

            if (!sortAscending.HasValue)
            {
                sortAscending = true;
            }

            var query = Context.Set<Model.Entity.Contact>().Where(x => ids.Contains(x.Id) && !x.DeleteDate.HasValue);

            switch (sort)
            {
                case Model.Enums.ContactSort.Name:
                    {
                        if (sortAscending.Value)
                        {
                            query = query.OrderBy(x => x.Name.ToLower());
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.Name.ToLower());
                        }
                    }

                    break;

                case Model.Enums.ContactSort.LastUse:
                    {
                        if (sortAscending.Value)
                        {
                            query = query.OrderBy(x => x.LastUseDate);
                        }
                        else
                        {
                            query = query.OrderByDescending(x => x.LastUseDate);
                        }
                    }
                    break;
            }


            if (page.HasValue && pageLenght.HasValue)
            {
                query = query.Take(pageLenght.Value).Skip(page.Value * pageLenght.Value);
            }

            return this.To(userId, query.Select(x => x.Id).ToList());
        }

        public override List<ModelAPI.DTO.Contact> To(int userId, List<int> ids)
        {
            return Context.Set<Model.Entity.Contact>()
                .Where(x =>
                   ids.Contains(x.Id) &&
                   !x.DeleteDate.HasValue
                )
                .Select(x => new ModelAPI.DTO.Contact()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Email = x.User.Email,
                    Picture = x.User.Picture,
                    Linked = x.UserId.HasValue,
                })
                .ToList()
                .OrderBy(x => ids.IndexOf(x.Id))
                .ToList();
        }
    }
}