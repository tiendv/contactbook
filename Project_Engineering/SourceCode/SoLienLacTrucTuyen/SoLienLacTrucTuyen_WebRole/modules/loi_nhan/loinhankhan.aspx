<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="LoiNhanKhan.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.LoiNhanKhan" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScripts">
        <script type="text/javascript">
            function popopAdd_CancelSave_Click() {
                $find('<%=MPEAdd.ClientID%>').hide();
                return false;
            }

            function popopEdit_CancelSave_Click() {
                var mPEEditID = $get('<%=HdfRptLoiNhanKhanMPEEdit.ClientID%>').value;
                $find(mPEEditID).hide();
                return false;
            }

            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptLoiNhanKhanMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }


        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            <table class="search">
                <tr>
                    <td>
                        Năm học:
                    </td>
                    <td style="width: 200px;">
                        <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Từ ngày:
                    </td>
                    <td style="width: 200px;">
                        <asp:TextBox ID="TxtTuNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarTuNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Top" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtTuNgay"
                            PopupButtonID="ImgCalendarTuNgay" PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="TxtTuNgay"
                            MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                    <td>
                        Xác nhận:
                    </td>
                    <td style="width: 180px">
                        <asp:DropDownList ID="DdlXacNhan" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mã học sinh:
                    </td>
                    <td style="width: 200px;">
                        <asp:TextBox ID="TxtMaHS" runat="server" Style="width: 150px;"></asp:TextBox>
                    </td>
                    <td>
                        Đến ngày:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtDenNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarDenNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Top" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtDenNgay"
                            PopupButtonID="ImgCalendarDenNgay" PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="TxtDenNgay"
                            MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm lời nhắn" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm lời nhắn khẩn mới" />
            <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAdd"
                PopupControlID="PnlPopupAdd" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupAdd"
                PopupDragHandleControlID="PnlDragPopupAdd">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfMaLoiNhanKhan" runat="server" />
            <asp:HiddenField ID="HdfRptLoiNhanKhanMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptLoiNhanKhanMPEEdit" runat="server" />
            <asp:HiddenField ID="HdfRptLoiNhanKhanMPEDetail" runat="server" />
            <asp:HiddenField ID="HdfRptLoiNhanKhanMPEDetailHS" runat="server" />
            <asp:Repeater ID="RptLoiNhanKhan" runat="server" OnItemCommand="RptLoiNhanKhan_ItemCommand"
                OnItemDataBound="RptLoiNhanKhan_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="left orderNo">
                            STT
                        </td>
                        <td class="middle" style="width: 20%">
                            Lời nhắn
                        </td>
                        <td class="middle">
                            Ngày
                        </td>
                        <td class="middle">
                            Mã học sinh
                        </td>
                        <td class="middle">
                            Tên học sinh
                        </td>
                        <td class="middle" style="width: 100px">
                            Xác nhận của phụ huynh
                        </td>
                        <td class="middle icon">
                            Sửa
                        </td>
                        <td class="right icon">
                            Xóa
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptMaLoiNhanKhan" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaLoiNhanKhan")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label28" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TieuDe")%>'></asp:Label>
                            <%--<asp:Label ID="LblTieuDe" runat="server" style="display:none"></asp:Label>
                                <asp:LinkButton ID="LbtnTieuDe" runat="server"
                                    Text='<%#DataBinder.Eval(Container.DataItem, "TieuDe")%>'
                                    style="text-decoration:underline; color:Blue;cursor:pointer;"
                                    CommandName="CmdDetailItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaLoiNhanKhan")%>'>
                                </asp:LinkButton>--%>
                            <%--<ajaxToolkit:ModalPopupExtender ID="MPEDetail" runat="server"                                         
                                    TargetControlID="LblClassName"
                                    PopupControlID="PnlPopupDetail"
                                    BackgroundCssClass="modalBackground"
                                    CancelControlID="ImgClosePopupDetail"
                                    PopupDragHandleControlID="PnlDragPopupDetail">
                                </ajaxToolkit:ModalPopupExtender>--%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "StrNgay")%>
                        </td>
                        <td style="height: 40px;">
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#"~/Modules/Hoc_Sinh/ChiTietHocSinh.aspx?HocSinh=" + DataBinder.Eval(Container.DataItem, "MaHocSinh")%>'
                                Target="_blank">
                                    <%#DataBinder.Eval(Container.DataItem, "MaHocSinhHienThi")%>
                            </asp:HyperLink>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TenHocSinh")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "XacNhan")%>
                        </td>
                        <td class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeEditItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaLoiNhanKhan")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakeEditItem"
                                PopupControlID="PnlPopupEdit" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit"
                                PopupDragHandleControlID="PnlDragPopupEdit">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TieuDe")%>' />
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
    </div>
    <div style="float: right; margin-top: -35px; padding-right: 30px;">
        <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="MainDataPager_Command" ViewStateMode="Enabled" />
    </div>
    <asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup ui-corner-all" Width="550px">
        <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupAddTitle" runat="server" Text="Thêm lời nhắn khẩn"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanelAdd" runat="server" style="padding: 5px 7px 10px 7px;">
            <ContentTemplate>
                <table style="width: 100%" class="inputBorder">
                    <tr>
                        <td style="width: 60px; vertical-align: top; padding-top: 3px;">
                            Tiêu đề:
                            <asp:Label ID="Label102" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        </td>
                        <td style="width: auto;" colspan="3">
                            <asp:TextBox ID="TxtTieuDeThem" runat="server" Style="width: 99%"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="TieuDeRequiredAdd" runat="server" ControlToValidate="TxtTieuDeThem"
                                ValidationGroup="AddLoiNhanKhan" ErrorMessage="Tiêu đề không được để trống" Display="Dynamic"
                                ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 60px; vertical-align: top; padding-top: 3px;">
                            Nội dung:
                        </td>
                        <td style="width: auto;" colspan="3">
                            <asp:TextBox ID="TxtNoiDungThem" runat="server" TextMode="MultiLine" Style="width: 99%; font-family: Arial"></asp:TextBox>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: text-top; padding-top: 3px;">
                            Ngành:
                        </td>
                        <td style="width: auto;">
                            <asp:DropDownList ID="DdlNganhHocThem" runat="server" Width="150px" AutoPostBack="true"
                                OnSelectedIndexChanged="DdlNganhThem_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="vertical-align: text-top; padding-top: 3px;">
                            Khối:
                        </td>
                        <td style="width: auto;">
                            <asp:DropDownList ID="DdlKhoiLopThem" runat="server" Width="150px" AutoPostBack="true"
                                OnSelectedIndexChanged="DdlKhoiLopThem_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: text-top; padding-top: 3px;">
                            Lớp:
                        </td>
                        <td style="width: auto; vertical-align: top">
                            <asp:DropDownList ID="DdlLopThem" runat="server" Width="150px" AutoPostBack="true"
                                OnSelectedIndexChanged="DdlLopHocThem_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td style="vertical-align: text-top; padding-top: 3px;">
                            Mã học sinh:
                            <asp:Label ID="Label15" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        </td>
                        <td style="width: 150px">
                            <asp:DropDownList ID="DdlHocSinhThem" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: text-top; padding-top: 3px;">
                            Ngày:
                            <asp:Label ID="Label16" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtNgayThem" runat="server" Width="125px"></asp:TextBox>
                            <asp:Image ID="ImgCalendarNgayThem" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                                ImageAlign="Top" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="TxtNgayThem"
                                PopupButtonID="ImgCalendarNgayThem" PopupPosition="Right">
                            </ajaxToolkit:CalendarExtender>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server" TargetControlID="TxtNgayThem"
                                MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                            </ajaxToolkit:MaskedEditExtender>
                            <br />
                            <asp:RequiredFieldValidator ID="NgayRequiredAdd" runat="server" ControlToValidate="TxtNgayThem"
                                ValidationGroup="AddLoiNhanKhan" ErrorMessage="Ngày không được để trống" Display="Dynamic"
                                ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="HocSinhRequiredAdd" runat="server" ControlToValidate="DdlHocSinhThem"
                                ValidationGroup="AddLoiNhanKhan" ErrorMessage="Mã học sinh không được để trống"
                                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <div style="padding: 10px 0px 0px 0px;">
                    <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    :Thông tin bắt buộc nhập<br />
                    <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
                    Thêm tiếp sau khi lưu
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 10px 0px">
            <asp:ImageButton ID="BtnSaveAdd" runat="server" OnClick="BtnSaveAdd_Click" ValidationGroup="AddLoiNhanKhan"
                ImageUrl="~/Styles/Images/button_save.png" CssClass="SaveButton" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" OnClientClick="return popopAdd_CancelSave_Click();"
                ImageUrl="~/Styles/Images/button_cancel.png" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="550px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" Text="Sửa lời nhắn khẩn"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table style="width: 100%;" class="inputBorder">
                <tr>
                    <td style="vertical-align: top; padding-top: 3px;">
                        Tiêu đề:
                    </td>
                    <td style="width: auto;" colspan="3" class="readOnlyTextBox">
                        <asp:Label ID="LblTieuDeSua" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 60px; vertical-align: top; padding-top: 3px;">
                        Nội dung:
                    </td>
                    <td style="width: auto;" colspan="3">
                        <asp:TextBox ID="TxtNoiDungSua" runat="server" TextMode="MultiLine" Style="width: 99%"></asp:TextBox>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Ngành:
                    </td>
                    <td style="width: 145px;" class="readOnlyTextBox">
                        <asp:Label ID="LblNganhHocSua" runat="server"></asp:Label>
                    </td>
                    <td style="vertical-align: text-top; padding-top: 3px; padding-left: 40px">
                        Khối:
                    </td>
                    <td style="width: 145px;" class="readOnlyTextBox">
                        <asp:Label ID="LblKhoiSua" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Lớp:
                    </td>
                    <td class="readOnlyTextBox">
                        <asp:Label ID="LblLopSua" runat="server"></asp:Label>
                    </td>
                    <td style="width: 100px; vertical-align: text-top; padding-top: 3px; padding-left: 40px">
                        Mã học sinh:
                    </td>
                    <td class="readOnlyTextBox">
                        <asp:Label ID="LblMaHocSinhSua" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Ngày:
                        <asp:Label ID="Label25" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtNgaySua" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarNgaySua" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Top" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="TxtNgaySua"
                            PopupButtonID="ImgCalendarNgaySua" PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" runat="server" TargetControlID="TxtNgaySua"
                            MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                        <br />
                        <asp:RequiredFieldValidator ID="NgayRequiredEdit" runat="server" ControlToValidate="TxtNgaySua"
                            ValidationGroup="EditLoiNhanKhan" ErrorMessage="Ngày không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <div style="padding: 10px 0px 0px 0px;">
                <asp:Label ID="Label26" runat="server" Text="*" ForeColor="Red"></asp:Label>
                :Thông tin bắt buộc nhập
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 10px 0px">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" OnClick="BtnSaveEdit_Click" ValidationGroup="EditLoiNhanKhan"
                ImageUrl="~/Styles/Images/button_save.png" CssClass="SaveButton" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" OnClientClick="return popopEdit_CancelSave_Click();"
                ImageUrl="~/Styles/Images/button_cancel.png" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa lời nhắn khẩn"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa lời nhắn khẩn này không?"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_yes.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="YesButton" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_no.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="NoButton" />
        </div>
    </asp:Panel>
</asp:Content>
