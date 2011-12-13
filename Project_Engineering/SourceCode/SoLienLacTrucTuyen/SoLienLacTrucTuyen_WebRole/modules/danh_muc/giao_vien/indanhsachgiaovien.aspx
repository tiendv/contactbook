<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true" 
CodeBehind="indanhsachgiaovien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.PrintTeacherPage" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <CR:CrystalReportViewer ID="Rpt_DanhSachGiaoVien" runat="server" AutoDataBind="true" />
</asp:Content>
