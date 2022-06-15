using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPW_GMS.Models
{
    public class LoginUserInfo
    {
        public string roleId { get; set; }
        public string loginUser { get; set; }
        public string  splitUser { get; set; }

        //for member login only
        public string memberId { get; set; }
    }
}