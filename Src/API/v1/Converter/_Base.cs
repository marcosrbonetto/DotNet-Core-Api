using System.Collections.Generic;

namespace ApiDebts.Src.API.v1.Converter
{
    public abstract class Base<E>
    {
        public DAO.DebtsContext Context { get; }
        public Base(DAO.DebtsContext context)
        {
            this.Context = context;
        }

        public E To(int usereId, int id)
        {
            List<E> data = this.To(usereId, new List<int>() { id });
            if (data.Count != 0)
            {
                return data[0];
            }

            return default(E);
        }

        public abstract List<E> To(int userId, List<int> ids);

    }
}