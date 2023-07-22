using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPW_GMS.Models
{
    //    DateTime.Now.ToString("MM/dd/yyyy")	05/29/2015
    //DateTime.Now.ToString("dddd, dd MMMM yyyy")	Friday, 29 May 2015
    //DateTime.Now.ToString("dddd, dd MMMM yyyy")	Friday, 29 May 2015 05:50
    //DateTime.Now.ToString("dddd, dd MMMM yyyy")	Friday, 29 May 2015 05:50 AM
    //DateTime.Now.ToString("dddd, dd MMMM yyyy") Friday, 29 May 2015 5:50
    //DateTime.Now.ToString("dddd, dd MMMM yyyy")	Friday, 29 May 2015 5:50 AM
    //DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")    Friday, 29 May 2015 05:50:06
    //DateTime.Now.ToString("MM/dd/yyyy HH:mm")	05/29/2015 05:50
    //DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")	05/29/2015 05:50 AM
    //DateTime.Now.ToString("MM/dd/yyyy H:mm")    05/29/2015 5:50
    //DateTime.Now.ToString("MM/dd/yyyy h:mm tt")	05/29/2015 5:50 AM
    //DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")    05/29/2015 05:50:06
    //DateTime.Now.ToString("MMMM dd")	May 29
    //DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK")	2015-05-16T05:50:06.7199222-04:00
    //DateTime.Now.ToString("ddd, dd MMM yyy HH’:’mm’:’ss ‘GMT’")	Fri, 16 May 2015 05:50:06 GMT
    //DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss")  2015-05-16T05:50:06
    //DateTime.Now.ToString("HH:mm")	05:50
    //DateTime.Now.ToString("hh:mm tt")	05:50 AM
    //DateTime.Now.ToString("H:mm")   5:50
    //DateTime.Now.ToString("h:mm tt")	5:50 AM
    //DateTime.Now.ToString("HH:mm:ss")   05:50:06
    //DateTime.Now.ToString("yyyy MMMM")	2015 May
    public class MemberInformation1
    {
        public string memberId { get; set; }
        public string fullname { get; set; }
        public string memberOption { get; set; }
        public string branch { get; set; }
        public string shift { get; set; }
        public string memberCatagory { get; set; }
        public string memberPaymentType { get; set; }
        public DateTime? memberDate { get; set; }
        public DateTime? memberBeginDate { get; set; }
        public DateTime? memberExpireDate { get; set; }

        public string contactNo { get; set; }
        public string email { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public string address { get; set; }
        public double finalAmount { get; set; }
        public string receiptNo { get; set; }
        public int dueAmount { get; set; }
        public string ActiveInActive { get; set; }
        public string gender { get; set; }
        public string memberSubCatagory { get; set; }
        public string imageLoc { get; set; }
        public List<MemberPaymentHistory> memberPaymentHistorys { get; set; }
        public List<MemberAttendance> memberAttendances { get; set; }
        public List<StaffAtt> staffAttendance { get; set; }

    }
    public class MemberInformation2
    {
        public string memberId { get; set; }
        public string fullname { get; set; }
        public string memberOption { get; set; }
        public string branch { get; set; }
        public string shift { get; set; }
        public string memberCatagory { get; set; }
        public string memberPaymentType { get; set; }

        private string _memberDate;
        public string memberDate
        {
            get { return _memberDate; }
            set { _memberDate = Convert.ToDateTime(value).ToString("ddd, dd MMMM yyyy"); }
        }
        private string _memberBeginDate;
        public string memberBeginDate
        {
            get { return _memberBeginDate; }
            set { _memberBeginDate = Convert.ToDateTime(value).ToString("ddd, dd MMMM yyyy"); }
        }

        private string _memberExpireDate;
        public string memberExpireDate
        {
            get { return _memberExpireDate; }
            set { _memberExpireDate = Convert.ToDateTime(value).ToString("ddd, dd MMMM yyyy"); }
        }
        public string contactNo { get; set; }
        public string email { get; set; }

        private string _dateOfBirth;
        public string dateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = Convert.ToDateTime(value).ToString("ddd, dd MMMM yyyy"); }
        }
        public string address { get; set; }
        public double finalAmount { get; set; }
        public string receiptNo { get; set; }
        public int dueAmount { get; set; }
        public string ActiveInActive { get; set; }
        public string gender { get; set; }
        public string memberSubCatagory { get; set; }
        public string imageLoc { get; set; }
        public List<MemberPaymentHistory> memberPaymentHistorys { get; set; }
        public List<MemberAttendance> memberAttendances { get; set; }
        public List<StaffAtt> staffAttendance { get; set; }
        public List<GuestAtt> guestAttendance { get; set; }

    }
    public class MemberPaymentHistory
    {
        public string memberId { get; set; }
        public string receiptNo { get; set; }
        //public string fullName { get; set; }
        private string _memberBeginDate;
        public string memberBeginDate
        {
            get { return _memberBeginDate; }
            set { _memberBeginDate = Convert.ToDateTime(value).ToString("ddd, dd MMMM yyyy"); }
        }

        private string _memberExpireDate;
        public string memberExpireDate
        {
            get { return _memberExpireDate; }
            set { _memberExpireDate = Convert.ToDateTime(value).ToString("ddd, dd MMMM yyyy"); }
        }

        public string memberOption { get; set; }
        public string memberCatagory { get; set; }
        public string memberPaymentType { get; set; }
        public string finalAmount { get; set; }
    }
    public class MemberAttendance
    {
        public string memberId { get; set; }
        public string fullName { get; set; }

        private string _checkin;
        public string checkin
        {
            get { return _checkin; }
            set { _checkin = Convert.ToDateTime(value).ToString("ddd, dd MMMM yyyy"); }
        }
        private string _checkout;
        public string checkout
        {
            get { return _checkout; }
            set
            {
                _checkout = value == "" ? "-" : Convert.ToDateTime(value).ToString("ddd, dd MMMM yyyy");
            }
        }
        public string branch { get; set; }
        public string checkinBranch { get; set; }
    }
    public class StaffAtt
    {
        public string memberId { get; set; }
        public string fullName { get; set; }

        private string _checkin;
        public string checkin
        {
            get { return _checkin; }
            set { _checkin = Convert.ToDateTime(value).ToString("ddd, dd MMMM yyyy"); }
        }
        private string _checkout;
        public string checkout
        {
            get { return _checkout; }
            set
            {
                _checkout = value == "" ? "-" : Convert.ToDateTime(value).ToString("ddd, dd MMMM yyyy");
            }
        }
        public string branch { get; set; }
        public string checkinBranch { get; set; }
        public string remark { get; set; }
        public bool lateFlag { get; set; }
    }
    public class GuestAtt
    {
        public string fullName { get; set; }
        public string email { get; set; }

        private string _checkin;
        public string checkin
        {
            get { return _checkin; }
            set { _checkin = Convert.ToDateTime(value).ToString("ddd, dd MMMM yyyy"); }
        }
        private string _checkout;
        public string checkout
        {
            get { return _checkout; }
            set
            {
                _checkout = value == "" ? "-" : Convert.ToDateTime(value).ToString("ddd, dd MMMM yyyy");
            }
        }
        public string branch { get; set; }
        public string checkinBranch { get; set; }
    }

}