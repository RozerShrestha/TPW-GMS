using Dapper;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using TPW_API.Models;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Repository;
using TPW_GMS.Services;

namespace TPW_GMS.Controllers
{
    [Authorize]
    public class AttendanceController : ApiController
    {
        public DbConFactory dbCon = new DbConFactory();
        private Logger _logger;
        public AttendanceController()
        {
            _logger = LogManager.GetLogger("f");
        }

        [Route("api/Attendance/")]
        [HttpGet]
        public IHttpActionResult Attendance(string memberId, string loginBranch, string type)
        {
            AttendanceResponse ar = new AttendanceResponse();
            //for members
            if (memberId.Contains("TPW"))
            {
                MemberAttandance m = new MemberAttandance();
                using (TPWDataContext db = new TPWDataContext())
                {
                    try
                    {
                        string trimMemberId = memberId.Trim();
                        string pendingPayment = "";
                        //bool isValid = false;
                        bool isValid = true;
                        var hr = DateTime.Now.Hour;
                        var attCount = 0;
                        var applicationUniversal = false;
                        List<string> pendingPaymentList = new List<string>();

                        if (type == "activate")
                        {
                            var sitem = db.StartStops.Where(p => p.memberId == trimMemberId).SingleOrDefault();
                            var mitem = db.MemberInformations.Where(ma => ma.memberId == trimMemberId).SingleOrDefault();
                            try
                            {
                                if (sitem.stopDate.HasValue && !sitem.startDate.HasValue)
                                {
                                    int stopLimit = Convert.ToInt32(sitem.stopLimit);
                                    DateTime startDate = DateTime.Now;
                                    DateTime stopDate = Convert.ToDateTime(sitem.stopDate);
                                    DateTime mExpireDate = Convert.ToDateTime(mitem.memberExpireDate);

                                    double stopDays = (startDate - stopDate).TotalDays;
                                    sitem.stopLimit = (stopLimit - 1);
                                    sitem.startDate = startDate;
                                    sitem.stopDays = Convert.ToInt32(stopDays);

                                    mitem.memberExpireDate = mExpireDate.AddDays(stopDays);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.Error(ex.Message);
                            }
                            finally
                            {
                                mitem.ActiveInactive = "Active";
                                db.SubmitChanges();
                            }
                        }

                        //var query = (from p in db.MemberInformations
                        //             where p.memberId == trimMemberId
                        //             select p).SingleOrDefault();


                        var query = (from p in db.MemberInformations
                                     join l in db.LockerMgs
                                     on p.memberId equals l.memberId
                                     into MemberInfoLockerGroup
                                     from loc in MemberInfoLockerGroup.DefaultIfEmpty()
                                     where p.memberId == trimMemberId
                                     select new
                                     {
                                         p,
                                         loc
                                     }).SingleOrDefault();

                        var yesterdays = DateTime.Now.AddDays(-1).Date;
                        var todays = DateTime.Now;

                        var sellItemQuery = (from p in db.SellItems
                                             where p.memberId == trimMemberId && p.isPaidItemSell == false
                                             select p);

                        var supplementQuery = (from s in db.SuplementSellings
                                               where s.customerIdSell == trimMemberId && s.isPaidSuplementSell == false
                                               select s);

                        var attendanceCount = (from a in db.MemberAttandances
                                               where a.memberId == trimMemberId && Convert.ToDateTime(a.checkin).Date > yesterdays
                                               select a);
                        var checkDueAmount = (from p in db.PaymentInfos
                                              where p.memberId == trimMemberId && p.dueAmount != 0
                                              select p.dueAmount).SingleOrDefault();

                        if (query != null)
                        {
                            if (query.p.ActiveInactive == "Active")
                            {
                                foreach (var item in sellItemQuery)
                                {
                                    pendingPayment += "(Merchan due amount)-" + item.itemTypeSell + "\n";
                                }
                                foreach (var supItems in supplementQuery)
                                {
                                    pendingPayment += "(Supplement due amount)-" + supItems.nameOfSuplement_Sell + ": " + supItems.finalPrice_Sell + "\n";
                                }
                                if (checkDueAmount != null)
                                {
                                    pendingPayment += "Gym Membership due amount: " + checkDueAmount + "\n";
                                }

                                //check if the member is from same branch or different
                                //member from same branch
                                if (query.p.branch.Equals(loginBranch))
                                {
                                    //in case of offhour
                                    if (query.p.memberOption == "OffHour")
                                    {
                                        isValid = hr >= 10 && hr <= 16 ? true : false;
                                    }
                                    //in case of normal customer
                                    else
                                    {
                                        isValid = true;
                                    }
                                }
                                //member from different branch
                                else
                                {
                                    applicationUniversal = true;
                                    isValid = query.p.universalMembershipLimit == 0 ? false : true;
                                    if (isValid)
                                    {
                                        //in case of offhour
                                        if (query.p.memberOption == "Offhour")
                                        {
                                            isValid = hr >= 10 && hr <= 16 ? true : false;
                                        }
                                        //in case of normal customer
                                        else
                                        {
                                            isValid = true;
                                        }
                                    }
                                }

                                if (attendanceCount.Count() == 0)
                                {
                                    if (isValid)
                                    {
                                        //insert into Attendace
                                        m.memberId = trimMemberId;
                                        m.checkin = DateTime.Now;
                                        m.checkout = DateTime.Now.AddHours(2);
                                        m.branch = query.p.branch;
                                        m.checkinBranch = loginBranch;
                                        db.MemberAttandances.InsertOnSubmit(m);
                                        if (applicationUniversal)
                                            query.p.universalMembershipLimit -= 1;

                                        db.SubmitChanges();
                                        db.Dispose();
                                        ar.message = "Member Attendance Successful";
                                    }
                                    attCount = 0;
                                }
                                else
                                {
                                    ar.message = "Member Attendance UnSuccessful";
                                    attCount = 1;
                                }
                            }
                            if (query.p.memberExpireDate < DateTime.Today)
                            {
                                ar.isMembershipExpired = true;
                            }
                           


                            ar.firstName = query.p.firstName;
                            ar.lastName = query.p.lastName;
                            ar.fullName = $"{query.p.firstName} {query.p.lastName}"; 
                            ar.branch = query.p.branch;
                            ar.universalMembershipLimit = query.p.universalMembershipLimit;
                            ar.membershipDate = NepaliDateService.EngToNep(Convert.ToDateTime(query.p.memberDate)).ToString();
                            ar.membershipBeginDate = NepaliDateService.EngToNep(Convert.ToDateTime(query.p.memberBeginDate)).ToString();
                            ar.membershipExpireDate = NepaliDateService.EngToNep(Convert.ToDateTime(query.p.memberExpireDate)).ToString();
                            if (query.loc != null)
                            {
                                ar.lockerRenewDate = NepaliDateService.EngToNep(Convert.ToDateTime(query.loc.renewDate)).ToString();
                                ar.lockerExpiredDate = NepaliDateService.EngToNep(Convert.ToDateTime(query.loc.expireDate)).ToString();
                                ar.isLockerExpired = query.loc.expireDate<DateTime.Today?true:false;
                            }
                            

                            ar.pendingPayment = pendingPayment;
                            ar.membershipOption = query.p.memberOption;
                            ar.membershipStatus = query.p.ActiveInactive;
                            ar.attendanceCount = attCount;
                            ar.isValid = isValid;
                            return Ok(ar);
                        }
                        else
                        {
                            ar.isValid = false;
                            ar.message = "Customer Not Found";
                            return Ok(ar);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                        return BadRequest("Attendance unsuccessful");
                    }
                }
            }
            //For Email Marketing 
            else if (memberId.Substring(0, 1) == "{")
            {
                var yesterday = DateTime.Now.AddDays(-1).Date;
                var emObj = JsonConvert.DeserializeObject<EmMarketing>(memberId);
                using (TPWDataContext db = new TPWDataContext())
                {
                    var item = db.EmMarketings.Where(p => p.email == emObj.email).SingleOrDefault();
                    var isAttendanceDone = db.MarketingAttendances.Any(p => p.email == emObj.email && Convert.ToDateTime(p.checkin).Date > yesterday);
                    ar.firstName = emObj.name;
                    ar.membershipOption = "Email Marketing Client";
                    if (!isAttendanceDone)
                    {
                        if (item.attCount < 5)
                        {
                            try
                            {
                                MarketingAttendance m = new MarketingAttendance()
                                {
                                    name = emObj.name,
                                    email = emObj.email,
                                    mobile = emObj.mobile,
                                    branch = emObj.branch,
                                    checkinBranch = loginBranch,
                                    checkin = DateTime.Now,
                                    checkout = DateTime.Now.AddHours(2)
                                };
                                db.MarketingAttendances.InsertOnSubmit(m);
                                item.attCount += 1;
                                db.SubmitChanges();
                                
                                ar.message = "Email Marketing Attendance Successful";
                                ar.isValid = true;
                            }
                            catch (Exception ex)
                            {
                                ar.message = ex.Message;
                            }
                        }
                        else
                        {
                            ar.message = "Time Period Expired";
                        }
                    }
                    else
                    {
                        ar.message = "Already Checked In";
                    }

                    return Ok(ar);
                }
            }
            //for Guest attendance
            else
            {
                var guestEmail = Service.DecryptString(memberId);
                var yesterday = DateTime.Now.AddDays(-1).Date;
                //var emObj = JsonConvert.DeserializeObject<Guest>(guestEmail);
                //Response r = new Response();
                using (TPWDataContext db = new TPWDataContext())
                {
                    var guestRecord = db.Guests.Where(p => p.email == guestEmail).SingleOrDefault();
                    var isAttendanceDone = db.GuestAttandances.Any(p => p.email == guestEmail && Convert.ToDateTime(p.checkin).Date > yesterday);
                    ar.firstName = guestRecord.name;
                    ar.membershipOption = "Guest";
                    ar.membershipBeginDate = guestRecord.fromDate.ToString();
                    ar.membershipExpireDate = guestRecord.toDate.ToString();
                    if (!isAttendanceDone)
                    {
                        //in case of from and to date
                        if (guestRecord.fromDate != null)
                        {
                            if (DateTime.Now <= guestRecord.toDate)
                            {
                                if (guestRecord.attCount < guestRecord.count)
                                {
                                    try
                                    {
                                        GuestAttandance m = new GuestAttandance()
                                        {
                                            guestId = guestRecord.id,
                                            name = guestRecord.name,
                                            email = guestRecord.email,
                                            checkinBranch = loginBranch,
                                            checkin = DateTime.Now,
                                            checkout = DateTime.Now.AddHours(2)
                                        };
                                        db.GuestAttandances.InsertOnSubmit(m);
                                        guestRecord.attCount += 1;
                                        db.SubmitChanges();

                                        ar.message = "Guest Attendance Successful";
                                        ar.isValid = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        ar.isValid = false;
                                        ar.message = ex.Message;
                                    }
                                }
                                else
                                {
                                    ar.isValid = false;
                                    ar.message = "Guest Attendance Limit has Exceed";
                                }
                            }
                            else
                            {
                                ar.isValid = false;
                                ar.message = "Time Period Expired";
                            }
                        }
                        //in case of count only
                        else
                        {
                            if (guestRecord.attCount < guestRecord.count)
                            {
                                try
                                {
                                    GuestAttandance m = new GuestAttandance()
                                    {
                                        guestId = guestRecord.id,
                                        name = guestRecord.name,
                                        email = guestRecord.email,
                                        checkinBranch = loginBranch,
                                        checkin = DateTime.Now,
                                        checkout = DateTime.Now.AddHours(2)
                                    };
                                    db.GuestAttandances.InsertOnSubmit(m);
                                    guestRecord.attCount += 1;
                                    db.SubmitChanges();

                                    ar.firstName = guestRecord.name;
                                    ar.membershipOption = "Guest";
                                    ar.membershipBeginDate = guestRecord.fromDate.ToString();
                                    ar.membershipExpireDate = guestRecord.toDate.ToString();
                                    ar.message = "Guest Login";
                                    ar.isValid = true;
                                }
                                catch (Exception ex)
                                {
                                    ar.isValid = false;
                                    ar.message = ex.Message;
                                }
                            }
                            else
                            {
                                ar.isValid = false;
                                ar.message = "Attendance Limit has exeed";
                            }
                        }

                    }
                    else
                    {
                        ar.isValid = false;
                        ar.attendanceCount = 1;
                        ar.message = "Already Checked In";
                    }

                    return Ok(ar);
                }
            }
        }

        [Route("api/GetMemberAttendanceList")]
        public IHttpActionResult GetMemberAttendanceList(string loginUser)
        {
            using (TPWDataContext db = new TPWDataContext())
            {

                var today = DateTime.Today;
                var attendanceToday = from p in db.MemberAttandances
                                      join a in db.MemberInformations on p.memberId equals a.memberId
                                      where DateTime.Compare(Convert.ToDateTime(p.checkin).Date, DateTime.Now.Date) == 0 && p.checkinBranch == loginUser
                                      orderby p.checkin descending
                                      select new
                                      {
                                          p.memberId,
                                          a.fullname,
                                          a.memberOption,
                                          p.checkin,
                                          p.checkout,
                                          p.branch,
                                          p.checkinBranch,
                                      };
                attendanceToday = attendanceToday.OrderByDescending(d => d.checkin);
                return Ok(attendanceToday.ToList());
            }
        }

        [Route("api/GetGuestAttendanceList")]
        public IHttpActionResult GetGuestAttendanceList(string loginUser)
        {
            using (TPWDataContext db = new TPWDataContext())
            {

                var today = DateTime.Today;
                var attendanceToday = from p in db.GuestAttandances
                                      where DateTime.Compare(Convert.ToDateTime(p.checkin).Date, DateTime.Now.Date) == 0 && p.checkinBranch == loginUser
                                      orderby p.checkin descending
                                      select new
                                      {
                                          p.id,
                                          p.name,
                                          p.checkin,
                                          p.checkout,
                                          p.checkinBranch,
                                      };
                attendanceToday = attendanceToday.OrderByDescending(d => d.checkin);
                return Ok(attendanceToday.ToList());
            }
        }

        [Route("api/GetEmailMarketingAttendanceList")]
        public IHttpActionResult GetEmailMarketingAttendanceList(string loginUser)
        {
            using (TPWDataContext db = new TPWDataContext())
            {

                var today = DateTime.Today;
                var attendanceToday = from p in db.MarketingAttendances
                                      where DateTime.Compare(Convert.ToDateTime(p.checkin).Date, DateTime.Now.Date) == 0 && p.checkinBranch == loginUser
                                      orderby p.checkin descending
                                      select new
                                      {
                                          p.id,
                                          p.name,
                                          p.checkin,
                                          p.checkout,
                                          p.checkinBranch,
                                      };
                attendanceToday = attendanceToday.OrderByDescending(d => d.checkin);
                return Ok(attendanceToday.ToList());
            }
        }

        [Route("api/StaffAttendance/")]
        [HttpGet]
        public string StaffAttendance(string encryptedMemberId, string loginBranch)
        {
            _logger.Info("##" + "Encrypted Message: " + encryptedMemberId);
            CultureInfo provider = CultureInfo.InvariantCulture;
            //encryptedMemberId="EdiM4UTvq/FyzUdeXuk2ijX+xSSmhPBxIpMPpzI7SIc="
            var decryptedMemberId = Service.DecryptString(encryptedMemberId);
            var splitData = Regex.Split(decryptedMemberId, @"//");
            var memberId = splitData[0];
            _logger.Info("##" + "DecryptedMessage " + memberId);
            using (TPWDataContext db = new TPWDataContext())
            {
                try
                {
                    var todayDate8 = splitData[1];
                    var dateFromEncryptedD = DateTime.ParseExact(todayDate8, "yyyyMMdd", provider);
                    var todayDate = DateTime.Today;
                    if (DateTime.Compare(dateFromEncryptedD, todayDate) != 0)
                    {
                        return "Invalid Request, Please scan through QR Code";
                    }

                    StaffAttandance s = new StaffAttandance();
                    string trimMemberId = memberId.Trim();

                    var staffInfo = (from p in db.Staffs
                                     where p.memberId == trimMemberId
                                     select p).SingleOrDefault();
                    //var staffEmail = (from em in db.MemberInformations
                    //                  where em.memberId == trimMemberId
                    //                  select em.email).SingleOrDefault();

                    var twoShift = staffInfo.from2 == "" ? false : true;
                    var yesterdays = DateTime.Now.AddDays(-1).Date;
                    var punchinTime = DateTime.Now;
                    var returnMessage = "";

                    var staffExtraExtenstionLimit = (from p in db.ExtraInformations
                                                     select p.staffLateExtension).SingleOrDefault();

                    var attendance = (from a in db.StaffAttandances
                                      where a.memberId == trimMemberId && Convert.ToDateTime(a.checkin).Date > yesterdays
                                      select a);

                    //General for both the shift
                    if (attendance.Count() == 0)
                    {
                        var limit = DateTime.Parse(staffInfo.from1).AddMinutes(Convert.ToDouble(staffExtraExtenstionLimit));
                        s.memberId = staffInfo.memberId;
                        s.checkin = punchinTime;
                        s.branch = staffInfo.associateBranch;
                        s.checkinBranch = loginBranch;
                        if (punchinTime > limit)
                        {
                            s.remark = "Late Punch In";
                            s.lateFlag = true;
                            string message = "Dear " + staffInfo.staffName + "," + Environment.NewLine + Environment.NewLine +
                                "Please Come in Time, Your PunchIn has been recored as Late PunchIn." + Environment.NewLine +
                                "Thank you." + Environment.NewLine + Environment.NewLine +
                                "Regards," + Environment.NewLine +
                                "The Physique Workshop";
                            //new Task(() =>
                            //{
                            //    MailService.SendEmailStaffAttendence(message, staffEmail, "Late PunchIn");
                            //}).Start();
                        }
                        else
                        {
                            s.remark = "On Time";
                            s.lateFlag = false;
                        }
                        db.StaffAttandances.InsertOnSubmit(s);
                        db.SubmitChanges();
                        _logger.Info("##" + "Staff ID: " + trimMemberId + " Staff Name: " + staffInfo.staffName);
                        if (punchinTime > limit)
                        {
                            insertIntoStaffDeduction(trimMemberId);
                        }


                        returnMessage = "Attendance Successful";
                        return returnMessage;
                    }
                    var staffLatestAttendance = db.StaffAttandances
                                .OrderByDescending(aa => aa.attendanceId)
                                .Where(aa => aa.memberId == trimMemberId && Convert.ToDateTime(aa.checkin).Date > yesterdays)
                                .First();

                    var attendanceCount = attendance.Count();
                    var checkout = attendance == null ? false : staffLatestAttendance.checkout == null ? false : true;
                    //for double shift
                    if (twoShift)
                    {
                        if (attendanceCount == 2 && staffLatestAttendance.checkout != null)
                        {
                            returnMessage = "Attendance Completed, Please Process Tomorrow";
                        }
                        else if (attendanceCount == 1 && staffLatestAttendance.checkout != null)
                        {
                            var limit = DateTime.Parse(staffInfo.from2).AddMinutes(Convert.ToDouble(staffExtraExtenstionLimit));
                            s.memberId = staffInfo.memberId;
                            s.checkin = punchinTime;
                            s.branch = staffInfo.associateBranch;
                            s.checkinBranch = loginBranch;
                            if (punchinTime > limit)
                            {
                                s.remark = "Late Punch In";
                                s.lateFlag = true;
                                string message = "Dear " + staffInfo.staffName + "," + Environment.NewLine + Environment.NewLine +
                                    "Please Come in Time, Your PunchIn has been recored as Late PunchIn." + Environment.NewLine +
                                    "Thank you." + Environment.NewLine + Environment.NewLine +
                                    "Regards," + Environment.NewLine +
                                    "The Physique Workshop";

                                new Task(() =>
                                {
                                    MailService.SendEmailStaffAttendence(message, trimMemberId, "Late PunchIn");
                                }).Start();
                            }
                            else
                            {
                                s.remark = "On Time";
                                s.lateFlag = false;
                            }
                            db.StaffAttandances.InsertOnSubmit(s);
                            db.SubmitChanges();
                            _logger.Info("##" + "Staff ID: " + trimMemberId + "Staff Name: " + staffInfo.staffName);
                            if (punchinTime > limit)
                            {
                                insertIntoStaffDeduction(trimMemberId);
                            }
                            returnMessage = "Checkin";
                        }
                        else if (!checkout)
                        {
                            returnMessage = "CheckOut";
                        }
                        return returnMessage;
                    }
                    //for single shift
                    else
                    {
                        if (attendanceCount == 1 && staffLatestAttendance.checkout != null)
                        {
                            returnMessage = "Attendance Completed, Please Process Tomorrow";
                        }
                        else if (!checkout)
                        {
                            returnMessage = "CheckOut";
                        }
                        return returnMessage;
                    }
                    //First time checkin
                }
                catch (Exception ex)
                {
                    _logger.Warn("##" + "Attendance Unsuccessful: " + memberId, ex.Message);
                    return "Attendance Unsuccessful";
                }
            }
        }

        [Route("api/GetStaffAttendanceList")]
        public IHttpActionResult GetStaffAttendanceList(string loginUser)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                    var today = DateTime.Today;
                    var attendanceToday = from p in db.StaffAttandances
                                          join a in db.MemberInformations on p.memberId equals a.memberId
                                          where DateTime.Compare(Convert.ToDateTime(p.checkin).Date, DateTime.Now.Date) == 0 && p.checkinBranch == loginUser
                                          orderby p.checkin descending
                                          select new
                                          {
                                              p.memberId,
                                              a.fullname,
                                              a.memberOption,
                                              p.checkin,
                                              p.checkout,
                                              p.branch,
                                              p.checkinBranch,
                                              p.remark,
                                              p.lateFlag
                                          };
                attendanceToday = attendanceToday.OrderByDescending(d => d.checkin);
                return Ok(attendanceToday.ToList());
            }
        }

        [Route("api/EMAttendance/")]
        [HttpGet]
        public IHttpActionResult EMAttendance(string emJsonData, string loginBranch)
        {
            var yesterday = DateTime.Now.AddDays(-1).Date;
            var emObj = JsonConvert.DeserializeObject<EmMarketing>(emJsonData);
            Response r = new Response();
            using (TPWDataContext db = new TPWDataContext())
            {
                var item = db.EmMarketings.Where(p => p.email == emObj.email).SingleOrDefault();
                var isAttendanceDone = db.MarketingAttendances.Any(p => p.email == emObj.email && Convert.ToDateTime(p.checkin).Date > yesterday);
                if (!isAttendanceDone)
                {
                    if (item.attCount < 5)
                    {
                        try
                        {
                            MarketingAttendance m = new MarketingAttendance()
                            {
                                name = emObj.name,
                                email = emObj.email,
                                mobile = emObj.mobile,
                                branch = emObj.branch,
                                checkinBranch = loginBranch,
                                checkin = DateTime.Now,
                                checkout = DateTime.Now.AddHours(2)
                            };
                            db.MarketingAttendances.InsertOnSubmit(m);
                            item.attCount += 1;
                            db.SubmitChanges();
                            r.status = 200;
                            r.message = "Success";
                        }
                        catch (Exception ex)
                        {
                            r.status = 400;
                            r.message = ex.Message;
                        }
                    }
                    else
                    {
                        r.status = 403;
                        r.message = "Time Period Expired";
                    }
                }
                else
                {
                    r.status = 409;
                    r.message = "Already Checked In";
                }

                return Ok(r);
            }
        }

        [Route("api/GuestAttendance/")]
        [HttpGet]
        public IHttpActionResult GuestAttendance(string emJsonData, string loginBranch)
        {
            emJsonData = Service.DecryptString(emJsonData);
            var yesterday = DateTime.Now.AddDays(-1).Date;
            var emObj = JsonConvert.DeserializeObject<Guest>(emJsonData);
            Response r = new Response();
            using (TPWDataContext db = new TPWDataContext())
            {
                var item = db.Guests.Where(p => p.email == emObj.email).SingleOrDefault();
                var isAttendanceDone = db.GuestAttandances.Any(p => p.email == emObj.email && Convert.ToDateTime(p.checkin).Date > yesterday);
                if (!isAttendanceDone)
                {
                    //in case of from and to date
                    if (item.fromDate != null)
                    {
                        if (DateTime.Now <= item.toDate)
                        {
                            if (item.attCount < item.count)
                            {
                                try
                                {
                                    GuestAttandance m = new GuestAttandance()
                                    {
                                        guestId = emObj.id,
                                        name = emObj.name,
                                        email = emObj.email,
                                        checkinBranch = loginBranch,
                                        checkin = DateTime.Now,
                                        checkout = DateTime.Now.AddHours(2)
                                    };
                                    db.GuestAttandances.InsertOnSubmit(m);
                                    item.attCount += 1;
                                    db.SubmitChanges();
                                    r.status = 200;
                                    r.message = "Success";
                                }
                                catch (Exception ex)
                                {
                                    r.status = 400;
                                    r.message = ex.Message;
                                }
                            }
                            else
                            {
                                r.status = 403;
                                r.message = "Count Limit Exceed";
                            }
                        }
                        else
                        {
                            r.status = 403;
                            r.message = "Time Period Expired";
                        }
                    }
                    //in case of count only
                    else
                    {
                        if (item.attCount < item.count)
                        {
                            try
                            {
                                GuestAttandance m = new GuestAttandance()
                                {
                                    guestId = emObj.id,
                                    name = emObj.name,
                                    email = emObj.email,
                                    checkinBranch = loginBranch,
                                    checkin = DateTime.Now,
                                    checkout = DateTime.Now.AddHours(2)
                                };
                                db.GuestAttandances.InsertOnSubmit(m);
                                item.attCount += 1;
                                db.SubmitChanges();
                                r.status = 200;
                                r.message = "Success";
                            }
                            catch (Exception ex)
                            {
                                r.status = 400;
                                r.message = ex.Message;
                            }
                        }
                        else
                        {
                            r.status = 403;
                            r.message = "Count Limit Exceed";
                        }
                    }
                    
                }
                else
                {
                    r.status = 409;
                    r.message = "Already Checked In";
                }

                return Ok(r);
            }
        }

        [Route("api/GetAttendanceHistoryAdmin/")]
        [HttpPost]
        public IHttpActionResult GetAttendanceHistoryAdmin(ReportParam r)
        {
            List<AttendanceHistoryResponse> result = new List<AttendanceHistoryResponse>();
            var startDate = NepaliDateService.NepToEng(r.startDate);
            var endDate = NepaliDateService.NepToEng(r.endDate);
            using (TPWDataContext db = new TPWDataContext())
            {
                switch (r.who)
                {
                    case "admin":
                        if (r.membershipOption == "Staff")
                        {
                            var staffAttendance = from p in db.MemberInformations
                                                  join q in db.StaffAttandances on p.memberId equals q.memberId
                                                  where  q.checkin >= Convert.ToDateTime(startDate) && q.checkin <= Convert.ToDateTime(endDate).AddDays(1)
                                                  select new AttendanceHistoryResponse
                                                  {
                                                      MemberId = p.memberId,
                                                      FullName = p.fullname,
                                                      CheckIn = q.checkin,
                                                      CheckOut = q.checkout,
                                                      Branch = q.branch,
                                                      CheckInBranch = q.checkinBranch,
                                                      Remark = q.remark,
                                                      LateFlag = q.lateFlag
                                                  };
                            result = staffAttendance.ToList();
                        }
                        else if (r.membershipOption == "Members")
                        {
                            if (r.branch == "0")
                            {
                                var attendance = from m in db.MemberInformations
                                                 join a in db.MemberAttandances on m.memberId equals a.memberId
                                                 where a.checkin >= Convert.ToDateTime(startDate) && a.checkin <= Convert.ToDateTime(endDate).AddDays(1)
                                                 select new AttendanceHistoryResponse
                                                 {
                                                     MemberId = m.memberId,
                                                     FullName = m.fullname,
                                                     MemberOption = m.memberOption,
                                                     CheckIn = a.checkin,
                                                     CheckOut = a.checkout,
                                                     Branch = a.branch,
                                                     CheckInBranch = a.checkinBranch
                                                 };
                                result = attendance.ToList();
                            }
                            else
                            {
                                var attendance = from m in db.MemberInformations
                                                 join a in db.MemberAttandances on m.memberId equals a.memberId
                                                 where a.branch==r.branch && a.checkin >= Convert.ToDateTime(startDate) && a.checkin <= Convert.ToDateTime(endDate).AddDays(1)
                                                 select new AttendanceHistoryResponse
                                                 {
                                                     MemberId = m.memberId,
                                                     FullName = m.fullname,
                                                     MemberOption = m.memberOption,
                                                     CheckIn = a.checkin,
                                                     CheckOut = a.checkout,
                                                     Branch = a.branch,
                                                     CheckInBranch = a.checkinBranch
                                                 };
                                result = attendance.ToList();
                            }
                            
                        }
                        else if (r.membershipOption == "Regular" || r.membershipOption == "OffHour" || r.membershipOption == "Universal")
                        {
                            var attendance = from m in db.MemberInformations
                                             join a in db.MemberAttandances on m.memberId equals a.memberId
                                             where a.checkin >= Convert.ToDateTime(startDate) && a.checkin <= Convert.ToDateTime(endDate).AddDays(1) && m.memberId == r.memberId
                                             select new AttendanceHistoryResponse
                                             {
                                                 MemberId = m.memberId,
                                                 FullName = m.fullname,
                                                 MemberOption = m.memberOption,
                                                 CheckIn = a.checkin,
                                                 CheckOut = a.checkout,
                                                 Branch = a.branch,
                                                 CheckInBranch = a.checkinBranch
                                             };
                            result = attendance.ToList();
                        }
                        else if (r.membershipOption == "Trainer" || r.membershipOption == "Gym Admin" || r.membershipOption == "Operation Manager" || r.membershipOption=="Intern")
                        {
                            var staffAttendance = from p in db.MemberInformations
                                                  join q in db.StaffAttandances on p.memberId equals q.memberId
                                                  where q.checkin >= Convert.ToDateTime(startDate) && q.checkin <= Convert.ToDateTime(endDate).AddDays(1) && p.memberId == r.memberId
                                                  select new AttendanceHistoryResponse
                                                  {
                                                      MemberId = p.memberId,
                                                      FullName = p.fullname,
                                                      CheckIn = q.checkin,
                                                      CheckOut = q.checkout,
                                                      Branch = q.branch,
                                                      CheckInBranch = q.checkinBranch,
                                                      Remark = q.remark,
                                                      LateFlag = q.lateFlag
                                                  };
                            result = staffAttendance.ToList();
                        }
                        else if(r.membershipOption=="Free User")
                        {
                            var attendance = from p in db.MarketingAttendances
                                             where p.checkin >= Convert.ToDateTime(startDate) && p.checkin <= Convert.ToDateTime(endDate).AddDays(1)
                                             select new AttendanceHistoryResponse
                                             {
                                                 MemberId = "-",
                                                 FullName = p.name,
                                                 MemberOption = "Free User",
                                                 CheckIn = p.checkin,
                                                 CheckOut = p.checkout,
                                                 Branch = p.branch,
                                                 CheckInBranch = p.checkinBranch
                                             };
                            result = attendance.ToList();
                        }
                        else if (r.membershipOption == "Guest")
                        {
                            var attendance=from p in db.GuestAttandances
                                           where p.checkin >= Convert.ToDateTime(startDate) && p.checkin <= Convert.ToDateTime(endDate).AddDays(1)
                                           select new AttendanceHistoryResponse
                                           {
                                               MemberId = "-",
                                               FullName = p.name,
                                               MemberOption = "Guest",
                                               CheckIn = p.checkin,
                                               CheckOut = p.checkout,
                                               Branch = "-",
                                               CheckInBranch = p.checkinBranch
                                           };
                            result = attendance.ToList();
                        }
                        break;
                    case "branch":
                        if (r.membershipOption == "Staff")
                        {
                            var staffAttendance = from p in db.MemberInformations
                                                  join q in db.StaffAttandances on p.memberId equals q.memberId
                                                  where q.branch == r.branch && q.checkin >= Convert.ToDateTime(startDate) && q.checkin <= Convert.ToDateTime(endDate).AddDays(1)
                                                  select new AttendanceHistoryResponse
                                                  {
                                                      MemberId = p.memberId,
                                                      FullName = p.fullname,
                                                      CheckIn = q.checkin,
                                                      CheckOut = q.checkout,
                                                      Branch = q.branch,
                                                      CheckInBranch = q.checkinBranch,
                                                      Remark = q.remark,
                                                      LateFlag = q.lateFlag
                                                  };
                            result = staffAttendance.ToList();
                        }
                        else if (r.membershipOption == "Members")
                        {
                            var attendance = from m in db.MemberInformations
                                             join a in db.MemberAttandances on m.memberId equals a.memberId
                                             where a.branch == r.branch && a.checkin >= Convert.ToDateTime(startDate) && a.checkin <= Convert.ToDateTime(endDate).AddDays(1)
                                             select new AttendanceHistoryResponse
                                             {
                                                 MemberId = m.memberId,
                                                 FullName = m.fullname,
                                                 MemberOption = m.memberOption,
                                                 CheckIn = a.checkin,
                                                 CheckOut = a.checkout,
                                                 Branch = a.branch,
                                                 CheckInBranch = a.checkinBranch
                                             };
                            result = attendance.ToList();
                        }
                        else if (r.membershipOption == "Regular" || r.membershipOption == "OffHour" || r.membershipOption == "Universal")
                        {
                            var attendance = from m in db.MemberInformations
                                             join a in db.MemberAttandances on m.memberId equals a.memberId
                                             where a.checkin >= Convert.ToDateTime(startDate) && a.checkin <= Convert.ToDateTime(endDate).AddDays(1) && m.memberId == r.memberId
                                             select new AttendanceHistoryResponse
                                             {
                                                 MemberId = m.memberId,
                                                 FullName = m.fullname,
                                                 MemberOption = m.memberOption,
                                                 CheckIn = a.checkin,
                                                 CheckOut = a.checkout,
                                                 Branch = a.branch,
                                                 CheckInBranch = a.checkinBranch
                                             };
                            result = attendance.ToList();
                        }
                        else if (r.membershipOption == "Trainer" || r.membershipOption == "Gym Admin" || r.membershipOption == "Operation Manager")
                        {
                            var staffAttendance = from p in db.MemberInformations
                                                  join q in db.StaffAttandances on p.memberId equals q.memberId
                                                  where q.checkin >= Convert.ToDateTime(startDate) && q.checkin <= Convert.ToDateTime(endDate).AddDays(1) && p.memberId == r.memberId
                                                  select new AttendanceHistoryResponse
                                                  {
                                                      MemberId = p.memberId,
                                                      FullName = p.fullname,
                                                      CheckIn = q.checkin,
                                                      CheckOut = q.checkout,
                                                      Branch = q.branch,
                                                      CheckInBranch = q.checkinBranch,
                                                      Remark = q.remark,
                                                      LateFlag = q.lateFlag
                                                  };
                            result = staffAttendance.ToList();
                        }
                        else if (r.membershipOption == "Guest")
                        {
                            var attendance = from p in db.GuestAttandances
                                             where p.checkin >= Convert.ToDateTime(startDate) && p.checkin <= Convert.ToDateTime(endDate).AddDays(1) && p.name==r.memberId
                                             select new AttendanceHistoryResponse
                                             {
                                                 MemberId = "-",
                                                 FullName = p.name,
                                                 MemberOption = "Guest",
                                                 CheckIn = p.checkin,
                                                 CheckOut = p.checkout,
                                                 Branch = "-",
                                                 CheckInBranch = p.checkinBranch
                                             };
                            result = attendance.ToList();
                        }
                        break;
                }

                return Ok(result);
            }
        }

        [Route("api/GetLateCountDetail")]
        [HttpPost]
        public IHttpActionResult GetLateCountDetail(ReportParam r)
        {
            try
            {
                using (var conn = dbCon.CreateConnection())
                {
                    var result = conn.Query("getStaffLateCheckin",
                        param: new { 
                            from = NepaliDateService.NepToEng(r.startDate), 
                            to= NepaliDateService.NepToEng(r.endDate)
                        }
                        , commandType: System.Data.CommandType.StoredProcedure).ToList();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        } 
        public void insertIntoStaffDeduction(string memberId)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                _logger.Info("##" + "##" + "Entered into Salary Deduction:");
                var sFlagCheck = (from c in db.StaffAttandances
                                  where c.memberId == memberId && c.lateFlag == true
                                  select c.lateFlag);
                var sFlagCount = sFlagCheck.Count();
                _logger.Info("##" + "Flag Check Count: " + sFlagCount);
                if (sFlagCount % 3 == 0)
                {
                    var sd = (from p in db.StaffSalaryDeductions
                              where p.memberId == memberId
                              select p).SingleOrDefault();
                    _logger.Info("##" + "Previous SD count:" + sd.count);
                    sd.count++;
                    _logger.Info("##" + "After SD Count:" + sd.count);
                    db.SubmitChanges();
                }
            }
        }
    }
}
