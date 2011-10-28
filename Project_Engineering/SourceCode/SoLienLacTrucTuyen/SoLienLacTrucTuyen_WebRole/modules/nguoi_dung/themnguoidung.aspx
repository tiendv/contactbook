<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="themnguoidung.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ThemNguoiDung" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScripts">
        <script type="text/javascript">
            $(document).ready(function () {
                $('.checkAll').click(function () {
                    $("input[type='checkbox']").attr('checked', $(".checkAll input[type='checkbox']").is(':checked'));
                });
            });
        </script>
    </div>

    <div class="loginBox ui-corner-all" style="width: 730px; min-height:300px">
        <asp:HiddenField ID="HdfIsSelectRoleParents" runat="server" Value="false" />
        <asp:CreateUserWizard ID="RegisterUserWizard" runat="server" style="width:100%; text-align:left;" 
            DisplaySideBar="true" SideBarStyle-Width="250px" SideBarStyle-CssClass="SideBarStyle" SideBarStyle-VerticalAlign="Top" 
            LoginCreatedUser="false" ContinueDestinationPageUrl="~/Modules/Nguoi_Dung/DanhSachNguoiDung.aspx"
            OnCreatingUser="RegisterUserWizard_CreatingUser" OnCreatedUser="RegisterUserWizard_CreatedUser">            
            <StartNavigationTemplate>
                <asp:ImageButton ID="StartNextButton" runat="server" style="margin:10px" CssClass="StepNextButton" CommandName="MoveNext" ImageUrl="~/Styles/Images/button_next_step.png" OnClick="BtnNext_Click" />
            </StartNavigationTemplate>
            <SideBarTemplate>
                <asp:ListView ID="SideBarList" runat="server" style="vertical-align:top">                    
                    <ItemTemplate>
                        <div class='<%#((RegisterUserWizard.ActiveStepIndex) == Container.DataItemIndex) ? "sideBarStep sideBarActivedStep" : "sideBarStep sideBarUnativedStep"%>'>
                            Bước <%#Container.DataItemIndex + 1%>: <asp:Label ID="SideBarLabel" runat="server"></asp:Label><br />
                        </div>                        
                    </ItemTemplate>
                </asp:ListView>
            </SideBarTemplate>

            <WizardSteps>
                <asp:WizardStep ID="SeleteRoleStep" runat="server" StepType="Start" Title="Chọn nhóm người dùng">
                    <div style="text-align:left; padding:10px">
                        <asp:Label ID="Label1" runat="server" Text="Nhóm người dùng:"></asp:Label>&nbsp;
                        <asp:DropDownList ID="DdlRoles" runat="server" Width="250px" AutoPostBack="true" OnSelectedIndexChanged="DdlRoles_SelectedIndexChanged"></asp:DropDownList><br /><br />
                        <asp:MultiView ID="MultiViewCtrl" runat="server">
                            <asp:View ID="ViewDefault" runat="server">
                            </asp:View>
                            <asp:View ID="ViewPhuHuynh" runat="server">
                                <div style="border: 1px solid blue" class="ui-corner-all">
                                    <div style="padding:3px 0px 3px 0px; border-bottom: 1px solid blue; text-align:center">
                                        <asp:Label ID="Label20" runat="server" Text="Thông tin truy vấn" Font-Bold="true"></asp:Label>
                                    </div>              
                                    <div style="padding:5px">
                                        <asp:Repeater ID="RptRoleBasedFunctions" runat="server">
                                            <HeaderTemplate>                                    
                                                <asp:CheckBox ID="ChkBxAllFunctions" runat="server" style="float:right" CssClass="checkAll" /><br /><br />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="HdfFunctionId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "FunctionId")%>' />
                                                <asp:Label ID="LblFunctionName" runat="server"><%#DataBinder.Eval(Container.DataItem, "FunctionName")%></asp:Label>&nbsp;
                                                <asp:CheckBox ID="ChkBxSelectedFunction" runat="server" style="float:right" /><br />
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </asp:View>                        
                            <asp:View ID="ViewGiaoVienChuNhiem" runat="server">
                                <div style="border: 1px solid blue" class="ui-corner-all">
                                    <div style="padding:3px 0px 3px 0px; border-bottom: 1px solid blue; text-align:center">
                                        <asp:Label ID="Label14" runat="server" Text="Chọn thông tin lớp" Font-Bold="true" ></asp:Label>
                                    </div>              
                                    <div style="padding:5px;">
                                        <table>
                                            <tr>
                                                <td style="height:27px"> Năm học: &nbsp;</td>
                                                <td>
                                                    <asp:DropDownList ID="DdlNamHocGVCN" runat="server" Width="150px"
                                                        AutoPostBack="true" onselectedindexchanged="DdlNamHocGVCN_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height:27px"> Khối: &nbsp;</td>
                                                <td>
                                                    <asp:DropDownList ID="DdlKhoiGVCN" runat="server" Width="150px"
                                                        AutoPostBack="true" onselectedindexchanged="DdlKhoiGVCN_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                                <td> &nbsp;&nbsp; Ngành: &nbsp;</td>
                                                <td>
                                                    <asp:DropDownList ID="DdlNganhGVCN" runat="server" Width="150px"
                                                        AutoPostBack="true" onselectedindexchanged="DdlNganhGVCN_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height:27px"> Lớp: &nbsp;</td>
                                                <td>
                                                    <asp:DropDownList ID="DdlLopHocGVCN" runat="server" Width="150px"
                                                        AutoPostBack="true" onselectedindexchanged="DdlLopHocGVCN_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>              
                                </div>
                            </asp:View>
                            <asp:View ID="ViewGiaoVienBoMon" runat="server">
                                <div style="border: 1px solid blue" class="ui-corner-all">
                                    <div style="padding:3px 0px 3px 0px; border-bottom: 1px solid blue; text-align:center">
                                        <asp:Label ID="Label13" runat="server" Text="Chọn thông tin lớp - môn" Font-Bold="true" ></asp:Label>
                                    </div>              
                                    <div style="padding:5px;">
                                        <table>
                                            <tr>
                                                <td style="height:27px"> Năm học: &nbsp;</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlNamHoc" runat="server" Width="150px"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height:27px"> Khối: &nbsp;</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlKhoiLops" runat="server" Width="150px"></asp:DropDownList>
                                                </td>
                                                <td> &nbsp;&nbsp; Ngành: &nbsp;</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlNganhHoc" runat="server" Width="150px"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height:27px"> Lớp: &nbsp;</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlLopHoc" runat="server" Width="150px"></asp:DropDownList>
                                                </td>
                                                <td> &nbsp;&nbsp; Môn: &nbsp;</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlMonHoc" runat="server" Width="150px"></asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>              
                                </div>
                                <div style="width:50px; margin:0px auto 0px auto; padding:5px 0px 5px 0px">
                                    <asp:ImageButton ID="BtnAddSubject" runat="server" CssClass="BtnAdd" ImageUrl="~/Styles/Images/button_add_with_text.png"/>
                                </div>
                                <div>
                                    <asp:Repeater ID="RptSelectedClassSubject" runat="server">
                                        <HeaderTemplate>
                                            <tr class="header ui-corner-right ui-corner-left">
                                                <td class="ui-corner-tl orderNo"> STT </td>
                                                <td style="width:30%"> 
                                                    Lớp học
                                                </td>
                                                <td>
                                                    Môn học
                                                 </td>
                                                <td id="thDeleteUser" runat="server" class="icon"> Xóa </td>
                                            </tr>
                                        </HeaderTemplate>                        
                                        <ItemTemplate>
                                            <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                                                <td style="height:40px; text-align:center">
                                                </td>
                                                <td style="height:40px;">                            
                                                </td>
                                                <td style="height:40px;"></td>
                                                <td id="tdDeleteUser" runat="server" class="icon" style="height:40px;">
                                                    <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png" CssClass="DeleteItemButton" CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "UserName")%>' />
                                                </td>
                                            </tr>
                                        </ItemTemplate>                        
                                        <FooterTemplate>
                                            <tr><td colspan="6" class="footer ui-corner-bl ui-corner-br"></td></tr>
                                        </FooterTemplate>
                                    </asp:Repeater>   
                                </div>
                            </asp:View>
                        </asp:MultiView>
                        <br /><asp:Label ID="LblStepError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                    </div>
                </asp:WizardStep>
                <asp:CreateUserWizardStep ID="CreateUserStep" runat="server" Title="Điền thông tin người dùng mới">
                    <CustomNavigationTemplate>
                        <asp:ImageButton ID="StepPreviousButton" runat="server" CommandName="MovePrevious" CausesValidation="False" CssClass="StepPreviousButton" Style="float:left; margin:15px" ImageUrl="~/Styles/Images/button_prev_step.png" />
                        <asp:ImageButton ID="StepNextButton" runat="server" CssClass="StepNextButton" CommandName="MoveNext" ValidationGroup="CreateUser" Style="float:right; margin:15px" ImageUrl="~/Styles/Images/button_next_step.png" />
                    </CustomNavigationTemplate>
                    <ContentTemplate>
                        <asp:HiddenField ID="HdfIsSelectRoleParents" runat="server" Value="false" />
                        <table style="width: 100%;">
                            <tr>
                                <td style="width:30%; height:27px; padding-left:10px">
                                    <asp:Label ID="Label1" runat="server" Text="Nhóm người dùng:"></asp:Label>
                                </td>
                                <td style="width:70%; padding-right:10px">
                                    <asp:Label ID="LblSelectedRole" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:27px; padding-left:10px">
                                    <asp:Label ID="LblTenNguoiDung" runat="server" Text="Tên người dùng:"></asp:Label>
                                    <asp:Label ID="Label2" runat="server" Text="*" Style="color: red"></asp:Label>
                                </td>
                                <td style="padding-right:10px">
                                    <asp:TextBox ID="UserName" runat="server" Style="width: 98%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" Display="Dynamic" ForeColor="Red" ErrorMessage="Tên người dùng không được để trống" ValidationGroup="CreateUser"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="UserNameCustomValidator" runat="server" ControlToValidate="UserName" Display="Dynamic" ForeColor="Red" ErrorMessage="Tên này đã tồn tại" ValidationGroup="CreateUser"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr id="HtmlTrTenThat" runat="server">
                                <td style="height:27px; padding-left:10px">
                                    <asp:Label ID="Label5" runat="server" Text="Tên thật:"></asp:Label>
                                    <asp:Label ID="Label4" runat="server" Text="*" Style="color: red"></asp:Label>
                                </td>
                                <td style="padding-right:10px">
                                    <asp:TextBox ID="TxtTenThat" runat="server" Style="width: 98%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RealNameRequired" runat="server" ControlToValidate="TxtTenThat" Display="Dynamic" ForeColor="Red" ErrorMessage="Tên thật không được để trống" ValidationGroup="CreateUser"></asp:RequiredFieldValidator>                                    
                                </td>
                            </tr>
                            <tr>
                                <td style="height:27px; padding-left:10px">
                                    <asp:Label ID="Label6" runat="server" Text="Mật khẩu:"></asp:Label>
                                    <asp:Label ID="Label7" runat="server" Text="*" Style="color: red;"></asp:Label>
                                </td>
                                <td style="padding-right:10px">
                                    <asp:TextBox ID="Password" runat="server" TextMode="Password" Style="width: 98%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="Password" Display="Dynamic" ForeColor="Red" ErrorMessage="Mật khẩu mới không được để trống" ToolTip="Mật khẩu không được để trống" ValidationGroup="CreateUser"></asp:RequiredFieldValidator>             
                                </td>
                            </tr>
                            <tr>
                                <td style="height:27px; padding-left:10px">
                                    <asp:Label ID="Label8" runat="server" Text="Nhắc lại mật khẩu:"></asp:Label>
                                    <asp:Label ID="Label9" runat="server" Text="*" Style="color: red"></asp:Label>
                                </td>
                                <td style="padding-right:10px">
                                    <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" Style="width: 98%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="height:27px; padding-left:10px">
                                    <asp:Label ID="Label10" runat="server" Text="Email:"></asp:Label>
                                    <asp:Label ID="Label18" runat="server" Text="*" Style="color: red"></asp:Label>
                                </td>
                                <td style="padding-right:10px">
                                    <asp:TextBox ID="Email" runat="server" Style="width: 98%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="HtmlTrThoiHan" runat="server" style="display: none;">
                                <td colspan="2" style="height: 27px; padding-left:10px; padding-right:10px">
                                    <asp:Panel ID="PnlThoiGianNhanTin" runat="server" GroupingText="Thời gian nhận thông tin">
                                        <asp:Label ID="Label3" runat="server" Text="Năm học:" style="margin-left:10px; margin-top:10px" />&nbsp;
                                        <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px" style="margin-top:5px"/><br />
                                        <div style="padding: 10px">
                                            <asp:RadioButton ID="RbtnHocKy1" runat="server" Text=" Học kì 1" GroupName="ThoiKiNhantin" />&nbsp;&nbsp;
                                            <asp:RadioButton ID="RbtnHocKy2" runat="server" Text=" Học kì 2" GroupName="ThoiKiNhantin"/>&nbsp;&nbsp;                                            
                                            <asp:RadioButton ID="RbtnCaNam" runat="server" Text=" Cả năm" Checked="true" GroupName="ThoiKiNhantin"/>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <div style="width: 400px; padding: 10px 10px 0px 10px;">
                            <asp:Label ID="Label11" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            <asp:Label ID="Label12" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:CreateUserWizardStep>                
                <asp:CompleteWizardStep ID="CompleteStep" runat="server" Title="Hoàn tất">
                    <ContentTemplate>
                        Thông tin người dùng được tạo thành công! <br />             
                        <div style="width:100%; padding: 10px 0px 10px 0px; float:right">
                            <asp:ImageButton ID="ContinueButton" runat="server" CommandName="ContinueButton" ImageUrl="~/Styles/Images/button_complete.png" CssClass="ContinueButton" OnClick="RegisterUserWizard_ContinueButtonClick" />
                        </div>
                    </ContentTemplate>
                </asp:CompleteWizardStep>
            </WizardSteps>            
        </asp:CreateUserWizard>       
    </div>
</asp:Content>
