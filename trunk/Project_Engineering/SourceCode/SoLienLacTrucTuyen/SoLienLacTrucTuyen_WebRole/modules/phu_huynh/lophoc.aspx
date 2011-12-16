<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="lophoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.DetailedClassPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <table style="width: 100%;" class="inputBorder">
        <tr>
            <td style="width: 60px; height: 27px">
                Năm học:
            </td>
            <td>
                <asp:DropDownList ID="DdlYear" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="DdlYear_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 50px; height: 27px">
                Lớp:
            </td>
            <td style="width: 150px;">
                <asp:Label ID="LblClassName" runat="server"></asp:Label>
            </td>
            <td style="width: 30px;">
                Ngành:
            </td>
            <td style="width: 150px;">
                <asp:Label ID="LblFacultyName" runat="server"></asp:Label>
            </td>
            <td style="width: 30px;">
                Khối:
            </td>
            <td>
                <asp:Label ID="LblGradeName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 27px">
                Sỉ số:
            </td>
            <td>
                <asp:Label ID="LblQuantity" runat="server"></asp:Label>&nbsp; học sinh
            </td>
        </tr>
        <tr>
            <td colspan="2" style="height: 27px;">
                Giáo viên chủ nhiệm:
            </td>
        </tr>
    </table>
    <table style="padding-left: 50px">
        <tr>
            <td style="height: 27px;">
                Họ tên:
            </td>
            <td>
                <asp:Label ID="LblFormerTeacherName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 90px; height: 27px;">
                Mã giáo viên:
            </td>
            <td>
                <asp:Label ID="LblFormerTeacherCode" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 27px;">
                Ngày sinh:
            </td>
            <td>
                <asp:Label ID="LblFormerTeacherBirthday" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 27px;">
                Giới tính:
            </td>
            <td>
                <asp:Label ID="LblFormerTeacherGender" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 27px;">
                Địa chỉ:
            </td>
            <td>
                <asp:Label ID="LblFormerTeacherAddress" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="height: 27px;">
                Điện thoại:
            </td>
            <td>
                <asp:Label ID="LblFormerTeacherPhone" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
