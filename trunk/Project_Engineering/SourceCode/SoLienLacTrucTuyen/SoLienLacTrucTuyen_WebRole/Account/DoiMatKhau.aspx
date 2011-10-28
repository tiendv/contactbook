<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true" CodeBehind="DoiMatKhau.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DoiMatKhau" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">    
    <div>
    <script type="text/javascript">
        function validateNewPassword(ctrl, args) {
            var minimumLength = <%= Membership.MinRequiredPasswordLength %>;
            if(args.Value.length < minimumLength){
                args.IsValid = false;
                ctrl.errormessage = 'Mật khẩu mới phải có độ dài tối thiểu ' 
                    + <%=Membership.MinRequiredPasswordLength%> + ' kí tự';
            }
            else{
                args.IsValid = true;
            }
        }
    </script>
</div>
        
    <div class="loginBox ui-corner-all">
        <asp:ChangePassword ID="ChangeUserPassword" runat="server"  style="width:100%"
            OnChangePasswordError="ChangeUserPassword_ChangePasswordError"
            CancelDestinationPageUrl="~/Modules/Trang_Chu/TrangChu.aspx">
            <ChangePasswordTemplate>
                <div style="margin:0px auto 0px auto; padding:10px 0px 20px 0px; text-align:center">
                    <h2>Đổi mật khẩu</h2>
                </div>           
                
                <table id="Table1" runat="server" style="width:100%">
                    <tr>
                        <td style="width:42%; height:27px; padding-left:10px;">
                            <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Mật khẩu cũ:</asp:Label>
                            <asp:Label ID="Label1" runat="server" style="color:#FF0000">*</asp:Label>
                        </td>
                        <td style="width:58%; padding-right:10px">
                            <asp:TextBox ID="CurrentPassword" runat="server" CssClass="passwordEntry" TextMode="Password" Width="100%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" 
                                ControlToValidate="CurrentPassword" ErrorMessage="Mật khẩu cũ không được để trống"
                                 Display="None" ToolTip="Mật khẩu cũ không được để trống"
                                ValidationGroup="ChangeUserPasswordValidationGroup"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server"
                                CssClass="CustomValidatorCalloutStyle" HighlightCssClass="highlight"
                                TargetControlID="CurrentPasswordRequired"></asp:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="height:27px; padding-left:10px;">
                            <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">Mật khẩu mới:</asp:Label>
                            <asp:Label ID="Label2" runat="server" style="color:#FF0000">*</asp:Label>
                        </td>
                        <td style="padding-right:10px">
                            <asp:TextBox ID="NewPassword" runat="server" CssClass="passwordEntry" TextMode="Password" Width="100%"></asp:TextBox>
                            <asp:PasswordStrength ID="PasswordStrength1" runat="server"
                                TargetControlID="NewPassword" DisplayPosition="RightSide" StrengthIndicatorType="Text"
                                PreferredPasswordLength="10" PrefixText="Độ mạnh: " 
                                TextStrengthDescriptions="Rất yếu; Yếu; Trung bình; Mạnh; Rất mạnh">
                            </asp:PasswordStrength>
                            <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                 Display="None" ErrorMessage="Mật khẩu mới không được để trống" ToolTip="Mật khẩu cũ không được để trống"
                                ValidationGroup="ChangeUserPasswordValidationGroup"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server"
                                CssClass="CustomValidatorCalloutStyle" HighlightCssClass="highlight"
                                TargetControlID="NewPasswordRequired"></asp:ValidatorCalloutExtender>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" 
                                ControlToValidate="NewPassword" ClientValidationFunction="validateNewPassword" Display="None"></asp:CustomValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server"
                                CssClass="CustomValidatorCalloutStyle" HighlightCssClass="highlight"
                                TargetControlID="CustomValidator1"></asp:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="height:27px; padding-left:10px;">
                            <asp:Label ID="ConfirmNewPasswordLabel" runat="server" 
                                AssociatedControlID="ConfirmNewPassword">Xác nhận mật khẩu mới:</asp:Label>
                            <asp:Label ID="Label3" runat="server" style="color:#FF0000">*</asp:Label>
                        </td>
                        <td style="padding-right:10px">
                            <asp:TextBox ID="ConfirmNewPassword" runat="server" 
                                CssClass="passwordEntry" TextMode="Password" Width="100%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" 
                                ControlToValidate="ConfirmNewPassword"
                                 Display="None" ErrorMessage="Xác nhận lại mật khẩu mới"
                                ValidationGroup="ChangeUserPasswordValidationGroup"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server"
                                CssClass="CustomValidatorCalloutStyle" HighlightCssClass="highlight"
                                TargetControlID="ConfirmNewPasswordRequired"></asp:ValidatorCalloutExtender>
                            <asp:CompareValidator ID="NewPasswordCompare" runat="server" 
                                ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" 
                                 Display="None"
                                ErrorMessage="Xác nhận mật khẩu mới không trùng khớp" 
                                ValidationGroup="ChangeUserPasswordValidationGroup"></asp:CompareValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server"
                                CssClass="CustomValidatorCalloutStyle" HighlightCssClass="highlight"
                                TargetControlID="NewPasswordCompare"></asp:ValidatorCalloutExtender>
                        </td>
                    </tr>
                </table>
                <div style="margin:0px auto 0px auto; padding:10px 0px 5px 0px; text-align:center" >
                    <asp:ImageButton ID="ChangePasswordPushButtons" runat="server" 
                        CommandName="ChangePassword" ValidationGroup="ChangeUserPasswordValidationGroup"
                        ImageUrl="~/Styles/Images/button_save.png"/>&nbsp;&nbsp;
                    <asp:ImageButton ID="CancelPushButton" runat="server" 
                        CausesValidation="False" CommandName="Cancel"
                        ImageUrl="~/Styles/Images/button_cancel.png"/>
                </div>
            </ChangePasswordTemplate>
            <SuccessTemplate>
                Mật khẩu đã được thay đổi thành công!
            </SuccessTemplate>
        </asp:ChangePassword>

    </div>
    <div>
        Lưu ý:
        <ul style="padding-left:20px">
            <li>Mật khẩu mới phải có độ dài tối thiểu <%= Membership.MinRequiredPasswordLength %> kí tự.</li>
        </ul>
    </div> 
</asp:Content>
