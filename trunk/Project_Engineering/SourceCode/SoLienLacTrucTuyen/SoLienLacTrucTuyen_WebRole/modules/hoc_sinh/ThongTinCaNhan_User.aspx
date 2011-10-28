<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true" CodeBehind="ThongTinCaNhan_User.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ThongTinCaNhan_User" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
<div>
    <script type="text/javascript">
        $(document).ready(function () {
            });            
    </script>

    <script language="javascript" type="text/javascript">
        function pageLoad() {         
        }
    </script>
</div>

<table>
    <tr>
        <td>
            <table class="search">
                <tr>
                    <td style="width:60px;">
                        <asp:Label ID="Label2" runat="server" Text="Năm học:"></asp:Label>
                    </td>
                    <td style="width:200px;">
                        <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>                            
                </tr>
            </table>
        </td>
        <td style="width:80px">
            <asp:ImageButton ID="BtnSearch" runat="server" 
                ImageUrl="~/Styles/Images/button_search_with_text.png" ToolTip="Tìm kiếm lớp học"
                onclick="BtnSearch_Click"/>
        </td>
    </tr>
</table>

<asp:HiddenField ID="HdfMaHocSinh" runat="server" />   
    <table style="width:100%; background-color: #E6F7F6; border:2px solid #9AA8F2; padding:10px 20px 10px 20px">                
        <tr>
            <td style="width:110px;">
                <asp:Label ID="Label1" runat="server" Text="Mã học sinh:"></asp:Label>
            </td>
            <td style="width:300px" class="readOnlyTextBox">
                <asp:Label ID="LblMaHocSinhHienThi" runat="server"></asp:Label>
            </td>
            <td style="width:80px; padding-left:50px">
                <asp:Label ID="Label4" runat="server" Text="Ngành:"></asp:Label>
            </td>
            <td style="width:130px" class="readOnlyTextBox">
                <asp:Label ID="LblNganhHoc" runat="server"></asp:Label>
            </td>
            <td style="vertical-align:top; float:right" rowspan="6">            
                <asp:Image ID="ImgHinhAnh" runat="server" Width="120px" Height="160px" ImageUrl="~/Styles/Images/avatar.png"
                    AlternateText="Hình ảnh" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black"/>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label25" runat="server" Text="Họ tên học sinh:"></asp:Label>
            </td>
            <td class="readOnlyTextBox">
                <asp:Label ID="LblHoTenHocSinh" runat="server"></asp:Label>
            </td>                                        
            <td style="width:80px; padding-left:50px">
                <asp:Label ID="Label3" runat="server" Text="Khối:"></asp:Label>
            </td>
            <td style="width:130px" class="readOnlyTextBox">
                <asp:Label ID="LblKhoiLop" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="vertical-align:middle;">
                <asp:Label ID="Label29" runat="server" Text="Giới tính:"></asp:Label>&nbsp;
            </td>
            <td style="width:100px" class="readOnlyTextBox">             
                <asp:Label ID="LblGioiTinh" runat="server"></asp:Label>
            </td>            
            <td style="width:80px; padding-left:50px">
                <asp:Label ID="Label14" runat="server" Text="Lớp:"></asp:Label>
            </td>
            <td style="width:130px" class="readOnlyTextBox">
                <asp:Label ID="LblLopHoc" runat="server"></asp:Label>
            </td>                            
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label8" runat="server" Text="Ngày sinh:"></asp:Label>
            </td>
            <td style="width:300px" class="readOnlyTextBox">
                <asp:Label ID="LblNgaySinhHocSinh" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="Nơi sinh:"></asp:Label>
            </td>
            <td style="width:300px" class="readOnlyTextBox">
                <asp:Label ID="LblNoiSinh" runat="server"></asp:Label>
            </td>                                        
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label33" runat="server" Text="Địa chỉ:"></asp:Label>
            </td>
            <td style="width:300px" class="readOnlyTextBox">
                <asp:Label ID="LblDiaChi" runat="server"></asp:Label>
            </td>                                        
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label34" runat="server" Text="Điện thoại:"></asp:Label>
            </td>
            <td style="width:300px" class="readOnlyTextBox">
                <asp:Label ID="LblDienThoai" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <table style="width:100%; background-color: #EAFCE4; border:2px solid #9AA8F2; padding:10px 20px 10px 20px; margin-top:10px">
        <tr>
            <td style="width:130px; vertical-align:top; padding-top:3px;">
                <asp:Label ID="Label10" runat="server" Text="Họ tên bố:"></asp:Label>
            </td>
            <td style="width:280px;" class="readOnlyTextBox">
                <asp:Label ID="LblHoTenBo" runat="server"></asp:Label>
            </td>
            <td style="width:80px; padding-left:50px">
                <asp:Label ID="Label12" runat="server" Text="Ngày sinh:"></asp:Label>
            </td>
            <td style="width:130px;" class="readOnlyTextBox">
                <asp:Label ID="LblNgaySinhBo" runat="server"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="vertical-align:top; padding-top:3px;">
                <asp:Label ID="Label13" runat="server" Text="Nghề nghiệp:"></asp:Label>
            </td>
            <td style="width:auto;" class="readOnlyTextBox input_textbox">
                <asp:Label ID="LblNgheNghiepBo" runat="server"></asp:Label>
            </td>
        </tr>    
    </table>
    <table style="width:100%; background-color: #EAFCE4; border:2px solid #9AA8F2; padding:10px 20px 10px 20px; margin-top:10px">
        <tr>
            <td style="width:130px; vertical-align:top; padding-top:3px;">
                <asp:Label ID="Label5" runat="server" Text="Họ tên mẹ:"></asp:Label>
            </td>
            <td style="width:280px;" class="readOnlyTextBox">
                <asp:Label ID="LblHoTenMe" runat="server"></asp:Label>
            </td>
            <td style="width:80px; padding-left:50px">
                <asp:Label ID="Label6" runat="server" Text="Ngày sinh:"></asp:Label>
            </td>
            <td style="width:130px;" class="readOnlyTextBox">
                <asp:Label ID="LblNgaySinhMe" runat="server"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="vertical-align:top; padding-top:3px;">
                <asp:Label ID="Label11" runat="server" Text="Nghề nghiệp:"></asp:Label>
            </td>
            <td style="width:auto;" class="readOnlyTextBox input_textbox">
                <asp:Label ID="LblNgheNghiepMe" runat="server"></asp:Label>
            </td>
        </tr>    
    </table>
    <table style="width:100%; background-color: #EAFCE4; border:2px solid #9AA8F2; padding:10px 20px 10px 20px; margin-top:10px">
        <tr>
            <td style="width:130px; vertical-align:top; padding-top:3px;">
                <asp:Label ID="Label17" runat="server" Text="Họ tên người đỡ đầu:"></asp:Label>
            </td>
            <td style="width:280px;" class="readOnlyTextBox">
                <asp:Label ID="LblHoTenNguoiDoDau" runat="server"></asp:Label>
            </td>
            <td style="width:80px; padding-left:50px">
                <asp:Label ID="Label18" runat="server" Text="Ngày sinh:"></asp:Label>
            </td>
            <td style="width:130px;" class="readOnlyTextBox">
                <asp:Label ID="LblNgaySinhNguoiDoDau" runat="server"></asp:Label>
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="vertical-align:top; padding-top:3px;">
                <asp:Label ID="Label20" runat="server" Text="Nghề nghiệp:"></asp:Label>
            </td>
            <td style="width:auto;" class="readOnlyTextBox input_textbox">
                <asp:Label ID="LblNgheNghiepNguoiDoDau" runat="server"></asp:Label>
            </td>
        </tr>    
    </table>
</asp:Content>
