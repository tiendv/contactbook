<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="hoatdong.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.StudentActivityPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script type="text/javascript">
            function popopAdd_CancelSave_Click() {
                $find('<%=MPEAdd.ClientID%>').hide();
                return false;
            }

            function popopEdit_CancelSave_Click() {
                var mPEEditID = $get('<%=HdfRptHoatDongMPEEdit.ClientID%>').value;
                $find(mPEEditID).hide();
                return false;
            }

            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptHoatDongMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }

            function validateDateTime(ctrl, args) {
                var ngay = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/modules/hoc_sinh/hocsinhservicepage.aspx/ValidateDateTime",
                    data: "{'dateTime':'" + ngay + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            args.IsValid = false;
                        } else {
                            args.IsValid = true;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }

            function validateHoatDongAdd(ctrl, args) {
                var tieuDe = $.trim(args.Value);
                var maHocSinh = $get('<%=HdfMaHocSinh.ClientID%>').value;
                var DdlHocKy = $get('<%=DdlHocKyThem.ClientID%>');
                var maHocKy = DdlHocKy.options[DdlHocKy.selectedIndex].value;
                var ngay = $get('<%=TxtNgayThem.ClientID%>').value;

                $.ajax({
                    type: "POST",
                    url: "/Modules/Hoc_Sinh/HocSinhServicePage.aspx/HoatDongExists",
                    data: "{'maHoatDong':'" + 0 + "','tieuDe':'" + tieuDe
                        + "','maHocSinh':'" + maHocSinh
                        + "','maHocKy':'" + maHocKy + "','ngay':'" + ngay + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            args.IsValid = false;
                        } else {
                            args.IsValid = true;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }

            function validateHoatDongEdit(ctrl, args) {
                var maHoatDong = $get('<%=HdfMaHoatDong.ClientID%>').value;
                var tieuDe = $get('<%=HdfTieuDe.ClientID%>').value;
                var maHocSinh = $get('<%=HdfMaHocSinh.ClientID%>').value;
                var maHocKy = $get('<%=HdfMaHocSinh.ClientID%>').value;
                var ngay = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Hoc_Sinh/HocSinhServicePage.aspx/HoatDongExists",
                    data: "{'maHoatDong':'" + maHoatDong + "','tieuDe':'" + tieuDe
                        + "','maHocSinh':'" + maHocSinh
                        + "','maHocKy':'" + maHocKy + "','ngay':'" + ngay + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            args.IsValid = false;
                        } else {
                            args.IsValid = true;
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            } 
        </script>
    </div>
    <div>
        <asp:Repeater ID="RptStudentFunctions" runat="server">
            <ItemTemplate>
                <asp:HyperLink ID="HlkStudentFunctionPage" runat="server" CssClass="tabHeader" NavigateUrl='<%#DataBinder.Eval(Container.DataItem, "PhysicalPath")%>'>
                    <%#DataBinder.Eval(Container.DataItem, "PageTitle")%>
                </asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="search">
                        <tr>
                            <td style="width: 60px;">
                                Năm học:
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 63px;">
                                Học kỳ:
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Từ ngày:
                            </td>
                            <td>
                                <asp:TextBox ID="TxtTuNgay" runat="server" Width="125px"></asp:TextBox>
                                <asp:Image ID="ImgCalendarTuNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                                    ImageAlign="Middle" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="TxtTuNgay"
                                    PopupButtonID="ImgCalendarTuNgay" PopupPosition="Right">
                                </ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="TxtTuNgay"
                                    MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                                </ajaxToolkit:MaskedEditExtender>
                            </td>
                            <td>
                                Đến ngày:
                            </td>
                            <td>
                                <asp:TextBox ID="TxtDenNgay" runat="server" Width="125px"></asp:TextBox>
                                <asp:Image ID="ImgCalendarDenNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                                    ImageAlign="Middle" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="TxtDenNgay"
                                    PopupButtonID="ImgCalendarDenNgay" PopupPosition="Right">
                                </ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender6" runat="server" TargetControlID="TxtDenNgay"
                                    MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                                </ajaxToolkit:MaskedEditExtender>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm hoạt động" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm hoạt động mới" CssClass="BtnAdd" />
            <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAdd"
                PopupControlID="PnlPopupAdd" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupAdd"
                PopupDragHandleControlID="PnlDragPopupAdd">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfSltActivityName" runat="server" />
            <asp:HiddenField ID="HdfMaHoatDong" runat="server" />
            <asp:HiddenField ID="HdfRptHoatDongMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptHoatDongMPEEdit" runat="server" />
            <asp:Repeater ID="RptHoatDong" runat="server" OnItemCommand="RptHoatDong_ItemCommand"
                OnItemDataBound="RptHoatDong_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LkBtnTenHoatDong" runat="server">Hoạt động</asp:LinkButton>
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LkBtnNgay" runat="server">Ngày</asp:LinkButton>
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LkBtnThaiDoThamGia" runat="server">Thái độ tham gia</asp:LinkButton>
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
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                            <asp:HiddenField ID="HdfRptMaHoatDong" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaHoatDong")%>' />
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TenHoatDong")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "StrNgay")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "ThaiDoThamGia")%>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeEditItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaHoatDong")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakeEditItem"
                                PopupControlID="PnlPopupEdit" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit"
                                PopupDragHandleControlID="PnlDragPopupEdit">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TenHoatDong")%>' />
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
    <asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupAddTitle" runat="server" CssClass="popup_header_title"
                Text="Thêm hoạt động"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td style="width: 105px; vertical-align: top; padding-top: 3px">
                        Tiêu đề:
                        <asp:Label ID="Label62" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtTieuDeThem" runat="server" CssClass="input_textbox"></asp:TextBox><br />
                        <asp:RequiredFieldValidator ID="TieuDeRequiredAdd" runat="server" ControlToValidate="TxtTieuDeThem"
                            ValidationGroup="AddHoatDong" ErrorMessage="Tiêu đề không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="TieuDeValidatorAdd" runat="server" ErrorMessage="Hoạt động với tiêu đề và thời gian này đã tồn tại"
                            ValidationGroup="AddHoatDong" Display="Dynamic" ForeColor="Red" ClientValidationFunction="validateHoatDongAdd"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; padding-top: 3px">
                        Mô tả:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtMoTaThem" runat="server" TextMode="MultiLine" CssClass="input_textbox"
                            Style="font-family: arial;">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; padding-top: 3px">
                        Cấp độ:
                    </td>
                    <td>
                        <asp:Label ID="Label77" runat="server" Text="Cá nhân" Style="font-style: italic;
                            font-weight: bold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Thái độ tham gia:
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlThaiDoThamGiaThem" runat="server" Width="98%">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Học kỳ:
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlHocKyThem" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; padding-top: 3px;">
                        Ngày:
                        <asp:Label ID="Label59" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtNgayThem" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarNgayThem" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Middle" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="TxtNgayThem"
                            PopupButtonID="ImgCalendarNgayThem" PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <asp:RequiredFieldValidator ID="NgayRequiredAdd" runat="server" ControlToValidate="TxtNgayThem"
                            ValidationGroup="AddHoatDong" ErrorMessage="Ngày không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="NgayExpression" runat="server" ControlToValidate="TxtNgayThem"
                            ValidationExpression="^(|([1-9])|(1[0-2]))\/(([1-9])|(1\d)|(2\d)|(3[0-1]))\/((\d{4}))$"
                            ValidationGroup="AddHoatDong" ErrorMessage="Ngày không hợp lệ" Display="Dynamic"
                            ForeColor="Red">
                        </asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="DateTimeValidatorAdd" runat="server" ControlToValidate="TxtNgayThem"
                            ValidationGroup="AddHoatDong" ErrorMessage="Ngày không hợp lệ" ClientValidationFunction="validateDateTime"
                            Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label65" runat="server" Text="*" ForeColor="Red"></asp:Label>
            :Thông tin bắt buộc nhập<br />
            <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
            Thêm tiếp sau khi lưu
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveAdd" runat="server" OnClick="BtnSaveAdd_Click" ValidationGroup="AddHoatDong"
                ImageUrl="~/Styles/Images/button_save.png" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" OnClientClick="return popopAdd_CancelSave_Click();"
                ImageUrl="~/Styles/Images/button_cancel.png" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="Sửa hoạt động"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td style="width: 105px; vertical-align: top; padding-top: 3px">
                        Tiêu đề:
                        <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td class="readOnlyTextBox">
                        <asp:Label ID="LblTieuDeSua" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; padding-top: 3px">
                        Mô tả:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtMoTaSua" runat="server" TextMode="MultiLine" CssClass="input_textbox"
                            Style="font-family: arial;">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; padding-top: 3px">
                        Cấp độ:
                    </td>
                    <td>
                        <asp:Label ID="Label72" runat="server" Text="Cá nhân" Style="font-style: italic;
                            font-weight: bold"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Thái độ tham gia:
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlThaiDoThamGiaSua" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Học kỳ:
                    </td>
                    <td class="readOnlyTextBox" style="width: 150px;">
                        <asp:Label ID="LblHocKySua" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; padding-top: 3px;">
                        Ngày:
                        <asp:Label ID="Label74" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtNgaySua" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarNgaySua" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Middle" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="TxtNgaySua"
                            PopupButtonID="ImgCalendarNgaySua" PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender8" runat="server" TargetControlID="TxtNgaySua"
                            MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                        <br />
                        <asp:RequiredFieldValidator ID="NgayRequiredEdit" runat="server" ControlToValidate="TxtNgaysua"
                            ValidationGroup="EditHoatDong" ErrorMessage="Ngày không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="NgayExpressionEdit" runat="server" ControlToValidate="TxtNgaySua"
                            ValidationExpression="^(|([1-9])|(1[0-2]))\/(([1-9])|(1\d)|(2\d)|(3[0-1]))\/((\d{4}))$"
                            ValidationGroup="EditHoatDong" ErrorMessage="Ngày không hợp lệ" Display="Dynamic"
                            ForeColor="Red">
                        </asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="DateTimeValidatorEdit" runat="server" ControlToValidate="TxtNgaySua"
                            ValidationGroup="EditHoatDong" ErrorMessage="Ngày không hợp lệ" ClientValidationFunction="validateDateTime"
                            Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                        <asp:CustomValidator ID="NgayValidatorEdit" runat="server" ErrorMessage="Hoạt động với tiêu đề và thời gian này đã tồn tại"
                            ControlToValidate="TxtNgaySua" ValidationGroup="EditHoatDong" Display="Dynamic"
                            ForeColor="Red" ClientValidationFunction="validateHoatDongEdit"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label76" runat="server" Text="*" ForeColor="Red"></asp:Label>
            <asp:Label ID="Label78" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" ValidationGroup="EditHoatDong" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnSaveEdit_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopEdit_CancelSave_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa hoạt động" CssClass="popup_header_title"></asp:Label>
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
    <div style="padding: 5px; vertical-align: middle;">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
    <asp:HiddenField ID="HdfMaHocSinh" runat="server" />
    <asp:HiddenField ID="HdfMaHocKy" runat="server" />
    <asp:HiddenField ID="HdfTieuDe" runat="server" />
    <asp:HiddenField ID="HdfNgay" runat="server" />
</asp:Content>
