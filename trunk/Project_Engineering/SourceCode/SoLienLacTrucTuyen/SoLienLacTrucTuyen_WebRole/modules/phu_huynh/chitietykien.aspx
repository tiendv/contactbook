<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="~/modules/y_kien/chitietykien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ModuleParents.DetailedParentsCommentPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div class="table_data ui-corner-all">
        <table style="width: 100px">
            <tr>
                <td style="width: 30%">
                    Thông tin:
                </td>
                <td style="width: 70%">
                    <asp:Label ID="LblInfor" runat="server" Width="400px" CssClass="readOnlyTextBox"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Tiêu đề:
                </td>
                <td>
                    <asp:Label ID="LblTitle" runat="server" CssClass="readOnlyTextBox"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Nội dung:
                </td>
                <td>
                    <asp:Label ID="LblContent" runat="server" CssClass="readOnlyTextBox"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Phản hồi:
                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtReply" Font-Names="Arial" TextMode="MultiLine" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div style="padding: 5px 7px 5px 7px;">
        <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
        :Thông tin bắt buộc nhập
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
        <asp:ImageButton ID="BtnReply" runat="server" ImageUrl="~/Styles/Images/button_save.png"
            OnClick="BtnReply_Click" ValidationGroup="AddConfirm" CssClass="SaveButton" />
        &nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
