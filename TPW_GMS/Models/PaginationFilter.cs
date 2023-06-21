using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPW_GMS.Models
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        private string _FieldSearch;
        public string FieldSearch
        {
            get
            {
                return _FieldSearch;
            }
            set
            {
                _FieldSearch = string.IsNullOrEmpty(value) ? "" : value;
            }
        }
    }
}