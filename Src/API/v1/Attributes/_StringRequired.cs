
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace ApiDebts.Src.API.v1.Attributes
{
    public abstract class StringRequired : BaseAttribute
    {

        public StringRequired(DAO.DebtsContext context) : base(context)
        {

        }


        public override void OnAuthorization(AuthorizationFilterContext context)
        {
            GetHeaderString(context, GetName());
        }

        public abstract string GetName();

        public override List<BaseAttributeHeader> GetSwaggerHeaders()
        {
            return new List<BaseAttributeHeader>()
            {
                new BaseAttributeHeader()
                {
                    Name = GetName(),
                    IsRequired = true,
                }
            };
        }
    }

}