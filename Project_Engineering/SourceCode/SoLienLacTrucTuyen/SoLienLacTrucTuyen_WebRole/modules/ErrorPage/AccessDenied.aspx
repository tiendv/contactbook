﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="AccessDenied.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.AccessDenied" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div class="loginBox ui-corner-all" style="padding: 20px; margin: 0px auto 0px auto">
        Bạn không có quyền truy cập chức năng này!<br />
        Chuyến đến &nbsp;
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/modules/Trang_Chu/TrangChu.aspx">Trang chủ</asp:HyperLink>
    </div>
</asp:Content>
