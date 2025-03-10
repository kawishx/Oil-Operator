using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Oil_Operation_V1
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection sqlconn = new SqlConnection(ConfigurationManager.ConnectionStrings["stdconn"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            bool boolReturnValue = false;

            SqlCommand command = new SqlCommand("SELECT * FROM Login where username='" + TextBox1.Text + "' and password='" + TextBox2.Text + "'", sqlconn);
            SqlDataReader Dr;
            sqlconn.Open();
            Dr = command.ExecuteReader();

            while (Dr.Read())
            {
                string s = Dr[1].ToString();
                if ((TextBox1.Text == Dr[0].ToString().Trim()) & (TextBox2.Text == Dr[1].ToString().Trim()))
                {
                    boolReturnValue = true;

                    Session["username"] = Dr[0].ToString().Trim();
                    Session["password"] = Dr[1].ToString().Trim();
                    //Session["userlevel"] = Dr[2].ToString().Trim();
                    //Session["email"] = Dr[3].ToString().Trim();
                }
            }

            if (boolReturnValue == true)
            {
                Response.Write("<script>alert('login success');</script>");
                Response.Redirect("Home.aspx");

            }
            else
            {
                Response.Write("<script>alert('login error');</script>");

            }
            sqlconn.Close();
            Dr.Close();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }
    }
}