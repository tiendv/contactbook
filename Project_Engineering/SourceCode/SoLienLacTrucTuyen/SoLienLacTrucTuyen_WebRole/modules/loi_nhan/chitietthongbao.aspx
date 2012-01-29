<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="chitietthongbao.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DetailMessage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div style="float: right; padding: 5px 0px 20px 0px">
        <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Styles/buttons/button_edit.png"
            OnClick="BtnEdit_Click" CssClass="BtnEditWithouDisable" />
    </div>
    <asp:Label ID="LblStudentInformation" runat="server" Font-Bold="true" ForeColor="Blue"
        Font-Size="15px"></asp:Label>
    <table style="width: 100%; padding: 10px 20px 10px 20px; clear: both" class="loginBox ui-corner-all">
        <tr>
            <td style="vertical-align: top; padding-top: 3px;">
                Tiêu đề:
            </td>
            <td>
                <asp:Label ID="LblTitle" runat="server" Style="font-size: 14px; font-weight: bold"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 50px; vertical-align: top; padding-top: 3px;">
            </td>
            <td style="float: right">
                <div style="padding: 5px 30px 5px 0px">
                    Thời gian thông báo:
                    <asp:Label ID="LblDate" runat="server"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 70px; vertical-align: top; padding-top: 3px;">
                Nội dung:
            </td>
            <td style="border-style: none">
                <div style="width: 99%; background-color: White" class="ui-corner-all comment_border">
                    <asp:Label ID="LblContent" runat="server" Height="250px" Style="width: 99%; height: 180px"></asp:Label>
                </div>
            </td>
        </tr>
        <tr id="trFeedback" runat="server">
            <td style="vertical-align: top; padding-top: 18px;">
                Phản hồi:
            </td>
            <td style="border-style: none">
                <div style="height: 210px; width: 100%; position: relative">
                    <div style="width: 99%; position: absolute; top: 14px; background-color: White" class="ui-corner-all comment_border">
                        <asp:Label ID="LblFeedback" runat="server" Height="250px" Style="width: 99%; height: 180px"></asp:Label>
                    </div>
                    <div style="position: absolute; top: 0px; left: 100px">
                        <img src="../../Styles/Images/comment_indicator.png" alt="CommentIndicator" style="width: 16px;
                            height: 18px;" />
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="padding: 5px; vertical-align: middle; clear: both">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/buttons/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/buttons/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" Style="margin: 10px 0px 0px 0px" />
    </div>
</asp:Content>
