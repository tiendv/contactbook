<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="chitietnguoidung.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ChiTietNguoiDung" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">    
        <table style="width: 100%" class="ui-corner-all">
            <tr>
                <td style="height: 23px">
                    Tên người dùng:
                </td>
                <td style="width: 300px">
                    <asp:Label ID="LblUserName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Tên thật:
                </td>
                <td class="input_textbox">
                    <asp:TextBox ID="TxtFullName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Nhóm người dùng:
                </td>
                <td style="width: 300px">
                    <asp:Label ID="LblRoleName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Email:
                </td>
                <td style="width: 150px;" class="input_textbox">
                    <asp:TextBox ID="TxtEmail" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Ngày tạo:
                </td>
                <td style="width: 150px;" class="input_textbox">
                    <asp:Label ID="LblCreatedDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Ngày đăng nhập gần nhất:
                </td>
                <td style="width: 150px;" class="input_textbox">
                    <asp:Label ID="LblLastedLoginDate" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
</asp:Content>
