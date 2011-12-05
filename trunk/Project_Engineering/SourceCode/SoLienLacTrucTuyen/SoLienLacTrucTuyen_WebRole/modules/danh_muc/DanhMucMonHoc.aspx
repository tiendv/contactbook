<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhmucmonhoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DanhMucMonHoc" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function pageLoad() {
                var modalPopupAdd = $find('<%=MPEAdd.ClientID%>');
                if (modalPopupAdd != null) {
                    modalPopupAdd.add_showing(function () {
                        $get('<%=TxtSubjectNameThem.ClientID%>').value = "";
                        var DdlNganh = $get('<%=DdlNganh.ClientID%>');
                        DdlNganh.options[DdlNganh.selectedIndex].value = DdlNganh.options[0].value;
                        var DdlKhoiLop = $get('<%=DdlKhoiLop.ClientID%>');
                        DdlKhoiLop.options[DdlKhoiLop.selectedIndex].value = DdlKhoiLop.options[0].value;
                    });
                }
            }

            function popopAdd_CancelSave_Click() {
                $find('<%=MPEAdd.ClientID%>').hide();
                return false;
            }

            function popopEdit_CancelSave_Click() {
                var mPEEditID = $get('<%=HdfRptMonHocMPEEdit.ClientID%>').value;
                $find(mPEEditID).hide();
                return false;
            }

            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptMonHocMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }

            function popopDetail_Close_Click() {
                var mPEDetailID = $get('<%=HdfRptMonHocMPEDetail.ClientID%>').value;
                $find(mPEDetailID).hide();
                return false;
            }

            function validateSubjectNameAdd(ctrl, args) {
                var hfOutput = $get('<%=hfOutputAdd.ClientID%>');
                var SubjectId = 0;
                var SubjectName = $.trim($('#<%=TxtSubjectNameThem.ClientID%>').val());
                var DdlNganhHoc = $get('<%=DdlNganhHocThem.ClientID%>');
                var FacultyId = DdlNganhHoc.options[DdlNganhHoc.selectedIndex].value;
                var DdlKhoiLop = $get('<%=DdlKhoiLopThem.ClientID%>');
                var GradeId = DdlKhoiLop.options[DdlKhoiLop.selectedIndex].value;
                $.ajax({
                    type: "POST",
                    url: "/Modules/Danh_Muc/DanhMucServicePage.aspx/CheckExistSubjectName",
                    data: "{'SubjectId':'" + SubjectId + "', 'SubjectName':'" + SubjectName + "'"
                                        + ", 'FacultyId':'" + FacultyId + "', 'GradeId':'" + GradeId + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            $get('<%=hfOutputAdd.ClientID%>').value = 'false';
                        } else {
                            $get('<%=hfOutputAdd.ClientID%>').value = 'true';
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert('Error');
                        hfOutput.value = 'false';
                    }
                });

                if ($get('<%=hfOutputAdd.ClientID%>').value == 'true') {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            }

            function validateSubjectNameEdit(ctrl, args) {
                var hfOutput = $get('<%=hfOutputEdit.ClientID%>');
                var SubjectId = $('#<%=HdfSubjectId.ClientID%>').val();
                var SubjectName = $.trim($('#<%=TxtSubjectNameSua.ClientID%>').val());
                var FacultyId = $get('<%=HdfFacultyIdSua.ClientID%>').value;
                var GradeId = $get('<%=HdfGradeIdSua.ClientID%>').value;
                $.ajax({
                    type: "POST",
                    url: "/Modules/Danh_Muc/DanhMucServicePage.aspx/CheckExistSubjectName",
                    data: "{'SubjectId':'" + SubjectId + "', 'SubjectName':'" + SubjectName + "'"
                                        + ", 'FacultyId':'" + FacultyId + "', 'GradeId':'" + GradeId + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            $get('<%=hfOutputEdit.ClientID%>').value = 'false';
                        } else {
                            $get('<%=hfOutputEdit.ClientID%>').value = 'true';
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert('Error');
                        hfOutput.value = 'false';
                    }
                });

                if ($get('<%=hfOutputEdit.ClientID%>').value == 'true') {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            }
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            Ngành:
            <asp:DropDownList ID="DdlNganh" runat="server" Width="150px">
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Khối:
            <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="150px">
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Môn học:
            <asp:TextBox ID="TxtSearchedSubject" runat="server"></asp:TextBox>
            <ajaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                TargetControlID="TxtSearchedSubject" WatermarkText="Tất cả">
            </ajaxToolkit:TextBoxWatermarkExtender>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </div>
        <div id="divButtonSearch" style="margin: -15px 0px 0px 0px">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm môn học" OnClick="BtnSearch_Click" Style="margin: 10px" CssClass="BtnSearch" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm môn học mới" CssClass="BtnAdd" />
            <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAdd"
                PopupControlID="PnlPopupAdd" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupAdd"
                PopupDragHandleControlID="PnlDragPopupAdd">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater ui-corner-all">
            <asp:HiddenField ID="HfdSelectedSubjectName" runat="server" />
            <asp:HiddenField ID="HdfFacultyName" runat="server" />
            <asp:HiddenField ID="HdfGradeName" runat="server" />
            <asp:HiddenField ID="HdfSubjectId" runat="server" />
            <asp:HiddenField ID="HdfRptMonHocMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptMonHocMPEEdit" runat="server" />
            <asp:HiddenField ID="HdfRptMonHocMPEDetail" runat="server" />
            <asp:Repeater ID="RptMonHoc" runat="server" OnItemCommand="RptMonHoc_ItemCommand"
                OnItemDataBound="RptMonHoc_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td style="width: 30%">
                            <asp:LinkButton ID="LinkButton2" runat="server">Môn học</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton3" runat="server">Ngành</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton4" runat="server">Khối</asp:LinkButton>
                        </td>
                        <td style="width: 100px">
                            <asp:LinkButton ID="LinkButton5" runat="server">Hê số điểm</asp:LinkButton>
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
                            <asp:HiddenField ID="HdfRptSubjectId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SubjectId")%>' />
                            <asp:HiddenField ID="HdfRptSubjectName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SubjectName")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="LblSubjectName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SubjectName")%>'></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="LblFacultyName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "FacultyName")%>'></asp:Label>
                            <asp:HiddenField ID="HdfFacultyName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "FacultyName")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="LblGradeName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "GradeName")%>'></asp:Label>
                            <asp:HiddenField ID="HdfGradeName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "GradeName")%>' />
                        </td>
                        <td style="height: 40px; text-align: right">
                            <asp:Label ID="Label15" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "MarkRatio")%>'>
                            </asp:Label>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeEditItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "SubjectName")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakeEditItem"
                                PopupControlID="PnlPopupEdit" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit"
                                PopupDragHandleControlID="PnlDragPopupEdit">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "SubjectName")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnFakeDeleteItem"
                                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="7" class="footer ui-corner-bl ui-corner-br">
                        </td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
        <div style="float: right; margin-top: -35px; padding-right: 30px;">
            <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="pager_Command" ViewStateMode="Enabled" />
        </div>
    </div>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa môn học" CssClass="popup_header_title"></asp:Label>
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
                Text="Thêm môn học"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td>
                        Tên:
                        <asp:Label ID="LblAsterisk1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtSubjectNameThem" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="SubjectNameRequiredAdd" runat="server" ControlToValidate="TxtSubjectNameThem"
                            ValidationGroup="AddMonHoc" ErrorMessage="Tên môn học không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="SubjectNameValidatorAdd" runat="server" ControlToValidate="TxtSubjectNameThem"
                            ValidationGroup="AddMonHoc" ErrorMessage="Môn học đã tồn tại" Display="Dynamic"
                            ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Ngành:
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlNganhHocThem" runat="server" Style="width: 99%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        <asp:Label ID="Label14" runat="server" Text="Khối:"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlKhoiLopThem" runat="server" Style="width: 99%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 80px;">
                        Hệ số điểm:
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtMarkRatioThem" runat="server" CssClass="input_textbox" Style="font-family: arial;
                            text-align: right"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="MarkRatioRequiredAdd" runat="server" ControlToValidate="TxtMarkRatioThem"
                            ValidationGroup="AddMonHoc" ErrorMessage="Hệ số điểm không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="MarkRatioRegExp" runat="server" ControlToValidate="TxtMarkRatioThem"
                            ValidationGroup="AddMonHoc" ValidationExpression="\d{1}" Display="Dynamic" ForeColor="Red"
                            ErrorMessage="Hệ số điểm phải là số tự nhiên trong khoảng 1 đến 9"></asp:RegularExpressionValidator>
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
                OnClick="BtnSaveAdd_Click" ValidationGroup="AddMonHoc" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopAdd_CancelSave_Click();" CssClass="CancelButton" />
        </div>
        <asp:HiddenField ID="hfOutputAdd" runat="server" Value="true" />
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="350px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="Sửa môn học"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td style="width: 80px; vertical-align: top; padding-top: 3px;">
                        Tên:
                        <asp:Label ID="Label9" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtSubjectNameSua" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="SubjectNameRequiredEdit" runat="server" ControlToValidate="TxtSubjectNameSua"
                            ValidationGroup="EditMonHoc" ErrorMessage="Tên môn học không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="SubjectNameValidatorEdit" runat="server" ControlToValidate="TxtSubjectNameSua"
                            ValidationGroup="EditMonHoc" ErrorMessage="Môn học đã tồn tại" Display="Dynamic"
                            ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Ngành:
                    </td>
                    <td style="width: auto;">
                        <asp:Label ID="LblFacultyNameSua" runat="server" Text="Label" Width="230px" CssClass="readOnlyTextBox"></asp:Label>
                        <asp:HiddenField ID="HdfFacultyIdSua" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Khối:
                    </td>
                    <td style="width: auto;">
                        <asp:Label ID="LblGradeNameSua" runat="server" Text="Label" Width="230px" CssClass="readOnlyTextBox"></asp:Label>
                        <asp:HiddenField ID="HdfGradeIdSua" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Hệ số điểm:
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtMarkRatioSua" runat="server" CssClass="input_textbox" Style="font-family: arial;
                            text-align: right"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="MarkRatioRequiredEdit" runat="server" ControlToValidate="TxtMarkRatioSua"
                            ValidationGroup="EditMonHoc" ErrorMessage="Hệ số điểm không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtMarkRatioSua"
                            ValidationGroup="EditMonHoc" ValidationExpression="\d{1}" Display="Dynamic" ForeColor="Red"
                            ErrorMessage="Hệ số điểm phải là số tự nhiên trong khoảng 1 đến 9"></asp:RegularExpressionValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label13" runat="server" Text="*" ForeColor="Red"></asp:Label>
            :Thông tin bắt buộc nhập
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnSaveEdit_Click" ValidationGroup="EditMonHoc" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopEdit_CancelSave_Click();" CssClass="CancelButton" />
        </div>
        <asp:HiddenField ID="hfOutputEdit" runat="server" Value="true" />
    </asp:Panel>
</asp:Content>
