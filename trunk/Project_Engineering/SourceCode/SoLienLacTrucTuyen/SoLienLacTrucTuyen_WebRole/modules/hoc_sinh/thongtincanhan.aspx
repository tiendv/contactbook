<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="thongtincanhan.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.thongtincanhan" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
    </div>
    <div>
        <asp:Label ID="Label1" runat="server" Text="THÔNG TIN CÁ NHÂN" CssClass="tabHeader"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="HlkKetQuaHocTap" runat="server" CssClass="tabHeader">KẾT QUẢ HỌC TẬP</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="HlkNgayNghiHoc" runat="server" CssClass="tabHeader">NGÀY NGHỈ HỌC</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="HlkHoatDong" runat="server" CssClass="tabHeader">HOẠT ĐỘNG</asp:HyperLink>
        <asp:ImageButton ID="BtnSua" runat="server" ImageUrl="~/Styles/Images/button_edit_with_text.png"
            OnClick="BtnSua_Click" />
        <table style="width: 100%; background-color: #E6F7F6; border: 2px solid #9AA8F2;
            padding: 10px 20px 10px 20px" class="ui-corner-all">
            <tr>
                <td style="width: 110px; height: 23px">
                    Mã học sinh:
                </td>
                <td style="width: 300px" class="readOnlyTextBox">
                    <asp:Label ID="LblMaHocSinhHienThi" runat="server"></asp:Label>
                </td>
                <td style="width: 80px; padding-left: 50px">
                    Năm học:
                </td>
                <td style="width: 150px;">
                    <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px" AutoPostBack="true"
                        OnSelectedIndexChanged="DdlNamHoc_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="vertical-align: top; float: right" rowspan="6">
                    <asp:Image ID="ImgHinhAnh" runat="server" Width="90px" Height="120px" ImageUrl="~/Styles/Images/avatar.png"
                        AlternateText="Hình ảnh" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black" />
                    <br />
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Họ tên học sinh:
                </td>
                <td class="readOnlyTextBox">
                    <asp:Label ID="LblHoTenHocSinh" runat="server"></asp:Label>
                </td>
                <td style="width: 80px; padding-left: 50px">
                    Lớp:
                </td>
                <td style="width: 130px;" class="readOnlyTextBox">
                    <asp:Label ID="LblLopHoc" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Giới tính:
                </td>
                <td style="width: 100px" class="readOnlyTextBox">
                    <asp:Label ID="LblGioiTinh" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Ngày sinh:
                </td>
                <td style="width: 300px" class="readOnlyTextBox">
                    <asp:Label ID="LblNgaySinhHocSinh" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Nơi sinh:
                </td>
                <td style="width: 300px" class="readOnlyTextBox">
                    <asp:Label ID="LblNoiSinh" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Địa chỉ:
                </td>
                <td style="width: 300px" class="readOnlyTextBox">
                    <asp:Label ID="LblDiaChi" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Điện thoại:
                </td>
                <td style="width: 300px" class="readOnlyTextBox">
                    <asp:Label ID="LblDienThoai" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table style="width: 100%; background-color: #EAFCE4; border: 2px solid #9AA8F2;
            padding: 10px 20px 10px 20px; margin-top: 10px" class="ui-corner-all">
            <tr>
                <td style="width: 130px; height: 23px">
                    Họ tên bố:
                </td>
                <td style="width: 280px;" class="readOnlyTextBox">
                    <asp:Label ID="LblHoTenBo" runat="server"></asp:Label>
                </td>
                <td style="width: 80px; padding-left: 50px">
                    Ngày sinh:
                </td>
                <td style="width: 130px;" class="readOnlyTextBox">
                    <asp:Label ID="LblNgaySinhBo" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Nghề nghiệp:
                </td>
                <td style="width: auto;" class="readOnlyTextBox input_textbox">
                    <asp:Label ID="LblNgheNghiepBo" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table style="width: 100%; background-color: #EAFCE4; border: 2px solid #9AA8F2;
            padding: 10px 20px 10px 20px; margin-top: 10px" class="ui-corner-all">
            <tr>
                <td style="width: 130px; height: 23px">
                    Họ tên mẹ:
                </td>
                <td style="width: 280px;" class="readOnlyTextBox">
                    <asp:Label ID="LblHoTenMe" runat="server"></asp:Label>
                </td>
                <td style="width: 80px; padding-left: 50px">
                    Ngày sinh:
                </td>
                <td style="width: 130px;" class="readOnlyTextBox">
                    <asp:Label ID="LblNgaySinhMe" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Nghề nghiệp:
                </td>
                <td style="width: auto;" class="readOnlyTextBox input_textbox">
                    <asp:Label ID="LblNgheNghiepMe" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <table style="width: 100%; background-color: #EAFCE4; border: 2px solid #9AA8F2;
            padding: 10px 20px 10px 20px; margin-top: 10px" class="ui-corner-all">
            <tr>
                <td style="width: 130px; height: 23px">
                    Họ tên người đỡ đầu:
                </td>
                <td style="width: 280px;" class="readOnlyTextBox">
                    <asp:Label ID="LblHoTenNguoiDoDau" runat="server"></asp:Label>
                </td>
                <td style="width: 80px; padding-left: 50px">
                    Ngày sinh:
                </td>
                <td style="width: 130px;" class="readOnlyTextBox">
                    <asp:Label ID="LblNgaySinhNguoiDoDau" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Nghề nghiệp:
                </td>
                <td style="width: auto;" class="readOnlyTextBox input_textbox">
                    <asp:Label ID="LblNgheNghiepNguoiDoDau" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="padding: 5px; vertical-align: middle;">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
</asp:Content>
