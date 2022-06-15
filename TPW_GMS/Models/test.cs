using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPW_GMS.Models
{
    public class Product
    {
        public string ProductType { get; set; }
        public string iconCode { get; set; }
        public List<SubProduct> subProducts { get; set; }
    }
    public class SubProduct
    {
        public string name { get; set; }
        public string applyLink { get; set; }
    }

}