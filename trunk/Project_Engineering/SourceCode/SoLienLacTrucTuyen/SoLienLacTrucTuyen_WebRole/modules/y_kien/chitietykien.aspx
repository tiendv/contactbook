<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="chitietykien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DetailedParentsCommentPage" %>

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
            </td>
            <td class="readOnlyTextBox">
                <asp:Label ID="LblFeedback" runat="server" Height="250px"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="width: 103px; margin: 0px auto 0px auto;">
        <asp:ImageButton ID="BtnFeedback" runat="server" ImageUrl="~/Styles/Images/button_feedback.png"
            OnClick="BtnFeedback_Click" />
    </div>
    <div style="vertical-align: middle;">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
</asp:Content>
