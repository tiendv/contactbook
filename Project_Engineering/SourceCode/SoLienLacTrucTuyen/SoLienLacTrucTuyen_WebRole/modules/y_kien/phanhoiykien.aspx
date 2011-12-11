<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="phanhoiykien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.FeedbackParentsCommentPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <table style="width: 100%; padding: 10px 20px 10px 20px; clear: both" class="loginBox ui-corner-all">
        <tr>
            <td>
            </td>
            <td class="readOnlyTextBox">
                <asp:Label ID="LblStudentInfo" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 80px; vertical-align: top; padding-top: 3px;">
                Ngày góp ý:
            </td>
            <td class="readOnlyTextBox">
                <asp:Label ID="LblDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top; padding-top: 3px;">
                Tiêu đề:
            </td>
            <td class="readOnlyTextBox">
                <asp:Label ID="LblTitle" runat="server" ViewStateMode="Enabled"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 70px; vertical-align: top; padding-top: 3px;">
                Nội dung:
            </td>
            <td class="readOnlyTextBox">
                <asp:Label ID="LblContent" runat="server" Height="250px" ViewStateMode="Enabled"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 70px; vertical-align: top; padding-top: 3px;">
                Phản hồi:
                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TxtFeedback" runat="server" TextMode="MultiLine" Height="230px"
                    Font-Names="Arial" CssClass="input_textbox"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFeedback" runat="server" ValidationGroup="AddFeedback"
                    Display="Dynamic" ForeColor="Red" ControlToValidate="TxtFeedback" ErrorMessage="Nội dung phản hồi không được để trống">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <div style="width: 170px; margin: 0px auto 0px auto;">
        <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/Styles/Images/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="AddFeedback" CssClass="SaveButton" />
        &nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
