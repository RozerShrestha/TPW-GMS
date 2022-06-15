using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPW_GMS.Models
{
    public class Email
    {
        

        public string message { get; set; }
        public List<string> fullName_email { get; set; }
        public string  subject { get; set; }
        public string type { get; set; }
        public string path { get; set; }
        public string toEmail { get; set; }
        public string package { get; set; }
        public string expiredDate { get; set; }
        public string fullname { get; set; }

    }
}