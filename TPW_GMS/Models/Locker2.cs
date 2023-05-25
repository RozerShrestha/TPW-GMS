using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TPW_GMS.Services;

namespace TPW_GMS.Models
{
    public class Locker2
    {
        public string memberId { get; set; }
        public string fullName { get; set; }
        public string mobileNumber { get; set; }
        public string branch { get; set; }
        public int? lockerNumber { get; set; }

        private string _renewDate;
        public string renewDate
        {
            get { return _renewDate; }
            set
            {

                _renewDate = value == "" ? "" : NepaliDateService.EngToNep(Convert.ToDateTime(value)).ToString();
            }
        }
        public string duration { get; set; }
        private string _expireDate;
        public string expireDate
        {
            get { return _expireDate; }
            set
            {
                _expireDate = value == "" ? "" : NepaliDateService.EngToNep(Convert.ToDateTime(value)).ToString();
            }
        }
        public bool? isExpired { get; set; }
        public bool? isAssigned { get; set; }
        public bool? flag { get; set; }
        public int? charge { get; set; }
        public string paymentMethod { get; set; }
        public string receiptNoStatic { get; set; }
        public string receiptNo { get; set; }
    }
    public class LockerBranch
    {
        public string branchName { get; set; }
        public List<Locker2> lockers { get; set; }
    }
}