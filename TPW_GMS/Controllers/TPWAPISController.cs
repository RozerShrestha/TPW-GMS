using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TPW_API.Models;
using TPW_GMS.Data;
using TPW_GMS.Models;
using TPW_GMS.Services;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog;
using System.Globalization;
using System.Text.RegularExpressions;
using TPW_GMS.Repository;
using Dapper;
using System.Diagnostics;

namespace TPW_GMS.Controllers
{
    [Authorize]
    public class TPWAPISController : ApiController
    {
        private TPWDataContext db = new TPWDataContext();
        public static string conString;
        public DbConFactory dbCon = new DbConFactory();
        private Logger _logger;

        public TPWAPISController()
        {
            _logger = LogManager.GetLogger("f");
        }

        [Route("api/UploadImageTest")]
        [HttpPost]
        //public async Task<HttpResponseMessage> PostFormData()
        public async Task<HttpResponseMessage> PostFormData()
        {
            //Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string root = HttpContext.Current.Server.MapPath("~/Image/MarketingImages/");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    var name = file.Headers.ContentDisposition.FileName;
                    name = name.Trim('"');
                    var localFileName = file.LocalFileName;
                    var filePath = Path.Combine(root, name);
                    File.Move(localFileName, filePath);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [Route("api/getMemberLoginInfo")]
        [HttpPost]
        public IHttpActionResult getMemberLoginInfo(LoginUserInfo l)
        {
            var userInfo = Service.getMemberInfo(l.memberId);
            return Ok(userInfo);
        }

        [Route("api/CheckOut/")]
        [HttpGet]
        public string CheckOut(string encryptedMemberId, string type)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var decryptedMemberId = Service.DecryptString(encryptedMemberId);
            var splitData = Regex.Split(decryptedMemberId, @"//");

            var memberId = splitData[0];
            try
            {
                if (type == "member")
                {
                    var memId = memberId.Trim();
                    var q = db.MemberAttandances.Where(p => p.memberId == memId);
                    var lastObj = q.OrderByDescending(item => item.attendanceId).First();
                    lastObj.checkout = DateTime.Now;
                    db.SubmitChanges();
                    return "Checked Out";
                }
                else
                {
                    var memId = memberId.Trim();
                    var q = db.StaffAttandances.Where(p => p.memberId == memId);
                    var lastObj = q.OrderByDescending(item => item.attendanceId).First();
                    lastObj.checkout = DateTime.Now;
                    db.SubmitChanges();
                    return "Checked Out";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        [Route("api/GetAllTrainerBooking")]
        public IEnumerable<TrainerBookingLog> GetAllTrainerBooking()
        {
            return db.TrainerBookingLogs;
        }

        [Route("api/GetAllTrainer")]
        public IEnumerable<Trainer> GetAllTrainer()
        {
            return db.Trainers;
        }

        [Route("api/GetAllStaff")]
        public IEnumerable<Staff> GetAllStaff()
        {
            return db.Staffs;
        }

        [Route("api/GetTotalMembershipCount")]
        [HttpGet]
        public IHttpActionResult GetTotalMembershipCount(DateTime startdate, DateTime enddate)
        {
            List<GeneralCountBranch> totalMembershipList = new List<GeneralCountBranch>();
            //connection string from web config
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["TPW_GMSConnectionString"];
            conString = mySetting.ConnectionString;
            using (SqlConnection conn = new SqlConnection(conString))
            using (var command = new SqlCommand("TotalMembershipCount", conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@startdate", SqlDbType.VarChar).Value = startdate;
                command.Parameters.Add("@enddate", SqlDbType.VarChar).Value = enddate;
                

                conn.Open();
                SqlDataReader rdr = null;
                rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    GeneralCountBranch pt = new GeneralCountBranch();
                    pt.branchName = rdr[0].ToString();
                    pt.Count = Convert.ToInt32(rdr[1]);
                    totalMembershipList.Add(pt);

                }
                rdr.Close();
                conn.Close();
                // command.ExecuteNonQuery();
            }
            return Ok(totalMembershipList);
        }

        [Route("api/GetTotalEarningGymGraph")]
        [HttpGet]
        public IHttpActionResult GetTotalEarningGymGraph(DateTime startdate, DateTime enddate, string branch)
        {
            try
            {
                List<DataPoint> totalEarningByBranchList = new List<DataPoint>();
                ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["TPW_GMSConnectionString"];
                //conString = "data source=TPW-VM;initial catalog=rozer-pc;persist security info=True;user id=sa;password=whitehat";
                conString = mySetting.ConnectionString;
                SqlConnection conn = new SqlConnection(conString);
                using (var command = new SqlCommand("EarningFromGYM", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@startdate", SqlDbType.VarChar).Value = startdate;
                    command.Parameters.Add("@enddate", SqlDbType.VarChar).Value = enddate;
                    command.Parameters.Add("@branch", SqlDbType.VarChar).Value = branch;
                    conn.Open();
                    SqlDataReader rdr = null;
                    rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        totalEarningByBranchList.Add(new DataPoint(rdr[0].ToString(), Convert.ToDouble(rdr[1])));
                    }
                    rdr.Close();
                    conn.Close();
                    // command.ExecuteNonQuery();
                }
                return Ok(totalEarningByBranchList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Route("api/GetAverageActiveMembers")]
        [HttpGet]
        public IHttpActionResult GetAverageActiveMembers(string branch)
        {
            try
            {
                List<DataPoint1> totalEarningByBranchList = new List<DataPoint1>();
                ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["TPW_GMSConnectionString"];
                //conString = "data source=TPW-VM;initial catalog=rozer-pc;persist security info=True;user id=sa;password=whitehat";
                conString = mySetting.ConnectionString;
                SqlConnection conn = new SqlConnection(conString);
                using (var command = new SqlCommand("AverageActiveMembers", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@branch", SqlDbType.VarChar).Value = branch;
                    conn.Open();
                    SqlDataReader rdr = null;
                    rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        totalEarningByBranchList.Add(new DataPoint1(Convert.ToDateTime(rdr[0]).ToShortDateString(), Convert.ToDouble(rdr[1])));
                    }
                    rdr.Close();
                    conn.Close();
                    // command.ExecuteNonQuery();
                }
                return Ok(totalEarningByBranchList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Route("api/GetAdmittedRenewData")]
        [HttpGet]
        public IHttpActionResult GetAdmittedRenewData(string startDate, string endDate, string type, string branch)
        {
            try
            {
                List<DataPoint> pieAdmittedRenew = new List<DataPoint>();
                using (var conn = dbCon.CreateConnection())
                {
                    var result = conn.Query("graphAdmittedAndRenew",
                        param: new { startDate, endDate, type, branch }
                        , commandType: System.Data.CommandType.StoredProcedure).ToList();
                    //pieAdmittedRenew.Add(new DataPoint(result))
                    //foreach(var r in result)
                    //{
                    //    pieAdmittedRenew.Add(new DataPoint(r.Description., ))
                    //}
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [Route("api/GetTotalBranchAverageActiveCountMonthly")]
        [HttpGet]
        public IHttpActionResult GetTotalBranchAverageActiveCountMonthly()
        {
            try
            {
                using (var conn = dbCon.CreateConnection())
                {
                    var result = conn.Query("TotalAverageActiveMonthly"
                        , commandType: System.Data.CommandType.StoredProcedure).ToList();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [Route("api/GetAverageBranchActiveCountPerday")]
        [HttpGet]
        public IHttpActionResult GetAverageBranchActiveCountPerday(DateTime dateToday)
        {
            try
            {
                using (var conn = dbCon.CreateConnection())
                {
                    var result = conn.Query("sp_averageActiveMemberPerDay",
                        param: new { dateToday}
                        , commandType: System.Data.CommandType.StoredProcedure).ToList();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [Route("api/GetMembershipOption")]
        [HttpGet]
        public IHttpActionResult GetMembershipOption(string startDate, string endDate, string branch)
        {
            try
            {
                List<DataPoint> pieMembershipOPtion = new List<DataPoint>();
                using (var conn = dbCon.CreateConnection())
                {
                    var result = conn.Query("sp_getMembershipOption",
                        param: new { startDate, endDate, branch }
                        , commandType: System.Data.CommandType.StoredProcedure).ToList();
                    foreach (var r in result)
                    {
                        pieMembershipOPtion.Add(new DataPoint(r.Description, Convert.ToDouble(r.Count)));
                    }
                    return Ok(pieMembershipOPtion);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [Route("api/GetAllSellSupplements")]
        public IEnumerable<SuplementSelling> GetAllSellSupplements()
        {
            var items = from p in db.SuplementSellings
                        orderby p.isPaidSuplementSell
                        select p;
            return items;
        }

        [Route("api/GetNewMembershipGraph")]
        public IHttpActionResult GetNewMembershipGraph(DateTime startdate, DateTime enddate)
        {
            try
            {
                List<DataPoint> newMemberList = new List<DataPoint>();
                ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["TPW_GMSConnectionString"];
                //conString = "data source=TPW-VM;initial catalog=rozer-pc;persist security info=True;user id=sa;password=whitehat";
                conString = mySetting.ConnectionString;
                SqlConnection conn = new SqlConnection(conString);
                using (var command = new SqlCommand("MembershipByMonth", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@startdate", SqlDbType.VarChar).Value = startdate;
                    command.Parameters.Add("@enddate", SqlDbType.VarChar).Value = enddate;

                    conn.Open();
                    SqlDataReader rdr = null;
                    rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        newMemberList.Add(new DataPoint(rdr[0].ToString(), Convert.ToDouble(rdr[1])));

                    }
                    rdr.Close();
                    conn.Close();
                    // command.ExecuteNonQuery();
                }
                return Ok(newMemberList);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [Route("api/GetMerchan")]
        [HttpGet]
        public IEnumerable<Merchan> GetAllMerchan(string branch)
        {
            if (branch == "admin")
                return db.Merchans;
            else
            {
                return db.Merchans.Where(a => a.branch == branch);
            }
        }

        [Route("api/GetSellItems")]
        public IEnumerable<SellItem> GetAllSetItems()
        {
            return db.SellItems;
        }

        [Route("api/GetInactiveMembershipCount")]
        public IHttpActionResult GetInactiveMembershipCount(DateTime startdate, DateTime enddate)
        {
            List<GeneralCountBranch> totalMembershipList = new List<GeneralCountBranch>();
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["TPW_GMSConnectionString"];
            conString = mySetting.ConnectionString;
            using (SqlConnection conn = new SqlConnection(conString))
            using (var command = new SqlCommand("InactiveMembershipCount", conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@startdate", SqlDbType.VarChar).Value = startdate;
                command.Parameters.Add("@enddate", SqlDbType.VarChar).Value = enddate;

                conn.Open();
                SqlDataReader rdr = null;
                rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    GeneralCountBranch pt = new GeneralCountBranch();
                    pt.branchName = rdr[0].ToString();
                    pt.Count = Convert.ToInt32(rdr[1]);
                    totalMembershipList.Add(pt);

                }
                rdr.Close();
                conn.Close();
                // command.ExecuteNonQuery();
            }
            return Ok(totalMembershipList);
        }

        [Route("api/GetGymTraffic")]
        public IHttpActionResult GetInfo()
        {
            List<GeneralCountBranch> totalMembershipList = new List<GeneralCountBranch>();
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["TPW_GMSConnectionString"];
            conString = mySetting.ConnectionString;
            using (SqlConnection conn = new SqlConnection(conString))
            using (var command = new SqlCommand("BranchRealTimeTraffic", conn))
            {
                try
                {
                    command.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    SqlDataReader rdr = null;
                    rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        GeneralCountBranch pt = new GeneralCountBranch();
                        pt.branchName = rdr[0].ToString();
                        pt.Count = Convert.ToInt32(rdr[1]);
                        totalMembershipList.Add(pt);

                    }
                    rdr.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    _logger.Error("##" + "Gym Traffic Controller:{0}", ex.Message);
                }
                // command.ExecuteNonQuery();
            }
            return Ok(totalMembershipList);
        }

        [Route("api/GetAllMember")]
        public IEnumerable<MemberInformation1> getAllMember(string branch)
        {
            if (branch == "admin" || branch=="superadmin")
            {
                var query = (from m in db.MemberInformations
                             join p in db.PaymentInfos
                             on m.memberId equals p.memberId
                             select new MemberInformation1
                             {
                                 memberId = m.memberId,
                                 fullname = m.fullname,
                                 memberOption = m.memberOption,
                                 branch = m.branch,
                                 shift = m.shift,
                                 memberCatagory = m.memberCatagory,
                                 memberPaymentType = m.memberPaymentType,
                                 memberDate = m.memberDate,
                                 memberBeginDate = m.memberBeginDate,
                                 memberExpireDate = m.memberExpireDate,
                                 contactNo = m.contactNo,
                                 receiptNo = p.receiptNo,
                                 dueAmount = (p.dueAmount == null) ? 0 : Convert.ToInt32(p.dueAmount),
                                 finalAmount = Convert.ToInt32(p.finalAmount),
                                 ActiveInActive = m.ActiveInactive,
                                 dateOfBirth = m.dateOfBirth,
                                 address = m.address

                             }).ToList();
                return query;
            }
            else
            {
                var query = (from m in db.MemberInformations
                             where m.branch == branch || m.memberOption == "Free User"
                             join p in db.PaymentInfos
                             on m.memberId equals p.memberId
                             select new MemberInformation1
                             {
                                 memberId = m.memberId,
                                 fullname = m.fullname,
                                 memberOption = m.memberOption,
                                 branch = m.branch,
                                 shift = m.shift,
                                 memberCatagory = m.memberCatagory,
                                 memberPaymentType = m.memberPaymentType,
                                 memberBeginDate = m.memberBeginDate,
                                 memberExpireDate = m.memberExpireDate,
                                 contactNo = m.contactNo,
                                 receiptNo = p.receiptNo,
                                 dueAmount = (p.dueAmount == null) ? 0 : Convert.ToInt32(p.dueAmount),
                                 finalAmount = Convert.ToInt32(p.finalAmount),
                                 ActiveInActive = m.ActiveInactive
                             }).ToList();
                return query;
            }
        }

        [Route("api/GetAllExpenditures")]
        public IEnumerable<Expenditure> GetAllExpenditures()
        {
            return db.Expenditures;
        }

        [Route("api/GetBuySupplements")]
        public IEnumerable<Suplement> GetAllBuySupplements()
        {
            return db.Suplements;
        }

        [Route("api/GetActiveMembership")]
        [HttpGet]
        public IHttpActionResult GetActiveMembership()
        {
            try
            {
                using (var conn = dbCon.CreateConnection())
                {
                    var result = conn.Query("ActiveMembershipCount",
                        param: new { dateToday=DateTime.Now }
                        , commandType: System.Data.CommandType.StoredProcedure).ToList();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [Route("api/GetActiveMembershipList")]
        [HttpGet]
        public IHttpActionResult GetActiveMembershipList(string branch)
        {
            try
            {
                using (var conn = dbCon.CreateConnection())
                {
                    var result = conn.Query("ActiveMembershipList",
                        param: new { branch }
                        , commandType: System.Data.CommandType.StoredProcedure).ToList();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }
        [Route("api/GetActiveHistoryMonth")]
        [HttpGet]
        public IHttpActionResult GetActiveHistoryMonth(string branch)
        {
            try
            {
                using (var conn = dbCon.CreateConnection())
                {
                    var result = conn.Query("ActiveHistoryMonth",
                        param: new { branch }
                        , commandType: System.Data.CommandType.StoredProcedure).ToList();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {

                return Ok(ex);
            }
        }

        [Route("api/GetCommissionLog")]
        public IEnumerable<ComissionPaymentLog> GetCommissionLog()
        {

            var items = db.ComissionPaymentLogs;
            return db.ComissionPaymentLogs;
        }

        [Route("api/GetJoinTPW")]
        public IEnumerable<JoinTPW> GetJoinTPW()
        {
            return db.JoinTPWs;
        }

        [Route("api/GetConsultation")]
        public IEnumerable<ConsultationLog> GetConsultation()
        {
            return db.ConsultationLogs;
        }

        [Route("api/AskQn")]
        public IEnumerable<AskQn> GetAskQuestion()
        {
            return db.AskQns;
        }

        [Route("api/ProfileUpload")]
        [HttpPost]
        public IHttpActionResult ProfileUpload(ProfileImageUpload p)
        {
            try
            {
                MemberInformation m = new MemberInformation();
                Byte[] bytes = Convert.FromBase64String(p.base64String);
                MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                var filepath = HttpContext.Current.Server.MapPath("~/Image/Members/") + p.memberId + ".jpg";
                image.Save(filepath);
                var item = (from c in db.MemberInformations
                            where c.memberId == p.memberId
                            select c).SingleOrDefault();

                item.imageLoc = "Image/Members/" + p.memberId + ".jpg";
                item.imageByte = bytes;
                db.SubmitChanges();
                return Json(new { status = "Success", message = "Image/Members/" + p.memberId + ".jpg" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/AskQn")]
        [HttpPatch]
        public IHttpActionResult AskQuestion(general g)
        {
            var askQnItem = (from p in db.AskQns
                             where p.askId == g.id
                             select p).SingleOrDefault();
            askQnItem.status = !askQnItem.status;
            db.SubmitChanges();
            return Ok("Successfully Submitted");
        }

        [Route("api/ResetPassword")]
        [HttpPatch]
        public IHttpActionResult ResetPassword(MemberLoginCredential m)
        {

            return Ok(Service.ResetPassword(m));
        }

        [Route("api/GetLockerData")]
        [HttpPost]
        public IHttpActionResult GetLockerData(LoginUserInfo lu)
        {
            //default empty is used for left join
            if (lu.loginUser == "admin")
            {
                var item = db.ExtraInformations.SingleOrDefault();
                var items = (from l in db.LockerMgs
                             join m in db.MemberInformations on l.memberId equals m.memberId
                             into joined
                             from j in joined.DefaultIfEmpty()
                             select new Locker2
                             {
                                 memberId = l.memberId,
                                 fullName = j.fullname,
                                 branch = l.branch,
                                 lockerNumber = l.lockerNumber,
                                 renewDate = l.renewDate == null ? "" : l.renewDate.ToString(),
                                 duration = l.duration,
                                 expireDate = l.expireDate == null ? "" : l.expireDate.ToString(),
                                 isExpired = l.expireDate < DateTime.Now ? true : false,
                                 isAssigned = l.memberId == null ? false : true,
                                 flag = l.flag,
                                 charge = l.amount,
                                 receiptNoStatic = item.currentNepaliDate + l.branch,
                                 receiptNo = l.receiptNo
                                 //MemberInformation=j,
                                 //LockerMg=l

                             }).ToList();


                return Ok(items);
            }
            else
            {
                var item = db.ExtraInformations.SingleOrDefault();
                var items = (from l in db.LockerMgs
                             join m in db.MemberInformations on l.memberId equals m.memberId
                             into joined
                             from j in joined.DefaultIfEmpty()
                             where l.branch.Equals(lu.loginUser)
                             select new Locker2
                             {
                                 memberId = l.memberId,
                                 fullName = j.fullname,
                                 branch = l.branch,
                                 lockerNumber = l.lockerNumber,
                                 renewDate = l.renewDate == null ? "" : l.renewDate.ToString(),
                                 duration = l.duration,
                                 expireDate = l.expireDate == null ? "" : l.expireDate.ToString(),
                                 isExpired = l.expireDate < DateTime.Now ? true : false,
                                 isAssigned = l.memberId == null ? false : true,
                                 flag = l.flag,
                                 charge = l.amount,
                                 receiptNoStatic = item.currentNepaliDate + l.branch,
                                 receiptNo = l.receiptNo
                                 //MemberInformation=j,
                                 //LockerMg=l

                             }).ToList();
                return Ok(items);
            }

        }

        [Route("api/InsertIntoLocker")]
        [HttpPost]
        public IHttpActionResult InsertIntoLocker(LockerMg lu)
        {
            try
            {
                LockerMg l = new LockerMg();
                l.branch = lu.branch;
                l.lockerNumber = lu.lockerNumber;
                l.flag = lu.flag;
                l.created = DateTime.Now;
                l.modified = DateTime.Now;
                db.LockerMgs.InsertOnSubmit(l);
                db.SubmitChanges();
                return Ok("Success");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [Route("api/AssignMemberIntoLocker")]
        [HttpPost]
        public IHttpActionResult AssignMemberIntoLocker(LockerMg lu)
        {
            try
            {
                LockerMg l = (from p in db.LockerMgs
                              where p.lockerNumber == lu.lockerNumber && p.branch == lu.branch
                              select p).SingleOrDefault();
                Report r = new Report();

                l.memberId = lu.memberId;
                l.renewDate = lu.renewDate;
                l.expireDate = lu.expireDate;
                l.flag = lu.flag;
                l.modified = DateTime.Now;
                l.duration = lu.duration + " Month";
                l.amount = lu.amount;
                l.receiptNo = lu.receiptNo;
                l.paymentMethod = lu.paymentMethod;
                r.memberId = lu.memberId;
                r.memberBeginDate = lu.renewDate;
                r.memberExpireDate = lu.expireDate;
                r.memberPaymentType = lu.duration + " Month";
                r.memberCatagory = "Locker";
                r.finalAmount = lu.amount;
                r.paymentMethod = lu.paymentMethod;
                r.receiptNo = lu.receiptNo;
                r.created = DateTime.Now;
                db.Reports.InsertOnSubmit(r);

                db.SubmitChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("api/ResetLocker")]
        [HttpPost]
        public IHttpActionResult ResetLocker(LockerMg lu)
        {
            try
            {
                var lockerItem = (from p in db.LockerMgs
                                  where p.lockerNumber == lu.lockerNumber && p.branch == lu.branch
                                  select p).SingleOrDefault();

                lockerItem.memberId = null;
                lockerItem.renewDate = null;
                lockerItem.expireDate = null;
                lockerItem.flag = true;
                lockerItem.modified = DateTime.Now;
                lockerItem.duration = null;
                lockerItem.amount = null;
                lockerItem.receiptNo = null;

                db.SubmitChanges();
                return Ok("Success");

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("api/GetMemberList")]
        [HttpGet]
        public IHttpActionResult GetMemberList(string branch, string memberId)
        {
            try
            {
                if (memberId == "null")
                {
                    var members = (from m in db.MemberInformations
                                   join l in db.LockerMgs on m.memberId equals l.memberId
                                   into joined
                                   from j in joined.DefaultIfEmpty()
                                   where m.branch.Equals(branch) && j.memberId == null
                                   orderby m.fullname
                                   select m.fullname).ToList();
                    return Ok(members);
                }
                else
                {
                    var members = (from p in db.MemberInformations
                                   where p.branch == branch && p.memberId == memberId
                                   select p.fullname).ToList();
                    return Ok(members);
                }





            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [Route("api/GetMemberId")]
        [HttpGet]
        public IHttpActionResult GetMemberId(string fullname, string branch)
        {
            try
            {
                var memberId = (from p in db.MemberInformations
                                where p.fullname == fullname && p.branch == branch
                                select p.memberId).ToList();
                return Ok(memberId);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [Route("api/GetLockerCharge")]
        [HttpGet]
        public IHttpActionResult GetLockerCharge(int duration)
        {
            try
            {
                string lockerCharge;
                var lockercrg = (from c in db.FeeTypes
                                 where c.membershipOption == "Locker"
                                 select c).SingleOrDefault();

                lockerCharge = duration == 1 ? lockercrg.oneMonth : duration == 3 ? lockercrg.threeMonth : duration == 6 ? lockercrg.sixMonth : duration == 12 ? lockercrg.twelveMonth : "0";
                return Ok(lockerCharge);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("api/GetAllBranch")]
        [HttpGet]
        public IHttpActionResult GetAllBranch()
        {
            var branchList = (from p in db.Logins
                              where p.roleId == 3
                              select p.username).ToList();
            return Ok(branchList);
        }

        [Route("api/GetBranchWiseLocker")]
        [HttpGet]
        public IHttpActionResult GetBranchWiseLocker()
        {
            var item = db.ExtraInformations.SingleOrDefault();
            var items = (from b in db.Logins
                         where !b.username.Contains("admin")
                         select b.username).ToList();

            List<LockerBranch> lockerBranch = new List<LockerBranch>();

            foreach (var i in items)
            {
                var branchWiseLocker = new LockerBranch();
                branchWiseLocker.branchName = i;
                branchWiseLocker.lockers = (from l in db.LockerMgs
                                            join m in db.MemberInformations on l.memberId equals m.memberId
                                            into joined
                                            from j in joined.DefaultIfEmpty()
                                            where l.branch == i
                                            orderby l.lockerNumber
                                            select new Locker2
                                            {
                                                memberId = l.memberId,
                                                fullName = j.fullname,
                                                branch = l.branch,
                                                lockerNumber = l.lockerNumber,
                                                renewDate = l.renewDate == null ? "" : l.renewDate.ToString(),
                                                duration = l.duration,
                                                expireDate = l.expireDate == null ? "" : l.expireDate.ToString(),
                                                isExpired = l.expireDate < DateTime.Now ? true : false,
                                                isAssigned = l.memberId == null ? false : true,
                                                flag = l.flag,
                                                charge = l.amount,
                                                paymentMethod = l.paymentMethod,
                                                receiptNoStatic = item.currentNepaliDate + l.branch,
                                                receiptNo = l.receiptNo
                                                //MemberInformation=j,
                                                //LockerMg=l
                                            }).ToList();
                lockerBranch.Add(branchWiseLocker);

            }
            return Ok(lockerBranch);
        }


        [Route("api/GetLog")]
        [HttpPost]
        public IHttpActionResult GetLog(general g)
        {
            string dt = g.dt; /*"2020-05-31"*/;
            string logPath = Path.Combine(HttpRuntime.AppDomainAppPath, "logs\\" + dt + ".log");
            string[] lines = { };
            List<logModel> logList = new List<logModel>();
            try
            {
                if (File.Exists(logPath))
                {
                    //string text = File.ReadAllText(logPath);
                    lines = File.ReadAllLines(logPath);
                    for (int i = lines.Length; i > 0; i--)
                    {
                        logModel ll = new logModel();
                        var l = lines[i - 1];
                        var splitLog = Regex.Split(l, @"##");
                        if (splitLog.Length == 2)
                        {
                            ll.First = splitLog[0];
                            ll.Second = splitLog[1];
                            logList.Add(ll);
                        }
                    }
                    //foreach (var l in lines)
                    //{
                    //    logModel ll = new logModel();
                    //    var splitLog = Regex.Split(l, @"##");
                    //    ll.First = splitLog[0];
                    //    ll.Second = splitLog[1];
                    //    logList.Add(ll);
                    //}
                }
                return Ok(logList);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }


        }

        [Route("api/GetFilesFromLocation")]
        [HttpGet]
        public IHttpActionResult GetFilesFromLocation()
        {
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, "Image\\MarketingImages");
            string[] fileList;
            try
            {
                fileList = Directory.GetFiles(path);
                return Ok(fileList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [Route("api/GetMemberListMultiple")]
        [HttpGet]
        public IHttpActionResult GetMemberListMultiple()
        {
            var items = (from p in db.MemberInformations
                         select p);
            List<multipleDropdownDataModel> gList = new List<multipleDropdownDataModel>();
            foreach (var item in items)
            {
                multipleDropdownDataModel g = new multipleDropdownDataModel();
                g.value = item.email;
                g.text = item.fullname + "##" + item.memberId;
                gList.Add(g);
            }
            return Ok(gList);
        }

        [Route("api/CheckDiscountCodeStatus")]
        [HttpGet]
        public IHttpActionResult CheckDiscountCodeStatus(string code)
        {
            using (var db=new TPWDataContext())
            {
                var check = db.Staffs.Where(p => p.discountCode == code && p.status==true).SingleOrDefault();
                var status = check == null ? "invalid" : "valid";
                return Ok(status);
            }
        }

        [Route("api/UpdateMemberInformation")]
        [HttpPost]
        public IHttpActionResult UpdateMemberInformation(MemberInformation m)
        {
            using (var db = new TPWDataContext())
            {
                try
                {
                    var item = db.MemberInformations.Where(p => p.memberId == m.memberId).FirstOrDefault();
                    item.callStatus = m.callStatus;
                    item.callRemark = m.callRemark;
                    db.SubmitChanges();
                    return Ok("Update Successfully");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                   
                }
               
            }
        }

        [Route("api/GetPaymentPending")]
        [HttpPost]
        public IHttpActionResult GetPaymentPending(ReportParam r)
        {
                try
                {
                    var d1 = DateTime.Now.AddMonths(-1);
                    var items = from c in db.MemberInformations
                               join p in db.PaymentInfos on c.memberId equals p.memberId
                               select new
                               {
                                   c.memberId,
                                   c.fullname,
                                   c.memberOption,
                                   c.email,
                                   c.emailStatus,
                                   c.memberCatagory,
                                   c.memberPaymentType,
                                   c.memberBeginDate,
                                   c.memberExpireDate,
                                   c.contactNo,
                                   c.branch,
                                   c.ActiveInactive,
                                   p.finalAmount,
                                   p.due
                               };
                if (r.branch != "ALL")
                    items = items.Where(k => k.branch == r.branch);

                if (r.reportType == "Active")
                    items = items.Where(k => k.memberExpireDate >= DateTime.Now && k.memberExpireDate <= DateTime.Now.AddDays(r.postPendingCheck));
                else if (r.reportType == "Due")
                    items = items.Where(k => k.due == true && k.memberExpireDate > d1 && k.memberExpireDate < DateTime.Now);
                else if (r.reportType == "ActivePending")
                {
                    items = items.Where(k => k.memberExpireDate >= DateTime.Now.AddDays(-5) && k.memberExpireDate <= DateTime.Now && 
                    (from p in db.MemberAttandances where p.checkin >= DateTime.Now.AddDays(-5) && p.checkin <= DateTime.Now
                     select new
                     {
                         p.memberId
                     }).Contains(new { memberId = (String)k.memberId }));
                }
                else if (r.reportType == "expiredOverOne")
                    items = items.Where(k => k.ActiveInactive == "InActive" && k.memberExpireDate < d1);
                else if (r.reportType == "paused")
                    items = items.Where(k => k.due == false && k.ActiveInactive == "InActive");
                else
                    items = items.Where(k => k.ActiveInactive == r.reportType);
                //
                if (r.flag != "0")
                    items = items.Where(k => k.emailStatus == Convert.ToBoolean(r.flag));
                   
               
                items = items.OrderByDescending(k => k.memberExpireDate);
                return Ok(items);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);

                }
        }

        [Route("api/GetGuest")]
        [HttpGet]
        public IEnumerable<Guest> GetGuest()
        {
            return db.Guests;
        }

        public void insertIntoStaffDeduction(string memberId)
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
