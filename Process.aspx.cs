using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Oil_Operation_V1
{
    public partial class Process : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadItems();
                //txtProductionDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        private void LoadItems()
        {
            string connString = ConfigurationManager.ConnectionStrings["stdconn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Load Input Items 
                string inputQuery = "SELECT DISTINCT item_code FROM Stock_Master";
                SqlCommand cmd = new SqlCommand(inputQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlInputItem.DataSource = reader;
                ddlInputItem.DataTextField = "item_code";
                ddlInputItem.DataValueField = "item_code";
                ddlInputItem.DataBind();
                ddlInputItem.Items.Insert(0, new ListItem("-- Select Item No --", ""));
                reader.Close();

                // Load Output Items 
                string outputQuery = "SELECT DISTINCT item_code FROM Item_Master";
                cmd = new SqlCommand(outputQuery, conn);
                reader = cmd.ExecuteReader();

                ddlOutputItem.DataSource = reader;
                ddlOutputItem.DataTextField = "item_code";
                ddlOutputItem.DataValueField = "item_code";
                ddlOutputItem.DataBind();
                ddlOutputItem.Items.Insert(0, new ListItem("-- Select Item No --", ""));
            }
        }

        protected void ddlInputItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            string itemNo = ddlInputItem.SelectedValue;
            ddlInputBatch.Items.Clear();

            string connString = ConfigurationManager.ConnectionStrings["stdconn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT DISTINCT batch_no FROM Stock_Master WHERE item_code = @ItemNo";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ItemNo", itemNo);
                SqlDataReader reader = cmd.ExecuteReader();

                ddlInputBatch.DataSource = reader;
                ddlInputBatch.DataTextField = "batch_no";
                ddlInputBatch.DataValueField = "batch_no";
                ddlInputBatch.DataBind();
                ddlInputBatch.Items.Insert(0, new ListItem("-- Select Batch No --", ""));
            }
        }

        protected void ddlInputBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Example: Fetch additional details related to the selected batch
            string batchNo = ddlInputBatch.SelectedValue;
            if (!string.IsNullOrEmpty(batchNo))
            {
                string connString = ConfigurationManager.ConnectionStrings["stdconn"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT qty_available FROM Stock_Master WHERE batch_no = @BatchNo";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@BatchNo", batchNo);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtAvailableQuantity.Text = reader["qty_available"].ToString();
                    }
                    reader.Close();
                }
            }
        }

        protected void btnSaveData_Click(object sender, EventArgs e)
        {
            string username = Session["Username"].ToString();
            string connString = ConfigurationManager.ConnectionStrings["stdconn"].ConnectionString;

            int issueQty = int.Parse(txtIssueQuantity.Text);
            int stockBalance;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string checkStockQuery = "SELECT qty_available FROM Stock_Master WHERE item_code = @ItemNo AND batch_no = @BatchID";
                SqlCommand cmd = new SqlCommand(checkStockQuery, conn);
                cmd.Parameters.AddWithValue("@ItemNo", ddlInputItem.SelectedValue);
                cmd.Parameters.AddWithValue("@BatchID", ddlInputBatch.SelectedValue);

                stockBalance = Convert.ToInt32(cmd.ExecuteScalar());

                if (issueQty > stockBalance)
                {
                    lblStockMessage.Text = "Insufficient stock balance!";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblStockMessage.Visible = true;
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string insertProcess = "INSERT INTO Process_tb (In_Item_no, In_batch_no, Issue_qty, Out_Item_no, Out_qty, Out_batch_no, production_date, warehouse, uname, datestamp) VALUES (@InItem, @InBatch, @IssueQty, @OutItem, @OutQty, @OutBatch, @ProdDate, @Warehouse, @User, @DateStamp)";
                    SqlCommand cmd = new SqlCommand(insertProcess, conn, transaction);
                    cmd.Parameters.AddWithValue("@InItem", ddlInputItem.SelectedValue);
                    cmd.Parameters.AddWithValue("@InBatch", ddlInputBatch.SelectedValue);
                    cmd.Parameters.AddWithValue("@IssueQty", issueQty);
                    cmd.Parameters.AddWithValue("@OutItem", ddlOutputItem.SelectedValue);
                    cmd.Parameters.AddWithValue("@OutQty", txtOutputQuantity.Text);
                    cmd.Parameters.AddWithValue("@OutBatch", txtOutputBatch.Text);  // No conversion to int here
                    cmd.Parameters.AddWithValue("@ProdDate", txtProductionDate.Text);
                    cmd.Parameters.AddWithValue("@Warehouse", txtWarehouse.Text);
                    cmd.Parameters.AddWithValue("@User", username);
                    cmd.Parameters.AddWithValue("@DateStamp", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    string updateStock = "UPDATE Stock_Master SET qty_available = qty_available - @IssueQty WHERE item_code = @ItemNo AND batch_no = @BatchID";
                    cmd = new SqlCommand(updateStock, conn, transaction);
                    cmd.Parameters.AddWithValue("@IssueQty", issueQty);
                    cmd.Parameters.AddWithValue("@ItemNo", ddlInputItem.SelectedValue);
                    cmd.Parameters.AddWithValue("@BatchID", ddlInputBatch.SelectedValue);
                    cmd.ExecuteNonQuery();

                    string updateOutputStock = "IF EXISTS (SELECT 1 FROM Stock_Master WHERE item_code = @OutItem AND batch_no = @OutBatch) " +
                                              "UPDATE Stock_Master SET qty_available = qty_available + @OutQty WHERE item_code = @OutItem AND batch_no = @OutBatch " +
                                              "ELSE INSERT INTO Stock_Master (item_code, warehouse, qty_available, batch_no, uname, datestamp) VALUES (@OutItem, @Warehouse, @OutQty, @OutBatch, @User, @DateStamp)";
                    cmd = new SqlCommand(updateOutputStock, conn, transaction);
                    cmd.Parameters.AddWithValue("@OutItem", ddlOutputItem.SelectedValue);
                    cmd.Parameters.AddWithValue("@Warehouse", txtWarehouse.Text);
                    cmd.Parameters.AddWithValue("@OutQty", txtOutputQuantity.Text);
                    cmd.Parameters.AddWithValue("@OutBatch", txtOutputBatch.Text);  
                    cmd.Parameters.AddWithValue("@User", username);
                    cmd.Parameters.AddWithValue("@DateStamp", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    transaction.Commit();
                    lblMessage.Text = "Process data saved and stock updated successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Visible = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    lblMessage.Text = "Error: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                }
            }
        }
    }
}
