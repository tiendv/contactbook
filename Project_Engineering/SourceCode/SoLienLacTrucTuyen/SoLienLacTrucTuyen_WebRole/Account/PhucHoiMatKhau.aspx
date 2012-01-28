<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true" CodeBehind="PhucHoiMatKhau.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.PhucHoiMatKhau" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">    
    <div class="loginBox ui-corner-all">
        <asp:Login ID="LoginCtrl" runat="server" style="width:100%"
            onloginerror="LoginCtrl_LoginError" onauthenticate="LoginCtrl_Authenticate">
            <LayoutTemplate>
                <div style="margin:0px auto 0px auto; padding:10px 0px 20px 0px; text-align:center">
                    <h2>Đăng nhập</h2>
                </div>                
                <table id="Table1" runat="server" style="width:100%">
                    <tr>
                        <td style="width:30%; height:27px; padding-left:10px;">
                            <asp:Label ID="LblTenNguoiDung" runat="server" Text="Tên người dùng:"></asp:Label>&nbsp;
                        </td>
                        <td style="width:70%; padding-right:10px">
                            <asp:TextBox ID="UserName" runat="server" Width="100%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" style="display:none; color:Red"
                                ControlToValidate="UserName" ErrorMessage="Tài khoản không được để trống" />
            	            <asp:ValidatorCalloutExtender ID="UserNameRequired_ValidatorCalloutExtender" runat="server" 
                                Enabled="True" TargetControlID="UserNameRequired">
                            </asp:ValidatorCalloutExtender>
            	            <asp:RegularExpressionValidator ID="UsernameValidator" runat="server" style="display:none; color:Red"
                                ControlToValidate="UserName"
                                ValidationExpression="[\w| ]*"
                                ErrorMessage="Tài khoản không hợp lệ" />
                            <asp:ValidatorCalloutExtender ID="UsernameValidator_ValidatorCalloutExtender" runat="server" 
                                Enabled="True" TargetControlID="UsernameValidator">
                            </asp:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:30%; height:27px; padding-left:10px">
                            <asp:Label ID="LblMatKhau" runat="server" Text="Mật khẩu:"></asp:Label>&nbsp;
                        </td>
                        <td style="padding-right:5px">
                            <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="98%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" style="display:none; color:Red"
                                ControlToValidate="Password" ErrorMessage="Mật khẩu không được để trống" />
                            <asp:ValidatorCalloutExtender ID="PasswordRequired_ValidatorCalloutExtender" runat="server" 
                                Enabled="True" TargetControlID="PasswordRequired">
                            </asp:ValidatorCalloutExtender>
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" style="display:none; color:Red"
                                ControlToValidate="Password"
                                ValidationExpression='[\w| !"§$%&amp;/()=\-?\*]*'
                                ErrorMessage="Mật khẩu không hợp lệ" />
                            <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender" runat="server" 
                                Enabled="True" TargetControlID="RegularExpressionValidator1">
                            </asp:ValidatorCalloutExtender>--%>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="height:27px;">    
                            <asp:CheckBox ID="RememberMe" runat="server" Text=" Ghi nhớ " />&nbsp;
                            <div style="color:Red">
                                <asp:Literal ID="FailureText" runat="server"/>
                            </div>
                        </td>
                    </tr>
                </table>
                <div style="margin:0px auto 0px auto; padding:10px 0px 5px 0px; text-align:center" >
                    <asp:ImageButton ID="Login" runat="server" CommandName="Login" ImageUrl="~/Styles/buttons/button_login.png"/>
                </div>
            </LayoutTemplate>            
        </asp:Login>
    </div>
</asp:Content>
