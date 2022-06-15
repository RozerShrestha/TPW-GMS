using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPW_GMS.Models
{
    public class ReportParam
    {
        public string branch { get; set; }
        public string reportType { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string memberId { get; set; }
        public string membershipOption { get; set; }
        public string catagory { get; set; }
        public string duration { get; set; }
        public string shift { get; set; }
        public string who { get; set; }
        public string membershipType { get; set; }
        public string flag { get; set; }
        public int postPendingCheck { get; set; }
    }
}