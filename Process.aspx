<%@ Page Title="Process Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Process.aspx.cs" Inherits="Oil_Operation_V1.Process" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
        }
        .container {
            width: 70%;
            margin: 40px auto;
            background-color: white;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
        }

        h2 {
            text-align: center;
            color: #2e7d32;
            margin-bottom: 30px;
        }

        .card {
            background-color: #fafafa;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
            margin-bottom: 20px;
        }

        h4 {
            color: #333;
            margin-bottom: 15px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        label {
            font-weight: bold;
            display: block;
            margin-bottom: 10px;
            font-size: 16px;
        }

        .form-control {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 16px;
            box-sizing: border-box;
        }

        .form-control:focus {
            border-color: #2e7d32;
            outline: none;
        }

        .btn-save {
            display: inline-block;
            background-color: #2e7d32;
            color: white;
            font-size: 18px;
            padding: 12px 25px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            text-align: center;
            width: auto;
            transition: background-color 0.3s;
        }

        .btn-save:hover {
            background-color: #1b5e20;
        }

        .btn-warning {
            display: inline-block;
            background-color: #fbc02d;
            color: white;
            font-size: 18px;
            padding: 12px 25px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            text-align: center;
            width: auto;
            transition: background-color 0.3s;
        }

        .btn-warning:hover {
            background-color: #c49000;
        }

        .text-center {
            text-align: center;
        }

        .text-danger {
            color: #d32f2f;
        }

        .text-success {
            color: #388e3c;
        }

        .font-weight-bold {
            font-weight: bold;
        }
    </style>

    <div class="container">
        <h2>Process Page</h2>

        <!-- Input Section -->
        <div class="card">
            <h4>Input Section</h4>
            <div class="form-group">
                <label for="ddlInputItem">Input Item:</label>
                <asp:DropDownList ID="ddlInputItem" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlInputItem_SelectedIndexChanged"></asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="ddlInputBatch">Input Batch:</label>
                <asp:DropDownList ID="ddlInputBatch" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlInputBatch_SelectedIndexChanged"></asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="txtAvailableQuantity">Available Quantity:</label>
                <asp:TextBox ID="txtAvailableQuantity" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtIssueQuantity">Issue Quantity:</label>
                <asp:TextBox ID="txtIssueQuantity" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <asp:Label ID="lblStockMessage" runat="server" CssClass="text-danger font-weight-bold" Visible="false"></asp:Label>
        </div>

        <!-- Output Section -->
        <div class="card">
            <h4>Output Section</h4>
            <div class="form-group">
                <label for="ddlOutputItem">Output Item:</label>
                <asp:DropDownList ID="ddlOutputItem" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="txtOutputQuantity">Output Quantity:</label>
                <asp:TextBox ID="txtOutputQuantity" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtOutputBatch">Output Batch:</label>
                <asp:TextBox ID="txtOutputBatch" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtProductionDate">Production Date:</label>
                <asp:TextBox ID="txtProductionDate" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtWarehouse">Warehouse:</label>
                <asp:TextBox ID="txtWarehouse" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <!-- Save Button -->
        <div class="text-center">
            <asp:Button ID="btnSaveData" runat="server" Text="Save Data" CssClass="btn-save" OnClick="btnSaveData_Click" />
        </div>

        <!-- Message Labels -->
        <div class="text-center mt-2">
            <asp:Label ID="lblMessage" runat="server" CssClass="text-success font-weight-bold" Visible="false"></asp:Label>
        </div>
    </div>
</asp:Content>
