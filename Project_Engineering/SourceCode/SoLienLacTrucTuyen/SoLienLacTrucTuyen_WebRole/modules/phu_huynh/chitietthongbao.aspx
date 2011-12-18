<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="chitietthongbao.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.DetailMessage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div style="padding: 5px 7px 10px 7px;">
        <table style="width: 100%" class="inputBorder">
            <tr>
                <td style="vertical-align: top; padding-top: 3px;">
                    Tiêu đề:
                </td>
                <td style="width: auto;" class="readOnlyTextBox input_textbox">
                    <asp:Label ID="LblTitle" runat="server" Style="width: 99%"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; padding-top: 3px;" class="style1">
                    Nội dung:
                </td>
                <td style="width: auto;" class="readOnlyTextBox input_textbox">
                    <asp:Label ID="LblContent" runat="server" Height="230px" Style="width: 99%; height: 250px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width:65px; vertical-align: top; padding-top: 3px;" class="style1">
                    Tình trạng:
                </td>
                <td style="width: auto;" class="readOnlyTextBox input_textbox">
                    <asp:Label ID="LblStatus" runat="server" Style="width: 99%;"></asp:Label>
                </td>
            </tr>
             <tr>
                <td style="vertical-align: top; padding-top: 3px;" class="style1">
                    Phản hồi:
                </td>
                <td style="width: auto;" class="input_textbox">
                    <asp:TextBox ID="TxtFeedback" runat="server" TextMode="MultiLine" Height="230px" Style="width: 100%; font-family:Arial; padding:3px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 203px; margin: 0px auto 0px auto; padding: 10px 0px 10px 0px">
        <asp:ImageButton ID="BtnConfirm" runat="server" OnClick="BtnConfirm_Click"
            ImageUrl="~/Styles/Images/button_confirm.png" />&nbsp;&nbsp;
        <asp:ImageButton ID="BtnClose" runat="server" OnClick="BtnClose_Click"
            ImageUrl="~/Styles/Images/button_close.png" />
    </div>
</asp:Content>
