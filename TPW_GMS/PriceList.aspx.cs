    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;
using TPW_GMS.Models;

namespace TPW_GMS.Views
{
    public partial class PriceList : System.Web.UI.Page
    {
        //private TPWDataContext db = new TPWDataContext();
        string roleId, loginUser, splitUser, type;
        int countRegular = 1, countOffHour = 1, countUniversal = 1, countPersonal = 1, countTrainerCommission = 1, countPerDay=1,countTenDays=1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitialCheck();
            if (!IsPostBack)
            {
                if (roleId == "1")
                {
                    pnlWorkoutChart.Enabled = true;
                    btnSubmit.Visible = true;
                }
                else
                {
                    pnlWorkoutChart.Enabled = false;
                    btnSubmit.Visible = false;
                }
                LoadData();
            }
        }
        public void InitialCheck()
        {
            LoginUserInfo l = Services.Service.checkSession();
            if (l == null)
                Response.Redirect("SignIn.aspx");
            else
            {
                roleId = l.roleId;
                loginUser = l.loginUser;
                splitUser = l.splitUser;
            }
        }
        protected void LoadData()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                try
                {
                    var regular = db.FeeTypes.Where(a => a.membershipOption.Equals("Regular"));
                    var offHour = db.FeeTypes.Where(a => a.membershipOption.Equals("OffHour"));
                    var universal = db.FeeTypes.Where(a => a.membershipOption.Equals("Universal"));
                    var personal = db.FeeTypes.Where(a => a.membershipOption.Equals("Personal Training"));
                    var locker = db.FeeTypes.Where(a => a.membershipOption.Equals("Locker"));
                    var perDay = db.FeeTypes.Where(a => a.membershipOption.Equals("PerDay"));
                    var tenDays = db.FeeTypes.Where(a => a.membershipOption.Equals("TenDays"));



                    var trainerCommission = (from p in db.TrainerClassCommissions select p.commission).ToList();

                    var items = (from p in db.ConsultationCharges where p.consChargeId == 1 select p).SingleOrDefault();
                    txtConsultationCharge.Text = items.consCharge;
                    txtConsultationFeeToTrainer.Text = items.consFeeToTrainer.ToString();


                    foreach (var tc in trainerCommission)
                    {
                        ((TextBox)pnlWorkoutChart.FindControl("txtTrainerClass" + countTrainerCommission)).Text = tc.ToString();
                        countTrainerCommission++;
                    }


                    foreach (var regu in regular)
                    {

                        ((TextBox)pnlWorkoutChart.FindControl("txtRegularAny" + countRegular + "_1month")).Text = regu.oneMonth;
                        ((TextBox)pnlWorkoutChart.FindControl("txtRegularAny" + countRegular + "_3month")).Text = regu.threeMonth;
                        ((TextBox)pnlWorkoutChart.FindControl("txtRegularAny" + countRegular + "_6month")).Text = regu.sixMonth;
                        ((TextBox)pnlWorkoutChart.FindControl("txtRegularAny" + countRegular + "_12month")).Text = regu.twelveMonth;
                        countRegular++;
                    }
                    foreach (var off in offHour)
                    {

                        ((TextBox)pnlWorkoutChart.FindControl("txtOffHourAny" + countOffHour + "_1month")).Text = off.oneMonth;
                        ((TextBox)pnlWorkoutChart.FindControl("txtOffHourAny" + countOffHour + "_3month")).Text = off.threeMonth;
                        ((TextBox)pnlWorkoutChart.FindControl("txtOffHourAny" + countOffHour + "_6month")).Text = off.sixMonth;
                        ((TextBox)pnlWorkoutChart.FindControl("txtOffHourAny" + countOffHour + "_12month")).Text = off.twelveMonth;
                        countOffHour++;
                    }
                    foreach (var uni in universal)
                    {

                        ((TextBox)pnlWorkoutChart.FindControl("txtUniversalAny" + countUniversal + "_1month")).Text = uni.oneMonth;
                        ((TextBox)pnlWorkoutChart.FindControl("txtUniversalAny" + countUniversal + "_3month")).Text = uni.threeMonth;
                        ((TextBox)pnlWorkoutChart.FindControl("txtUniversalAny" + countUniversal + "_6month")).Text = uni.sixMonth;
                        ((TextBox)pnlWorkoutChart.FindControl("txtUniversalAny" + countUniversal + "_12month")).Text = uni.twelveMonth;
                        countUniversal++;
                    }
                    foreach (var per in personal)
                    {

                        ((TextBox)pnlWorkoutChart.FindControl("txtPersonal" + countPersonal + "_1month")).Text = per.oneMonth;
                        ((TextBox)pnlWorkoutChart.FindControl("txtPersonal" + countPersonal + "_3month")).Text = per.threeMonth;
                        ((TextBox)pnlWorkoutChart.FindControl("txtPersonal" + countPersonal + "_6month")).Text = per.sixMonth;
                        ((TextBox)pnlWorkoutChart.FindControl("txtPersonal" + countPersonal + "_12month")).Text = per.twelveMonth;
                        countPersonal++;
                    }
                    foreach (var perd in perDay)
                    {
                        ((TextBox)pnlWorkoutChart.FindControl("txtPerDayAny" + countPerDay)).Text = perd.oneTenDays;
                        countPerDay++;
                    }
                    foreach (var ten in tenDays)
                    {
                        ((TextBox)pnlWorkoutChart.FindControl("txtTenDaysAny" + countTenDays)).Text = ten.oneTenDays;
                        countTenDays++;
                    }
                    foreach (var loc in locker)
                    {
                        txtLocker1Month.Text = loc.oneMonth;
                        txtLocker3Month.Text = loc.threeMonth;
                        txtLocker6Month.Text = loc.sixMonth;
                        txtLocker12Month.Text = loc.twelveMonth;
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        protected void SaveData(string option, string type, string oneTen, string one, string three, string six, string twelve)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                FeeType f = new FeeType();
                f.membershipOption = option;
                f.membershipType = type;
                f.oneTenDays = oneTen;
                f.oneMonth = one;
                f.threeMonth = three;
                f.sixMonth = six;
                f.twelveMonth = twelve;
                db.FeeTypes.InsertOnSubmit(f);
                db.SubmitChanges();
            }
        }
        protected void GetValue(string option)
        {
            for (int i = 1; i <= 3; i++)
            {
                string one = ((TextBox)pnlWorkoutChart.FindControl("txt" + option + "Any" + i + "_1month")).Text;
                string three = ((TextBox)pnlWorkoutChart.FindControl("txt" + option + "Any" + i + "_3month")).Text;
                string six = ((TextBox)pnlWorkoutChart.FindControl("txt" + option + "Any" + i + "_6month")).Text;
                string twelve = ((TextBox)pnlWorkoutChart.FindControl("txt" + option + "Any" + i + "_12month")).Text;

                SaveData(option, "Any" + i,"", one, three, six, twelve);
            }
        }
        protected void GetValue1(string option)
        {
            for (int i = 1; i <= 3; i++)
            {
                string one_ten= ((TextBox)pnlWorkoutChart.FindControl("txt" + option + "Any" + i)).Text;
                SaveData(option, "Any" + i, one_ten,"","","","");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                delete();
                for (int i = 1; i <= 3; i++)
                {
                    if (i == 1)
                        GetValue("Regular");
                    else if (i == 2)
                        GetValue("OffHour");
                    else if (i == 3)
                        GetValue("Universal");
                }
                for (int j = 1; j <= 2; j++)
                {
                    string one = ((TextBox)pnlWorkoutChart.FindControl("txtPersonal" + j + "_1month")).Text;
                    string three = ((TextBox)pnlWorkoutChart.FindControl("txtPersonal" + j + "_3month")).Text;
                    string six = ((TextBox)pnlWorkoutChart.FindControl("txtPersonal" + j + "_6month")).Text;
                    string twelve = ((TextBox)pnlWorkoutChart.FindControl("txtPersonal" + j + "_12month")).Text;

                    if (j == 1)
                    {
                        GetValue1("PerDay");
                        type = "1 Person";
                    }
                    else if (j == 2)
                    {
                        GetValue1("TenDays");
                        type = "2 or more";
                    }

                    SaveData("Personal Training", type, "", one, three, six, twelve);
                }
                SaveData("Locker", "Locker", "", txtLocker1Month.Text, txtLocker3Month.Text, txtLocker6Month.Text, txtLocker12Month.Text);
                updateTrainerComissionPercentage();
                updateConsultationCharge();
            }
        }
        protected void delete()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                db.FeeTypes.DeleteAllOnSubmit(db.FeeTypes);
                db.SubmitChanges();
            }
        }
        protected void updateTrainerComissionPercentage()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                for (int i = 1; i <= 3; i++)
                {
                    var item_P = db.TrainerClassCommissions.ToList();

                    if (item_P.Count == 0)
                    {
                        string[] trainerClass = new string[] { "Class A", "Class B", "Class C" };
                        TrainerClassCommission t = new TrainerClassCommission();
                        t.trainerClassId = i;
                        t.catagory = trainerClass[i - 1];
                        t.commission = Convert.ToInt32(((TextBox)pnlWorkoutChart.FindControl("txtTrainerClass" + i)).Text);
                        db.TrainerClassCommissions.InsertOnSubmit(t);
                    }
                    else
                    {
                        var item = (from p in db.TrainerClassCommissions
                                    where p.trainerClassId == i
                                    select p).SingleOrDefault();
                        TextBox tx = (TextBox)pnlWorkoutChart.FindControl("txtTrainerClass" + i);
                        item.commission = Convert.ToInt32(tx.Text);
                    }
                }
                db.SubmitChanges();
            }
        }
        protected void updateConsultationCharge()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var item_c = db.ConsultationCharges.ToList();
                if (item_c.Count == 0)
                {
                    ConsultationCharge c = new ConsultationCharge();
                    c.consChargeId = 1;
                    c.consCharge = txtConsultationCharge.Text;
                    c.consFeeToTrainer = Convert.ToInt32(txtConsultationFeeToTrainer.Text);
                    db.ConsultationCharges.InsertOnSubmit(c);
                }
                else
                {
                    var item = (from p in db.ConsultationCharges
                                where p.consChargeId == 1
                                select p).SingleOrDefault();
                    item.consCharge = txtConsultationCharge.Text;
                    item.consFeeToTrainer = Convert.ToInt32(txtConsultationFeeToTrainer.Text);
                }
                db.SubmitChanges();
            }
        }
    }
}