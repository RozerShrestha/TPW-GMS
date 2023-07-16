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
            else if(r.reportType== "renewed")
            {
                var itemsRenewed = (from m in db.MemberInformations
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
                    itemsRenewed = itemsRenewed.Where(c => c.branch == r.branch);
                if (r.membershipOption != "ALL")
                    itemsRenewed = itemsRenewed.Where(c => c.memberOption == r.membershipOption);
                if (r.catagory != "ALL")
                    itemsRenewed = itemsRenewed.Where(c => c.memberCatagory == r.catagory);
                if (r.duration != "ALL")
                    itemsRenewed = itemsRenewed.Where(c => c.memberPaymentType == r.duration);
                if (r.shift != "ALL")
                    itemsRenewed = itemsRenewed.Where(c => c.shift == r.shift);

                //itemsRenewed = itemsRenewed.OrderBy(d => d.updatedDate);
                return Ok(itemsRenewed);
            }
            else if(r.reportType=="extended")
            {
                var itemExtended=(from m in db.MemberInformationLogs
                                  where m.createdDate >= Convert.ToDateTime(r.startDate) && 
                                  m.createdDate <= Convert.ToDateTime(r.endDate) && 
                                  m.renewExtend == r.reportType && (m.memberOption == "Regular" || m.memberOption == "OffHour" || m.memberOption == "Universal")
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
                                      m.receiptNo,
                                      m.paidAmount,
                                      m.dueAmount,
                                      m.dueClearAmount,
                                      m.finalAmount



                                  }).AsEnumerable();
                if (r.branch != "ALL")
                    itemExtended = itemExtended.Where(c => c.branch == r.branch);
                if (r.membershipOption != "ALL")
                    itemExtended = itemExtended.Where(c => c.memberOption == r.membershipOption);
                if (r.catagory != "ALL")
                    itemExtended = itemExtended.Where(c => c.memberCatagory == r.catagory);
                if (r.duration != "ALL")
                    itemExtended = itemExtended.Where(c => c.memberPaymentType == r.duration);
                if (r.shift != "ALL")
                    itemExtended = itemExtended.Where(c => c.shift == r.shift);

                return Ok(itemExtended);
            }
            else
            {
                return null;
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
                itemList = db.MemberInformations.Where(k=> k.memberOption != "Trainer" && k.memberOption != "Operation Manager" && k.memberOption != "Gym Admin").ToList();
            else
                itemList = db.MemberInformations.Where(k =>k.branch==r.branch &&(k.memberOption != "Trainer" && k.memberOption != "Operation Manager" && k.memberOption != "Gym Admin")).ToList();

            if (r.reportType == "0")
            {
                itemList = itemList.Where(p => p.memberExpireDate <= d1).ToList();
            }
            else if (r.reportType == "1")
            {
                itemList = itemList.Where(p => p.memberExpireDate >= d1 && p.memberExpireDate <= DateTime.Now).ToList();
            }
            else if(r.reportType== "ExCientCallBack")
            {
                itemList = itemList.Where(p => p.memberExpireDate >= NepaliDateService.NepToEng(r.startDate) && p.memberExpireDate <= NepaliDateService.NepToEng(r.endDate)).ToList();
                itemList = itemList.OrderBy(d => d.memberExpireDate).ToList();
            }
            else if(r.reportType== "PaymentReminder")
            {
                itemList = itemList.Where(p => p.memberExpireDate >= NepaliDateService.NepToEng(r.startDate) && p.memberExpireDate <= NepaliDateService.NepToEng(r.endDate)).ToList();
                itemList = itemList.OrderBy(d => d.memberExpireDate).ToList();
            }
            else if(r.reportType== "NewAdmissionCallBack")
            {
                itemList = itemList.Where(p => p.memberDate >= NepaliDateService.NepToEng(r.startDate) && p.memberDate <= NepaliDateService.NepToEng(r.endDate)).ToList();
                itemList = itemList.OrderBy(d => d.memberDate).ToList();
            }
            
            itemList=itemList.OrderBy(d=>d.branch).ToList();
            return Ok(itemList);
        }

        [Route("api/GetAbsentCallBackList")]
        [HttpPost]
        public IHttpActionResult GetAbsentCallBackList(ReportParam r)
        {
            try
            {
                r.startDate = NepaliDateService.NepToEng(r.startDate).ToString();
                using (var conn = dbCon.CreateConnection())
                {
                    var result = conn.Query("AbsentCallBackList",
                        param: new { @wedDate= r.startDate, r.branch }
                        , commandType: System.Data.CommandType.StoredProcedure).ToList();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [Route("api/GetRandomCallList")]
        [HttpPost]
        public IHttpActionResult GetRandomCallList(ReportParam r)
        {
            try
            {
               var result=(from p in db.RandomCallListMonthlies
                           where p.dateCreated==NepaliDateService.NepToEng("2080-03-30").ToString()
                           select p).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [Route("api/GetLockerExpiry/")]
        [HttpPost]
        public IHttpActionResult GetLockerExpiry(ReportParam r)
        {
            var dateOneMonth = DateTime.Now.AddMonths(-1);
            var dateSevenDays = DateTime.Now.AddDays(7);
            List<dynamic> lockerItem = new List<dynamic>();

            //var query = (from l in db.LockerMgs
            //             join m in db.MemberInformations
            //             on l.memberId equals m.memberId
            //             into MemberInfoLockerGroup
            //             from mem in MemberInfoLockerGroup.DefaultIfEmpty()
            //             select new
            //             {
            //                 l,
            //                 mem 
            //             }).ToList<dynamic>();

            if (r.branch == "ALL")
                //lockerItem = db.LockerMgs.ToList();
                //lockerItem = (from l in db.LockerMgs
                //            join m in db.MemberInformations
                //            on l.memberId equals m.memberId
                //            select new
                //            {
                //                l,
                //                m
                //            }).ToList<dynamic>();
                lockerItem = (from l in db.LockerMgs
                             join mem in db.MemberInformations
                             on l.memberId equals mem.memberId
                             into MemberInfoLockerGroup
                             from m in MemberInfoLockerGroup.DefaultIfEmpty()
                             select new
                             {
                                 l,
                                 m
                             }).ToList<dynamic>();
            else
                //lockerItem = (from l in db.LockerMgs
                //            join m in db.MemberInformations
                //            on l.memberId equals m.memberId
                //            where l.branch == r.branch
                //            select new
                //            {
                //                l,
                //                m
                //            }).ToList<dynamic>();
                lockerItem = (from l in db.LockerMgs
                              join mem in db.MemberInformations
                              on l.memberId equals mem.memberId
                              into MemberInfoLockerGroup
                              from m in MemberInfoLockerGroup.DefaultIfEmpty()
                              where l.branch == r.branch
                              select new
                              {
                                  l,
                                  m
                              }).ToList<dynamic>();
            //if reportType is Active
            if (r.reportType == "1")
            {
                lockerItem = lockerItem.Where(p => p.l.expireDate >= DateTime.Now).ToList();
            }
            //if reportType is Expired
            if (r.reportType == "2")
            {
                lockerItem = lockerItem.Where(p => p.l.expireDate < DateTime.Now).ToList();
            }
            //if reportType is Expired within 7 days
            else if (r.reportType == "3")
            {
                lockerItem = lockerItem.Where(p => p.l.expireDate >= DateTime.Now && p.l.expireDate <= dateSevenDays ).ToList();
               
            }
            lockerItem = lockerItem.OrderBy(d => d.l.lockerNumber).ToList();
            return Ok(lockerItem);
        }
    }

}
