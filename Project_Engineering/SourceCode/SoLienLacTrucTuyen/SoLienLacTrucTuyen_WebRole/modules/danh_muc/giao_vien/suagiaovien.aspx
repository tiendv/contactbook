<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="suagiaovien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.suagiaovien" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            $(document).ready(function () {
                $(".testSubject").datepicker({});
            });
        </script>
    </div>
    <div style="width: 600px; padding: 10px;" class="loginBox ui-corner-all">
        <table style="width: 400px; float: left">
            <tr>
                <td style="width: 90px; height: 27px;">
                    Mã giáo viên:
                </td>
                <td>
                    <asp:Label ID="LblUserIdHienThi" runat="server" Width="293px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 27px;">
                    Họ tên:
                    <asp:Label ID="Label13" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtTenGiaoVien" runat="server" CssClass="input_textbox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="TenGiaoVienRequired" runat="server" ControlToValidate="TxtTenGiaoVien"
                        ValidationGroup="EditGiaoVien" ErrorMessage="Tên giáo viên không được để trống"
                        Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="height: 27px;">
                    Ngày sinh:
                    <asp:Label ID="Label17" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtNgaySinh" runat="server"></asp:TextBox>
                    <asp:Image ID="BtnCalendarSua" runat="server" ImageUrl="~/Styles/Images/calendar.png" />
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtNgaySinh"
                        PopupButtonID="BtnCalendarSua" PopupPosition="Right">
                    </ajaxToolkit:CalendarExtender>
                    <br />
                    <asp:RequiredFieldValidator ID="NgaySinhRequired" runat="server" ControlToValidate="TxtNgaySinh"
                        ValidationGroup="EditGiaoVien" ErrorMessage="Ngày sinh không được để trống" Display="Dynamic"
                        ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="DateOfBirthCustomValidator" runat="server" ControlToValidate="TxtNgaySinh"
                        ValidationGroup="EditGiaoVien" ErrorMessage="Ngày sinh không hợp lệ" Display="Dynamic" ForeColor="Red"
                        OnServerValidate="DateOfBirthCustomValidator_ServerValidate">
                    </asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td style="height: 27px;">
                    Giới tính:
                    <asp:Label ID="Label36" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    Nam
                    <asp:RadioButton ID="RbtnNam" runat="server" GroupName="GioiTinh" />&nbsp;&nbsp;
                    Nữ
                    <asp:RadioButton ID="RbtnNu" runat="server" Checked="true" GroupName="GioiTinh" />
                </td>
            </tr>
            <tr>
                <td style="height: 27px;">
                    Địa chỉ:
                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtDiaChi" runat="server" CssClass="input_textbox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="DiaChiRequired" runat="server" ControlToValidate="TxtDiaChi"
                        ValidationGroup="EditGiaoVien" ErrorMessage="Địa chỉ không được để trống" Display="Dynamic"
                        ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="height: 27px;">
                    Điện thoại:
                </td>
                <td style="width: auto;">
                    <asp:TextBox ID="TxtDienThoai" runat="server" CssClass="input_textbox"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                        TargetControlID="TxtDienThoai" FilterType="Numbers">
                    </ajaxToolkit:FilteredTextBoxExtender>
                </td>
            </tr>
        </table>
        <div style="width: 100px; text-align: center; vertical-align: top; float: right">
            <asp:Image ID="ImgHinhAnh" runat="server" Width="90px" Height="120px" ImageUrl="~/Styles/Images/avatar.png"
                AlternateText="Hình ảnh" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black" />
            <asp:ImageButton ID="BtnDuyetAnhSua" runat="server" Style="padding-top: 5px" ImageUrl="~/Styles/buttons/button_browse.png" />
        </div>
        <div style="clear: both">
        </div>
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px; clear: both">
        <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="EditGiaoVien" CssClass="SaveButton" />&nbsp;&nbsp;&nbsp;
        <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
