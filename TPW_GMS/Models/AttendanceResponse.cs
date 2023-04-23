﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPW_GMS.Models
{
    public class AttendanceResponse
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string branch { get; set; }
        public int? universalMembershipLimit { get; set; }
        public string membershipDate { get; set; }
        public string membershipBeginDate { get; set; }
        public string membershipExpireDate { get; set; }
        public string pendingPayment { get; set; }
        public string membershipOption { get; set; }
        public string membershipStatus { get; set; }
        public int attendanceCount { get; set; }
        public bool isValid { get; set; }
        public string message { get; set; }
        public bool isExpired { get; set; } = false;
    }
}