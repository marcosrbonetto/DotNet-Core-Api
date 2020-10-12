namespace ApiDebts.Src.Scurity
{
    public class Base
    {
        public DAO.DebtsContext Context { get; }

        public Base(DAO.DebtsContext context)
        {
            this.Context = context;
        }
    }
}