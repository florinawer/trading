using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingApp.Web.Dto.Pagination
{
    public class PagedResponseDTO<T> where T : class
    {
        //contiene el nr de paginas y el tamaño
        //public PaginationFilterDTO PaginationFilter { get; set; }
        //para su utilización posterior en el front
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        //datos
        public T data { get; set; }

    }
}
