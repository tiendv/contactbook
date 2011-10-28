<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhmucdanhhieu.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DanhMucDanhHieuPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptDanhHieuMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }

            function popopEdit_Cancel_Click() {
                var mPEEditID = $get('<%=HdfRptDanhHieuMPEEdit.ClientID%>').value;
                $find(mPEEditID).hide();
                return false;
            }

            function validateTenDanhHieuAdd(ctrl, args) {
                var hfOutput = $get('<%=hfOutputAdd.ClientID%>');
                var maDanhHieu = 0;
                var tenDanhHieu = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Danh_Muc/DanhMucServicePage.aspx/CheckExistTenDanhHieu",
                    data: "{'maDanhHieu':'" + maDanhHieu + "', 'tenDanhHieu':'" + tenDanhHieu + "'}",
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

            function validateTenDanhHieuEdit(ctrl, args) {
                var hfOutput = $get('<%=hfOutputEdit.ClientID%>');
                var maDanhHieu = $('#<%=HdfMaDanhHieu.ClientID%>').val();
                var tenDanhHieu = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Danh_Muc/DanhMucServicePage.aspx/CheckExistTenDanhHieu",
                    data: "{'maDanhHieu':'" + maDanhHieu + "', 'tenDanhHieu':'" + tenDanhHieu + "'}",
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
            <asp:Label ID="Label9" runat="server" Text="Tên danh hiệu:"></asp:Label>&nbsp;
            <asp:TextBox ID="TxtSearchDanhHieu" runat="server" Width="150px"></asp:TextBox>&nbsp;&nbsp;
            <ajaxToolkit:TextBoxWatermarkExtender ID="DanhHieuWatermark" runat="server" TargetControlID="TxtSearchDanhHieu"
                WatermarkText="Tất cả">
            </ajaxToolkit:TextBoxWatermarkExtender>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm danh hiệu" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" OnClick="BtnAdd_Click" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm danh hiệu mới" CssClass="BtnAdd" />
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfMaDanhHieu" runat="server" />
            <asp:HiddenField ID="HdfRptDanhHieuMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptDanhHieuMPEEdit" runat="server" />
            <asp:Repeater ID="RptDanhHieu" runat="server" OnItemCommand="RptDanhHieu_ItemCommand"
                OnItemDataBound="RptDanhHieu_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td style="width: 30%">
                            <asp:LinkButton ID="LinkButton1" runat="server">Danh hiệu</asp:LinkButton>
                        </td>
                        <td>
                            Mô tả
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
                            <asp:HiddenField ID="HdfRptMaDanhHieu" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaDanhHieu")%>' />
                            <asp:HiddenField ID="HdfRptTenDanhHieu" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "TenDanhHieu")%>' />
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TenDanhHieu")%>
                        </td>
                        <td>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeEditItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaDanhHieu")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakeEditItem"
                                PopupControlID="PnlPopupEdit" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit"
                                PopupDragHandleControlID="PnlDragPopupEdit">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TenDanhHieu")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnFakeDeleteItem"
                                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
                            </ajaxToolkit:ModalPopupExtender>
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
            <cc1:DataPager ID="MainDataPager" runat="server" OfClause="/" PageClause="TRANG"
                OnCommand="pager_Command" PageSize="10" ViewStateMode="Enabled" LastClause=">>"
                GenerateHiddenHyperlinks="False" CompactModePageCount="3" GenerateFirstLastSection="True"
                GenerateGoToSection="False" FirstClause="<<" BackToFirstClause="Trở về trang đầu"
                BackToPageClause="Trở về trang" GoToLastClause="Đến trang cuối" NextToPageClause="Đến trang"
                ShowResultClause="Hiển thị kết quả" ToClause="đến" />
        </div>
    </div>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa danh hiệu" CssClass="popup_header_title"></asp:Label>
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
        <div style="width: 170px; margin: 10px auto 0px auto; padding: 0px 0px 5px 0px;">
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
                Text="Thêm danh hiệu"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td style="width: 15%; vertical-align: top; padding-top: 3px;">
                        <asp:Label ID="Label6" runat="server" Text="Tên:"></asp:Label>
                        <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtTenDanhHieu" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:HiddenField ID="hfOutputAdd" runat="server" Value="true" />
                        <asp:RequiredFieldValidator ID="TenDanhHieuRequiredAdd" runat="server" ControlToValidate="TxtTenDanhHieu"
                            ValidationGroup="AddDanhHieu" ErrorMessage="Tên danh hiệu không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="TenDanhHieuValidatorAdd" runat="server" ControlToValidate="TxtTenDanhHieu"
                            ValidationGroup="AddDanhHieu" ClientValidationFunction="validateTenDanhHieuAdd"
                            ErrorMessage="danh hiệu đã tồn tại" Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
            <asp:Label ID="Label4" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label><br />
            <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
            <asp:Label ID="Label1" runat="server" Text="Thêm tiếp sau khi lưu"></asp:Label>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnSaveAdd_Click" ValidationGroup="AddDanhHieu" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopAdd_CancelSave_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="Sửa danh hiệu"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td style="width: 15%; vertical-align: top; padding-top: 3px;">
                        <asp:Label ID="Label2" runat="server" Text="Tên:"></asp:Label>
                        <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtSuaTenDanhHieu" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:HiddenField ID="hfOutputEdit" runat="server" Value="true" />
                        <asp:RequiredFieldValidator ID="TenDanhHieuRequiredEdit" runat="server" ControlToValidate="TxtSuaTenDanhHieu"
                            ValidationGroup="EditDanhHieu" ErrorMessage="Tên danh hiệu không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="TenDanhHieuValidatorEdit" runat="server" ControlToValidate="TxtSuaTenDanhHieu"
                            ValidationGroup="EditDanhHieu" ClientValidationFunction="validateTenDanhHieuEdit"
                            ErrorMessage="danh hiệu đã tồn tại" Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label11" runat="server" Text="*" ForeColor="Red"></asp:Label>
            <asp:Label ID="Label12" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>
        </div>
        <div style="width: 170px; margin: 0px auto
        0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnSaveEdit_Click" ValidationGroup="EditDanhHieu" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopEdit_Cancel_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
