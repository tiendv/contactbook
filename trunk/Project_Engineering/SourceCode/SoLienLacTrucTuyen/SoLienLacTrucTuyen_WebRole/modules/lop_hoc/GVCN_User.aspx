<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true" CodeBehind="GVCN_User.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.GVCN_User" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
<table>
    <tr>
        <td>
            <table class="search">
                <tr>
                    <td style="width:60px;">
                        <asp:Label ID="Label2" runat="server" Text="Năm học:"></asp:Label>
                    </td>
                    <td style="width:200px;">
                        <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>                            
                </tr>
            </table>
        </td>
        <td style="width:80px">
            <asp:ImageButton ID="BtnSearch" runat="server" 
                ImageUrl="~/Styles/Images/button_search_with_text.png" ToolTip="Tìm kiếm lớp học"
                onclick="BtnSearch_Click"/>
        </td>
    </tr>
</table>
<div style="width:470px; margin:0px auto 0px auto; padding:0px 10px 0px 10px; border: 2px solid #0000C8; background-color: #F1F8F5;">
    <table cellpadding="5px">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Họ và tên:"></asp:Label>
            </td>
            <td>
                <asp:Label ID="LblHoTen" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Ngày sinh:"></asp:Label>
            </td>
            <td>
                <asp:Label ID="LblNgaySinh" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Giới tính:"></asp:Label>
            </td>
            <td>
                <asp:Label ID="LblGioiTinh" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="Địa chỉ liên lạc:"></asp:Label>
            </td>
            <td>
                <asp:Label ID="LblDiaChi" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label9" runat="server" Text="Điện thoại:"></asp:Label>
            </td>
            <td>
                <asp:Label ID="LblDienThoai" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</div>
</asp:Content>
