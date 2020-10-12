using System.Collections.Generic;

namespace ApiDebts.Src.API.v1.Attributes
{
    public class WithTokenId : StringRequired
    {
        private static string HEADER_NAME = ConfigurationProvider.Get().Headers.TokenId;

        public WithTokenId(DAO.DebtsContext context) : base(context)
        {

        }

        public override string GetName()
        {
            return HEADER_NAME;
        }
    }
}