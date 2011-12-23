<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="chitietykien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.DetailedParentsCommentPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div style="padding:5px 0px 5px 0px; float:right">
        <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Styles/Images/button_edit_with_text.png"
            OnClick="BtnEdit_Click" />
    </div>
    <table style="width: 100%; padding: 10px 20px 10px 20px; clear:both" class="loginBox ui-corner-all">
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
    <div style="vertical-align: middle;">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
</asp:Content>
