<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="nhomnguoidung.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.RolesPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript" runat="server">
        <script language="javascript" type="text/javascript">
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

            function validateAddRoleName(ctrl, args) {
                var hdfOutput = $get('<%=hdfOutputAdd.ClientID%>');
                var roleName = $.trim(args.Value).toLowerCase();
                $.ajax({
                    type: "POST",
                    url: "/Modules/Nguoi_Dung/NguoiDungServicePage.aspx/RoleExists",
                    data: "{'roleName':'" + roleName + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            args.IsValid = false;
                        } else {
                            args.IsValid = true;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        args.IsValid = false;
                    }
                });
            }

            function validateEditRoleName(ctrl, args) {
                var hdfOutput = $get('<%=hdfOutputEdit.ClientID%>');
                var oldRoleName = $get('<%=hdfEditingRoleName.ClientID%>').value.toLowerCase();
                var newRoleName = $.trim(args.Value).toLowerCase();

                if (oldRoleName != newRoleName) {
                    $.ajax({
                        type: "POST",
                        url: "/Modules/Nguoi_Dung/NguoiDungServicePage.aspx/RoleExists",
                        data: "{'roleName':'" + oldRoleName + "','newRoleName':'" + newRoleName + "'}",
                        contentType: "application/json; charset=utf-8",
                        success: function (serverResponseData) {
                            if (serverResponseData.d == true) {
                                hdfOutput.value = 'false';
                            } else {
                                hdfOutput.value = 'true';
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert('Error');
                            hdfOutput.value = 'false';
                        }
                    });

                    if ($get('<%=hdfOutputEdit.ClientID%>').value == "true") {
                        args.IsValid = true;
                    } else {
                        args.IsValid = false;
                    }
                }
                else {
                    args.IsValid = true;
                }
            }

            function pageLoad() {
                var MPEAdd = $find('<%=MPEAdd.ClientID%>');
                if (MPEAdd != null) {
                    MPEAdd.add_showing(function () {
                        $get('<%=TxtRoleNameAdd.ClientID%>').value = "";
                        $get('<%=TxtRoleDescriptionAdd.ClientID%>').value = "";
                    });

                    MPEAdd.add_hidden(function () {
                        $get('<%=TxtRoleNameAdd.ClientID%>').value = "";
                        $get('<%=TxtRoleDescriptionAdd.ClientID%>').value = "";
                    });
                }
            }
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            Tên nhóm người dùng:&nbsp;
            <asp:TextBox ID="TxtSearchNhomNguoiDung" runat="server" Width="150px"></asp:TextBox>&nbsp;&nbsp;
            <ajaxToolkit:TextBoxWatermarkExtender ID="RoleNameWatermark" runat="server" TargetControlID="TxtSearchNhomNguoiDung"
                WatermarkText="Tất cả">
            </ajaxToolkit:TextBoxWatermarkExtender>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" CssClass="BtnSearch" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm nhóm người dùng" OnClick="BtnSearch_Click" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAddRole" runat="server" CssClass="BtnAdd" ImageUrl="~/Styles/buttons/button_add.png"
                ToolTip="Thêm nhóm người dùng mới" />
            <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAddRole"
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
            <asp:HiddenField ID="HdfRoleName" runat="server" />
            <asp:Repeater ID="RptRoles" runat="server">
                <HeaderTemplate>
                    <tr class="header ui-corner-right ui-corner-left">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td class="middle" style="width: 30%">
                            Nhóm người dùng
                        </td>
                        <td class="middle">
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
                            <asp:HiddenField ID="HdfRptMaNhomNguoiDung" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "RoleId")%>' />
                            <asp:HiddenField ID="HdfRptTenNhomNguoiDung" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "RoleName")%>' />
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "DisplayedName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "Description")%>
                        </td>
                        <td class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="6" class="footer ui-corner-bl ui-corner-br">
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
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa nhóm người dùng"></asp:Label>
            <img id="imgClosePopupConfirmDelete" src="../../Styles/buttons/popup_button_close.png"
                alt="close" class="button_close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa nhóm người dùng đã chọn không?"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_save.png" CssClass="SaveButton"
                OnClick="BtnOKDeleteItem_Click" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png" CssClass="CancelButton"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupAddTitle" runat="server" CssClass="popup_header_title"
                Text="Thêm nhóm người dùng"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table style="width: 100%;" class="inputBorder">
                <tr>
                    <td style="width: 60px; vertical-align: top; padding-top: 3px;">
                        Tên:
                        <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtRoleNameAdd" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RoleNameRequiredAdd" runat="server" Display="Dynamic"
                            ForeColor="Red" ControlToValidate="TxtRoleNameAdd" ErrorMessage="Tên nhóm người dùng không được để trống<br/>"
                            ValidationGroup="AddRoleGroup"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="RoleNameValidatorAdd" runat="server" Display="Dynamic" ForeColor="Red"
                            ControlToValidate="TxtRoleNameAdd" ErrorMessage="Tên nhóm người dùng này đã tồn tại"
                            ValidationGroup="AddRoleGroup"></asp:CustomValidator>
                        <asp:HiddenField ID="hdfOutputAdd" runat="server" Value="true" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Mô tả:
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtRoleDescriptionAdd" runat="server" TextMode="MultiLine" CssClass="input_textbox"
                            Style="font-family: arial;"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div style="padding: 10px 0px 0px 0px;">
                <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
                : Thông tin bắt buộc nhập
                <br />
                <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
                Thêm tiếp sau khi lưu
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 10px 0px">
            <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                CssClass="SaveButton" ValidationGroup="AddRoleGroup" OnClick="BtnSaveAdd_Click" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                CssClass="CancelButton" OnClientClick="return popopAdd_CancelSave_Click();" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="Sửa nhóm người dùng"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table style="width: 100%;" class="inputBorder">
                <tr>
                    <td style="width: 60px; vertical-align: top; padding-top: 3px;">
                        Tên:
                        <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtRoleNameEdit" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RoleNameRequiredEdit" runat="server" Display="Dynamic"
                            ForeColor="Red" ControlToValidate="TxtRoleNameEdit" ErrorMessage="Tên nhóm người dùng không được để trống"
                            ValidationGroup="EditRoleGroup"></asp:RequiredFieldValidator>
                        <%--<asp:RegularExpressionValidator ID="REV_TxtRoleNameEdit" runat="server" Display="Dynamic" ForeColor="Red" ControlToValidate="TxtRoleNameEdit" ValidationExpression="[\w| ]*" ErrorMessage="Tên nhóm không hợp lệ<br/>" ValidationGroup="EditRoleGroup"/>--%>
                        <asp:CustomValidator ID="RoleNameValidatorEdit" runat="server" Display="Dynamic"
                            ForeColor="Red" ControlToValidate="TxtRoleNameEdit" ErrorMessage="Tên nhóm người dùng này đã tồn tại"
                            ValidationGroup="EditRoleGroup"></asp:CustomValidator>
                        <asp:HiddenField ID="hdfEditingRoleName" runat="server" />
                        <asp:HiddenField ID="hdfOutputEdit" runat="server" Value="true" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Mô tả:
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtDescriptionNhomNguoiDungSua" runat="server" TextMode="MultiLine"
                            CssClass="input_textbox" Style="font-family: arial; height: 300%"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div style="padding: 10px 0px 0px 0px;">
                <asp:Label ID="Label11" runat="server" Text="*" ForeColor="Red"></asp:Label>
                : Thông tin bắt buộc nhập
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 10px 0px">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                CssClass="SaveButton" OnClick="BtnSaveEdit_Click" ValidationGroup="EditRoleGroup" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                CssClass="CancelButton" OnClientClick="return popopEdit_Cancel_Click();" />
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
                <asp:Label ID="Label6" runat="server" Text="Một vài nhóm người dùng không thể xóa vì thông tin đang được sử dụng"></asp:Label>
                <br />
            </div>
        </div>
        <div style="width: 85px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="BtnClose" runat="server" ImageUrl="~/Styles/buttons/button_close.png"
                OnClientClick="return popopInfoInUse_Close();" CssClass="BtnClose" />
        </div>
    </asp:Panel>
</asp:Content>
