﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="suahocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.EditStudentPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script type="text/javascript">
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
        </script>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="padding: 10px 0px 10px 0px">
                Năm học:
                <asp:Label ID="LblYear" runat="server" Width="150px" CssClass="readOnlyTextBox"></asp:Label>
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
                <asp:RequiredFieldValidator ID="LopHocRequired" runat="server" ControlToValidate="DdlLopHoc"
                    ErrorMessage="Lớp học không được để trống" ValidationGroup="AddHocSinh" Display="Dynamic"
                    ForeColor="Red">
                </asp:RequiredFieldValidator>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table style="width: 100%; background-color: #E6F7F6; border: 2px solid #9AA8F2;
        padding: 10px 20px 10px 20px" class="ui-corner-all">
        <tr>
            <td style="width: 110px; vertical-align: top; padding-top: 7px;">
                Mã học sinh:
                <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td>
                <asp:HiddenField ID="HdfOldStudentCode" runat="server" Value="true" />
                <asp:TextBox ID="TxtMaHocSinhHienThi" runat="server" CssClass="input_textbox"></asp:TextBox>
                <asp:RequiredFieldValidator ID="MaHocSinhRequired" runat="server" ValidationGroup="AddHocSinh"
                    Display="Dynamic" ForeColor="Red" ControlToValidate="TxtMaHocSinhHienThi" ErrorMessage="Mã học sinh không được để trống">
                </asp:RequiredFieldValidator>
                <asp:HiddenField ID="hdfOutput" runat="server" Value="true" />
                <asp:CustomValidator ID="MaHocSinhValidator" runat="server" ValidationGroup="AddHocSinh"
                    Display="Dynamic" ForeColor="Red" ControlToValidate="TxtMaHocSinhHienThi" 
                    ErrorMessage="Mã học sinh đã tồn tại">
                </asp:CustomValidator>
            </td>
            <td style="vertical-align: top;" rowspan="6">
                <div style="float: right; text-align: center;">
                    <asp:Image ID="ImgHinhAnh" runat="server" Width="90px" Height="120px" ImageUrl="~/Styles/Images/avatar.png"
                        AlternateText="Hình ảnh" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black" />
                    <br />
                    <asp:ImageButton ID="BtnDuyetHinhAnh" runat="server" Style="padding-top: 5px" ImageUrl="~/Styles/Images/button_browse.png" />
                    <ajaxToolkit:AsyncFileUpload ID="AsyncFileUpload1" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top; padding-top: 7px;">
                Họ tên học sinh:
                <asp:Label ID="Label28" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td style="width: auto;">
                <asp:TextBox ID="TxtTenHocSinh" runat="server" CssClass="input_textbox" Text="Nguyen Anh Duy"></asp:TextBox>
                <asp:RequiredFieldValidator ID="TenHocSinhRequired" runat="server" ValidationGroup="AddHocSinh"
                    Display="Dynamic" ForeColor="Red" ControlToValidate="TxtTenHocSinh" ErrorMessage="Tên học sinh không được để trống">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: middle; padding-top: 7px;">
                Giới tính:
                <asp:Label ID="Label30" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td style="width: auto;">
                Nam
                <asp:RadioButton ID="RbtnNam" runat="server" GroupName="GioiTinh" Checked="true" />&nbsp;&nbsp;
                Nữ
                <asp:RadioButton ID="RbtnNu" runat="server" GroupName="GioiTinh" />
            </td>
        </tr>
        <tr>
            <td>
                Ngày sinh:
                <asp:Label ID="Label14" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TxtNgaySinhHocSinh" runat="server"></asp:TextBox>
                <asp:Image ID="ImgCalendarNgaySinhHocSinh" runat="server" ImageUrl="~/Styles/Images/calendar.png" />
                <br />
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtNgaySinhHocSinh"
                    PopupButtonID="ImgCalendarNgaySinhHocSinh" Format="MM/dd/yyyy" PopupPosition="Right">
                </ajaxToolkit:CalendarExtender>
                <asp:RequiredFieldValidator ID="NgaySinhHocSinhRequired" runat="server" ValidationGroup="AddHocSinh"
                    Display="Dynamic" ForeColor="Red" ControlToValidate="TxtNgaySinhHocSinh" ErrorMessage="Ngày sinh học sinh không được để trống">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Nơi sinh:
            </td>
            <td style="width: 300px;">
                <asp:TextBox ID="TxtNoiSinh" runat="server" CssClass="input_textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Địa chỉ:
                <asp:Label ID="Label6" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td style="width: 300px;">
                <asp:TextBox ID="TxtDiaChi" runat="server" CssClass="input_textbox" Text="Quan 9, Tp.HCM"></asp:TextBox>
                <asp:RequiredFieldValidator ID="DiaChiRequired" runat="server" ValidationGroup="AddHocSinh"
                    Display="Dynamic" ForeColor="Red" ControlToValidate="TxtDiaChi" ErrorMessage="Địa chỉ không được để trống">
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Điện thoại:
            </td>
            <td style="width: auto;">
                <asp:TextBox ID="TxtDienThoai" runat="server" CssClass="input_textbox"></asp:TextBox>
            </td>
            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                TargetControlID="TxtDienThoai" FilterType="Numbers">
            </ajaxToolkit:FilteredTextBoxExtender>
        </tr>
    </table>
    <br />
    <br />
    <asp:Label ID="Label9" runat="server" Text="Thông tin phụ huynh" ForeColor="#591DB3"
        Font-Size="14px" Font-Bold="true">
    </asp:Label>
    <br />
    <br />
    <asp:Label ID="Label23" runat="server" Text="Lưu ý" Font-Bold="true" Font-Italic="true"
        Font-Underline="true"></asp:Label>
    <asp:Label ID="Label22" runat="server" Text=": Ít nhất thông tin họ tên bố hoặc họ tên mẹ hoặc họ tên người đỡ đầu phải được cung cấp"></asp:Label>
    <br />
    <asp:Label ID="LblErrorPhuHuynh" runat="server" Text="Chưa nhập thông tin phụ huynh"
        ForeColor="Red" Style="display: none"></asp:Label>
    <table style="width: 100%; background-color: #EAFCE4; border: 2px solid #9AA8F2;
        padding: 10px 20px 10px 20px; margin-top: 10px" class="ui-corner-all">
        <tr>
            <td style="width: 125px; vertical-align: top; padding-top: 3px;">
                Họ tên bố:
            </td>
            <td style="width: 300px;">
                <asp:TextBox ID="TxtHoTenBo" runat="server" CssClass="input_textbox" Text="Nguyen Van A"></asp:TextBox>
            </td>
            <td style="width: 80px; padding-left: 50px">
                Ngày sinh:
            </td>
            <td>
                <asp:TextBox ID="TxtNgaySinhBo" runat="server"></asp:TextBox>
                <asp:Image ID="ImgCalendarNgaySinhBo" runat="server" ImageUrl="~/Styles/Images/calendar.png" />
                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtNgaySinhBo"
                    PopupButtonID="ImgCalendarNgaySinhBo" Format="dd/MM/yyyy" PopupPosition="Right">
                </ajaxToolkit:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top; padding-top: 3px;">
                Nghề nghiệp:
            </td>
            <td style="width: auto;">
                <asp:TextBox ID="TxtNgheNghiepBo" runat="server" CssClass="input_textbox"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table style="width: 100%; background-color: #EAFCE4; border: 2px solid #9AA8F2;
        padding: 10px 20px 10px 20px; margin-top: 10px" class="ui-corner-all">
        <tr>
            <td style="width: 125px; vertical-align: top; padding-top: 3px;">
                Họ tên mẹ:
            </td>
            <td style="width: 300px;">
                <asp:TextBox ID="TxtHoTenMe" runat="server" CssClass="input_textbox"></asp:TextBox>
            </td>
            <td style="width: 80px; padding-left: 50px">
                Ngày sinh:
            </td>
            <td>
                <asp:TextBox ID="TxtNgaySinhMe" runat="server"></asp:TextBox>
                <asp:Image ID="ImgCalendarNgaySinhMe" runat="server" ImageUrl="~/Styles/Images/calendar.png" />
                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="TxtNgaySinhMe"
                    PopupButtonID="ImgCalendarNgaySinhMe" Format="dd/MM/yyyy" PopupPosition="Right">
                </ajaxToolkit:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top; padding-top: 3px;">
                Nghề nghiệp:
            </td>
            <td style="width: auto;">
                <asp:TextBox ID="TxtNgheNghiepMe" runat="server" CssClass="input_textbox"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table style="width: 100%; background-color: #EAFCE4; border: 2px solid #9AA8F2;
        padding: 10px 20px 10px 20px; margin-top: 10px" class="ui-corner-all">
        <tr>
            <td style="width: 125px; vertical-align: top; padding-top: 3px;">
                Họ tên người đỡ đầu:
            </td>
            <td style="width: 300px;">
                <asp:TextBox ID="TxtHoTenNguoiDoDau" runat="server" CssClass="input_textbox"></asp:TextBox>
            </td>
            <td style="width: 80px; padding-left: 50px">
                Ngày sinh:
            </td>
            <td>
                <asp:TextBox ID="TxtNgaySinhNguoiDoDau" runat="server"></asp:TextBox>
                <asp:Image ID="ImgCalendarNgaySinhNguoiDoDau" runat="server" ImageUrl="~/Styles/Images/calendar.png" />
                <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="TxtNgaySinhNguoiDoDau"
                    PopupButtonID="ImgCalendarNgaySinhNguoiDoDau" Format="dd/MM/yyyy" PopupPosition="Right">
                </ajaxToolkit:CalendarExtender>
                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" runat="server" TargetControlID="TxtNgaySinhNguoiDoDau"
                    MaskType="Date" Mask="99/99/9999" UserDateFormat="DayMonthYear" ErrorTooltipEnabled="true">
                </ajaxToolkit:MaskedEditExtender>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top; padding-top: 3px;">
                Nghề nghiệp:
            </td>
            <td style="width: auto;">
                <asp:TextBox ID="TxtNgheNghiepNguoiDoDau" runat="server" CssClass="input_textbox"></asp:TextBox>
            </td>
        </tr>
    </table>
    <br />
    <div style="padding-top: 5px">
        <asp:Label ID="Label15" runat="server" Text="*" ForeColor="Red"></asp:Label>
        :Thông tin bắt buộc nhập
        <br />
        <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
        <asp:Label ID="Label100" runat="server" Text="Thêm tiếp sau khi lưu"></asp:Label>
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto;">
        <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/Styles/Images/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="AddHocSinh" CssClass="SaveButton" />
        &nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
