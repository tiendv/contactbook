<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="themnamhoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.CategoryAddYear" Culture="vi-VN" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <table style="width: 100%;" class="inputBorder">
        <tr>
            <td style="width: 70px; height: 27px; vertical-align: top; padding-top: 3px;">
                Năm học:
            </td>
            <td style="width: 200px; height: 20px">
                <asp:Label ID="LblYearName" runat="server"></asp:Label>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="height: 27px;">
                Học kì 1:
            </td>
            <td style="width: 300px">
                Thời gian bắt đầu:
                <asp:TextBox ID="TxtFirstTermBeginDate" runat="server" Width="125px"></asp:TextBox>
                <asp:Image ID="ImgFirstTermBeginDate" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                    ImageAlign="Top" />
                <ajaxToolkit:CalendarExtender ID="CalendaFirstTermBeginDate" runat="server" TargetControlID="TxtFirstTermBeginDate"
                    PopupButtonID="ImgFirstTermBeginDate" PopupPosition="Right" Format="dd/MM/yyyy">
                </ajaxToolkit:CalendarExtender>
                <br />
            </td>
            <td>
                Thời gian kết thúc:
                <asp:TextBox ID="TxtFirstTermEndDate" runat="server" Width="125px"></asp:TextBox>
                <asp:Image ID="ImgFirstTermEndDate" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                    ImageAlign="Top" />
                <ajaxToolkit:CalendarExtender ID="CalendarFirstTermEndDate" runat="server" TargetControlID="TxtFirstTermEndDate"
                    PopupButtonID="ImgFirstTermEndDate" PopupPosition="Right" Format="dd/MM/yyyy">
                </ajaxToolkit:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFirstTermBeginDate" runat="server" ControlToValidate="TxtFirstTermBeginDate"
                    ErrorMessage="Thời gian bắt đầu HK 1 không được để trống" Display="Dynamic" ValidationGroup="AddYear"
                    ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFirstTermEndDate" runat="server" ControlToValidate="TxtFirstTermEndDate"
                    ErrorMessage="Thời gian kết thúc HK 1 không được để trống" Display="Dynamic"
                    ValidationGroup="AddYear" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareToFirstTermStartDate" runat="server" ControlToValidate="TxtFirstTermEndDate"
                    ControlToCompare="TxtFirstTermBeginDate" Type="Date" Operator="GreaterThan" Display="Dynamic"
                    ValidationGroup="AddYear" ForeColor="Red" ErrorMessage="Thời gian phải lớn hơn thời gian bắt đầu HK 1"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td style="height: 27px;">
                Học kì 2:
            </td>
            <td style="width: 300px">
                Thời gian bắt đầu:
                <asp:TextBox ID="TxtSecondTermBeginDate" runat="server" Width="125px"></asp:TextBox>
                <asp:Image ID="ImgSecondTermBeginDate" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                    ImageAlign="Top" />
                <ajaxToolkit:CalendarExtender ID="CalendaSecondTermBeginDate" runat="server" TargetControlID="TxtSecondTermBeginDate"
                    PopupButtonID="ImgSecondTermBeginDate" PopupPosition="Right" Format="dd/MM/yyyy">
                </ajaxToolkit:CalendarExtender>
            </td>
            <td>
                Thời gian kết thúc:
                <asp:TextBox ID="TxtSecondTermEndDate" runat="server" Width="125px"></asp:TextBox>
                <asp:Image ID="ImgSecondTermEndDate" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                    ImageAlign="Top" />
                <ajaxToolkit:CalendarExtender ID="CalendarSecondTermEndDate" runat="server" TargetControlID="TxtSecondTermEndDate"
                    PopupButtonID="ImgSecondTermEndDate" PopupPosition="Right" Format="dd/MM/yyyy">
                </ajaxToolkit:CalendarExtender>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredSecondTermBeginDate" runat="server" ControlToValidate="TxtSecondTermBeginDate"
                    ErrorMessage="Thời gian đầu HK 2 không được để trống" Display="Dynamic" ValidationGroup="AddYear"
                    ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareToFirstTermEndDate" runat="server" ControlToValidate="TxtSecondTermBeginDate"
                    ControlToCompare="TxtFirstTermEndDate" Type="Date" Operator="GreaterThan" Display="Dynamic"
                    ValidationGroup="AddYear" ForeColor="Red" ErrorMessage="Thời gian phải lớn hơn thời gian kết thúc HK 1"></asp:CompareValidator>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredSecondTermEndDate" runat="server" ControlToValidate="TxtSecondTermEndDate"
                    ErrorMessage="Thời gian kết thúc HK 2 không được để trống" Display="Dynamic"
                    ValidationGroup="AddYear" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareToSecondTermStartDate" runat="server" ControlToValidate="TxtSecondTermEndDate"
                    ControlToCompare="TxtSecondTermBeginDate" Operator="GreaterThan" Type="Date"
                    Display="Dynamic" ValidationGroup="AddYear" ForeColor="Red" ErrorMessage="Thời gian phải lớn hơn thời gian bắt đầu HK 2"></asp:CompareValidator>
            </td>
        </tr>
    </table>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
        <asp:ImageButton ID="BtnSave" runat="server" OnClick="BtnSave_Click" ValidationGroup="AddYear"
            ImageUrl="~/Styles/buttons/button_save.png" CssClass="SaveButton" />&nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" OnClick="BtnCancel_Click" ImageUrl="~/Styles/buttons/button_cancel.png"
            CssClass="CancelButton" />
    </div>
</asp:Content>
