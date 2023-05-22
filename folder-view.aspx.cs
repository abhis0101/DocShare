using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

namespace DocShare
{
    public partial class folder_view : System.Web.UI.Page
    {
        Dbconnection db = new Dbconnection();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usercode"] != null)
            {

            }
            else
            {
                Response.Redirect("login.aspx");
            }
            if (!IsPostBack)
            {
                bindView();
            }
        }

        public void bindView()
        {
            String query = "select * from file_details as f join folder_group as fd on f.folderid = fd.id where f.folderid= '" + Request.QueryString["folderid"].ToString() + "' and  fd.usercode = '" + Session["usercode"].ToString() + "'";
            DataSet ds = new DataSet();
            ds = db.ExecuteDataSet(query);

            DataList1.DataSource = ds.Tables[0];
            DataList1.DataBind();
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void Remove(string id)
        {
            folder_view fv = new folder_view();
            Dbconnection dbconnection = new Dbconnection();

            string sql = "delete from file_details where id = " + id + "";

            int result = dbconnection.executeQuery(sql);

            if (result > 0)
            {
                fv.bindView();
            }
        }
    }
}