<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="importhocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ImportStudentPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
<%--        <script type="text/javascript">
            $(document).ready(function () {
                $('#<%=BtnSave.ClientID%>').click(function (event) {
                    var hotenBo = $.trim($('#<%=TxtHoTenBo.ClientID%>').val());
                    var hotenMe = $.trim($('#<%=TxtHoTenMe.ClientID%>').val());
                    var hotenNguoiDoDau = $.trim($('#<%=TxtHoTenNguoiDoDau.ClientID%>').val());

                    if (hotenBo.length == 0 && hotenMe.length == 0
                        && hotenNguoiDoDau.length == 0) {
                        $('#<%=LblErrorPhuHuynh.ClientID%>').show();
                        event.preventDefault();
                    } else {
                        $('#<%=LblErrorPhuHuynh.ClientID%>').hide();
                    }
                });
            });        
        </script>
        <script type="text/javascript">
            function ValidateMaHocSinh(ctl, args) {
                var maHocSinhHienThi = args.Value;
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "/Modules/Hoc_Sinh/HocSinhServicePage.aspx/ExistMaHocSinhHienThi",
                    data: "{'maHocSinh':'" + 0 + "','maHocSinhHienThi':'" + maHocSinhHienThi + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            $get('<%=hdfOutput.ClientID%>').value = 'false';
                        } else {
                            $get('<%=hdfOutput.ClientID%>').value = 'true';
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(thrownError);
                        $get('<%=hdfOutput.ClientID%>').value = 'false';
                    }
                });

                if ($get('<%=hdfOutput.ClientID%>').value == 'true') {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            }
        </script>--%>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="padding: 10px 0px 10px 0px">
                Năm học:
                <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px" AutoPostBack="true"
                    OnSelectedIndexChanged="DdlNamHoc_SelectedIndexChanged">
                </asp:DropDownList>
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
                &nbsp;
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>    
    Chọn file cần import  <asp:FileUpload id="FileUpload1"                 
           runat="server">
       </asp:FileUpload>                  
    <br />
    <br />
    <br />
    <div style="width: 170px; margin: 0px auto 0px auto;">
        <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/Styles/Images/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="AddHocSinh" CssClass="SaveButton" />
        &nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
<asp:Content ID="Content4" runat="server" contentplaceholderid="head">
</asp:Content>

