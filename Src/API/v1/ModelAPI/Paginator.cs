using System.Collections.Generic;

namespace ApiDebts.Src.API.v1.ModelAPI
{
    public class Paginator<T>
    {
        public int Page { get; set; }
        public int PageLenght { get; set; }
        public int Count { get; set; }
        public List<T> Data { get; set; }
    }
}