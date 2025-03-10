using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace Oil_Operation_V1
{
    public partial class SalesInvoice : System.Web.UI.Page
    {
        private string connString = ConfigurationManager.ConnectionStrings["stdconn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                BindItems();
            }
        }

        private void BindItems()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT item_code, item_desc FROM Item_Master";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    BindDropdown(ddlItem1, dt);
                    BindDropdown(ddlItem2, dt);
                    BindDropdown(ddlItem3, dt);
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        private void BindDropdown(DropDownList ddl, DataTable dt)
        {
            ddl.DataSource = dt;
            ddl.DataTextField = "item_code";
            ddl.DataValueField = "item_code";
            ddl.DataBind();
            ddl.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Item--", ""));
        }

        protected void ddlItem1_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleItemSelection(ddlItem1, txtItemName1);
        }

        protected void ddlItem2_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleItemSelection(ddlItem2, txtItemName2);
        }

        protected void ddlItem3_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleItemSelection(ddlItem3, txtItemName3);
        }

        private void HandleItemSelection(DropDownList ddl, TextBox itemNameBox)
        {
            if (!string.IsNullOrEmpty(ddl.SelectedValue))
            {
                UpdateItemName(ddl.SelectedValue, itemNameBox);
            }
        }

        private void UpdateItemName(string item_code, TextBox itemNameBox)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT item_desc FROM Item_Master WHERE item_code = @ItemCode";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ItemCode", item_code);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        itemNameBox.Text = reader["item_desc"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }

        protected void txtQuantity1_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice(txtQuantity1, txtUnitPrice1, txtTotalPrice1);
        }

        protected void txtQuantity2_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice(txtQuantity2, txtUnitPrice2, txtTotalPrice2);
        }

        protected void txtQuantity3_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice(txtQuantity3, txtUnitPrice3, txtTotalPrice3);
        }

        protected void txtUnitPrice1_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice(txtQuantity1, txtUnitPrice1, txtTotalPrice1);
        }

        protected void txtUnitPrice2_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice(txtQuantity2, txtUnitPrice2, txtTotalPrice2);
        }

        protected void txtUnitPrice3_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice(txtQuantity3, txtUnitPrice3, txtTotalPrice3);
        }

        private void CalculateTotalPrice(TextBox quantityBox, TextBox unitPriceBox, TextBox totalPriceBox)
        {
            if (decimal.TryParse(quantityBox.Text, out decimal quantity) && decimal.TryParse(unitPriceBox.Text, out decimal unitPrice))
            {
                totalPriceBox.Text = (quantity * unitPrice).ToString("0.00");
                UpdateTotalInvoice();
            }
        }

        private void UpdateTotalInvoice()
        {
            decimal totalInvoice = 0;
            totalInvoice += GetTextBoxValue(txtTotalPrice1);
            totalInvoice += GetTextBoxValue(txtTotalPrice2);
            totalInvoice += GetTextBoxValue(txtTotalPrice3);
            totalInvoice += GetTextBoxValue(txtVAT);

            txtTotalInvoice.Text = totalInvoice.ToString("0.00");
        }

        private decimal GetTextBoxValue(TextBox textBox)
        {
            return decimal.TryParse(textBox.Text, out decimal value) ? value : 0;
        }

        protected void btnConfirmPrint_Click(object sender, EventArgs e)
        {
            string username = Session["Username"]?.ToString();
            if (string.IsNullOrEmpty(username))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            string invoiceNumber = "INV-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            string pdfFilePath = Server.MapPath("~/GeneratedPDFs/") + invoiceNumber;

            if (!Directory.Exists(Server.MapPath("~/GeneratedPDFs/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/GeneratedPDFs/"));
            }

            // Save sales data
            SaveSalesData(invoiceNumber, username);

            // Generate the PDF
            GeneratePDFInvoice(pdfFilePath);

            // Display download link
            lblMessage.Text = "Invoice generated successfully. <a href='GeneratedPDFs/" + invoiceNumber + "' target='_blank'>Download Invoice</a>";
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }

        private void SaveSalesData(string invoiceNumber, string username)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["stdconn"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO Sales (invoice_number, customer, Item1, Item1_qty, Item1_price, Item2, Item2_qty, Item2_price, Item3, Item3_qty, Item3_price, VAT, Tot_price, uname, datestamp) " +
                               "VALUES (@InvoiceNumber, @CustomerName, @ItemCode1, @Quantity1, @UnitPrice1, @ItemCode2, @Quantity2, @UnitPrice2, @ItemCode3, @Quantity3, @UnitPrice3, @VAT, @TotalInvoice, @User, @DateStamp)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@InvoiceNumber", invoiceNumber);
                    cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
                    cmd.Parameters.AddWithValue("@ItemCode1", ddlItem1.SelectedValue);
                    cmd.Parameters.AddWithValue("@Quantity1", txtQuantity1.Text);
                    cmd.Parameters.AddWithValue("@UnitPrice1", txtUnitPrice1.Text);
                    cmd.Parameters.AddWithValue("@ItemCode2", ddlItem2.SelectedValue);
                    cmd.Parameters.AddWithValue("@Quantity2", txtQuantity2.Text);
                    cmd.Parameters.AddWithValue("@UnitPrice2", txtUnitPrice2.Text);
                    cmd.Parameters.AddWithValue("@ItemCode3", ddlItem3.SelectedValue);
                    cmd.Parameters.AddWithValue("@Quantity3", txtQuantity3.Text);
                    cmd.Parameters.AddWithValue("@UnitPrice3", txtUnitPrice3.Text);
                    cmd.Parameters.AddWithValue("@VAT", txtVAT.Text);
                    cmd.Parameters.AddWithValue("@TotalInvoice", txtTotalInvoice.Text);
                    cmd.Parameters.AddWithValue("@User", username);
                    cmd.Parameters.AddWithValue("@DateStamp", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void GeneratePDFInvoice(string filePath)
        {
            try
            {
                Document pdfDoc = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

                // Open the document for writing
                pdfDoc.Open();

                // Add a title
                pdfDoc.Add(new Paragraph("Sales Invoice", FontFactory.GetFont("Arial", 16, Font.BOLD)));
                pdfDoc.Add(new Paragraph("Invoice Number: " + Path.GetFileName(filePath), FontFactory.GetFont("Arial", 12)));

                // Add customer details
                pdfDoc.Add(new Paragraph("Customer Name: " + txtCustomerName.Text, FontFactory.GetFont("Arial", 12)));

                // Add table for items
                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;

                // Table header
                table.AddCell("Item Code");
                table.AddCell("Item Name");
                table.AddCell("Quantity");
                table.AddCell("Unit Price");
                table.AddCell("Total Price");

                // Add Item 1 data
                table.AddCell(ddlItem1.SelectedValue);
                table.AddCell(txtItemName1.Text);
                table.AddCell(txtQuantity1.Text);
                table.AddCell(txtUnitPrice1.Text);
                table.AddCell(txtTotalPrice1.Text);

                // Add Item 2 data
                table.AddCell(ddlItem2.SelectedValue);
                table.AddCell(txtItemName2.Text);
                table.AddCell(txtQuantity2.Text);
                table.AddCell(txtUnitPrice2.Text);
                table.AddCell(txtTotalPrice2.Text);

                // Add Item 3 data
                table.AddCell(ddlItem3.SelectedValue);
                table.AddCell(txtItemName3.Text);
                table.AddCell(txtQuantity3.Text);
                table.AddCell(txtUnitPrice3.Text);
                table.AddCell(txtTotalPrice3.Text);

                // Add VAT and total invoice amount
                table.AddCell("");
                table.AddCell("VAT:");
                table.AddCell("");
                table.AddCell("");
                table.AddCell(txtVAT.Text);

                table.AddCell("");
                table.AddCell("Total Invoice:");
                table.AddCell("");
                table.AddCell("");
                table.AddCell(txtTotalInvoice.Text);

                // Add table to the PDF
                pdfDoc.Add(table);

                // Close the document
                pdfDoc.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error generating PDF: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

    }
}
