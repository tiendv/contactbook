<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="suanguoidung.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ModifyUserPage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <table style="width: 600px" class="ui-corner-all">
        <tr>
            <td style="height: 23px; width: 70px">
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
                <asp:TextBox ID="TxtFullName" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 23px">
                Email:
            </td>
            <td style="width: 150px;" class="input_textbox">
                <asp:TextBox ID="TxtEmail" runat="server" Width="300px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height: 23px">
                Trạng thái:
            </td>
            <td>
                <asp:DropDownList ID="DDLStatus" runat="server" Width="300px">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
        <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="ModifyUser" CssClass="SaveButton" />&nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
