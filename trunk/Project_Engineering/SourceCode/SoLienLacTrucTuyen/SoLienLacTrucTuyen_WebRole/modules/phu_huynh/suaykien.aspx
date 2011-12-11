<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="suaykien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.EditParentsCommentPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <table style="width: 100%; padding: 10px 20px 10px 20px" class="loginBox ui-corner-all">
        <tr>
            <td style="vertical-align: top; padding-top: 7px;">
                Tiêu đề:
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TxtTitle" runat="server" CssClass="input_textbox"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredTitle" runat="server" ValidationGroup="AddComment"
                    Display="Dynamic" ForeColor="Red" ControlToValidate="TxtTitle" ErrorMessage="Tiêu đề không được để trống">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 70px; vertical-align: top; padding-top: 7px;">
                Nội dung:
                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TxtContent" runat="server" TextMode="MultiLine" Height="230px" Font-Names="Arial" CssClass="input_textbox"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredContent" runat="server" ValidationGroup="AddComment"
                    Display="Dynamic" ForeColor="Red" ControlToValidate="TxtContent" ErrorMessage="Nội dung không được để trống">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <div style="width: 170px; margin: 0px auto 0px auto;">
        <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/Styles/Images/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="AddComment" CssClass="SaveButton" />
        &nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
