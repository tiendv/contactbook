<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="chitietthongbao.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.DetailMessage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div style="padding: 5px 7px 10px 7px;">
        <p style="padding: 5px; font-size: 16px; font-weight: bold; text-align: center">
            <asp:Label ID="LblTitle" runat="server" Style="width: 99%; font-weight: bold"></asp:Label>
        </p>
        <p class="ui-corner-all readOnlyTextBox text_content" style="padding: 5px">
            <asp:Label ID="LblContent" runat="server" Height="250px" Style="width: 99%; height: 250px"></asp:Label>
        </p>
        <div style="height: 250px; width: 100%; position: relative">
            <div style="width: 99%; position: absolute; top: 14px;" class="ui-corner-all comment_border">
                <asp:TextBox ID="TxtFeedback" runat="server" TextMode="MultiLine" Height="230px"
                    Style="width: 99%; font-family: Arial; padding: 3px; border-style: none"></asp:TextBox>
                <ajaxToolkit:TextBoxWatermarkExtender ID="FeedbackWatermark" runat="server" TargetControlID="TxtFeedback"
                    WatermarkText="Nhập xác nhận thông báo ....">
                </ajaxToolkit:TextBoxWatermarkExtender>
            </div>
            <div style="position: absolute; top: 0px">
                <img src="../../Styles/Images/comment_indicator.png" alt="CommentIndicator" style="width: 16px;
                    height: 18px; margin: 0px 0px 0px 100px" />
            </div>
        </div>
    </div>
    <asp:Panel ID="PnlButtons" runat="server" Width="203px" Style="margin: 0px auto 0px auto;
        padding: 10px 0px 10px 0px">
        <asp:ImageButton ID="BtnConfirm" runat="server" OnClick="BtnConfirm_Click" ImageUrl="~/Styles/buttons/button_confirm.png" CssClass="BtnConfirm" />&nbsp;&nbsp;
        <asp:ImageButton ID="BtnClose" runat="server" OnClick="BtnClose_Click" ImageUrl="~/Styles/buttons/button_close.png" CssClass="BtnClose" />
    </asp:Panel>
</asp:Content>
