<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateGRN.aspx.cs" Inherits="Oil_Operation_V1.CreateGRN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <style>
      body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
        }
        .container {
            width: 50%;
            margin: 40px auto;
            background: white;
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

        .btn-create {
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

        .btn-create:hover {
            background-color: #1b5e20;
        }
/*        .btn-print {
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

        .btn-print:hover {
            background-color: #1b5e20;
        }*/


    </style>
    
    <div class="container">
        <h2>Create Goods Receipt Note (GRN)</h2>

        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Bold="True"></asp:Label>

        <div class="form-group">
            <label for="txtItem">Item:</label>
            <asp:TextBox ID="txtItem" runat="server" CssClass="form-control" required></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtItemNum">Item Number:</label>
            <asp:TextBox ID="txtItemNum" runat="server" CssClass="form-control" required></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtQuantity">Quantity:</label>
            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" required></asp:TextBox>
        </div>

        <div class="form-group">
            <label for="txtReceiptNumber">Receipt Number:</label>
            <asp:TextBox ID="txtReceiptNumber" runat="server" CssClass="form-control" required></asp:TextBox>
        </div>

        <div class="form-group">
            <label for="txtBatchNo">Batch No:</label>
            <asp:TextBox ID="txtBatchNo" runat="server" CssClass="form-control" required></asp:TextBox>
        </div>

        <div class="form-group">
            <label for="txtReceiptDate">Receipt Date:</label>
            <asp:TextBox ID="txtReceiptDate" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
        </div>

        <div class="form-group">
            <label for="ddlWarehouse">To Warehouse:</label>
            <asp:DropDownList ID="ddlWarehouse" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>

        <div class="form-group">
            <label for="txtDate">Date:</label>
            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>

        <div class="form-group">
            <asp:Button ID="btnCreate" runat="server" Text="Create & Print GRN" CssClass="btn-create" OnClick="btnCreate_Click" />
        </div>


    </div>
</asp:Content>
