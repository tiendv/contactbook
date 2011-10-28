<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true" CodeBehind="DangNhap.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DangNhap" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">    
    <div class="loginBox ui-corner-all">
        <asp:Login ID="LoginCtrl" runat="server" style="width:100%" onloginerror="LoginCtrl_LoginError" onauthenticate="LoginCtrl_Authenticate">
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
                            <asp:TextBox ID="UserName" runat="server" Width="98%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" Display="Dynamic" ForeColor="Red" ControlToValidate="UserName" ErrorMessage="Tài khoản không được để trống" />
            	            <%--<asp:RegularExpressionValidator ID="UsernameValidator" runat="server" Display="Dynamic" ForeColor="Red" ControlToValidate="UserName" ValidationExpression="[\w| ]*" ErrorMessage="Tài khoản không hợp lệ" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:30%; height:27px; padding-left:10px">
                            <asp:Label ID="LblMatKhau" runat="server" Text="Mật khẩu:"></asp:Label>&nbsp;
                        </td>
                        <td style="padding-right:10px">
                            <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="98%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Mật khẩu không được để trống" Display="Dynamic" ForeColor="Red" />                            
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
                    <tr>
                        <td></td>
                        <td style="height:27px;">    
                            <asp:HyperLink ID="HlkPhucHoiMatKhau" runat="server" NavigateUrl="~/Account/PhucHoiMatKhau.aspx">Quên mật khẩu?</asp:HyperLink>
                        </td>
                    </tr>
                </table>
                <div style="margin:0px auto 0px auto; padding:10px 0px 15px 0px; text-align:center" >
                    <asp:ImageButton ID="Login" runat="server" CommandName="Login" ImageUrl="~/Styles/Images/button_login.png" CssClass="BtnLogin"/>
                </div>
            </LayoutTemplate>            
        </asp:Login>
    </div>
</asp:Content>
