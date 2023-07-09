<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginMember.aspx.cs" Inherits="TPW_GMS.LoginMember" %>

<!DOCTYPE html>

<head runat="server">
    <title></title>
    <link href="Assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Assets/css/styleMemberLogin.css" rel="stylesheet" />
    <script src="Assets/jquery/jquery.min.js"></script>
    
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <!-- Form Module-->
            <div class="module form-module">
                <div class="togglea">
                    <img src="Assets/css/images/tpwlogo.png" style="margin-left:30px" alt="Logo" title="Logo" width="138" /><br />
                    <span class="display1">THE PHYSIQUE</span ><span class="display2">WORKSHOP</span>
                </div>
                <div class="form">
                    <h2>Login to your account</h2>
                    <div>
                        <asp:TextBox ID="txtUserName" runat="server" placeholder="mobile number" CssClass="form-control"></asp:TextBox>
                        <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" placeholder="Password" CssClass="form-control"></asp:TextBox>

                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-block btn-info ripple-effect" OnClick="btnSubmit_Click" OnClientClick="getToken()" Text="Login" />
                    </div>
                </div>
                <div class="form">
                    <h2>Enter your credential and new password</h2>
                    <div>
                        <input id="txtResetUsername" runat="server" type="text" placeholder="Username" />
                        <input id="txtResetPassword" runat="server" type="password" placeholder="Password" />
                        <input id="txtResetNewPassword" runat="server" type="password" placeholder="New Password" />
                        <asp:Button ID="btnResetPassword" runat="server" CssClass="btn btn-block btn-danger" OnClick="btnResetPassword_Click" Text="Submit" />
                    </div>
                </div>
                <div class="labelInfo">
                    <asp:Label ID="lblInfo" runat="server" CssClass="text-primary"></asp:Label>
                </div>
                <div class="cta">Change your Password</div>
            </div>
        </div>
    </form>
</body>
<script>
    $('.cta').click(function () {
        // Switches the Icon
        $(this).children('i').toggleClass('fa-pencil');
        // Switches the forms  
        $('.form').animate({
            height: "toggle",
            'padding-top': 'toggle',
            'padding-bottom': 'toggle',
            opacity: "toggle"
        }, "slow");
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
            data: requestParam,
            contentType: "application/x-www-form-urlencoded",
            dataType: "json",
            success: function (data) {
                bearer = JSON.parse(JSON.stringify(data));
                token = bearer.access_token;
                window.localStorage.setItem('auth_token_staff', token);
            },
            failure: function (response) {

                //alert(response.responseText);
            },
            error: function (response) {
                //alert(response.responseText);
            }
        })
    }
    </script>
</html>
