using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Oil_Operation_V1
{
    public partial class CreateGRN : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                LoadWarehouses();
            }
        }

        private void LoadWarehouses()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["stdconn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT Warehouse FROM Stock_Master", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    ddlWarehouse.DataSource = reader;
                    ddlWarehouse.DataTextField = "Warehouse";
                    ddlWarehouse.DataValueField = "Warehouse";
                    ddlWarehouse.DataBind();
                }
            }
            ddlWarehouse.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Warehouse --", ""));
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            string username = Session["Username"]?.ToString();
            if (string.IsNullOrEmpty(username))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["stdconn"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("INSERT INTO GRN (Item, item_no, Qty, Receipt_no, Batch_no, Receipt_date, To_warehouse, date_entered, uname, datestamp) " +
                                                        "VALUES (@Item, @ItemNumber, @Quantity, @ReceiptNumber, @BatchNo, @ReceiptDate, @Warehouse, @Date, @EnteredBy, @DateStamp)", conn))
                {
                    cmd.Parameters.AddWithValue("@Item", txtItem.Text);
                    cmd.Parameters.AddWithValue("@ItemNumber", txtItemNum.Text);
                    cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@ReceiptNumber", txtReceiptNumber.Text);
                    cmd.Parameters.AddWithValue("@BatchNo", txtBatchNo.Text);
                    cmd.Parameters.AddWithValue("@ReceiptDate", txtReceiptDate.Text);
                    cmd.Parameters.AddWithValue("@Warehouse", ddlWarehouse.SelectedValue);
                    cmd.Parameters.AddWithValue("@Date", txtDate.Text);
                    cmd.Parameters.AddWithValue("@EnteredBy", username);
                    cmd.Parameters.AddWithValue("@DateStamp", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmdStock = new SqlCommand("INSERT INTO Stock_Master (Item, item_code, Warehouse, qty_available, batch_no, uname, datestamp) " +
                                                            "VALUES (@Item, @ItemNumber, @Warehouse, @Quantity, @BatchNo, @EnteredBy, @DateStamp)", conn))
                {
                    cmdStock.Parameters.AddWithValue("@Item", txtItem.Text);
                    cmdStock.Parameters.AddWithValue("@ItemNumber", txtItemNum.Text);
                    cmdStock.Parameters.AddWithValue("@Warehouse", ddlWarehouse.SelectedValue);
                    cmdStock.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                    cmdStock.Parameters.AddWithValue("@BatchNo", txtBatchNo.Text);
                    cmdStock.Parameters.AddWithValue("@EnteredBy", username);
                    cmdStock.Parameters.AddWithValue("@DateStamp", DateTime.Now);

                    cmdStock.ExecuteNonQuery();
                }
            }

            string pdfFileName = "GRN_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            string pdfFilePath = Server.MapPath("~/GeneratedPDFs/") + pdfFileName;

            if (!Directory.Exists(Server.MapPath("~/GeneratedPDFs/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/GeneratedPDFs/"));
            }

            GeneratePDF(pdfFilePath);

            lblMessage.Text = "GRN Created Successfully! <a href='GeneratedPDFs/" + pdfFileName + "' target='_blank'>Download PDF (Expires in 5 seconds)</a>";
            lblMessage.ForeColor = System.Drawing.Color.Green;

            ScriptManager.RegisterStartupScript(this, GetType(), "RefreshPage",
                "setTimeout(function() { window.location.href = 'CreateGRN.aspx'; }, 6000);", true);
        }





        private void GeneratePDF(string filePath)
        {
            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();

            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            Paragraph title = new Paragraph("Goods Receipt Note (GRN)", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            document.Add(title);
            document.Add(new Paragraph("\n"));

            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 1, 2 });

            AddTableRow(table, "Item", txtItem.Text);
            AddTableRow(table, "Item Number", txtItemNum.Text);
            AddTableRow(table, "Quantity", txtQuantity.Text);
            AddTableRow(table, "Receipt Number", txtReceiptNumber.Text);
            AddTableRow(table, "Batch No", txtBatchNo.Text);
            AddTableRow(table, "Receipt Date", txtReceiptDate.Text);
            AddTableRow(table, "To Warehouse", ddlWarehouse.SelectedValue);
            AddTableRow(table, "Date Entered", txtDate.Text);
            AddTableRow(table, "Entered By", Session["Username"]?.ToString());

            document.Add(table);
            document.Add(new Paragraph("\n"));

            document.Add(new Paragraph("Approver 1: __________________________"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("Approver 2: __________________________"));

            document.Close();
        }


        private void AddTableRow(PdfPTable table, string label, string value)
        {
            PdfPCell cellLabel = new PdfPCell(new Phrase(label, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            PdfPCell cellValue = new PdfPCell(new Phrase(value, FontFactory.GetFont(FontFactory.HELVETICA, 12)));
            table.AddCell(cellLabel);
            table.AddCell(cellValue);
        }
    }
}
