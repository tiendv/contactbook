﻿<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
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
        <p>
            <asp:TextBox ID="TxtFeedback" runat="server" TextMode="MultiLine" Height="230px"
                Style="width: 99%; font-family: Arial; padding: 3px"></asp:TextBox>
        </p>
    </div>
    <asp:Panel ID="PnlButtons" runat="server" Width="203px" Style="margin: 0px auto 0px auto;
        padding: 10px 0px 10px 0px">
        <asp:ImageButton ID="BtnConfirm" runat="server" OnClick="BtnConfirm_Click" ImageUrl="~/Styles/Images/button_confirm.png" />&nbsp;&nbsp;
        <asp:ImageButton ID="BtnClose" runat="server" OnClick="BtnClose_Click" ImageUrl="~/Styles/Images/button_close.png" />
    </asp:Panel>
</asp:Content>
