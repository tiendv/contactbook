<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="chitietnguoidung.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DetailUserPage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div style="float: right">
        <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Styles/buttons/button_edit.png"
            OnClick="BtnEdit_Click" CssClass="BtnEditWithouDisable" />
    </div>
    <table style="width: 600px; clear: both" class="ui-corner-all">
        <tr>
            <td style="height: 23px;">
                Tên người dùng:
            </td>
            <td>
                <asp:Label ID="LblUserName" runat="server" Width="300px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 23px">
                Tên thật:
            </td>
            <td>
                <asp:Label ID="LblFullName" runat="server" Width="300px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 23px">
                Nhóm người dùng:
            </td>
            <td>
                <asp:Label ID="LblRoleName" runat="server" Width="300px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 23px">
                Email:
            </td>
            <td style="width: 150px;">
                <asp:Label ID="LblEmail" runat="server" Width="300px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 23px">
                Ngày tạo:
            </td>
            <td>
                <asp:Label ID="LblCreatedDate" runat="server" Width="300px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 23px; width: 110px">
                Ngày đăng nhập gần nhất:
            </td>
            <td>
                <asp:Label ID="LblLastedLoginDate" runat="server" Width="300px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 23px">
                Trạng thái:
            </td>
            <td>
                <asp:Label ID="LblStatus" runat="server" Width="300px"></asp:Label>
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
