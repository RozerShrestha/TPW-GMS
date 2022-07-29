using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPW_GMS.Models
{
    public class Utility
    {
        public static String generateMemberId(String Root)
        {
            return String.Format("{0}-{1}", Root, DateTime.Now.ToString("yydd-HHmmssff"));
        }
    }
}