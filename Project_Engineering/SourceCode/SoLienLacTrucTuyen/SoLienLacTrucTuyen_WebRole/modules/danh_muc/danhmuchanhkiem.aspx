﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhmuchanhkiem.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.CategoryConduct" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function pageLoad() {
                var modalPopupAdd = $find('<%=MPEAdd.ClientID%>')
                if (modalPopupAdd != null) {
                    modalPopupAdd.add_showing(function () {
                        $get('<%=TxtConductName.ClientID%>').value = "";
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

            function validateConductNameAdd(ctrl, args) {
                var ConductName = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Danh_Muc/DanhMucServicePage.aspx/CheckExistConductName",
                    data: "{'oldConductName':'" + '' + "', 'newConductName':'" + ConductName + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            args.IsValid = false;
                        } else {
                            args.IsValid = true;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert('Error');
                        args.IsValid = false;
                    }
                });
            }

            function validateConductNameEdit(ctrl, args) {
                var oldConductName = $('#<%=HdfEditedConductName.ClientID%>').val();
                var newConductName = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Danh_Muc/DanhMucServicePage.aspx/CheckExistConductName",
                    data: "{'oldConductName':'" + oldConductName + "', 'newConductName':'" + newConductName + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            args.IsValid = false;
                        } else {
                            args.IsValid = true;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert('Error');
                        args.IsValid = false;
                    }
                });
            }
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            Tên hạnh kiểm:
            <asp:TextBox ID="TxtSearchHanhKiem" runat="server" Width="150px"></asp:TextBox>&nbsp;&nbsp;
            <ajaxToolkit:TextBoxWatermarkExtender ID="HanhKiemWatermark" runat="server" TargetControlID="TxtSearchHanhKiem"
                WatermarkText="Tất cả">
            </ajaxToolkit:TextBoxWatermarkExtender>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm hạnh kiểm" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/buttons/button_add.png"
                ToolTip="Thêm hạnh kiểm mới" CssClass="BtnAdd" />
            <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAdd"
                PopupControlID="PnlPopupAdd" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupAdd"
                PopupDragHandleControlID="PnlDragPopupAdd">
            </ajaxToolkit:ModalPopupExtender>
            <asp:ImageButton ID="BtnEdit" runat="server" OnClick="BtnEdit_Click" ImageUrl="~/Styles/buttons/button_edit.png"
                ToolTip="Sửa khối lớp" CssClass="BtnEdit" />
            <asp:ImageButton ID="BtnFakedEdit" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakedEdit"
                PopupControlID="PnlPopupEdit" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit"
                PopupDragHandleControlID="PnlDragPopupEdit">
            </ajaxToolkit:ModalPopupExtender>
            <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Styles/buttons/button_delete.png"
                ToolTip="Xóa khối lớp" CssClass="BtnDelete" />
            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnDelete"
                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfEditedConductName" runat="server" />
            <asp:HiddenField ID="HdfConductId" runat="server" />
            <asp:HiddenField ID="HdfRptHanhKiemMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptHanhKiemMPEEdit" runat="server" />
            <asp:Repeater ID="RptHanhKiem" runat="server" OnItemDataBound="RptHanhKiem_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LinkButton1" runat="server">Hạnh kiểm</asp:LinkButton>
                        </td>
                        <td id="thSelectAll" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelectAll" runat="server" CssClass="selectAll" />
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptConductId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ConductId")%>' />
                            <asp:HiddenField ID="HdfRptConductName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ConductName")%>' />
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "ConductName")%>
                        </td>
                        <td id="tdSelect" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="5" class="footer ui-corner-bl ui-corner-br">
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
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa hạnh kiểm" CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa hạnh kiểm đã chọn không?"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupAddTitle" runat="server" CssClass="popup_header_title"
                Text="Thêm hạnh kiểm"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td style="width: 15%; vertical-align: top; padding-top: 3px;">
                        Tên:
                        <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtConductName" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ConductNameRequiredAdd" runat="server" ControlToValidate="TxtConductName"
                            ValidationGroup="AddHanhKiem" ErrorMessage="Tên hạnh kiểm không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="ConductNameValidatorAdd" runat="server" ControlToValidate="TxtConductName"
                            ValidationGroup="AddHanhKiem" ErrorMessage="Hạnh kiểm đã tồn tại" Display="Dynamic"
                            ForeColor="Red"></asp:CustomValidator>
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
            <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnSaveAdd_Click" ValidationGroup="AddHanhKiem" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopAdd_CancelSave_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="Sửa hạnh kiểm"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td style="width: 15%; vertical-align: top; padding-top: 3px;">
                        Tên:
                        <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtSuaConductName" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ConductNameRequiredEdit" runat="server" ControlToValidate="TxtSuaConductName"
                            ValidationGroup="EditHanhKiem" ErrorMessage="Tên hạnh kiểm không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="ConductNameValidatorEdit" runat="server" ControlToValidate="TxtSuaConductName"
                            ValidationGroup="EditHanhKiem" ErrorMessage="Hạnh kiểm đã tồn tại" Display="Dynamic"
                            ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label11" runat="server" Text="*" ForeColor="Red"></asp:Label>
            :Thông tin bắt buộc nhập
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnSaveEdit_Click" ValidationGroup="EditHanhKiem" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopEdit_Cancel_Click();" CssClass="CancelButton" />
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
                <asp:Label ID="Label6" runat="server" Text="Một vài hạnh kiểm không thể xóa vì thông tin đang được sử dụng"></asp:Label>
                <br />
            </div>
        </div>
        <div style="width: 85px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="BtnClose" runat="server" ImageUrl="~/Styles/buttons/button_close.png"
                OnClientClick="return popopInfoInUse_Close();" CssClass="BtnClose" />
        </div>
    </asp:Panel>
</asp:Content>
