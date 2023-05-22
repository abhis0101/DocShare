using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;

namespace DocShare
{
    public partial class cameraModule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usercode"] != null)
            {

            }
            else
            {
                Response.Redirect("login.aspx");
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void send_email()
        {
            string to = "dkt2098@gmail.com"; //To address    
            string from = "prajapatipradeep1101@gmail.com"; //From address    
            MailMessage message = new MailMessage(from, to);

            Random random = new Random();
            int OTP = random.Next(0000, 9999);

            string OTPNumber = OTP.ToString();

            string mailbody = "Your OTP is : " + OTPNumber;
            message.Subject = "OTP for QR";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new System.Net.NetworkCredential()
            {
                UserName = "prajapatipradeep1101@gmail.com",
                Password = "28551101"
            };
            client.EnableSsl = true;
            try
            {
                string sendEmail = ConfigurationManager.AppSettings["SendEmail"].ToString();
                if (sendEmail.ToLower() == "true")
                {
                    client.Send(message);
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string generateOtp(string id)
        {
            Random random = new Random();
            int otpNum = random.Next(0000, 9999);

            string OTP = otpNum.ToString();

            Dbconnection db = new Dbconnection();

            HttpContext.Current.Session["current_qr_id"] = id;

            string Query = "insert into tbl_Otp_Number values('" + OTP + "', '" + id + "','" + HttpContext.Current.Session["usercode"].ToString() + "')";

            int result = db.executeQuery(Query);

            return OTP;
        }

        private bool CheckOtp(string otpNum)
        {
            if (otpNum == null)
            {
                return false;
            }
            else
            {
                DataSet ds;
                Dbconnection db = new Dbconnection();

                string Query = "select otp_number from tbl_Otp_Number where otp_number = '" + otpNum + "'";

                ds = db.ExecuteDataSet(Query);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string dbOtpNum = (string)ds.Tables[0].Rows[0]["otp_number"];

                    if (otpNum.Equals(dbOtpNum))
                    {
                        Response.Redirect("ScanedQrList.aspx");
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected void verify_Click(object sender, EventArgs e)
        {
            string Num = otp.Text.Trim();

            if (Num.Length > 0)
            {
                CheckOtp(Num);
            }
        }
    }
}