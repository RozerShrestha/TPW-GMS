using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TPW_GMS.Data;

namespace TPW_GMS
{
    public partial class QRMessageBroadCase : System.Web.UI.Page
    {
        private TPWDataContext db = new TPWDataContext();
        public string path = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadGymMember();
            }
        }
        public void loadGymMember()
        {
            ddlGymMember.Items.Clear();
            var items = (from p in db.MemberInformations
                         select p);
            items = items.OrderByDescending(p => p.fullname);
            foreach (var item in items)
            {
                ddlGymMember.Items.Insert(0, new ListItem(item.fullname+"##"+item.memberId, item.email));
            }
            db.Dispose();
        }
    }
}