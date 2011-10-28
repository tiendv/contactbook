<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhmucthaidothamgia.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DanhMucThaiDoThamGia" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function pageLoad() {
                var modalPopupAdd = $find('<%=MPEAdd.ClientID%>');
                if (modalPopupAdd != null) {
                    modalPopupAdd.add_showing(function () {
                        $get('<%=TxtTenThaiDoThamGia.ClientID%>').value = "";
                    });
                }
            }

            function popopAdd_CancelSave_Click() {
                $find('<%=MPEAdd.ClientID%>').hide();
                return false;
            }

            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptThaiDoThamGiaMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }

            function popopEdit_Cancel_Click() {
                var mPEEditID = $get('<%=HdfRptThaiDoThamGiaMPEEdit.ClientID%>').value;
                $find(mPEEditID).hide();
                return false;
            }

            function validateTenThaiDoThamGiaAdd(ctrl, args) {
                var hfOutput = $get('<%=hfOutputAdd.ClientID%>');
                var tenThaiDoThamGia = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Danh_Muc/DanhMucServicePage.aspx/CheckExistTenThaiDoThamGia",
                    data: "{'maThaiDoThamGia':'" + 0 + "', 'tenThaiDoThamGia':'" + tenThaiDoThamGia + "'}",
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

            function validateTenThaiDoThamGiaEdit(ctrl, args) {
                var hfOutput = $get('<%=hfOutputEdit.ClientID%>');
                var maThaiDoThamGia = $('#<%=HdfMaThaiDoThamGia.ClientID%>').val();
                var tenThaiDoThamGia = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Danh_Muc/DanhMucServicePage.aspx/CheckExistTenThaiDoThamGia",
                    data: "{'maThaiDoThamGia':'" + maThaiDoThamGia + "', 'tenThaiDoThamGia':'" + tenThaiDoThamGia + "'}",
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
            Tên thái độ tham gia:&nbsp;
            <asp:TextBox ID="TxtSearchThaiDoThamGia" runat="server" Width="150px"></asp:TextBox>&nbsp;&nbsp;&nbsp;
            <ajaxToolkit:TextBoxWatermarkExtender ID="ThaiDoThamGiaWatermark" runat="server"
                TargetControlID="TxtSearchThaiDoThamGia" WatermarkText="Tất cả">
            </ajaxToolkit:TextBoxWatermarkExtender>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" OnClick="BtnSearch_Click" ToolTip="Tìm kiếm thái độ tham gia"
                Width="78px" Height="28px" ImageUrl="~/Styles/Images/button_search_with_text.png"
                CssClass="BtnSearch" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm thái độ tham gia mới" CssClass="BtnAdd" />
            <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAdd"
                PopupControlID="PnlPopupAdd" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupAdd"
                PopupDragHandleControlID="PnlDragPopupAdd">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfMaThaiDoThamGia" runat="server" />
            <asp:HiddenField ID="HdfRptThaiDoThamGiaMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptThaiDoThamGiaMPEEdit" runat="server" />
            <asp:Repeater ID="RptThaiDoThamGia" runat="server" OnItemCommand="RptThaiDoThamGia_ItemCommand"
                OnItemDataBound="RptThaiDoThamGia_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LlkBtnThaiDo" runat="server">Thái độ tham gia</asp:LinkButton>
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
                            <asp:HiddenField ID="HdfRptMaThaiDoThamGia" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaThaiDoThamGia")%>' />
                            <asp:HiddenField ID="HdfRptTenThaiDoThamGia" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "TenThaiDoThamGia")%>' />
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TenThaiDoThamGia")%>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeEditItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaThaiDoThamGia")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakeEditItem"
                                PopupControlID="PnlPopupEdit" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit"
                                PopupDragHandleControlID="PnlDragPopupEdit">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TenThaiDoThamGia")%>' />
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
            <cc1:DataPager ID="MainDataPager" runat="server" OfClause="/" PageClause="TRANG" OnCommand="pager_Command"
                PageSize="10" ViewStateMode="Enabled" LastClause=">>" GenerateHiddenHyperlinks="False"
                CompactModePageCount="3" GenerateFirstLastSection="True" GenerateGoToSection="False"
                FirstClause="<<" BackToFirstClause="Trở về trang đầu" BackToPageClause="Trở về trang"
                GoToLastClause="Đến trang cuối" NextToPageClause="Đến trang" ShowResultClause="Hiển thị kết quả"
                ToClause="đến" />
        </div>
    </div>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa thái độ tham gia"
                CssClass="popup_header_title"></asp:Label>
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
                Text="Thêm thái độ tham gia"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png"
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
                        <asp:TextBox ID="TxtTenThaiDoThamGia" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:HiddenField ID="hfOutputAdd" runat="server" Value="true" />
                        <asp:RequiredFieldValidator ID="TenThaiDoThamGiaRequiredAdd" runat="server" ControlToValidate="TxtTenThaiDoThamGia"
                            ValidationGroup="AddThaiDoThamGia" ErrorMessage="Tên thái độ tham gia không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="TenThaiDoThamGiaValidatorAdd" runat="server" ControlToValidate="TxtTenThaiDoThamGia"
                            ValidationGroup="AddThaiDoThamGia" ClientValidationFunction="validateTenThaiDoThamGiaAdd"
                            ErrorMessage="Thái độ tham gia đã tồn tại" Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
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
            <asp:ImageButton ID="BtnSaveAdd" runat="server" OnClick="BtnSaveAdd_Click" ValidationGroup="AddThaiDoThamGia"
                ImageUrl="~/Styles/Images/button_save.png" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" OnClientClick="return popopAdd_CancelSave_Click();"
                ImageUrl="~/Styles/Images/button_cancel.png" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="Sửa thái độ tham gia"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png"
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
                        <asp:TextBox ID="TxtSuaTenThaiDoThamGia" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:HiddenField ID="hfOutputEdit" runat="server" Value="true" />
                        <asp:RequiredFieldValidator ID="TenThaiDoThamGiaRequiredEdit" runat="server" ControlToValidate="TxtSuaTenThaiDoThamGia"
                            ValidationGroup="EditThaiDoThamGia" ErrorMessage="Tên thái độ tham gia không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="TenThaiDoThamGiaValidatorEdit" runat="server" ControlToValidate="TxtTenThaiDoThamGia"
                            ValidationGroup="EditThaiDoThamGia" ClientValidationFunction="validateTenThaiDoThamGiaEdit"
                            ErrorMessage="Thái độ tham gia đã tồn tại" Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label11" runat="server" Text="*" ForeColor="Red"></asp:Label>
            :Thông tin bắt buộc nhập
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" OnClick="BtnSaveEdit_Click" ValidationGroup="EditThaiDoThamGia"
                ImageUrl="~/Styles/Images/button_save.png" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" OnClientClick="return popopEdit_Cancel_Click();"
                ImageUrl="~/Styles/Images/button_cancel.png" CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
