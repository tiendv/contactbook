<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="detailschool.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.SchoolDetailPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <table style="width: 100%; padding: 10px 0px 10px 0px" class="loginBox ui-corner-all">
        <tr>
            <td style="width: 90px; height: 27px; padding-left: 10px">
                Tên trường:
            </td>
            <td colspan="2">
                <asp:Label ID="LblSchoolName" runat="server"></asp:Label>
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
                 <asp:Label ID="LblProvince" runat="server"></asp:Label>
            </td>
            <td style="height: 27px; padding-left: 50px">
                Huyện/Quận:
                <asp:Label ID="LblDistrict" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 27px; padding-left: 10px">
                Địa chỉ:
            </td>
            <td colspan="2">
                <asp:Label ID="LblAddress" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 27px; padding-left: 10px">
                Điện thoại:
            </td>
            <td>
                <asp:Label ID="LblPhone" runat="server"></asp:Label>
            </td>
            <td style="height: 27px; padding-left: 50px">
                Email:
                <asp:Label ID="LblEmail" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="padding: 5px; vertical-align: middle;">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/buttons/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/buttons/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
</asp:Content>
