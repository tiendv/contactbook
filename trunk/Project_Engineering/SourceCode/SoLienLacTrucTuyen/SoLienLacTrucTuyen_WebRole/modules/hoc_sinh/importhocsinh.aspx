<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="importhocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ImportStudentPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">

    <div id="divScript">
<script language="javascript" type="text/javascript">
//    function pageLoad() {
//        ShowPopup();
//        setTimeout(HidePopup, 2000);
//    }

    function ShowPopup() {
        $find('modalpopup').show();        
    }

    function HidePopup() {
        $find('modalpopup').hide();        
    }
</script>
    </div>

    <asp:Panel ID="pnlPleaseWait" runat="server" BackColor="White">             
    <img src="../../Styles/Images/ajax-loader.gif" alt="Please wait" />
         </asp:Panel>
    <asp:Button runat="server" ID="HiddenForModal" style="display: none" />
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2"
            runat="server" BehaviorID="modalpopup" 
            TargetControlID="HiddenForModal"
            PopupControlID="pnlPleaseWait"
            DropShadow = "True"
            BackgroundCssClass = "modalBackground"            
            >
        </ajaxToolkit:ModalPopupExtender>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="padding: 10px 0px 10px 0px">
                Năm học:&nbsp;
                <asp:Label ID="LblYear" runat="server" Width="150px" CssClass="readOnlyTextBox"></asp:Label>
                <asp:HiddenField ID="HdfYearId" runat="server" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Ngành:
                <asp:DropDownList ID="DdlNganh" runat="server" Width="150px" AutoPostBack="true"
                    OnSelectedIndexChanged="DdlNganh_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Khối:
                <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="150px" AutoPostBack="true"
                    OnSelectedIndexChanged="DdlKhoiLop_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Lớp:
                <asp:DropDownList ID="DdlLopHoc" runat="server" Width="150px">
                </asp:DropDownList>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    Download template import tại đây
    <asp:LinkButton ID="LkBtnDownload" runat="server" OnClick="LkBtnDownload_Click" Text='Tại đây'>
    </asp:LinkButton>
    <br />
    <br />
    
    <div style="vertical-align: middle">
    Chọn file cần import
        <asp:FileUpload ID="FileUpload1" runat="server"></asp:FileUpload>&nbsp;&nbsp;&nbsp;
        <asp:ImageButton ID="BtnUpload" runat="server" ImageUrl="~/Styles/buttons/button_upload.png"
            OnClick="BtnUpload_Click" CssClass="UploadButton" ValidationGroup="ImportFile" />
    </div>
    

    <asp:RequiredFieldValidator ID="RequiredClass" runat="server" ErrorMessage="Chưa chọn lớp học"
        ControlToValidate="DdlLopHoc" ValidationGroup="ImportFile" Display="Dynamic"
        ForeColor="Red">
    </asp:RequiredFieldValidator>
    <asp:Label ID="lbError" runat="server" ForeColor="Red" Text="Label"></asp:Label>
    <br />
    <div class="table_data ui-corner-all">
        <div>
            <asp:Label ID="LblImportError" runat="server" Text="Có lỗi xảy ra! Vui lòng tải lại file"
                Style="font-size: 14px; font-weight: bold; color: Red" Visible="false"></asp:Label>
            <asp:Label ID="LblImportSuccess" runat="server" Text="Thêm danh sách học sinh thành công!"
                Style="font-size: 14px; font-weight: bold;" Visible="false"></asp:Label>
        </div>
        <table class="repeater">
            <asp:Repeater ID="RptHocSinh" runat="server" OnItemDataBound="RptHocSinh_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td style="width: 150px;" class="ui-corner-tl">
                            Mã học sinh
                        </td>
                        <td>
                            Thông tin học sinh
                        </td>
                        <td style="width: 40%">
                            Tình trạng
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "StudentCode")%>
                        </td>
                        <td style="height: 40px;">
                            Họ tên:
                            <%#DataBinder.Eval(Container.DataItem, "FullName")%>, &nbsp;&nbsp; Giới tính:
                            <%#DataBinder.Eval(Container.DataItem, "StringGender")%>, &nbsp;&nbsp; Ngày sinh:
                            <%#DataBinder.Eval(Container.DataItem, "StringDateOfBirth")%>
                        </td>
                        <td style="height: 40px">
                            <span style="color: Black">
                                <%#DataBinder.Eval(Container.DataItem, "ImportStatus")%></span> <span style="color: Red">
                                    <%#DataBinder.Eval(Container.DataItem, "Error")%></span>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto;">
        <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
            OnClick="BtnSave_Click" CssClass="SaveButton" ValidationGroup="ImportFile" />
        
        &nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />

        <p>Click vào button dưới để test loading</p>
        <asp:ImageButton ID="btnTest" runat="server" ImageUrl="~/Styles/buttons/button_complete.png"
           OnClientClick="ShowPopup();" OnClick="BtnTest_Click" CssClass="CancelButton" />

    </div>
</asp:Content>
