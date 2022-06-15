using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPW_GMS.Models
{
    public class EmailContent
    {
        public string memberId { get; set; }
        public string userName { get; set; }
        public string branch { get; set; }
        public string password { get; set; }
        public string membershipOption { get; set; }
        public string catagoryType { get; set; }
        public string membershipDate { get; set; }
        public string membershipPaymentType { get; set; }

        public string membershipBeginDate { get; set; }
        public string membershipExpireDate { get; set; }
        public string email { get; set; }
        public string fullname { get; set; }
        public string contactNo { get; set; }
        public string dateOfBirth { get; set; }
        public string address { get; set; }
        public string discountCode { get; set; }
        public string lockerNumber { get; set; }
        public string lockerRenewDate { get; set; }
        public string lockerExpireDate { get; set; }
        public string finalAmount { get; set; }
        public string paidAmount { get; set; }
        public string dueAmount { get; set; }
    }
}