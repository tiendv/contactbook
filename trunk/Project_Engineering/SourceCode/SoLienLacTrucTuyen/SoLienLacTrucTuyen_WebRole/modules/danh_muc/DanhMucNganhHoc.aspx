<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhmucnganhhoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.FacultyCategoryPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function pageLoad() {
                var modalPopupAdd = $find('<%=MPEAdd.ClientID%>');
                if (modalPopupAdd != null) {
                    modalPopupAdd.add_showing(function () {
                        $get('<%=TxtFacultyName.ClientID%>').value = "";
                        $get('<%=TxtDescriptionNganhHoc.ClientID%>').value = "";
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

            function validateFacultyNameThem(ctrl, args) {
                var hfOutput = $get('<%=hfOutput_Them.ClientID%>');
                var FacultyId = 0;
                var FacultyName = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Danh_Muc/DanhMucServicePage.aspx/CheckExistFacultyName",
                    data: "{'FacultyId':'" + FacultyId + "','FacultyName':'" + FacultyName + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            $get('<%=hfOutput_Them.ClientID%>').value = 'false';
                        } else {
                            $get('<%=hfOutput_Them.ClientID%>').value = 'true';
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert('Error');
                        hfOutput.value = 'false';
                    }
                });

                if ($get('<%=hfOutput_Them.ClientID%>').value == 'true') {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            }

            function validateFacultyNameSua(ctrl, args) {
                var hfOutput = $get('<%=hfOutput_Edit.ClientID%>');
                var FacultyId = $('#<%=HdfFacultyId.ClientID%>').val();
                var FacultyName = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Danh_Muc/DanhMucServicePage.aspx/CheckExistFacultyName",
                    data: "{'FacultyId':'" + FacultyId + "','FacultyName':'" + FacultyName + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            $get('<%=hfOutput_Edit.ClientID%>').value = 'false';
                        } else {
                            $get('<%=hfOutput_Edit.ClientID%>').value = 'true';
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert('Error');
                        hfOutput.value = 'false';
                    }
                });

                if ($get('<%=hfOutput_Edit.ClientID%>').value == 'true') {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            }
            
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            Tên ngành:&nbsp;
            <asp:TextBox ID="TxtSearchNganhHoc" runat="server" Width="150px"></asp:TextBox>&nbsp;&nbsp;
            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="TxtSearchNganhHoc"
                WatermarkText="Tất cả">
            </asp:TextBoxWatermarkExtender>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" OnClick="BtnSearch_Click" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm ngành học" CssClass="BtnSearch" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm ngành học mới" CssClass="BtnAdd" />
            <asp:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAdd" PopupControlID="PnlPopupAdd"
                BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupAdd" PopupDragHandleControlID="PnlDragPopupAdd">
            </asp:ModalPopupExtender>
            <asp:ImageButton ID="BtnEdit" runat="server" OnClick="BtnEdit_Click" ImageUrl="~/Styles/buttons/button_edit.png"
                ToolTip="Sửa ngành học" CssClass="BtnEdit" />
            <asp:ImageButton ID="BtnFakedEdit" runat="server" style="display:none"/>
            <asp:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakedEdit" PopupControlID="PnlPopupEdit"
                BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit" PopupDragHandleControlID="PnlDragPopupEdit">
            </asp:ModalPopupExtender>
            <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Styles/buttons/button_delete.png"
                ToolTip="Xóa ngành học" CssClass="BtnDelete"/>
            <asp:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnDelete"
                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
            </asp:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfEditedFacultyName" runat="server" />
            <asp:HiddenField ID="HdfDeletedFacultyName" runat="server" />
            <asp:HiddenField ID="HdfFacultyId" runat="server" />
            <asp:HiddenField ID="HdfRptNganhHocMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptNganhHocMPEEdit" runat="server" />
            <asp:Repeater ID="RptNganhHoc" runat="server" OnItemCommand="RptNganhHoc_ItemCommand"
                OnItemDataBound="RptNganhHoc_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td class="middle" style="width: 30%">
                            <asp:LinkButton ID="LkBtnFacultyName" runat="server">Ngành</asp:LinkButton>
                        </td>
                        <td>
                            Mô tả
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
                            <asp:HiddenField ID="HdfRptFacultyId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "FacultyId")%>' />
                            <asp:HiddenField ID="HdfRptFacultyName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "FacultyName")%>' />
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "FacultyName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "Description")%>
                        </td>
                        <td class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
                        </td>
                    </tr>
                </ItemTemplate>               
            </asp:Repeater>
        </table>
        <div style="height: 40px; float: right; margin-top: 0px; padding-right: 30px;">
            <br />
            <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="MainDataPager_Command"
                ViewStateMode="Enabled" />
        </div>
    </div>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa ngành Học" CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa ngành học đã chọn hay không?"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 15px 0px 5px 0px;">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup ui-corner-all" Width="500px">
        <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupAddTitle" runat="server" CssClass="popup_header_title"
                Text="Thêm ngành học"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td style="width: 40px; vertical-align: top; padding-top: 3px;">
                        Tên:
                        <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtFacultyName" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="FacultyNameRequiredAdd" runat="server" ControlToValidate="TxtFacultyName"
                            ValidationGroup="AddNganhHoc" ErrorMessage="Tên ngành học không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="FacultyNameValidatorAdd" runat="server" ControlToValidate="TxtFacultyName"
                            ValidationGroup="AddNganhHoc" ErrorMessage="Ngành học đã tồn tại" Display="Dynamic"
                            ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Mô tả:
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtDescriptionNganhHoc" runat="server" Height="100px" TextMode="MultiLine"
                            CssClass="input_textbox" Style="font-family: arial;"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
            :Thông tin bắt buộc nhập<br />
            <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
            Thêm tiếp sau khi lưu
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnSaveAdd_Click" ValidationGroup="AddNganhHoc" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopAdd_CancelSave_Click();" CssClass="CancelButton" />
        </div>
        <asp:HiddenField ID="hfOutput_Them" runat="server" Value="true" />
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="500px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="Sửa ngành học"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table style="width: 100%;" class="inputBorder">
                <tr>
                    <td style="width: 40px; vertical-align: top; padding-top: 3px;">
                        Tên:
                        <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtFacultyNameEdit" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="FacultyNameRequiredEdit" runat="server" ControlToValidate="TxtFacultyNameEdit"
                            ValidationGroup="EditNganhHoc" ErrorMessage="Tên ngành học không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="FacultyNameValidatorEdit" runat="server" ControlToValidate="TxtFacultyNameEdit"
                            ValidationGroup="EditNganhHoc" ErrorMessage="Ngành học đã tồn tại" Display="Dynamic"
                            ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Mô tả:
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtSuaDescriptionNganhHoc" runat="server" Height="100px" TextMode="MultiLine"
                            CssClass="input_textbox" Style="font-family: arial;"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div style="padding: 5px 0px 5px 0px;">
                <asp:Label ID="Label11" runat="server" Text="*" ForeColor="Red"></asp:Label>
                :Thông tin bắt buộc nhập
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" OnClick="BtnSaveEdit_Click" ValidationGroup="EditNganhHoc"
                ImageUrl="~/Styles/Images/button_save.png" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" OnClientClick="return popopEdit_Cancel_Click();"
                ImageUrl="~/Styles/Images/button_cancel.png" CssClass="CancelButton" />
        </div>
        <asp:HiddenField ID="hfOutput_Edit" runat="server" Value="true" />
    </asp:Panel>
</asp:Content>
