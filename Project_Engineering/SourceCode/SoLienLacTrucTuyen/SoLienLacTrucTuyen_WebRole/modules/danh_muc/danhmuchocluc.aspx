<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhmuchocluc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.CategoryLearningAptitudePage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('.MarkTextBox').keydown(function (event) {
                // Allow only backspace and delete
                if (event.keyCode == 46 || event.keyCode == 8) { // delete  & backspace 
                    // let it happen, don't do anything
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if ((event.keyCode < 48 || event.keyCode > 57) // differ from "0" -> "9"
                        && (event.keyCode < 96 || event.keyCode > 105) // differ from "numpad 0" -> "numpad 9"
                        && event.keyCode != 9 // differ from "TAB"
                        && event.keyCode != 32 && event.keyCode != 190 // differ "Esc" from and "."
                        && event.keyCode != 37 && event.keyCode != 39) { // differ from "left arrow " and "right arrow"
                        event.preventDefault();
                    }
                }
            });
        });
    </script>
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            var modalPopupAdd = $find('<%=MPEAdd.ClientID%>')
            if (modalPopupAdd != null) {
                modalPopupAdd.add_showing(function () {
                    $get('<%=TxtLearningAptitudeNameThem.ClientID%>').value = "";
                    $get('<%=TxtDTBTuThem.ClientID%>').value = "";
                    $get('<%=TxtDTBDenThem.ClientID%>').value = "";
                });
            }
        }

        function popopAdd_CancelSave_Click() {
            $find('<%=MPEAdd.ClientID%>').hide();
            return false;
        }

        function popopConfirmDelete_CancelDelete_Click() {
            $find('<%=MPEDelete.ClientID%>').hide();
            return false;
        }

        function popopEdit_Cancel_Click() {
            $find('<%=MPEEdit.ClientID%>').hide();
            return false;
        }

        function popopInfoInUse_Close() {
            $find('<%=MPEInfoInUse.ClientID%>').hide();
            return false;
        }
    </script>
    <table class="search">
        <tr>
            <td>
                Tên học lực:
            </td>
            <td>
                <asp:TextBox ID="TxtSearchHocLuc" runat="server" Width="150px"></asp:TextBox>&nbsp;
                <ajaxToolkit:TextBoxWatermarkExtender ID="HocLucWatermark" runat="server" TargetControlID="TxtSearchHocLuc"
                    WatermarkText="Tất cả">
                </ajaxToolkit:TextBoxWatermarkExtender>
            </td>
            <td>
                <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                    ToolTip="Tìm kiếm học lực" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
            </td>
        </tr>
    </table>
    <table class="table_data">
        <tr class="add">
            <td>
                <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/buttons/button_add.png"
                    ToolTip="Thêm học lực mới" CssClass="BtnAdd" />
                <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAdd"
                    PopupControlID="PnlPopupAdd" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupAdd"
                    PopupDragHandleControlID="PnlDragPopupAdd">
                </ajaxToolkit:ModalPopupExtender>
                <asp:ImageButton ID="BtnEdit" runat="server" OnClick="BtnEdit_Click" ImageUrl="~/Styles/buttons/button_edit.png"
                    ToolTip="Sửa học lực" CssClass="BtnEdit" />
                <asp:ImageButton ID="BtnFakedEdit" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakedEdit"
                    PopupControlID="PnlPopupEdit" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit"
                    PopupDragHandleControlID="PnlDragPopupEdit">
                </ajaxToolkit:ModalPopupExtender>
                <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Styles/buttons/button_delete.png"
                    ToolTip="Xóa học lực" CssClass="BtnDelete" />
                <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnDelete"
                    PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                    PopupDragHandleControlID="PnlDragPopupConfirmDelete">
                </ajaxToolkit:ModalPopupExtender>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="repeater">
                    <asp:HiddenField ID="HdfEditedConductName" runat="server" />
                    <asp:HiddenField ID="HdfLearningAptitudeId" runat="server" />
                    <asp:Repeater ID="RptHocLuc" runat="server" OnItemDataBound="RptHocLuc_ItemDataBound">
                        <HeaderTemplate>
                            <tr class="header">
                                <td class="ui-corner-tl orderNo">
                                    STT
                                </td>
                                <td class="middle" style="width: 400px">
                                    Học lực
                                </td>
                                <td class="middle">
                                    Điểm trung bình bắt đầu
                                </td>
                                <td class="middle">
                                    Điểm trung bình kết thúc
                                </td>
                                <td id="thSelectAll" runat="server" class="icon" style="height: 40px;">
                                    <asp:CheckBox ID="CkbxSelectAll" runat="server" CssClass="selectAll" />
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                                <td style="height: 40px; text-align: center">
                                    <%# (PagerMain.CurrentIndex - 1) * PagerMain.PageSize + Container.ItemIndex + 1 %>
                                    <asp:HiddenField ID="HdfRptLearningAptitudeId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "LearningAptitudeId")%>' />
                                    <asp:HiddenField ID="HdfRptLearningAptitudeName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "LearningAptitudeName")%>' />
                                </td>
                                <td style="height: 40px;">
                                    <%#DataBinder.Eval(Container.DataItem, "LearningAptitudeName")%>
                                </td>
                                <td style="height: 40px; text-align: right">
                                    <%#DataBinder.Eval(Container.DataItem, "BeginAverageMark")%>
                                </td>
                                <td style="height: 40px; text-align: right">
                                    <%#DataBinder.Eval(Container.DataItem, "EndAverageMark")%>
                                </td>
                                <td id="tdSelect" runat="server" class="icon" style="height: 40px;">
                                    <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="6" class="footer">
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
                <div style="float: right; margin-top: -35px; padding-right: 30px;">
                    <cc1:DataPager ID="PagerMain" runat="server" OnCommand="PagerMain_Command" ViewStateMode="Enabled" />
                </div>
            </td>
        </tr>
    </table>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa học lực"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa học lực đã chọn không?"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="ButtonSave" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="ButtonCancel" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup ui-corner-all" Width="450px">
        <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupAddTitle" runat="server" CssClass="popup_header_title"
                Text="Thêm học lực"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <table style="padding: 5px 7px 10px 7px;">
            <tr>
                <td>
                    <table class="inputBorder" style="width: 100%;">
                        <tr>
                            <td style="width: 100px; vertical-align: top; padding-top: 3px;">
                                Tên:
                                <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width: auto;" colspan="4">
                                <asp:TextBox ID="TxtLearningAptitudeNameThem" runat="server" CssClass="input_textbox"></asp:TextBox>
                                <br />
                                <asp:RequiredFieldValidator ID="LearningAptitudeNameRequiredAdd" runat="server" ControlToValidate="TxtLearningAptitudeNameThem"
                                    Display="Dynamic" ErrorMessage="Tên học lực không được để trống" ForeColor="Red"
                                    ValidationGroup="AddLearningAptitude"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="LearningAptitudeNameValidatorAdd" runat="server" ControlToValidate="TxtLearningAptitudeNameThem"
                                    Display="Dynamic" ErrorMessage="Học lực đã tồn tại" ForeColor="Red" ValidationGroup="AddLearningAptitude"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: text-top; padding-top: 3px;">
                                Điểm trung bình:
                            </td>
                            <td style="width: 40px; vertical-align: top; padding-top: 3px;">
                                Từ:
                                <asp:Label ID="Label13" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtDTBTuThem" runat="server" Width="60px" MaxLength="3" Style="text-align: right"
                                    CssClass="MarkTextBox"></asp:TextBox>
                                (Giá trị từ 0 đến 10)
                                <br />
                                <asp:RequiredFieldValidator ID="BeginMarkRequiredAdd" runat="server" ControlToValidate="TxtDTBTuThem"
                                    Display="Dynamic" ErrorMessage="Khoảng điểm trung bình không được để trống" ForeColor="Red"
                                    ValidationGroup="AddLearningAptitude"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="BeginMarkCustomValidatorAdd" runat="server" ControlToValidate="TxtDTBTuThem"
                                    Display="Dynamic" ErrorMessage="Khoảng điểm trung bình không hợp lệ" ForeColor="Red"
                                    ValidationGroup="AddLearningAptitude"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="width: 40px; vertical-align: top; padding-top: 3px;">
                                Đến:
                                <asp:Label ID="Label15" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtDTBDenThem" runat="server" Width="60px" MaxLength="3" Style="text-align: right"
                                    CssClass="MarkTextBox"></asp:TextBox>
                                (Giá trị từ 0 đến 10)
                                <br />
                                <asp:RequiredFieldValidator ID="EndMarkRequiredAdd" runat="server" ControlToValidate="TxtDTBDenThem"
                                    Display="Dynamic" ErrorMessage="Khoảng điểm trung bình không được để trống" ForeColor="Red"
                                    ValidationGroup="AddLearningAptitude"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="EndMarkCustomValidatorAdd" runat="server" ControlToValidate="TxtDTBDenThem"
                                    Display="Dynamic" ErrorMessage="Khoảng điểm trung bình không hợp lệ" ForeColor="Red"
                                    ValidationGroup="AddLearningAptitude"></asp:CustomValidator>
                                <asp:CompareValidator ID="CompareValidatorAdd" runat="server" Display="Dynamic" ForeColor="Red"
                                    ErrorMessage="Điểm kết thúc phải lớn hơn điểm bắt đầu" ControlToValidate="TxtDTBDenThem"
                                    ControlToCompare="TxtDTBTuThem" ValidationGroup="AddLearningAptitude" Operator="GreaterThan"
                                    Type="Double">
                                </asp:CompareValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="padding: 5px 0px 5px 0px;">
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                :Thông tin bắt buộc nhập
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
                                Thêm tiếp sau khi lưu
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 170px; margin: 0px auto 0px auto;">
                        <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                            OnClick="BtnSaveAdd_Click" ValidationGroup="AddLearningAptitude" CssClass="SaveButton" />
                        &nbsp;
                        <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                            OnClientClick="return popopAdd_CancelSave_Click();" CssClass="CancelButton" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="450px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="Sửa học lực"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <table style="padding: 5px 7px 10px 7px;">
            <tr>
                <td>
                    <table class="inputBorder" style="width: 100%;">
                        <tr>
                            <td style="width: 100px; vertical-align: top; padding-top: 3px;">
                                Tên:
                                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width: auto;" colspan="4">
                                <asp:TextBox ID="TxtLearningAptitudeNameSua" runat="server" CssClass="input_textbox"></asp:TextBox>
                                <br />
                                <asp:RequiredFieldValidator ID="LearningAptitudeNameRequiredModify" runat="server"
                                    ControlToValidate="TxtLearningAptitudeNameSua" Display="Dynamic" ErrorMessage="Tên học lực không được để trống"
                                    ForeColor="Red" ValidationGroup="ModifyLearningAptitude"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="LearningAptitudeNameValidatorModify" runat="server" ControlToValidate="TxtLearningAptitudeNameSua"
                                    Display="Dynamic" ErrorMessage="Học lực đã tồn tại" ForeColor="Red" ValidationGroup="ModifyLearningAptitude"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: text-top; padding-top: 3px;">
                                Điểm trung bình:
                            </td>
                            <td style="width: 40px; vertical-align: top; padding-top: 3px;">
                                Từ:
                                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtDTBTuSua" runat="server" Width="60px" MaxLength="3" Style="text-align: right"
                                    CssClass="MarkTextBox"></asp:TextBox>
                                (Giá trị từ 0 đến 10)
                                <br />
                                <asp:RequiredFieldValidator ID="BeginMarkRequiredModify" runat="server" ControlToValidate="TxtDTBTuSua"
                                    Display="Dynamic" ErrorMessage="Khoảng điểm trung bình không được để trống" ForeColor="Red"
                                    ValidationGroup="ModifyLearningAptitude"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="BeginMarkCustomValidatorModify" runat="server" ControlToValidate="TxtDTBTuSua"
                                    Display="Dynamic" ErrorMessage="Khoảng điểm trung bình không hợp lệ" ForeColor="Red"
                                    ValidationGroup="ModifyLearningAptitude"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="width: 40px; vertical-align: top; padding-top: 3px;">
                                Đến:
                                <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtDTBDenSua" runat="server" Width="60px" MaxLength="3" Style="text-align: right"
                                    CssClass="MarkTextBox"></asp:TextBox>
                                (Giá trị từ 0 đến 10)
                                <br />
                                <asp:RequiredFieldValidator ID="EndMarkRequiredModify" runat="server" ControlToValidate="TxtDTBDenSua"
                                    Display="Dynamic" ErrorMessage="Khoảng điểm trung bình không được để trống" ForeColor="Red"
                                    ValidationGroup="ModifyLearningAptitude"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="EndMarkCustomValidatorModify" runat="server" ControlToValidate="TxtDTBDenSua"
                                    Display="Dynamic" ErrorMessage="Khoảng điểm trung bình không hợp lệ" ForeColor="Red"
                                    ValidationGroup="ModifyLearningAptitude"></asp:CustomValidator>
                                <asp:CompareValidator ID="CompareValidatorModify" runat="server" Display="Dynamic"
                                    ForeColor="Red" ErrorMessage="Điểm kết thúc phải lớn hơn điểm bắt đầu" ControlToValidate="TxtDTBDenSua"
                                    ControlToCompare="TxtDTBTuSua" ValidationGroup="ModifyLearningAptitude" Operator="GreaterThan"
                                    Type="Double">
                                </asp:CompareValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="padding: 5px 0px 5px 0px;">
                        <tr>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                <asp:Label ID="Label12" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 170px; margin: 0px auto 0px auto;">
                        <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                            OnClick="BtnSaveEdit_Click" CssClass="SaveButton" />
                        &nbsp;
                        <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                            OnClientClick="return popopEdit_Cancel_Click();" CssClass="CancelButton"/>
                    </div>
                </td>
            </tr>
        </table>
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
                <asp:Label ID="Label6" runat="server" Text="Một vài học lực không thể xóa vì thông tin đang được sử dụng"></asp:Label>
                <br />
            </div>
        </div>
        <div style="width: 85px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="BtnClose" runat="server" ImageUrl="~/Styles/buttons/button_close.png"
                OnClientClick="return popopInfoInUse_Close();" CssClass="BtnClose" />
        </div>
    </asp:Panel>
</asp:Content>
