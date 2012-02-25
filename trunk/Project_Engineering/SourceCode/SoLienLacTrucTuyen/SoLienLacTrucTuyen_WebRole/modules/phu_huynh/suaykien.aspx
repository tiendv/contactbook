<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="suaykien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.EditParentsCommentPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <table style="width: 100%; padding: 10px 20px 0px 20px" class="loginBox ui-corner-all">
        <tr>
            <td style="vertical-align: top; padding-top: 7px;">
                Tiêu đề:
                <span class="required">*</span>
            </td>
            <td class="readOnlyTextBox">
                <asp:Label ID="LblTitle" runat="server" ViewStateMode="Enabled"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 70px; vertical-align: top; padding-top: 7px;">
                Nội dung:
                <span class="required">*</span>
            </td>
            <td>
                <asp:TextBox ID="TxtContent" runat="server" TextMode="MultiLine" Height="230px" Font-Names="Arial" CssClass="input_textbox"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredContent" runat="server" ValidationGroup="AddComment"
                    Display="Dynamic" ForeColor="Red" ControlToValidate="TxtContent" ErrorMessage="Nội dung không được để trống">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <div>
        <asp:Label ID="Label15" runat="server" Text="*" ForeColor="Red"></asp:Label>
        :Thông tin bắt buộc nhập
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto;">
        <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="AddComment" CssClass="SaveButton" />
        &nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
