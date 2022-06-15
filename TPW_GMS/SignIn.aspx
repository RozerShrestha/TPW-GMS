<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="TPW_GMS.SignIn" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Assets/css/login.css" rel="stylesheet" />
    <title>The Physique Workshop</title>
    
</head>
<body>
    <form id="form" runat="server">
        <asp:HiddenField ID="hidSoftwareExpireDateCheck" runat="server" />
            <div class="panel">
                <div class="cont">
                    <div class="demo">
                        <img class="bgvid inner" src="Assets/Images/background.jpg" />
                        <div class="login">
                            <div class="logo">
                                <img src="Assets/Images/TPW_login_logo.png" width="300px" height="300px" />
                            </div>
                            <div class="login__form">
                                <div class="login__row">
                                    <svg class="login__icon name svg-icon" viewBox="0 0 20 20">
                                        <path d="M0,20 a10,8 0 0,1 20,0z M10,0 a4,4 0 0,1 0,8 a4,4 0 0,1 0,-8" />
                                    </svg>
                                    <asp:TextBox ID="txtUserName" class="login__input" placeholder="Username" autofocus runat="server"></asp:TextBox>
                                </div>
                                <div class="login__row">
                                    <svg class="login__icon pass svg-icon" viewBox="0 0 20 20">
                                        <path d="M0,20 20,20 20,8 0,8z M10,13 10,16z M4,8 a6,8 0 0,1 12,0" />
                                    </svg>
                                    <asp:TextBox ID="txtPassword" TextMode="Password" placeholder="password" class="login__input pass" runat="server"></asp:TextBox>
                                    <br />
                                </div>
                                <asp:Button ID="btnLogina" runat="server" Text="Login" CssClass="logins" OnClick="btnLogin_Click" OnClientClick="getToken()" />
                                <%--<asp:Button ID="btnLogin" runat="server" Text="SignUp" class="login__submits"></asp:Button>--%>
                                <%--<a class="login__submit">Sign up</a>--%>
                                <br />
                                <br />
                                <br />
                                <asp:Label ID="lblNotification" Style="font-weight: bold" class="login__input" runat="server"></asp:Label>
                                <div>
                                    <%--<h1 style="color: white; display: inline-block">Add New Branch</h1>--%>
                                    <%--<asp:Label ID="lblSignup" runat="server" CssClass="login__submit" Text="Add New Branch"></asp:Label>--%>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>  
    </form>
</body>
<script src="Assets/js/jquery-2.1.3.min.js"></script>
<script src="Assets/js/login.js"></script>
<script>
    $(document).ready(function () {

    });
    function getToken() {
        var bearer = "";
        var requestParam = {
            grant_type: 'password',
            username: $('#txtUserName').val(),
            password: $('#txtPassword').val()
        };
        $.ajax({
            type: "POST",
            url: "/token",
            async:false,
            data: requestParam,
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",
            success: function (data) {
                bearer = JSON.parse(JSON.stringify(data));
                token = bearer.access_token;
                window.localStorage.setItem('auth_token', token);
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        })
    }
</script>
</html>
