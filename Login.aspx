<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Oil_Operation_V1.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            padding: 0;
            background: url('Images/Oil.jpg') no-repeat center center fixed;
            background-size: cover;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        .container {
            display: flex;
            justify-content: space-between;
            align-items: center;
            width: 80%;
            max-width: 1200px;
            background: rgba(255, 255, 255, 0.9);
            border-radius: 10px;
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
            overflow: hidden;
        }

        .left-section {
            flex: 1;
            padding: 2rem;
            background-color: #006699;
            color: #ffffff;
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
        }

        .left-section img {
            width: 150px;
            margin-bottom: 1rem;
        }

        .left-section h1 {
            font-size: 2.5rem;
            font-weight: bold;
            text-align: center;
            text-transform: uppercase;
            margin: 0;
        }

        .login-form {
            flex: 1;
            padding: 2rem;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
        }

        .login-form form {
            width: 100%;
            max-width: 400px;
        }

        .login-form div {
            margin-bottom: 1rem;
        }

        .login-form label {
            font-size: 1rem;
            font-weight: bold;
            color: #333;
        }

        .login-form input[type="text"], 
        .login-form input[type="password"] {
            width: 100%;
            padding: 10px;
            font-size: 1rem;
            border: 1px solid #ccc;
            border-radius: 5px;
            margin-top: 0.5rem;
        }

        .btn-login {
            width: 100%;
            padding: 10px;
            font-size: 1rem;
            background-color: #006699;
            color: #fff;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .btn-login:hover {
            background-color: #004d66;
        }

        #Button2 {
            background-color: #999;
            margin-top: 1rem;
        }

        #Button2:hover {
            background-color: #666;
        }

        .footer {
            text-align: center;
            margin-top: 2rem;
            font-size: 0.9rem;
            color: #555;
        }
    </style>
</head>
<body>
    <div class="container">
        <!-- Left Section -->
        <div class="left-section">
            <img src="Images/logo.png" alt="Renuka Group Logo" />
            <h1>Oil Operation System</h1>
        </div>

        <!-- Right Section: Login Form -->
        <div class="login-form">
            <form id="form1" runat="server">
                <div>
                    <asp:Label ID="Label1" runat="server" Text="Welcome" ForeColor="#000066" Font-Size="XX-Large"></asp:Label>
                </div>
                <div>
                    <asp:Label ID="Label2" runat="server" Text="Username"></asp:Label>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </div>
                <div>
                    <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                    <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox>
                </div>
                <div>
                    <input type="checkbox" id="showPassword" onchange="document.getElementById('TextBox2').type=this.checked? 'text': 'password'" />
                    <label class="checkbox-label">Show Password</label>
                </div>
                <div>
                    <asp:Button CssClass="btn-login" ID="Button1" runat="server" Text="Login" OnClick="Button1_Click" />
                    <asp:Button CssClass="btn-login" ID="Button2" runat="server" Text="Cancel" OnClick="Button2_Click" />
                </div>
            </form>
            <div class="footer">
                © 2025 Renuka Group ICT. All Rights Reserved.
            </div>
        </div>
    </div>
</body>
</html>
