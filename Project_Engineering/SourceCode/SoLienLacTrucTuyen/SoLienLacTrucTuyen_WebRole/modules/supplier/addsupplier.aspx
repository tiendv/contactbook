<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="addsupplier.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.AddSchoolPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <table style="width: 100%; padding: 10px 0px 10px 0px" class="loginBox ui-corner-all">
        <tr>
            <td style="width: 90px; height: 27px; padding-left: 10px">
                Tên trường:
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="TxtSchoolName" runat="server" Width="98%"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="SchoolNameRequired" runat="server" Display="Dynamic"
                    ValidationGroup="AddSchool" ForeColor="Red" ControlToValidate="TxtSchoolName"
                    ErrorMessage="Tên trường không được để trống" />
                <asp:CustomValidator ID="SchoolNameCustomValidator" runat="server" ErrorMessage="Đã tồn tại tên trường này ở tỉnh/thành, huyện/quận đã chọn"
                    ValidationGroup="AddSchool" Display="Dynamic" ControlToValidate="TxtSchoolName"
                    ForeColor="Red">
                </asp:CustomValidator>
            </td>
            <td style="width: 20px; padding-left: 20px;">
                Logo:
            </td>
            <td rowspan="3" style="width: 150px; height: 27px; padding: 0px 10px 0px 10px;">
                <asp:Image ID="ImgLogo" runat="server" Width="100px" Height="100px" ImageUrl="~/Styles/Images/noImageAvailable.jpg" />
                <br />
                <asp:FileUpload ID="FileUploadLogo" runat="server" />
                <asp:RegularExpressionValidator ID="FileUpLoadValidator" runat="server" ErrorMessage="Định dạng file không hợp lệ"
                    ForeColor="Red" ValidationExpression="[a-zA-Z0_9].*\b(.jpeg|.JPEG|.jpg|.JPG|.jpe|.JPE|.png|.PNG|.tiff|.TIFF|.gif|.GIF)\b"
                    ControlToValidate="FileUploadLogo" ValidationGroup="AddSchool">
                </asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td style="height: 27px; padding-left: 10px">
                Tỉnh/Thành:
            </td>
            <td>
                <asp:DropDownList ID="DdlProvinces" runat="server" Width="200px" AutoPostBack="true"
                    OnSelectedIndexChanged="DdlProvinces_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td style="height: 27px; padding-left: 50px">
                Huyện/Quận:
                <asp:DropDownList ID="DdlDistricts" runat="server" Width="170px">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="DistrictRequired" runat="server" ControlToValidate="DdlDistricts"
                    ValidationGroup="AddSchool" ErrorMessage="Huyện/quận không được để trống" Display="Dynamic"
                    ForeColor="Red" />
            </td>
        </tr>
        <tr>
            <td style="height: 27px; padding-left: 10px">
                Địa chỉ:
                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="TxtAddress" runat="server" Width="98%" TextMode="MultiLine" Font-Names="Arial"></asp:TextBox>
                <asp:RequiredFieldValidator ID="AddressRequired" runat="server" ControlToValidate="TxtAddress"
                    ValidationGroup="AddSchool" ErrorMessage="Địa chỉ không được để trống" Display="Dynamic"
                    ForeColor="Red" />
            </td>
        </tr>
        <tr>
            <td style="height: 27px; padding-left: 10px">
                Điện thoại:
                <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TxtPhone" runat="server" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="PhoneRequired" runat="server" ControlToValidate="TxtPhone"
                    ValidationGroup="AddSchool" ErrorMessage="Điện thoại không được để trống" Display="Dynamic"
                    ForeColor="Red" />
            </td>
            <td style="height: 27px; padding-left: 50px">
                Email:
                <asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:TextBox ID="TxtEmail" runat="server" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="TxtEmail"
                    ValidationGroup="AddSchool" ErrorMessage="Email không được để trống" Display="Dynamic"
                    ForeColor="Red" />
                <asp:CustomValidator ID="EmailCustomValidator" runat="server" ErrorMessage="Email không tồn tại"
                    ValidationGroup="AddSchool" Display="Dynamic" ControlToValidate="TxtEmail"
                    ForeColor="Red">
                </asp:CustomValidator>
            </td>
        </tr>
    </table>
    <div style="padding: 5px 7px 5px 7px;">
        <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
        :Thông tin bắt buộc nhập<br />
        <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
        Thêm tiếp sau khi lưu
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
        <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/Styles/Images/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="AddSchool" CssClass="SaveButton" />&nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
