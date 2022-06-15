using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPW_GMS.Models
{
    public class AttendanceHistoryResponse
    {
        public string MemberId { get; set; }
        public string FullName { get; set; }
        public string MemberOption { get; set; }
        public string Type { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string Branch { get; set; }
        public string CheckInBranch { get; set; }
        public string Remark { get; set; }
        public bool? LateFlag { get; set; }
    }
}