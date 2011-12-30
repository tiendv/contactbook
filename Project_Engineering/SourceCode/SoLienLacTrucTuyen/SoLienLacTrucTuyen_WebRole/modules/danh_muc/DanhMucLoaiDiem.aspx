<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhmucloaidiem.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DanhMucLoaiDiem" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function pageLoad() {
                var modalPopupAdd = $find('<%=MPEAdd.ClientID%>');
                if (modalPopupAdd != null) {
                    $find('<%=MPEAdd.ClientID%>').add_showing(function () {
                        $get('<%=TxtMarkTypeName.ClientID%>').value = "";
                        $get('<%=TxtMarkTypeName.ClientID%>').focus();
                        $get('<%=TxtMarkRatioLoaiDiemAdd.ClientID%>').value = "";
                    });
                }
            }

            function popopAdd_CancelSave_Click() {
                $find('<%=MPEAdd.ClientID%>').hide();
                return false;
            }

            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptLoaiDiemMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }

            function popopEdit_Cancel_Click() {
                var mPEEditID = $get('<%=HdfRptLoaiDiemMPEEdit.ClientID%>').value;
                $find(mPEEditID).hide();
                return false;
            }

            function validateMarkTypeNameAdd(ctrl, args) {
                var hfOutput = $get('<%=hfOutputAdd.ClientID%>');
                var markTypeName = $.trim(args.Value);
                alert("{'markTypeName':'" + markTypeName + "'}");
                $.ajax({
                    type: "POST",
                    url: "/modules/danh_muc/DanhMucServicePage.aspx/MarkTypeNameExists",
                    data: "{'markTypeName':'" + markTypeName + "'}",
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

            function validateMarkTypeNameEdit(ctrl, args) {
                var hfOutput = $get('<%=hfOutputEdit.ClientID%>');
                var oldMarkTypeName = $('#<%=HdfEditedMarkTypeName.ClientID%>');
                var newMarkTypeName = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/modules/danh_muc/DanhMucServicePage.aspx/MarkTypeNameExists",
                    data: "{'oldMarkTypeName':'" + oldMarkTypeName + "', 'newMarkTypeName':'" + newMarkTypeName + "'}",
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
        <script language="javascript" type="text/javascript">
            $(document).ready(function () {
                $("#<%=TxtMaxMarksPerTermAdd.ClientID%>").keydown(function (event) {
                    // Allow only backspace and delete
                    if (event.keyCode == 46 || event.keyCode == 8) {
                        // let it happen, don't do anything
                    }
                    else {
                        // Ensure that it is a number and stop the keypress
                        if ((event.keyCode < 48 || event.keyCode > 57)
                        && (event.keyCode < 96 || event.keyCode > 105)) {
                            event.preventDefault();
                        }
                    }
                });

                $("#<%=TxtMarkRatioLoaiDiemAdd.ClientID%>").keydown(function (event) {
                    // Allow only backspace and delete
                    if (event.keyCode == 46 || event.keyCode == 8) {
                        // let it happen, don't do anything
                    }
                    else {
                        // Ensure that it is a number and stop the keypress
                        if ((event.keyCode < 48 || event.keyCode > 57)
                        && (event.keyCode < 96 || event.keyCode > 105)) {
                            event.preventDefault();
                        }
                    }
                });
            });
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            <asp:Label ID="Label9" runat="server" Text="Tên loại điểm:"></asp:Label>&nbsp;
            <asp:TextBox ID="TxtSearchLoaiDiem" runat="server" Width="150px"></asp:TextBox>&nbsp;&nbsp;
            <ajaxToolkit:TextBoxWatermarkExtender ID="LoaiDiemWatermark" runat="server" TargetControlID="TxtSearchLoaiDiem"
                WatermarkText="Tất cả">
            </ajaxToolkit:TextBoxWatermarkExtender>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm loại điểm" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm loại điểm mới" CssClass="BtnAdd" />
            <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAdd"
                PopupControlID="PnlPopupAdd" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupAdd"
                PopupDragHandleControlID="PnlDragPopupAdd">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfSltMarkTypeName" runat="server" />
            <asp:HiddenField ID="HdfSltMarkTypeId" runat="server" />
            <asp:HiddenField ID="HdfEditedMarkTypeName" runat="server" />
            <asp:HiddenField ID="HdfRptLoaiDiemMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptLoaiDiemMPEEdit" runat="server" />
            <asp:Repeater ID="RptLoaiDiem" runat="server" OnItemCommand="RptLoaiDiem_ItemCommand"
                OnItemDataBound="RptLoaiDiem_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton1" runat="server">Loại điểm</asp:LinkButton>
                        </td>
                        <td style="width: 100px">
                            <asp:LinkButton ID="LinkButton2" runat="server">Hệ số điểm</asp:LinkButton>
                        </td>
                        <td>
                            Số điểm tối đa/Học kì
                        </td>
                        <td>
                            Dùng tính điểm trung bình
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
                            <%# (PagerMain.CurrentIndex - 1) * PagerMain.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptMarkTypeId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MarkTypeId")%>' />
                            <asp:HiddenField ID="HdfRptMarkTypeName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MarkTypeName")%>' />
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "MarkTypeName")%>
                        </td>
                        <td style="height: 40px; text-align: right">
                            <%#DataBinder.Eval(Container.DataItem, "MarkRatio")%>
                        </td>
                        <td style="height: 40px; text-align: right">
                            <%#DataBinder.Eval(Container.DataItem, "MaxQuantity")%>
                        </td>
                        <td style="height: 40px; text-align: right">
                            <%#((bool)DataBinder.Eval(Container.DataItem, "IsUsedForCalculatingAvg") == true)? "Có": "Không" %>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeEditItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MarkTypeName")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakeEditItem"
                                PopupControlID="PnlPopupEdit" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit"
                                PopupDragHandleControlID="PnlDragPopupEdit">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MarkTypeName")%>' />
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
            <cc1:DataPager ID="PagerMain" runat="server" OfClause="/" PageClause="TRANG" OnCommand="PagerMain_Command"
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
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa loại điểm" CssClass="popup_header_title"></asp:Label>
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
                Text="Thêm loại điểm"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td style="width: 80px; vertical-align: top; padding-top: 3px;">
                        <asp:Label ID="Label6" runat="server" Text="Tên:"></asp:Label>
                        <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtMarkTypeName" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="MarkTypeNameRequiredAdd" runat="server" ControlToValidate="TxtMarkTypeName"
                            ValidationGroup="AddLoaiDiem" ErrorMessage="Tên loại điểm không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="MarkTypeNameValidatorAdd" runat="server" ControlToValidate="TxtMarkTypeName"
                            ValidationGroup="AddLoaiDiem" ErrorMessage="Loại điểm đã tồn tại" Display="Dynamic"
                            ForeColor="Red"></asp:CustomValidator>
                    </td>
                    <asp:HiddenField ID="hfOutputAdd" runat="server" Value="true" />
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        <asp:Label ID="Label8" runat="server" Text="Hệ số điểm:"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtMarkRatioLoaiDiemAdd" runat="server" CssClass="input_textbox"
                            Style="font-family: arial; text-align: right">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="MarkRatioRequiredAdd" runat="server" ControlToValidate="TxtMarkRatioLoaiDiemAdd"
                            ValidationGroup="AddLoaiDiem" ErrorMessage="Hệ số điểm không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="MarkRatioRegExpAdd" runat="server" ControlToValidate="TxtMarkRatioLoaiDiemAdd"
                            ValidationGroup="AddLoaiDiem" ValidationExpression="\d{1}" Display="Dynamic"
                            ForeColor="Red" ErrorMessage="Hệ số điểm phải là số tự nhiên trong khoảng 1 đến 9"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tối đa điểm/Học kì:
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtMaxMarksPerTermAdd" runat="server" CssClass="input_textbox" Style="font-family: arial;
                            text-align: right"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="MaxMarksPerTermRequiredAdd" runat="server" ControlToValidate="TxtMaxMarksPerTermAdd"
                            ValidationGroup="AddLoaiDiem" ErrorMessage="Tối đa điểm/Học kì không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tính ĐTB:
                    </td>
                    <td>
                        Có&nbsp;<asp:RadioButton ID="RbtnYesAdd" runat="server" GroupName="RbtnGroupAdd" />&nbsp;&nbsp;
                        Không&nbsp;<asp:RadioButton ID="RbtnCancelAdd" runat="server" GroupName="RbtnGroupAdd"
                            Checked="true" /><br />
                        <asp:Label ID="LblAppCalAvgMarkAdd" runat="server" ForeColor="Red" Text="Đã tồn tại loại điểm khác dùng tính ĐTB"
                            Visible="false"></asp:Label>
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
            <asp:ImageButton ID="BtnSaveAdd" runat="server" ValidationGroup="AddLoaiDiem" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnSaveAdd_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopAdd_CancelSave_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="Sửa loại điểm"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td style="width: 80px; vertical-align: top; padding-top: 3px;">
                        <asp:Label ID="Label2" runat="server" Text="Tên:"></asp:Label>
                        <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtSuaMarkTypeName" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:HiddenField ID="hfOutputEdit" runat="server" Value="true" />
                        <asp:RequiredFieldValidator ID="MarkTypeNameRequiredEdit" runat="server" ControlToValidate="TxtSuaMarkTypeName"
                            ValidationGroup="EditLoaiDiem" ErrorMessage="Tên loại điểm không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="MarkTypeNameValidatorEdit" runat="server" ControlToValidate="TxtSuaMarkTypeName"
                            ValidationGroup="EditLoaiDiem" ErrorMessage="Loại điểm đã tồn tại" Display="Dynamic"
                            ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label10" runat="server" Text="Hệ số điểm:"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtMarkRatioLoaiDiemSua" runat="server" CssClass="input_textbox"
                            Style="font-family: arial; text-align: right">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="MarkRatioRequiredEdit" runat="server" ControlToValidate="TxtMarkRatioLoaiDiemSua"
                            ValidationGroup="EditLoaiDiem" ErrorMessage="Hệ số điểm không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="MarkRatioRegExpEdit" runat="server" ControlToValidate="TxtMarkRatioLoaiDiemSua"
                            ValidationGroup="EditLoaiDiem" ValidationExpression="\d{1}" Display="Dynamic"
                            ForeColor="Red" ErrorMessage="Hệ số điểm phải là số tự nhiên trong khoảng 1 đến 9"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tối đa điểm/Học kì:
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtMaxMarksPerTermEdit" runat="server" CssClass="input_textbox"
                            Style="font-family: arial; text-align: right"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="MaxMarksPerTermRequiredEdit" runat="server" ControlToValidate="TxtMaxMarksPerTermEdit"
                            ValidationGroup="EditLoaiDiem" ErrorMessage="Tối đa điểm/Học kì không được để trống"
                            Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tính ĐTB:
                    </td>
                    <td>
                        Có&nbsp;<asp:RadioButton ID="RbtnYesEdit" runat="server" GroupName="RbtnGroupEdit" />&nbsp;&nbsp;
                        Không&nbsp;<asp:RadioButton ID="RbtnCancelEdit" runat="server" GroupName="RbtnGroupEdit"
                            Checked="true" /><br />
                        <asp:Label ID="LblAppCalAvgMarkEdit" runat="server" ForeColor="Red" Text="Đã tồn tại loại điểm khác dùng tính ĐTB"
                            Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label11" runat="server" Text="*" ForeColor="Red"></asp:Label>
            <asp:Label ID="Label12" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnSaveEdit_Click" ValidationGroup="EditLoaiDiem" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopEdit_Cancel_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
