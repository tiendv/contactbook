<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="lophoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.DetailedClassPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <table style="width: 100%;" class="inputBorder">
        <tr>
            <td style="width: 80px; vertical-align: top; padding-top: 3px;">
                Năm học:
            </td>
            <td style="width: auto;">
                <asp:DropDownList ID="DdlYear" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="DdlYear_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 50px; vertical-align: top; padding-top: 3px;">
                Lớp:
            </td>
            <td style="width: auto;" class="readOnlyTextBox input_textbox">
                <asp:Label ID="LblClassName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: text-top; padding-top: 3px;">
                Ngành:
            </td>
            <td style="width: auto;" class="readOnlyTextBox input_textbox">
                <asp:Label ID="LblFacultyName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: text-top; padding-top: 3px;">
                Khối:
            </td>
            <td style="width: auto;" class="readOnlyTextBox input_textbox">
                <asp:Label ID="LblGradeName" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: text-top; padding-top: 3px;">
                Sỉ số:
            </td>
            <td style="width: auto;" class="readOnlyTextBox input_textbox">
                <asp:Label ID="LblQuantity" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: text-top; padding-top: 3px;">
                GVCN:
            </td>
            <td style="width: auto;" class="readOnlyTextBox input_textbox">
                <asp:Label ID="LblFormerTeacherName" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
