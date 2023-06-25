using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPW_GMS.Models
{
    public class Response
    {
        public int status { get; set; }
        public string message { get; set; }
    }
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public List<T> Datas { get; set; }

        public PagedResponse(int pageNumber, int pageSize, int totalPages, int totalRecords, List<T> datas)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalRecords = totalRecords;
            Datas = datas;
        }
    }

}