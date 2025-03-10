<%@ Page Title="Sales Invoice" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesInvoice.aspx.cs" Inherits="Oil_Operation_V1.SalesInvoice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .container {
            width: 80%;
            margin: 40px auto;
            background-color: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }

        h2 {
            text-align: center;
            color: #2e7d32;
            margin-bottom: 20px;
        }

        .form-group {
            margin-bottom: 15px;
        }

        label {
            font-weight: bold;
            display: block;
            margin-bottom: 5px;
        }

        .form-control {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 16px;
        }

        .form-control:focus {
            border-color: #2e7d32;
            outline: none;
        }

        .btn-submit {
            display: block;
            width: 100%;
            background-color: #2e7d32;
            color: white;
            padding: 10px;
            font-size: 18px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            transition: background 0.3s;
        }

        .btn-submit:hover {
            background-color: #1b5e20;
        }

        /* Table Styles */
        .table {
            width: 100%;
            margin-top: 20px;
            border-collapse: collapse;
        }

        .table th, .table td {
            padding: 12px;
            text-align: left;
            border: 1px solid #ccc;
        }

        .table th {
            background-color: #f4f4f4;
        }

        .table td input {
            width: 100%;
        }

        /* VAT and Total Styles */
        .form-group-total {
            margin-top: 20px;
        }
    </style>

    <div class="container">
        <h2>Sales Invoice</h2>

        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="True"></asp:Label>

        <!-- Customer Name -->
        <div class="form-group">
            <label for="txtCustomerName">Customer Name:</label>
            <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" required></asp:TextBox>
        </div>

        <!-- Item Details Table -->
        <table class="table">
            <thead>
                <tr>
                    <th>Item Code</th>
                    <th>Item Name</th>
                    <th>Quantity</th>
                    <th>Unit Price</th>
                    <th>Total Price</th>
                </tr>
            </thead>
            <tbody>
                <!-- Row for Item 1 -->
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlItem1" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlItem1_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemName1" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuantity1" runat="server" CssClass="form-control" OnTextChanged="txtQuantity1_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUnitPrice1" runat="server" CssClass="form-control" OnTextChanged="txtUnitPrice1_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTotalPrice1" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>

                <!-- Row for Item 2 -->
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlItem2" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlItem2_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemName2" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuantity2" runat="server" CssClass="form-control" OnTextChanged="txtQuantity2_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUnitPrice2" runat="server" CssClass="form-control" OnTextChanged="txtUnitPrice2_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTotalPrice2" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>

                <!-- Row for Item 3 -->
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlItem3" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlItem3_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemName3" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtQuantity3" runat="server" CssClass="form-control" OnTextChanged="txtQuantity3_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUnitPrice3" runat="server" CssClass="form-control" OnTextChanged="txtUnitPrice3_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTotalPrice3" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
            </tbody>
        </table>

        <!-- VAT and Total -->
        <div class="form-group-total">
            <label for="txtVAT">VAT Amount:</label>
            <asp:TextBox ID="txtVAT" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
        </div>

        <div class="form-group-total">
            <label for="txtTotalInvoice">Total Invoice Amount:</label>
            <asp:TextBox ID="txtTotalInvoice" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>

        <!-- Confirm and Print Button -->
        <div class="form-group-total">
            <asp:Button ID="btnConfirmPrint" runat="server" Text="Confirm & Print Invoice" CssClass="btn-submit" OnClick="btnConfirmPrint_Click" />
        </div>

    </div>
</asp:Content>
