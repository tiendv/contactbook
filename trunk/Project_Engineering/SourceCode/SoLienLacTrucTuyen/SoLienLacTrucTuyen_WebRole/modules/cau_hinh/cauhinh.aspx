<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="cauhinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ConfigurationPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div style="padding: 10px 0px 10px 0px; float: right">
        <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Styles/buttons/button_edit.png"
            OnClick="BtnEdit_Click" CssClass="BtnEditWithouDisable" />
    </div>
    <asp:Label ID="Label1" runat="server" CssClass="Title" Style="clear: both">Thông tin trường</asp:Label>
    <table style="width: 100%; padding: 10px 0px 10px 0px" class="loginBox ui-corner-all">
        <tr>
            <td style="width: 70px; height: 27px; padding-left: 10px">
                Tên trường:
            </td>
            <td colspan="2">
                <asp:Label ID="LblSchoolName" runat="server" Font-Bold="true"></asp:Label>
            </td>
            <td style="width: 20px; padding-left: 20px;">
                Logo:
            </td>
            <td rowspan="3" style="width: 150px; height: 27px; padding: 0px 10px 0px 10px;">
                <asp:Image ID="ImgLogo" runat="server" Width="100px" Height="100px" AlternateText="Logo" />
            </td>
        </tr>
        <tr>
            <td style="height: 27px; padding-left: 10px">
                Tỉnh/Thành:
            </td>
            <td>
                <asp:Label ID="LblProvince" runat="server" Font-Bold="true"></asp:Label>
            </td>
            <td style="height: 27px; padding-left: 50px">
                Huyện/Quận:
                <asp:Label ID="LblDistrict" runat="server" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 27px; padding-left: 10px">
                Địa chỉ:
            </td>
            <td colspan="2">
                <asp:Label ID="LblAddress" runat="server" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 27px; padding-left: 10px">
                Điện thoại:
            </td>
            <td>
                <asp:Label ID="LblPhone" runat="server" Font-Bold="true"></asp:Label>
            </td>
            <td style="height: 27px; padding-left: 50px">
                Email:
                <asp:Label ID="LblEmail" runat="server" Font-Bold="true"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
