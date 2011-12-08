﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="ngaynghihoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.StudentAbsentPage" %>

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
                var mPEEditID = $get('<%=HdfRptNgayNghiMPEEdit.ClientID%>').value;
                $find(mPEEditID).hide();
                return false;
            }

            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptNgayNghiMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }            
        </script>
        <script language="javascript" type="text/javascript">
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

            function validateNgayNghiAdd(ctrl, args) {
                var maHocSinh = $get('<%=HdfMaHocSinh.ClientID%>').value;
                var DdlHocKy = $get('<%=DdlHocKyThem.ClientID%>');
                var TermId = DdlHocKy.options[DdlHocKy.selectedIndex].value;
                var ngay = $.trim(args.Value);
                var DdlBuoi = $get('<%=DdlBuoiThem.ClientID%>');
                var SessionId = DdlBuoi.options[DdlBuoi.selectedIndex].value;
                $.ajax({
                    type: "POST",
                    url: "/modules/hoc_sinh/hocsinhservicepage.aspx/NgayNghiHocExists",
                    data: "{'maNgayNghiHoc':'" + 0 + "','maHocSinh':'" + maHocSinh
                        + "','TermId':'" + TermId + "','ngay':'" + ngay
                        + "','SessionId':'" + SessionId + "'}",
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

            function validateNgayNghiEdit(ctrl, args) {
                var maNgayNghiHoc = $get('<%=HdfMaNgayNghiHoc.ClientID%>').value;
                var maHocSinh = $get('<%=HdfMaHocSinh.ClientID%>').value;
                var DdlHocKy = $get('<%=DdlHocKySua.ClientID%>');
                var TermId = DdlHocKy.options[DdlHocKy.selectedIndex].value;
                var ngay = $.trim(args.Value);
                var DdlBuoi = $get('<%=DdlBuoiSua.ClientID%>');
                var SessionId = DdlBuoi.options[DdlBuoi.selectedIndex].value;

                $.ajax({
                    type: "POST",
                    url: "/Modules/Hoc_Sinh/HocSinhServicePage.aspx/NgayNghiHocExists",
                    data: "{'maNgayNghiHoc':'" + maNgayNghiHoc + "','maHocSinh':'" + maHocSinh
                        + "','TermId':'" + TermId + "','ngay':'" + ngay
                        + "','SessionId':'" + SessionId + "'}",
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
    <asp:HiddenField ID="HdfMaHocSinh" runat="server" />
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
                                <ajaxtoolkit:calendarextender id="CalendarExtender1" runat="server" targetcontrolid="TxtTuNgay"
                                    popupbuttonid="ImgCalendarTuNgay" popupposition="Right">
                                </ajaxtoolkit:calendarextender>
                                <ajaxtoolkit:maskededitextender id="MaskedEditExtender1" runat="server" targetcontrolid="TxtTuNgay"
                                    masktype="Date" mask="99/99/9999" errortooltipenabled="true">
                                </ajaxtoolkit:maskededitextender>
                            </td>
                            <td>
                                Đến ngày:
                            </td>
                            <td>
                                <asp:TextBox ID="TxtDenNgay" runat="server" Width="125px"></asp:TextBox>
                                <asp:Image ID="ImgCalendarDenNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                                    ImageAlign="Middle" />
                                <ajaxtoolkit:calendarextender id="CalendarExtender2" runat="server" targetcontrolid="TxtDenNgay"
                                    popupbuttonid="ImgCalendarDenNgay" popupposition="Right">
                                </ajaxtoolkit:calendarextender>
                                <ajaxtoolkit:maskededitextender id="MaskedEditExtender2" runat="server" targetcontrolid="TxtDenNgay"
                                    masktype="Date" mask="99/99/9999" errortooltipenabled="true">
                                </ajaxtoolkit:maskededitextender>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm ngày nghỉ học" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm ngày nghỉ học mới" CssClass="BtnAdd" />
            <ajaxtoolkit:modalpopupextender id="MPEAdd" runat="server" targetcontrolid="BtnAdd"
                popupcontrolid="PnlPopupAdd" backgroundcssclass="modalBackground" cancelcontrolid="ImgClosePopupAdd"
                popupdraghandlecontrolid="PnlDragPopupAdd">
            </ajaxtoolkit:modalpopupextender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfMaNgayNghiHoc" runat="server" />
            <asp:HiddenField ID="HdfRptNgayNghiMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptNgayNghiMPEEdit" runat="server" />
            <asp:Repeater ID="RptNgayNghi" runat="server" OnItemCommand="RptNgayNghi_ItemCommand"
                OnItemDataBound="RptNgayNghi_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td class="middle" style="width: 20%">
                            <asp:LinkButton ID="LkBtnNgay" runat="server">Ngày</asp:LinkButton>
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LkBtnBuoi" runat="server">Buổi</asp:LinkButton>
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LkBtnCoPhep" runat="server">Có phép</asp:LinkButton>
                        </td>
                        <td class="middle" style="width: 150px">
                            Lý do
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LkBtnXacNhan" runat="server">Phụ huynh xác nhận</asp:LinkButton>
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
                            <asp:HiddenField ID="HdfRptMaNgayNghiHoc" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "AbsentId")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label40" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Date")%>'></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label41" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Session")%>'></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label42" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "IsAsked")%>'></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label43" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Reason")%>'></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label44" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Confirmed")%>'></asp:Label>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeEditItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CommandEdit" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AbsentId")%>' />
                            <ajaxtoolkit:modalpopupextender id="MPEEdit" runat="server" targetcontrolid="BtnFakeEditItem"
                                popupcontrolid="PnlPopupEdit" backgroundcssclass="modalBackground" cancelcontrolid="ImgClosePopupEdit"
                                popupdraghandlecontrolid="PnlDragPopupEdit">
                            </ajaxtoolkit:modalpopupextender>
                        </td>
                        <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CommandName="CmdDeleteItem"/>
                            <ajaxtoolkit:modalpopupextender id="MPEDelete" runat="server" targetcontrolid="BtnFakeDeleteItem"
                                popupcontrolid="PnlPopupConfirmDelete" backgroundcssclass="modalBackground" cancelcontrolid="imgClosePopupConfirmDelete"
                                popupdraghandlecontrolid="PnlDragPopupConfirmDelete">
                            </ajaxtoolkit:modalpopupextender>
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
            <cc1:datapager id="MainDataPager" runat="server" ofclause="/" pageclause="TRANG"
                oncommand="MainDataPager_Command" pagesize="10" viewstatemode="Enabled" lastclause=">>"
                generatehiddenhyperlinks="False" compactmodepagecount="3" generatefirstlastsection="True"
                generategotosection="False" firstclause="<<" backtofirstclause="Trở về trang đầu"
                backtopageclause="Trở về trang" gotolastclause="Đến trang cuối" nexttopageclause="Đến trang"
                showresultclause="Hiển thị kết quả" toclause="đến" />
        </div>
    </div>
    <asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup ui-corner-all" Width="300px">
        <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupAddTitle" runat="server" CssClass="popup_header_title"
                Text="Thêm ngày nghỉ học"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td style="width: 70px; vertical-align: text-top; padding-top: 3px;">
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
                        <asp:Label ID="LblAsterisk1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtNgayThem" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarNgayThem" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Middle" />
                        <ajaxtoolkit:calendarextender id="CalendarExtender3" runat="server" targetcontrolid="TxtNgayThem"
                            popupbuttonid="ImgCalendarNgayThem" popupposition="Right">
                        </ajaxtoolkit:calendarextender>
                        <br />
                        <asp:RequiredFieldValidator ID="NgayRequiredAdd" runat="server" ValidationGroup="GroupAdd"
                            ControlToValidate="TxtNgayThem" Display="Dynamic" ForeColor="Red" ErrorMessage="Ngày không được để trống"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="NgayExpression" runat="server" ControlToValidate="TxtNgayThem"
                            ValidationExpression="^(|([1-9])|(1[0-2]))\/(([1-9])|(1\d)|(2\d)|(3[0-1]))\/((\d{4}))$"
                            ValidationGroup="GroupAdd" ErrorMessage="Ngày không hợp lệ" Display="Dynamic"
                            ForeColor="Red">
                        </asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="DateTimeValidatorAdd" runat="server" ControlToValidate="TxtNgayThem"
                            ValidationGroup="GroupAdd" ErrorMessage="Ngày không hợp lệ" ClientValidationFunction="validateDateTime"
                            Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                        <asp:CustomValidator ID="NgayValidatorAdd" runat="server" ControlToValidate="TxtNgayThem"
                            ValidationGroup="GroupAdd" ErrorMessage="Ngày nghỉ này đã tồn tại" ClientValidationFunction="validateNgayNghiAdd"
                            Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Buổi:
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlBuoiThem" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Xin phép:
                    </td>
                    <td>
                        <asp:RadioButton ID="RbtnCo" runat="server" GroupName="XinPhepThem" Checked="true" />
                        Có&nbsp;&nbsp;
                        <asp:RadioButton ID="RbtnKhong" runat="server" GroupName="XinPhepThem" />
                        Không
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; padding-top: 3px">
                        Lý do:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtLyDoThem" runat="server" TextMode="MultiLine" CssClass="input_textbox"
                            Style="font-family: arial;">
                        </asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label31" runat="server" Text="*" ForeColor="Red"></asp:Label>
            :Thông tin bắt buộc nhập<br />
            <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
            <asp:Label ID="Label35" runat="server" Text="Thêm tiếp sau khi lưu"></asp:Label>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveAdd" runat="server" OnClick="BtnSaveAdd_Click" ValidationGroup="GroupAdd"
                ImageUrl="~/Styles/Images/button_save.png" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopAdd_CancelSave_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="300px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="Sửa ngày nghỉ học"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table class="inputBorder" style="width: 100%;">
                <tr>
                    <td style="width: 70px; vertical-align: text-top; padding-top: 3px;">
                        Học kỳ:
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlHocKySua" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; padding-top: 3px;">
                        Ngày:
                        <asp:Label ID="Label46" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtNgaySua" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarNgaySua" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Middle" />
                        <ajaxtoolkit:calendarextender id="CalendarExtender4" runat="server" targetcontrolid="TxtNgaySua"
                            popupbuttonid="ImgCalendarNgaySua" popupposition="Right">
                        </ajaxtoolkit:calendarextender>
                        <br />
                        <asp:RequiredFieldValidator ID="NgayRequiredEdit" runat="server" ValidationGroup="EditNgayNghiHoc"
                            ControlToValidate="TxtNgaySua" Display="Dynamic" ForeColor="Red" ErrorMessage="Ngày không được để trống"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="NgayExpressionEdit" runat="server" ControlToValidate="TxtNgaySua"
                            ValidationExpression="^(|([1-9])|(1[0-2]))\/(([1-9])|(1\d)|(2\d)|(3[0-1]))\/((\d{4}))$"
                            ValidationGroup="EditNgayNghiHoc" ErrorMessage="Ngày không hợp lệ" Display="Dynamic"
                            ForeColor="Red">
                        </asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="DateTimeValidatorEdit" runat="server" ControlToValidate="TxtNgaySua"
                            ValidationGroup="EditNgayNghiHoc" ErrorMessage="Ngày không hợp lệ" ClientValidationFunction="validateDateTime"
                            Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                        <asp:CustomValidator ID="NgayValidatorEdit" runat="server" ValidationGroup="EditNgayNghiHoc"
                            ControlToValidate="TxtNgaySua" Display="Dynamic" ForeColor="Red" ClientValidationFunction="validateNgayNghiEdit"
                            ErrorMessage="Ngày nghỉ này đã tồn tại"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Buổi:
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlBuoiSua" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Xin phép:
                    </td>
                    <td>
                        <asp:RadioButton ID="RbtnCoSua" runat="server" GroupName="XinPhepSua" Checked="true" />
                        Có&nbsp;&nbsp;
                        <asp:RadioButton ID="RbtnKhongSua" runat="server" GroupName="XinPhepSua" />
                        <asp:Label ID="Label50" runat="server" Text="Không"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; padding-top: 3px">
                        Lý do:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtLyDoSua" runat="server" TextMode="MultiLine" CssClass="input_textbox"
                            Style="font-family: arial;">
                        </asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding: 5px 7px 5px 7px;">
            <asp:Label ID="Label52" runat="server" Text="*" ForeColor="Red"></asp:Label>
            :Thông tin bắt buộc nhập
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" OnClick="BtnSaveEdit_Click" ValidationGroup="EditNgayNghiHoc"
                ImageUrl="~/Styles/Images/button_save.png" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopEdit_CancelSave_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa ngày nghỉ học"
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
    <div style="padding: 5px; vertical-align: middle;">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
</asp:Content>
