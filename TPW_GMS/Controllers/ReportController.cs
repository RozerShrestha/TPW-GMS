using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TPW_API.Models;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Repository;
using TPW_GMS.Services;

namespace TPW_GMS.Controllers
{
    [Authorize]
    public class ReportController : ApiController
    {
        private TPWDataContext db = new TPWDataContext();
        public static string conString;
        public DbConFactory dbCon = new DbConFactory();

        [Route("api/GetAdmittedAndRenew/")]
        [HttpPost]
        public IHttpActionResult GetAdmittedAndRenew(ReportParam r)
        {
            r.startDate = NepaliDateService.NepToEng(r.startDate).ToString();
            r.endDate = NepaliDateService.NepToEng(r.endDate).ToString();

            if (r.reportType == "newAdmitted")
            {

                var itemNewAdmissions = (from m in db.MemberInformations
                                         join p in db.Reports on m.memberId equals p.memberId
                                         where m.memberDate >= Convert.ToDateTime(r.startDate) && m.memberDate <= Convert.ToDateTime(r.endDate) && p.renewExtend == r.reportType && (m.memberOption == "Regular" || m.memberOption == "OffHour" || m.memberOption == "Universal")
                                         select new
                                         {
                                             m.memberId,
                                             m.fullname,
                                             m.contactNo,
                                             m.shift,
                                             m.memberDate,
                                             m.memberBeginDate,
                                             m.memberExpireDate,
                                             m.memberOption,
                                             m.memberCatagory,
                                             m.gender,
                                             m.memberPaymentType,
                                             m.email,
                                             m.branch,
                                             p.paidAmount,
                                             p.dueAmount,
                                             p.dueClearAmount,
                                             p.finalAmount,
                                             p.receiptNo,
                                             p.created
                                         });

                if (r.branch != "ALL")
                    itemNewAdmissions = itemNewAdmissions.Where(c => c.branch == r.branch);
                if (r.membershipOption != "ALL")
                    itemNewAdmissions = itemNewAdmissions.Where(c => c.memberOption == r.membershipOption);
                if (r.catagory != "ALL")
                    itemNewAdmissions = itemNewAdmissions.Where(c => c.memberCatagory == r.catagory);
                if (r.duration != "ALL")
                    itemNewAdmissions = itemNewAdmissions.Where(c => c.memberPaymentType == r.duration);
                if (r.shift != "ALL")
                    itemNewAdmissions = itemNewAdmissions.Where(c => c.shift == r.shift);

                itemNewAdmissions = itemNewAdmissions.OrderBy(d => d.memberDate);
                return Ok(itemNewAdmissions);
            }
            else
            {
                var items = (from m in db.MemberInformations
                             join p in db.Reports on m.memberId equals p.memberId
                             where p.created >= Convert.ToDateTime(r.startDate) && p.created <= Convert.ToDateTime(r.endDate) && p.renewExtend == r.reportType && (m.memberOption== "Regular" || m.memberOption== "OffHour" || m.memberOption== "Universal")
                             select new
                             {
                                 m.memberId,
                                 m.fullname,
                                 m.contactNo,
                                 m.shift,
                                 m.memberDate,
                                 m.memberBeginDate,
                                 m.memberExpireDate,
                                 m.memberOption,
                                 m.memberCatagory,
                                 m.gender,
                                 m.memberPaymentType,
                                 m.email,
                                 m.branch,
                                 p.paidAmount,
                                 p.dueAmount,
                                 p.dueClearAmount,
                                 p.finalAmount,
                                 p.receiptNo,
                                 p.created
                             }).AsEnumerable();
                if (r.branch != "ALL")
                    items = items.Where(c => c.branch == r.branch);
                if (r.membershipOption != "ALL")
                    items = items.Where(c => c.memberOption == r.membershipOption);
                if (r.catagory != "ALL")
                    items = items.Where(c => c.memberCatagory == r.catagory);
                if (r.duration != "ALL")
                    items = items.Where(c => c.memberPaymentType == r.duration);
                if (r.shift != "ALL")
                    items = items.Where(c => c.shift == r.shift);

                //items = items.OrderBy(d => d.updatedDate);
                return Ok(items);
            }

        }

        [Route("api/GetDailyReport/")]
        [HttpPost]
        public IHttpActionResult GetDailyReport(ReportParam r)
        {
            try
            {
                r.startDate = NepaliDateService.NepToEng(r.startDate).ToString();
                r.endDate = NepaliDateService.NepToEng(r.endDate).ToString();
                using (var conn = dbCon.CreateConnection())
                {
                    var result = conn.Query("sp_getDailyReport",
                        param: new { r.startDate, r.endDate, r.reportType }
                        , commandType: System.Data.CommandType.StoredProcedure).ToList();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [Route("api/GetEmailMarketingCSVList/")]
        [HttpPost]
        public IHttpActionResult GetEmailMarketingCSVList(ReportParam r)
        {
            var monthEarlier = DateTime.Now.AddMonths(-1);
            var items = (from p in db.MemberInformations
                         select p).ToList();
            //for individual branch
            if(r.branch != "All")
                items = items.Where(c => c.branch == r.branch).ToList();
            //for Active and Inactive Members
            if(r.membershipType!="ALL" && r.membershipType!= "1_Month_Expired")
                items = items.Where(c => c.ActiveInactive == r.membershipType).ToList();
            if(r.membershipType== "1_Month_Expired")
                items = items.Where(c => c.memberExpireDate > monthEarlier && c.memberExpireDate < DateTime.Now).ToList();

            items = items.OrderBy(d => d.memberExpireDate).ToList();
                return Ok(items);
        }

        [Route("api/GetMemberExpiry/")]
        [HttpPost]
        public IHttpActionResult GetMemberExpiry(ReportParam r)
        {
            var d1 = DateTime.Now.AddMonths(-1);
            List<MemberInformation> itemList = new List<MemberInformation>();
            if(r.branch=="ALL")
                itemList = db.MemberInformations.Where(k=> k.memberOption != "Trainer" && k.memberOption != "Super Admin" && k.memberOption != "Gym Admin").ToList();
            else
                itemList = db.MemberInformations.Where(k =>k.branch==r.branch &&(k.memberOption != "Trainer" && k.memberOption != "Super Admin" && k.memberOption != "Gym Admin")).ToList();

            if (r.reportType == "0")
            {
                itemList = itemList.Where(p => p.memberExpireDate <= d1).ToList();
                itemList = itemList.OrderBy(d => d.memberExpireDate).ToList();
            }
            else if (r.reportType == "1")
            {
                itemList = itemList.Where(p => p.memberExpireDate >= d1 && p.memberExpireDate <= DateTime.Now).ToList();
                itemList = itemList.OrderBy(d => d.memberExpireDate).ToList();
            }
            return Ok(itemList);
        }

        [Route("api/GetLockerExpiry/")]
        [HttpPost]
        public IHttpActionResult GetLockerExpiry(ReportParam r)
        {
            var d1 = DateTime.Now.AddMonths(-1);
            List<dynamic> itemList = new List<dynamic>();
            if (r.branch == "ALL")
                //itemList = db.LockerMgs.ToList();
                itemList = (from l in db.LockerMgs
                            join m in db.MemberInformations
                            on l.memberId equals m.memberId
                            select new
                            {
                                l,
                                m
                            }).ToList<dynamic>();
            else
                itemList = (from l in db.LockerMgs
                            join m in db.MemberInformations
                            on l.memberId equals m.memberId
                            where l.branch== r.branch
                            select new
                            {
                                l,
                                m
                            }).ToList<dynamic>();

            if (r.reportType == "0")
            {
                itemList = itemList.Where(p => p.l.expireDate <= d1).ToList();
                itemList = itemList.OrderBy(d => d.l.expireDate).ToList();
            }
            else if (r.reportType == "1")
            {
                itemList = itemList.Where(p => p.l.expireDate >= d1 && p.l.expireDate <= DateTime.Now).ToList();
                itemList = itemList.OrderBy(d => d.l.expireDate).ToList();
            }
            return Ok(itemList);
        }
    }

}
