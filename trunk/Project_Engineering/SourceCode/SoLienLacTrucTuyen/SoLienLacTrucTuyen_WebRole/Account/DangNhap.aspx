<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="DangNhap.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.LoginPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div class="loginBox ui-corner-all">
        <asp:Login ID="LoginCtrl" runat="server" Style="width: 100%" OnLoginError="LoginCtrl_LoginError"
            OnLoggedIn="LoginCtrl_OnLoggedIn" OnAuthenticate="LoginCtrl_Authenticate">
            <LayoutTemplate>
                <div style="margin: 0px auto 0px auto; padding: 10px 0px 20px 0px; text-align: center">
                    <h2>
                        Đăng nhập</h2>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 30%; height: 27px; padding-left: 10px">
                                    Tỉnh/Thành:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlProvinces" runat="server" Width="95%" AutoPostBack="true"
                                        OnSelectedIndexChanged="DdlProvinces_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 27px; padding-left: 10px">
                                    Huyện/Quận:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlDistricts" runat="server" Width="95%" AutoPostBack="true"
                                        OnSelectedIndexChanged="DdlDistricts_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; height: 27px; padding-left: 10px">
                                    Trường:
                                </td>
                                <td>
                                    <asp:DropDownList ID="DDlSchools" runat="server" Width="95%">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="SchoolRequired" runat="server" ControlToValidate="DDlSchools"
                                        ValidationGroup="LoginGroup" ForeColor="Red" Display="Dynamic" ErrorMessage="Chưa chọn trường học"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <table id="Table1" runat="server" style="width: 100%">
                    <tr>
                        <td style="width: 30%; height: 27px; padding-left: 10px;">
                            <asp:Label ID="LblTenNguoiDung" runat="server" Text="Tên người dùng:"></asp:Label>&nbsp;
                        </td>
                        <td style="width: 70%; padding-right: 10px">
                            <asp:TextBox ID="UserName" runat="server" Width="98%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ValidationGroup="LoginGroup" Display="Dynamic"
                                ForeColor="Red" ControlToValidate="UserName" ErrorMessage="Tài khoản không được để trống" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%; height: 27px; padding-left: 10px">
                            <asp:Label ID="LblMatKhau" runat="server" Text="Mật khẩu:"></asp:Label>&nbsp;
                        </td>
                        <td style="padding-right: 10px">
                            <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="98%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ValidationGroup="LoginGroup" ControlToValidate="Password"
                                ErrorMessage="Mật khẩu không được để trống" Display="Dynamic" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="height: 27px;">
                            <asp:CheckBox ID="RememberMe" runat="server" Text=" Ghi nhớ " />&nbsp;
                            <div style="color: Red">
                                <asp:Literal ID="FailureText" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="height: 27px;">
                            <asp:HyperLink ID="HlkPhucHoiMatKhau" runat="server" NavigateUrl="~/Account/PhucHoiMatKhau.aspx">Quên mật khẩu?</asp:HyperLink>
                        </td>
                    </tr>
                </table>
                <div style="margin: 0px auto 0px auto; padding: 10px 0px 15px 0px; text-align: center">
                    <asp:ImageButton ID="Login" runat="server" CommandName="Login" ValidationGroup="LoginGroup" ImageUrl="~/Styles/Images/button_login.png"
                        CssClass="BtnLogin" />
                </div>
            </LayoutTemplate>
        </asp:Login>
    </div>
</asp:Content>
