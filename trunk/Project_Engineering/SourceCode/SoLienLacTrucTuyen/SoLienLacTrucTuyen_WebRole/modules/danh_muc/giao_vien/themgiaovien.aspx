<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="themgiaovien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.themgiaovien" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function validateMaGiaoVien(ctrl, args) {
                var hfOutput = $get('<%=hfOutput.ClientID%>');
                var maGiaoVien = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/modules/danh_muc/giao_vien/GiaoVienServicePage.aspx/MaGiaoVienExists",
                    data: "{'maGiaoVienHienThi':'" + maGiaoVien + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            $get('<%=hfOutput.ClientID%>').value = 'false';
                        } else {
                            $get('<%=hfOutput.ClientID%>').value = 'true';
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert('Error');
                        hfOutput.value = 'false';
                    }
                });

                if ($get('<%=hfOutput.ClientID%>').value == 'true') {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            }
        </script>
    </div>
    <div style="width: 600px; padding: 10px;" class="loginBox ui-corner-all">
        <table style="width: 400px; float: left">
            <tr>
                <td style="width: 90px; height: 27px;">
                    Mã giáo viên:
                    <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtMaGiaoVienHienThi" runat="server" CssClass="input_textbox"></asp:TextBox>
                    <asp:HiddenField ID="hfOutput" runat="server" Value="true" />
                    <asp:RequiredFieldValidator ID="MaGiaoVienHienThiRequired" runat="server" ControlToValidate="TxtMaGiaoVienHienThi"
                        ValidationGroup="AddGiaoVien" ErrorMessage="Mã giáo viên không được để trống"
                        Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="MaGiaoVienValidator" runat="server" ControlToValidate="TxtMaGiaoVienHienThi"
                        ValidationGroup="AddGiaoVien" ClientValidationFunction="validateMaGiaoVien" ErrorMessage="Mã giáo viên đã tồn tại"
                        Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td style="height: 27px;">
                    <asp:Label ID="Label12" runat="server" Text="Họ tên:"></asp:Label>
                    <asp:Label ID="Label13" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="TxtTenGiaoVien" runat="server" CssClass="input_textbox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="TenGiaoVienRequired" runat="server" ControlToValidate="TxtTenGiaoVien"
                        ValidationGroup="AddGiaoVien" ErrorMessage="Tên giáo viên không được để trống"
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
                        PopupButtonID="BtnCalendarSua" Format="dd/MM/yyyy" PopupPosition="Right">
                    </ajaxToolkit:CalendarExtender>
                    <asp:RequiredFieldValidator ID="NgaySinhRequired" runat="server" ControlToValidate="TxtNgaySinh"
                        ValidationGroup="AddGiaoVien" ErrorMessage="Ngày sinh không được để trống" Display="Dynamic"
                        ForeColor="Red"></asp:RequiredFieldValidator>
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
                        ValidationGroup="AddGiaoVien" ErrorMessage="Địa chỉ không được để trống" Display="Dynamic"
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
            <asp:ImageButton ID="BtnDuyetAnhSua" runat="server" Style="padding-top: 5px" ImageUrl="~/Styles/Images/button_browse.png" />
        </div>
        <div style="clear: both">
        </div>
    </div>
    <div style="width: 600px; padding-left: 10px; margin: 0px auto 0px auto">
        <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
        :Thông tin bắt buộc nhập<br />
        <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
        Thêm tiếp sau khi lưu
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px; clear: both">
        <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/Images/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="AddGiaoVien" CssClass="SaveButton" />&nbsp;&nbsp;&nbsp;
        <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
