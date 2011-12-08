<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="PageReportStudents.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.PageReportStudents" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer2" runat="server" AutoDataBind="true" />        
    </div>
</asp:Content>
