<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="addsupplier.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.AddSchoolPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <table style="width: 100%; padding: 10px 0px 10px 0px" class="loginBox ui-corner-all">
        <tr>
            <td style="width: 90px; height: 27px; padding-left: 10px">
                Tên trường:
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TxtSchoolName" runat="server" Width="98%"></asp:TextBox>
                <asp:RequiredFieldValidator ID="SchoolNameRequired" runat="server" Display="Dynamic"
                    ValidationGroup="AddSchool" ForeColor="Red" ControlToValidate="TxtSchoolName"
                    ErrorMessage="Tên trường không được để trống" />
            </td>
            <td rowspan="4" style="width: 90px; height: 27px; padding: 0px 10px 0px 10px; vertical-align: top">
                Logo:
                <asp:Image ID="ImgPhoto" runat="server" Width="100px" Height="100px" />
                <br />
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="height: 27px; padding-left: 10px">
                Địa chỉ:
                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TxtAddress" runat="server" Width="98%" TextMode="MultiLine"></asp:TextBox>
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
                <asp:TextBox ID="TxtPhone" runat="server" Width="98%"></asp:TextBox>
                <asp:RequiredFieldValidator ID="PhoneRequired" runat="server" ControlToValidate="TxtPhone"
                    ValidationGroup="AddSchool" ErrorMessage="Điện thoại không được để trống" Display="Dynamic"
                    ForeColor="Red" />
            </td>
        </tr>
        <tr>
            <td style="height: 27px; padding-left: 10px">
                Email:
                <asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="TxtEmail" runat="server" Width="98%"></asp:TextBox>
                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="TxtEmail"
                    ValidationGroup="AddSchool" ErrorMessage="Email không được để trống" Display="Dynamic"
                    ForeColor="Red" />
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
        <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
            OnClick="BtnSave_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
