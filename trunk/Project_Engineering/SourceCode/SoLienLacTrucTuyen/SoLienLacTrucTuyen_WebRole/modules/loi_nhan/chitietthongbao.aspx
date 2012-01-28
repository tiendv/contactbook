<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="chitietthongbao.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DetailMessage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div style="float: right; padding: 5px 0px 20px 0px">
        <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Styles/buttons/button_edit.png"
            OnClick="BtnEdit_Click" CssClass="BtnEditWithouDisable" />
    </div>
    <p style="padding: 5px">
        <asp:Label ID="LblStudentInformation" runat="server" Font-Bold="true" ForeColor="Blue" Font-Size="15px"></asp:Label>
    </p>
    <div style="padding: 5px 7px 10px 7px;">
        <p style="padding: 5px; font-size: 16px; font-weight: bold; text-align: center">
            <asp:Label ID="LblTitle" runat="server" Style="width: 99%; font-weight: bold"></asp:Label>
        </p>
        <p class="ui-corner-all readOnlyTextBox" style="padding: 5px">
            <asp:Label ID="LblContent" runat="server" Height="250px" Style="width: 99%; height: 250px"></asp:Label>
        </p>
        <div style="height: 250px; width: 100%; position: relative">
            <div style="width: 99%; position: absolute; top: 14px;" class="ui-corner-all comment_border">
                <asp:Label ID="LblFeedback" runat="server" Height="250px" Style="width: 99%; height: 250px"></asp:Label>
            </div>
            <div style="position: absolute; top: 0px; left: 100px">
                <img src="../../Styles/Images/comment_indicator.png" alt="CommentIndicator" style="width: 16px;
                    height: 18px;" />
            </div>
        </div>
    </div>
    <div style="padding: 5px; vertical-align: middle; clear:both">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/buttons/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
</asp:Content>
