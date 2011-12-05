<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhmuctiet.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DanhMucTietPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function pageLoad() {
                var modalPopupAdd = $find('<%=MPEAdd.ClientID%>');
                if (modalPopupAdd != null) {
                    modalPopupAdd.add_showing(function () {
                        $get('<%=TxtTeachingPeriodNameHocThem.ClientID%>').value = "";
                    });
                }
            }

            function popopAdd_CancelSave_Click() {
                $find('<%=MPEAdd.ClientID%>').hide();
                return false;
            }

            function popopEdit_CancelSave_Click() {
                var mPEEditID = $get('<%=HdfRptTietHocMPEEdit.ClientID%>').value;
                $find(mPEEditID).hide();
                return false;
            }

            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptTietHocMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            Tiết:
            <asp:TextBox ID="TxtSearchTiet" runat="server"></asp:TextBox>
            <ajaxToolkit:TextBoxWatermarkExtender ID="SearchTietWatermark" runat="server" TargetControlID="TxtSearchTiet"
                WatermarkText="Tất cả">
            </ajaxToolkit:TextBoxWatermarkExtender>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Buổi:
            <asp:DropDownList ID="DdlBuoi" runat="server" Width="150px">
            </asp:DropDownList>
        </div>
        <div id="divButtonSearch" style="margin: -15px 0px 0px 10px">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm tiết học" OnClick="BtnSearch_Click" Style="margin: 10px" CssClass="BtnSearch" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm tiết học mới" CssClass="BtnAdd" />
            <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAdd"
                PopupControlID="PnlPopupAdd" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupAdd"
                PopupDragHandleControlID="PnlDragPopupAdd">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater ui-corner-all">
            <asp:HiddenField ID="HdfSltTeachingPeriodName" runat="server" />
            <asp:HiddenField ID="HdfTeachingPeriodIdHoc" runat="server" />
            <asp:HiddenField ID="HdfRptTietHocMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptTietHocMPEEdit" runat="server" />
            <asp:HiddenField ID="HdfRptTietHocMPEDetail" runat="server" />
            <asp:Repeater ID="RptTietHoc" runat="server" OnItemCommand="RptTietHoc_ItemCommand"
                OnItemDataBound="RptTietHoc_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td style="width: 30%">
                            <asp:LinkButton ID="LinkButton2" runat="server">Tiết học</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton3" runat="server">Buổi</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton4" runat="server">Thứ tự</asp:LinkButton>
                        </td>
                        <td>
                            Thời điểm bắt đầu
                        </td>
                        <td>
                            Thời điểm kết thúc
                        </td>
                        <td id="thEdit" runat="server" class="icon">
                            Sửa
                        </td>
                        <td id="thDelete" runat="server" class="icon">
                            Xóa
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptTeachingPeriodIdHoc" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "TeachingPeriodId")%>' />
                            <asp:HiddenField ID="HdfRptTeachingPeriodNameHoc" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "TeachingPeriodName")%>' />
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TeachingPeriodName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "SessionName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TeachingPeriodOrder")%>
                        </td>
                        <td style="height: 40px; text-align: right">
                            <%#DataBinder.Eval(Container.DataItem, "StringBeginTime")%>
                        </td>
                        <td style="height: 40px; text-align: right">
                            <%#DataBinder.Eval(Container.DataItem, "StringEndTime")%>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeEditItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TeachingPeriodId")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakeEditItem"
                                PopupControlID="PnlPopupEdit" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit"
                                PopupDragHandleControlID="PnlDragPopupEdit">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TeachingPeriodName")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnFakeDeleteItem"
                                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
                            </ajaxToolkit:ModalPopupExtender>
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
            <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="MainDataPager_Command"
                ViewStateMode="Enabled" />
        </div>
    </div>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa tiết học" CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupAddTitle" runat="server" CssClass="popup_header_title"
                Text="Thêm tiết học"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td>
                        Tên:
                        <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtTeachingPeriodNameHocThem" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="TeachingPeriodNameHocRequiredAdd" runat="server" ControlToValidate="TxtTeachingPeriodNameHocThem"
                            ValidationGroup="AddTietHoc" ErrorMessage="Tên tiết học không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="TeachingPeriodNameHocValidatorAdd" runat="server" ControlToValidate="TxtTeachingPeriodNameHocThem"
                            ValidationGroup="AddTietHoc" ErrorMessage="Tiết học đã tồn tại" Display="Dynamic"
                            ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Buổi:
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlBuoiAdd" runat="server" Style="width: 98%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Thứ tự:
                        <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtThuTuAdd" runat="server" CssClass="input_textbox" Style="font-family: arial;
                            text-align: right"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" MaskType="Number"
                            TargetControlID="TxtThuTuAdd" Mask="9">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:RequiredFieldValidator ID="MarkRatioRequiredAdd" runat="server" ControlToValidate="TxtThuTuAdd"
                            ValidationGroup="AddTietHoc" ErrorMessage="Hệ số điểm không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="ThuTuRegExp" runat="server" ControlToValidate="TxtThuTuAdd"
                            ValidationGroup="AddTietHoc" ValidationExpression="\d{1}" Display="Dynamic" ForeColor="Red"
                            ErrorMessage="Thứ tự phải là số tự nhiên trong khoảng 1 đến 9"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Thời gian bắt đầu:
                        <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtThoiGianBatDauAdd" runat="server" Width="150px"></asp:TextBox>
                        (theo giờ 24)<br />
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" MaskType="Time"
                            TargetControlID="TxtThoiGianBatDauAdd" UserTimeFormat="TwentyFourHour" Mask="99:99">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:RequiredFieldValidator ID="ThoiGianKetThucRequiredAdd" runat="server" ControlToValidate="TxtThoiGianKetThucAdd"
                            ValidationGroup="AddTietHoc" ErrorMessage="Thời gian bắt đầu không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Thời gian kết thúc:
                        <asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtThoiGianKetThucAdd" runat="server" Width="150px"></asp:TextBox>
                        (theo giờ 24)<br />
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" MaskType="Time"
                            TargetControlID="TxtThoiGianKetThucAdd" UserTimeFormat="TwentyFourHour" Mask="99:99">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtThoiGianKetThucAdd"
                            ValidationGroup="AddTietHoc" ErrorMessage="Thời gian kết thức không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
            :Thông tin bắt buộc nhập<br />
            <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
            Thêm tiếp sau khi lưu
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnSaveAdd_Click" ValidationGroup="AddTietHoc" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopAdd_CancelSave_Click();" CssClass="CancelButton" />
        </div>
        <asp:HiddenField ID="hfOutputAdd" runat="server" Value="true" />
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="sửa tiết học"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td>
                        Tên:
                        <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtTeachingPeriodNameHocEdit" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="TeachingPeriodNameHocRequiredEdit" runat="server" ControlToValidate="TxtTeachingPeriodNameHocEdit"
                            ValidationGroup="EditTietHoc" ErrorMessage="Tên tiết học không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="TeachingPeriodNameHocValidatorEdit" runat="server" ControlToValidate="TxtTeachingPeriodNameHocEdit"
                            ValidationGroup="EditTietHoc" ErrorMessage="Tiết học đã tồn tại" Display="Dynamic"
                            ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Buổi:
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlBuoiEdit" runat="server" Style="width: 98%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Thứ tự:
                        <asp:Label ID="Label6" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtThuTuEdit" runat="server" CssClass="input_textbox" Style="font-family: arial;
                            text-align: right"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" runat="server" MaskType="Number"
                            TargetControlID="TxtThuTuEdit" Mask="9">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:RequiredFieldValidator ID="MarkRatioRequiredEdit" runat="server" ControlToValidate="TxtThuTuEdit"
                            ValidationGroup="EditTietHoc" ErrorMessage="Hệ số điểm không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtThuTuEdit"
                            ValidationGroup="EditTietHoc" ValidationExpression="\d{1}" Display="Dynamic"
                            ForeColor="Red" ErrorMessage="Thứ tự phải là số tự nhiên trong khoảng 1 đến 9"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Thời gian bắt đầu:
                        <asp:Label ID="Label8" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtThoiGianBatDauEdit" runat="server" Width="150px"></asp:TextBox>
                        (theo giờ 24)<br />
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server" MaskType="Time"
                            TargetControlID="TxtThoiGianBatDauEdit" UserTimeFormat="TwentyFourHour" Mask="99:99">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:RequiredFieldValidator ID="ThoiGianKetThucRequiredEdit" runat="server" ControlToValidate="TxtThoiGianKetThucEdit"
                            ValidationGroup="EditTietHoc" ErrorMessage="Thời gian bắt đầu không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Thời gian kết thúc:
                        <asp:Label ID="Label9" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtThoiGianKetThucEdit" runat="server" Width="150px"></asp:TextBox>
                        (theo giờ 24)<br />
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender6" runat="server" MaskType="Time"
                            TargetControlID="TxtThoiGianKetThucEdit" UserTimeFormat="TwentyFourHour" Mask="99:99">
                        </ajaxToolkit:MaskedEditExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtThoiGianKetThucEdit"
                            ValidationGroup="EditTietHoc" ErrorMessage="Thời gian kết thức không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label10" runat="server" Text="*" ForeColor="Red"></asp:Label>
            :Thông tin bắt buộc nhập<br />
            <asp:CheckBox ID="CkbEditAfterSave" runat="server" />
            sửa tiếp sau khi lưu
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnSaveEdit_Click" ValidationGroup="EditTietHoc" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopEdit_CancelSave_Click();" CssClass="CancelButton" />
        </div>
        <asp:HiddenField ID="hfOutputEdit" runat="server" Value="true" />
    </asp:Panel>
</asp:Content>
