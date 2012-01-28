<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="suathongbao.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.MessageModifyPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <p style="padding: 5px">
        <asp:Label ID="LblTitle" runat="server" Font-Bold="true" ForeColor="Blue" Font-Size="15px"></asp:Label>
    </p>    
    <table style="width: 100%; padding: 10px; border: 2px solid #0000C8; background-color: #F8FDFD;"
        class="inputBorder ui-corner-all">
        <tr>
            <td style="width: 60px; vertical-align: top; padding-top: 3px;">
                Tiêu đề:
                <asp:Label ID="Label102" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td style="width: auto;" colspan="3">
                <asp:TextBox ID="TxtTitle" runat="server" Style="width: 99%; font-weight: bold" MaxLength="100"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="TitleRequired" runat="server" ControlToValidate="TxtTitle"
                    ValidationGroup="ModifyMessage" ErrorMessage="Tiêu đề không được để trống" Display="Dynamic"
                    ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 80px; vertical-align: top; padding-top: 3px;">
                Nội dung:
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td style="width: auto;" colspan="3">
                <asp:TextBox ID="TxtContent" runat="server" TextMode="MultiLine" MaxLength="500"
                    Style="width: 99%; height: 100px; font-family: Arial"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="ContentRequired" runat="server" ControlToValidate="TxtContent"
                    ValidationGroup="ModifyMessage" ErrorMessage="Nội dung không được để trống" Display="Dynamic"
                    ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="padding: 10px 0px 0px 0px;">
                    <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    :Thông tin bắt buộc nhập<br />
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 15px 0px 5px 0px">
        <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="ModifyMessage" CssClass="SaveButton" />&nbsp;
        <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
