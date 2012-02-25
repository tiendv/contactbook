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
                var TermId = DdlHocKy.options[DdlHocKy.selectedIndex].value;
                var ngay = $get('<%=TxtNgayThem.ClientID%>').value;

                $.ajax({
                    type: "POST",
                    url: "/Modules/Hoc_Sinh/HocSinhServicePage.aspx/HoatDongExists",
                    data: "{'maHoatDong':'" + 0 + "','tieuDe':'" + tieuDe
                        + "','maHocSinh':'" + maHocSinh
                        + "','TermId':'" + TermId + "','ngay':'" + ngay + "'}",
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
                var TermId = $get('<%=HdfMaHocSinh.ClientID%>').value;
                var ngay = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Hoc_Sinh/HocSinhServicePage.aspx/HoatDongExists",
                    data: "{'maHoatDong':'" + maHoatDong + "','tieuDe':'" + tieuDe
                        + "','maHocSinh':'" + maHocSinh
                        + "','TermId':'" + TermId + "','ngay':'" + ngay + "'}",
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
    <div style="padding: 10px 0px 10px 0px; font-size: 14px; font-weight: bold">
        Họ và tên:
        <asp:Label ID="LblStudentName" runat="server"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;Mã học sinh:
        <asp:Label ID="LblStudentCode" runat="server"></asp:Label>
        <br />
    </div>
    <div style="padding: 0px 0px 20px 0px">
        <asp:Repeater ID="RptStudentFunctions" runat="server" OnItemDataBound="RptStudentFunctions_ItemDataBound"
            OnItemCommand="RptStudentFunctions_ItemCommand">
            <ItemTemplate>
                <asp:LinkButton ID="LkBtnStudentPage" runat="server" CssClass="tabHeader" CommandName="Redirect"
                    CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PhysicalPath")%>'><%#DataBinder.Eval(Container.DataItem, "PageTitle")%></asp:LinkButton>
                <asp:HiddenField ID="HdfPhysicalPath" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "PhysicalPath")%>' />
                &nbsp;&nbsp;&nbsp;&nbsp;
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
                                    PopupButtonID="ImgCalendarTuNgay" PopupPosition="Right" Format="dd/MM/yyyy">
                                </ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server" TargetControlID="TxtTuNgay"
                                    MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                                </ajaxToolkit:MaskedEditExtender>
                                <br />
                                <asp:RequiredFieldValidator ID="BeginDateRequired" runat="server" ControlToValidate="TxtTuNgay"
                                    ValidationGroup="AddHoatDong" ErrorMessage="Ngày không được để trống" Display="Dynamic"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="BeginDateCustom" runat="server" ErrorMessage="Ngày không hợp lệ"
                                    ValidationGroup="AddHoatDong" Display="Dynamic" ForeColor="Red" ControlToValidate="TxtTuNgay"></asp:CustomValidator>
                            </td>
                            <td>
                                Đến ngày:
                            </td>
                            <td>
                                <asp:TextBox ID="TxtDenNgay" runat="server" Width="125px"></asp:TextBox>
                                <asp:Image ID="ImgCalendarDenNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                                    ImageAlign="Middle" />
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="TxtDenNgay"
                                    PopupButtonID="ImgCalendarDenNgay" PopupPosition="Right" Format="dd/MM/yyyy">
                                </ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender6" runat="server" TargetControlID="TxtDenNgay"
                                    MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                                </ajaxToolkit:MaskedEditExtender>
                                <br />
                                <asp:RequiredFieldValidator ID="EndDateRequired" runat="server" ControlToValidate="TxtDenNgay"
                                    ValidationGroup="AddHoatDong" ErrorMessage="Ngày không được để trống" Display="Dynamic"
                                    ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="EndDateValidator" runat="server" ErrorMessage="Ngày không hợp lệ"
                                    ValidationGroup="AddHoatDong" Display="Dynamic" ForeColor="Red" ControlToValidate="TxtDenNgay"></asp:CustomValidator>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm hoạt động" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/buttons/button_add.png"
                ToolTip="Thêm hoạt động mới" CssClass="BtnAdd" />
            <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAdd"
                PopupControlID="PnlPopupAdd" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupAdd"
                PopupDragHandleControlID="PnlDragPopupAdd">
            </ajaxToolkit:ModalPopupExtender>
            <asp:ImageButton ID="BtnEdit" runat="server" OnClick="BtnEdit_Click" ImageUrl="~/Styles/buttons/button_edit.png"
                ToolTip="Sửa hoạt động" CssClass="BtnEdit" />
            <asp:ImageButton ID="BtnFakedEdit" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakedEdit"
                PopupControlID="PnlPopupEdit" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit"
                PopupDragHandleControlID="PnlDragPopupEdit">
            </ajaxToolkit:ModalPopupExtender>
            <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Styles/buttons/button_delete.png"
                ToolTip="Xóa hoạt động" CssClass="BtnDelete" />
            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnDelete"
                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
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
            <asp:Repeater ID="RptHoatDong" runat="server" OnItemDataBound="RptHoatDong_ItemDataBound">
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
                        <td id="thSelectAll" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelectAll" runat="server" CssClass="selectAll" />
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
                        <td id="tdSelect" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
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
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/buttons/popup_button_close.png"
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
                        <asp:TextBox ID="TxtDescriptionThem" runat="server" TextMode="MultiLine" CssClass="input_textbox"
                            Style="font-family: arial;">
                        </asp:TextBox>
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
                            PopupButtonID="ImgCalendarNgayThem" PopupPosition="Right" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>
                        <asp:RequiredFieldValidator ID="NgayRequiredAdd" runat="server" ControlToValidate="TxtNgayThem"
                            ValidationGroup="AddHoatDong" ErrorMessage="Ngày không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <%--<asp:RegularExpressionValidator ID="NgayExpression" runat="server" ControlToValidate="TxtNgayThem"
                            ValidationExpression="^(|([1-9])|(1[0-2]))\/(([1-9])|(1\d)|(2\d)|(3[0-1]))\/((\d{4}))$"
                            ValidationGroup="AddHoatDong" ErrorMessage="Ngày không hợp lệ" Display="Dynamic"
                            ForeColor="Red">
                        </asp:RegularExpressionValidator>--%>
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
                ImageUrl="~/Styles/buttons/button_save.png" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" OnClientClick="return popopAdd_CancelSave_Click();"
                ImageUrl="~/Styles/buttons/button_cancel.png" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="400px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="Sửa hoạt động"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/buttons/popup_button_close.png"
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
                        <asp:TextBox ID="TxtDescriptionSua" runat="server" TextMode="MultiLine" CssClass="input_textbox"
                            Style="font-family: arial;">
                        </asp:TextBox>
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
                            PopupButtonID="ImgCalendarNgaySua" PopupPosition="Right" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender8" runat="server" TargetControlID="TxtNgaySua"
                            MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                        <br />
                        <asp:RequiredFieldValidator ID="NgayRequiredEdit" runat="server" ControlToValidate="TxtNgaysua"
                            ValidationGroup="EditHoatDong" ErrorMessage="Ngày không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <%--<asp:RegularExpressionValidator ID="NgayExpressionEdit" runat="server" ControlToValidate="TxtNgaySua"
                            ValidationExpression="^(|([1-9])|(1[0-2]))\/(([1-9])|(1\d)|(2\d)|(3[0-1]))\/((\d{4}))$"
                            ValidationGroup="EditHoatDong" ErrorMessage="Ngày không hợp lệ" Display="Dynamic"
                            ForeColor="Red">
                        </asp:RegularExpressionValidator>--%>
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
            <asp:ImageButton ID="BtnSaveEdit" runat="server" ValidationGroup="EditHoatDong" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnSaveEdit_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopEdit_Cancel_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa hoạt động" CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa hoạt động này không?"></asp:Label>
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
    <div style="padding: 5px; vertical-align: middle;">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/buttons/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/buttons/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
    <asp:HiddenField ID="HdfMaHocSinh" runat="server" />
    <asp:HiddenField ID="HdfTermId" runat="server" />
    <asp:HiddenField ID="HdfTieuDe" runat="server" />
    <asp:HiddenField ID="HdfNgay" runat="server" />
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
                <asp:Label ID="Label6" runat="server" Text="Một vài hoạt động không thể xóa vì thông tin đang được xác nhận bởi phụ huynh"></asp:Label>
                <br />
            </div>
        </div>
        <div style="width: 85px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="BtnClose" runat="server" ImageUrl="~/Styles/buttons/button_close.png"
                OnClientClick="return popopInfoInUse_Close();" CssClass="BtnClose" />
        </div>
    </asp:Panel>
</asp:Content>
