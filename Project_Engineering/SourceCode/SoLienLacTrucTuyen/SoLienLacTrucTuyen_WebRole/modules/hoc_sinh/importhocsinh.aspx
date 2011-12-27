<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="importhocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ImportStudentPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="padding: 10px 0px 10px 0px">
                Năm học:&nbsp;
                <asp:Label ID="LblYear" runat="server" Width="150px" CssClass="readOnlyTextBox"></asp:Label>
                <asp:HiddenField ID="HdfYearId" runat="server" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Ngành:
                <asp:DropDownList ID="DdlNganh" runat="server" Width="150px" AutoPostBack="true"
                    OnSelectedIndexChanged="DdlNganh_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Khối:
                <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="150px" AutoPostBack="true"
                    OnSelectedIndexChanged="DdlKhoiLop_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Lớp:
                <asp:DropDownList ID="DdlLopHoc" runat="server" Width="150px">
                </asp:DropDownList>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    Download template import tại đây
    <asp:LinkButton ID="lbtDownload" runat="server" OnClick="lbtDownload_Click" Text='Tại đây'>
    </asp:LinkButton>
    <br />
    <br />
    Chọn file cần import
    <asp:FileUpload ID="FileUpload1" runat="server"></asp:FileUpload>
    <br />
    <br />
    <asp:RequiredFieldValidator ID="RequiredClass" runat="server" ErrorMessage="Chưa chọn lớp học"
        ControlToValidate="DdlLopHoc" ValidationGroup="ImportFile" Display="Dynamic"
        ForeColor="Red">
    </asp:RequiredFieldValidator>
    <asp:Label ID="lbError" runat="server" ForeColor="Red" Text="Label"></asp:Label>
    <br />
    <div style="width: 170px; margin: 0px auto 0px auto;">
        <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/Styles/Images/button_save.png"
            OnClick="BtnSave_Click" CssClass="SaveButton" ValidationGroup="ImportFile" />
        &nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
