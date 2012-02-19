<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhsachnguoidung.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.UserListPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript" runat="server">
        <script language="javascript" type="text/javascript">
            function popopConfirmDelete_CancelDelete_Click() {
                $find('<%=MPEDelete.ClientID%>').hide();
                return false;
            }

            function popopInfoInUse_Close() {
                $find('<%=MPEInfoInUse.ClientID%>').hide();
                return false;
            }

            function popopActivateReport_Close() {
                $find('<%=MPEActivateReport.ClientID%>').hide();
                return false;
            }
            
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            Nhóm người dùng:&nbsp;
            <asp:DropDownList ID="DdlRoles" runat="server" Width="200px" />
            &nbsp; &nbsp; &nbsp; &nbsp; Người dùng:
            <asp:TextBox ID="TxtSearchUserName" runat="server" Width="150px"></asp:TextBox>&nbsp;&nbsp;
            <ajaxToolkit:TextBoxWatermarkExtender ID="UserNameWatermark" runat="server" TargetControlID="TxtSearchUserName"
                WatermarkText="Tất cả">
            </ajaxToolkit:TextBoxWatermarkExtender>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" CssClass="BtnSearch" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm người dùng" OnClick="BtnSearch_Click" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnActivate" runat="server" ImageUrl="~/Styles/buttons/button_activate.png"
                ToolTip="Kích hoạt người dùng" OnClick="BtnActivate_Click" CssClass="BtnActivate" />
            <asp:ImageButton ID="BtnDeactivate" runat="server" ImageUrl="~/Styles/buttons/button_deactivate.png"
                ToolTip="Hủy kích hoạt người dùng" OnClick="BtnDeactivate_Click" CssClass="BtnDeactivate" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="BtnAddUser" runat="server" CssClass="BtnAdd" ImageUrl="~/Styles/buttons/button_add.png"
                ToolTip="Thêm người dùng mới" OnClick="BtnAdd_Click" />
            <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Styles/buttons/button_edit.png"
                ToolTip="Sửa người dùng" OnClick="BtnEdit_Click" CssClass="BtnEdit" />
            <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Styles/buttons/button_delete.png"
                ToolTip="Xóa người dùng" OnClick="BtnOKDeleteItem_Click" CssClass="BtnDelete" />
            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnDelete"
                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfUserName" runat="server" />
            <asp:HiddenField ID="HdfRptUserMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptUserMPEEdit" runat="server" />
            <asp:Repeater ID="RptUser" runat="server" OnItemCommand="RptUser_ItemCommand">
                <HeaderTemplate>
                    <tr class="header ui-corner-right ui-corner-left">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td style="width: 20%">
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="SortByUserName" ForeColor="White">Người dùng</asp:LinkButton>
                        </td>
                        <td>
                            Tên thật
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="SortByUserName" ForeColor="White">Nhóm người dùng</asp:LinkButton>
                        </td>
                        <td>
                            Email
                        </td>
                        <td>
                            Trạng thái
                        </td>
                        <td class="icon">
                            <asp:CheckBox ID="CkbxSelectAll" runat="server" CssClass="selectAll" />
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptMaNhomNguoiDung" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "UserId")%>' />
                            <asp:HiddenField ID="HdfRptTenNhomNguoiDung" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "UserName")%>' />
                            <asp:HiddenField ID="HdfRptActualUserName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ActualUserName")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:LinkButton ID="LkBtnUserName" runat="server" CommandName="CmdDetailItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "UserId")%>'><%#DataBinder.Eval(Container.DataItem, "UserName")%>
                            </asp:LinkButton>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "FullName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "RoleDisplayedName")%>
                        </td>
                        <td style="height: 40px">
                            <asp:Label ID="LblEmail" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Email")%>'></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "StringStatus")%>
                        </td>
                        <td class="icon">
                            <asp:CheckBox ID="CkbxSelectUser" CssClass="select" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="8" class="footer ui-corner-bl ui-corner-br">
                        </td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
        <div style="float: right; margin-top: -35px; padding-right: 30px;">
            <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="DataPager_Command" ViewStateMode="Enabled"
                PageSize="15" />
        </div>
    </div>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa người dùng"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa người dùng đã chọn không?"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:ImageButton ID="BtnFakedInUse" runat="server" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="MPEInfoInUse" runat="server" TargetControlID="BtnFakedInUse"
        PopupControlID="PnlInfoInUse" BackgroundCssClass="modalBackground" CancelControlID="imgPnlInfoInUse"
        PopupDragHandleControlID="PnlInfoInUseDrag">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="PnlInfoInUse" runat="server" CssClass="popup ui-corner-all" Width="350px">
        <asp:Panel ID="PnlInfoInUseDrag" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="Label4" runat="server" Text="Thông tin đang được sử dụng" CssClass="popup_header_title"></asp:Label>
            <img id="imgPnlInfoInUse" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="Label6" runat="server" Text="Một vài người dùng không thể xóa vì thông tin đang được sử dụng"></asp:Label>
                <br />
            </div>
        </div>
        <div style="width: 85px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="BtnClose" runat="server" ImageUrl="~/Styles/buttons/button_close.png"
                OnClientClick="return popopInfoInUse_Close();" CssClass="BtnClose" />
        </div>
    </asp:Panel>
    <asp:ImageButton ID="BtnFakedCantActivate" runat="server" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="MPEActivateReport" runat="server" TargetControlID="BtnFakedCantActivate"
        PopupControlID="PnlActivateReport" BackgroundCssClass="modalBackground" CancelControlID="imgActivateReport"
        PopupDragHandleControlID="PnlActivateReportDrag">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="PnlActivateReport" runat="server" CssClass="popup ui-corner-all" Width="350px">
        <asp:Panel ID="PnlActivateReportDrag" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="Label1" runat="server" Text="Thông báo kích hoạt" CssClass="popup_header_title"></asp:Label>
            <img id="imgActivateReport" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image3" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="Label2" runat="server" Text="Một vài người dùng không thể kích hoạt vì không có thông tin email"></asp:Label>
                <br />
            </div>
        </div>
        <div style="width: 85px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Styles/buttons/button_close.png"
                OnClientClick="return popopActivateReport_Close();" CssClass="BtnClose" />
        </div>
    </asp:Panel>
</asp:Content>
