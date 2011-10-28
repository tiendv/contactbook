<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="themdanhhieu.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ThemDanhHieuPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function popopAdd_CancelSave_Click() {
                $find('<%=MPEAdd.ClientID%>').hide();
                return false;
            }

            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptMoTaDanhHieuMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }

            function validateTenDanhHieuAdd(ctrl, args) {
                var hfOutput = $get('<%=hfOutputAdd.ClientID%>');
                var tenDanhHieu = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Danh_Muc/DanhMucServicePage.aspx/CheckExistTenDanhHieu",
                    data: "{'tenDanhHieu':'" + tenDanhHieu + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            $get('<%=hfOutputAdd.ClientID%>').value = 'false';
                        } else {
                            $get('<%=hfOutputAdd.ClientID%>').value = 'true';
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        hfOutput.value = 'false';
                    }
                });

                if ($get('<%=hfOutputAdd.ClientID%>').value == 'true') {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            }
        </script>
    </div>
    <div style="width: 500px; padding: 15px 5px 15px 5px" class="loginBox ui-corner-all table_data">
        Tên:
        <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
        <asp:TextBox ID="TxtTenDanhHieu" runat="server"></asp:TextBox>
        <asp:HiddenField ID="hfOutputAdd" runat="server" Value="true" />
        <asp:RequiredFieldValidator ID="TenDanhHieuRequiredAdd" runat="server" ControlToValidate="TxtTenDanhHieu"
            ValidationGroup="AddDanhHieu" ErrorMessage="Tên danh hiệu không được để trống"
            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
        <asp:CustomValidator ID="TenDanhHieuValidatorAdd" runat="server" ControlToValidate="TxtTenDanhHieu"
            ValidationGroup="AddDanhHieu" ClientValidationFunction="validateTenDanhHieuAdd"
            ErrorMessage="Danh hiệu đã tồn tại" Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/Images/button_add_with_text.png"
                CssClass="BtnAdd" />
            <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAdd"
                PopupControlID="PnlPopupAdd" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupAdd"
                PopupDragHandleControlID="PnlDragPopupAdd">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfThuTu" runat="server" />
            <asp:HiddenField ID="HdfRptMoTaDanhHieuMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptDanhHieuMPEEdit" runat="server" />
            <asp:Repeater ID="RptMoTaDanhHieu" runat="server" OnItemCommand="RptMoTaDanhHieu_ItemCommand">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td>
                            Học lực
                        </td>
                        <td>
                            Hạnh kiểm
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
                            <asp:HiddenField ID="HdfThuTu" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ThuTu")%>' />
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TenHocLuc")%>
                        </td>
                        <td>
                            <%#DataBinder.Eval(Container.DataItem, "TenHanhKiem")%>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeEditItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" />
                            <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakeEditItem"
                                PopupControlID="PnlPopupEdit" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit"
                                PopupDragHandleControlID="PnlDragPopupEdit">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" style="display:none" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ThuTu")%>' />
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
        <div style="display: none">
            <cc1:DataPager ID="MainDataPager" runat="server" ViewStateMode="Enabled" />
        </div>
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
            OnClick="BtnCancelAdd_Click" CssClass="CancelButton" />
    </div>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa mô tả danh hiệu"
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
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px;">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupAddTitle" runat="server" CssClass="popup_header_title"
                Text="Thêm mô tả danh hiệu"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table style="width: 100%">
                <tr>
                    <td>
                        Học lực:
                    </td>
                    <td style="width: auto">
                        <asp:DropDownList ID="DdlHocLucAdd" runat="server" Style="width: 200px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Hạnh kiểm:
                    </td>
                    <td style="width: auto">
                        <asp:DropDownList ID="DdlHanhKiemAdd" runat="server" Style="width: 200px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:CheckBox ID="CheckBox1" runat="server" />
            Thêm tiếp sau khi lưu
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSelect" runat="server" OnClick="BtnSelect_Click" ImageUrl="~/Styles/Images/button_save.png"
                CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopAdd_CancelSave_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
