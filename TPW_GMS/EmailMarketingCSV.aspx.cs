using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;

namespace TPW_GMS
{
    public partial class EmailMarketingCSV : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadBranch();
            }
        }
        protected void loadBranch()
        {
            using (TPWDataContext db = new TPWDataContext())
            {
                var branchName = (from p in db.Logins
                                  where !p.firstname.Contains("admin")
                                  select p.username);
                branch.DataSource = branchName;
                branch.DataBind();
                branch.Items.Insert(0, new ListItem("All", "All"));
            }
        }
    }
}