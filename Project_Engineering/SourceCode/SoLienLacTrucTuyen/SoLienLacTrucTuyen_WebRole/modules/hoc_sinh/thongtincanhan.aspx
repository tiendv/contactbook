<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="thongtincanhan.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.StudentPersonalPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div style="padding: 10px 0px 10px 0px; font-size: 14px; font-weight: bold">
        Họ và tên:
        <asp:Label ID="LblStudentName" runat="server"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;Mã học sinh:
        <asp:Label ID="LblStudentCode" runat="server"></asp:Label>
        <br />
    </div>
    <div>
        <div>
            <asp:Repeater ID="RptStudentFunctions" runat="server" OnItemDataBound="RptStudentFunctions_ItemDataBound"
                OnItemCommand="RptStudentFunctions_ItemCommand">
                <ItemTemplate>
                    <asp:LinkButton ID="LkBtnStudentPage" runat="server" CssClass="tabHeader" CommandName="Redirect"
                        CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PhysicalPath")%>'><%#DataBinder.Eval(Container.DataItem, "PageTitle")%></asp:LinkButton>
                    <asp:HiddenField ID="HdfPhysicalPath" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "PhysicalPath")%>' />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div style="padding: 10px 0px 10px 0px; float: right">
            <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Styles/buttons/button_edit.png"
                OnClick="BtnEdit_Click" CssClass="BtnEditWithouDisable" />
        </div>
        <table style="width: 100%; background-color: #E6F7F6; border: 2px solid #9AA8F2;
            padding: 10px 20px 10px 20px; clear: both" class="ui-corner-all">
            <tr>
                <td style="width: 110px; height: 23px">
                    Mã học sinh:
                </td>
                <td style="width: 300px">
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
                    <asp:Image ID="ImgPhoto" runat="server" Width="90px" Height="120px" AlternateText="Hình ảnh học sinh"
                        BorderStyle="Solid" BorderWidth="1px" BorderColor="Black" />
                    <br />
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Họ tên học sinh:
                </td>
                <td>
                    <asp:Label ID="LblHoTenHocSinh" runat="server"></asp:Label>
                </td>
                <td style="width: 80px; padding-left: 50px">
                    Lớp:
                </td>
                <td style="width: 130px;">
                    <asp:Label ID="LblLopHoc" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Giới tính:
                </td>
                <td style="width: 100px">
                    <asp:Label ID="LblGioiTinh" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Ngày sinh:
                </td>
                <td style="width: 300px">
                    <asp:Label ID="LblNgaySinhHocSinh" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Nơi sinh:
                </td>
                <td style="width: 300px">
                    <asp:Label ID="LblNoiSinh" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Địa chỉ:
                </td>
                <td style="width: 300px">
                    <asp:Label ID="LblDiaChi" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Điện thoại:
                </td>
                <td style="width: 300px">
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
                <td style="width: 280px;">
                    <asp:Label ID="LblHoTenBo" runat="server"></asp:Label>
                </td>
                <td style="width: 80px; padding-left: 50px">
                    Ngày sinh:
                </td>
                <td style="width: 130px;">
                    <asp:Label ID="LblNgaySinhBo" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Nghề nghiệp:
                </td>
                <td style="width: auto;">
                    <asp:Label ID="LblNgheNghiepBo" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td style="width: 130px; height: 23px">
                    Họ tên mẹ:
                </td>
                <td style="width: 280px;">
                    <asp:Label ID="LblHoTenMe" runat="server"></asp:Label>
                </td>
                <td style="width: 80px; padding-left: 50px">
                    Ngày sinh:
                </td>
                <td style="width: 130px;">
                    <asp:Label ID="LblNgaySinhMe" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Nghề nghiệp:
                </td>
                <td style="width: auto;">
                    <asp:Label ID="LblNgheNghiepMe" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td style="width: 130px; height: 23px">
                    Họ tên người đỡ đầu:
                </td>
                <td style="width: 280px;">
                    <asp:Label ID="LblHoTenNguoiDoDau" runat="server"></asp:Label>
                </td>
                <td style="width: 80px; padding-left: 50px">
                    Ngày sinh:
                </td>
                <td style="width: 130px;">
                    <asp:Label ID="LblNgaySinhNguoiDoDau" runat="server"></asp:Label>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td style="height: 23px">
                    Nghề nghiệp:
                </td>
                <td style="width: auto;">
                    <asp:Label ID="LblNgheNghiepNguoiDoDau" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="padding: 5px; vertical-align: middle;">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/buttons/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/buttons/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
</asp:Content>
