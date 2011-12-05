<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="thongbaolop.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ThongBao" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div>
        <script type="text/javascript">
            $(document).ready(function () {

                $('#<%=BtnSaveAdd.ClientID%>').click(function (event) {
                    var tieuDe = $.trim($('#<%=TxtTieuDeThem.ClientID%>').val());
                    var $errortieuDe = $('#<%=LblErrorTieuDeThem.ClientID%>');
                    if (tieuDe.length == 0) {
                        $errortieuDe.show();
                        event.preventDefault();
                    } else {
                        $errortieuDe.hide();
                    }

                    var maHocSinh = $.trim($('#<%=DdlHocSinhThem.ClientID%>').text());
                    var $errorMaHocSinh = $('#<%=LblErrorMaHocSinhThem.ClientID%>');
                    if (maHocSinh.length == 0) {
                        $errorMaHocSinh.show();
                        event.preventDefault();
                    } else {
                        $errorMaHocSinh.hide();
                    }

                    var ngay = $.trim($('#<%=TxtNgayThem.ClientID%>').val());
                    var $errorNgay = $('#<%=LblErrorNgayThem.ClientID%>');
                    if (ngay.length == 0) {
                        $errorNgay.show();
                        event.preventDefault();
                    } else {
                        $errorNgay.hide();
                    }
                });
            });
        </script>
        <script type="text/javascript">
            function popopAdd_CancelSave_Click() {
                $find('<%=MPEAdd.ClientID%>').hide();
                return false;
            }

            function popopEdit_CancelSave_Click() {
                var mPEEditID = $get('<%=HdfRptThongBaoMPEEdit.ClientID%>').value;
                $find(mPEEditID).hide();
                return false;
            }

            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptThongBaoMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }


        </script>
    </div>
    <table>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table class="search">
                            <tr>
                                <td style="width: 60px;">
                                    <asp:Label ID="Label1" runat="server" Text="Năm học:"></asp:Label>
                                </td>
                                <td style="width: 200px;">
                                    <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 70px;">
                                    <asp:Label ID="Label3" runat="server" Text="Từ ngày:"></asp:Label>
                                </td>
                                <td style="width: 200px;">
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
                                <td style="width: 60px;">
                                    <asp:Label ID="Label4" runat="server" Text="Xác nhận:"></asp:Label>
                                </td>
                                <td style="width: 180px">
                                    <asp:DropDownList ID="DdlXacNhan" runat="server" Width="157px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 80px;">
                                    <asp:Label ID="Label2" runat="server" Text="Mã học sinh:"></asp:Label>
                                </td>
                                <td style="width: 200px;">
                                    <asp:TextBox ID="TxtMaHS" runat="server" Style="width: 144px;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Đến ngày:"></asp:Label>
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
            </td>
            <td style="width: 80px; vertical-align: top; padding-top: 5px">
                <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                    ToolTip="Tìm kiếm lớp học" />
            </td>
        </tr>
    </table>
    <table class="table_data">
        <tr class="add">
            <td>
                <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/Images/button_add_with_text.png"
                    ToolTip="Thêm thông báo mới" />
                <ajaxtoolkit:modalpopupextender id="MPEAdd" runat="server" targetcontrolid="BtnAdd"
                    popupcontrolid="PnlPopupAdd" backgroundcssclass="modalBackground" cancelcontrolid="ImgClosePopupAdd"
                    popupdraghandlecontrolid="PnlDragPopupAdd">
            </ajaxtoolkit:modalpopupextender>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="repeater">
                    <asp:HiddenField ID="HdfMaThongBao" runat="server" />
                    <asp:HiddenField ID="HdfRptThongBaoMPEDelete" runat="server" />
                    <asp:HiddenField ID="HdfRptThongBaoMPEEdit" runat="server" />
                    <asp:HiddenField ID="HdfRptThongBaoMPEDetail" runat="server" />
                    <asp:HiddenField ID="HdfRptThongBaoMPEDetailHS" runat="server" />
                    <asp:Repeater ID="RptThongBao" runat="server" OnItemCommand="RptThongBao_ItemCommand"
                        OnItemDataBound="RptThongBao_ItemDataBound">
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
                                    <asp:HiddenField ID="HdfRptMaThongBao" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaThongBao")%>' />
                                </td>
                                <td style="height: 40px;">
                                    <asp:Label ID="Label28" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TieuDe")%>'></asp:Label>
                                    <%--<asp:Label ID="LblTieuDe" runat="server" style="display:none"></asp:Label>
                                <asp:LinkButton ID="LbtnTieuDe" runat="server"
                                    Text='<%#DataBinder.Eval(Container.DataItem, "TieuDe")%>'
                                    style="text-decoration:underline; color:Blue;cursor:pointer;"
                                    CommandName="CmdDetailItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaThongBao")%>'>
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
                                        CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaThongBao")%>' />
                                    <ajaxtoolkit:modalpopupextender id="MPEEdit" runat="server" targetcontrolid="BtnFakeEditItem"
                                        popupcontrolid="PnlPopupEdit" backgroundcssclass="modalBackground" cancelcontrolid="ImgClosePopupEdit"
                                        popupdraghandlecontrolid="PnlDragPopupEdit">
                                </ajaxtoolkit:modalpopupextender>
                                </td>
                                <td class="icon" style="height: 40px;">
                                    <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                                    <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                        CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TieuDe")%>' />
                                    <ajaxtoolkit:modalpopupextender id="MPEDelete" runat="server" targetcontrolid="BtnFakeDeleteItem"
                                        popupcontrolid="PnlPopupConfirmDelete" backgroundcssclass="modalBackground" cancelcontrolid="imgClosePopupConfirmDelete"
                                        popupdraghandlecontrolid="PnlDragPopupConfirmDelete">
                                </ajaxtoolkit:modalpopupextender>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="8" class="footer">
                                    <div class="left">
                                    </div>
                                    <div class="right">
                                    </div>
                                    <div class="middle">
                                    </div>
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
                <div style="float: right; margin-top: -35px; padding-right: 30px;">
                    <cc1:datapager id="MainDataPager" runat="server" ofclause="/" pageclause="TRANG" oncommand="MainDataPager_Command"
                        pagesize="10" viewstatemode="Enabled" lastclause=">>" generatehiddenhyperlinks="False"
                        compactmodepagecount="3" generatefirstlastsection="True" generategotosection="False"
                        firstclause="<<" backtofirstclause="Trở về trang đầu" backtopageclause="Trở về trang"
                        gotolastclause="Đến trang cuối" nexttopageclause="Đến trang" showresultclause="Hiển thị kết quả"
                        toclause="đến" />
                </div>
            </td>
        </tr>
    </table>
    <asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup" Width="550px">
        <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header">
            <asp:Label ID="LblPnlPopupAddTitle" runat="server" CssClass="popup_header_title"
                Text="Thêm thông báo"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table style="padding: 5px 7px 10px 7px;">
                    <tr>
                        <td>
                            <div class="inputBorder">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 60px; vertical-align: top; padding-top: 3px;">
                                            <asp:Label ID="Label101" runat="server" Text="Tiêu đề:"></asp:Label>&nbsp;
                                            <asp:Label ID="Label102" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td style="width: auto;" colspan="3">
                                            <asp:TextBox ID="TxtTieuDeThem" runat="server" CssClass="input_textbox"></asp:TextBox>
                                            <asp:Label ID="LblErrorTieuDeThem" runat="server" Text="Tiêu đề không được để trống"
                                                CssClass="error hide_error">
                                            </asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 60px; vertical-align: top; padding-top: 3px;">
                                            <asp:Label ID="Label9" runat="server" Text="Nội dung:"></asp:Label>
                                        </td>
                                        <td style="width: auto;" colspan="3">
                                            <asp:TextBox ID="TxtNoiDungThem" runat="server" TextMode="MultiLine" CssClass="input_textbox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: text-top; padding-top: 3px;">
                                            <asp:Label ID="Label8" runat="server" Text="Ngành:"></asp:Label>
                                        </td>
                                        <td style="width: auto;">
                                            <asp:DropDownList ID="DdlNganhHocThem" runat="server" Width="150px" AutoPostBack="true"
                                                OnSelectedIndexChanged="DdlNganhThem_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="vertical-align: text-top; padding-top: 3px;">
                                            <asp:Label ID="Label14" runat="server" Text="Khối:"></asp:Label>
                                        </td>
                                        <td style="width: auto;">
                                            <asp:DropDownList ID="DdlKhoiLopThem" runat="server" Width="150px" AutoPostBack="true"
                                                OnSelectedIndexChanged="DdlKhoiLopThem_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: text-top; padding-top: 3px;">
                                            <asp:Label ID="Label10" runat="server" Text="Lớp:"></asp:Label>
                                        </td>
                                        <td style="width: auto; vertical-align: top">
                                            <asp:DropDownList ID="DdlLopThem" runat="server" Width="150px" AutoPostBack="true"
                                                OnSelectedIndexChanged="DdlLopHocThem_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 100px; vertical-align: text-top; padding-top: 3px;">
                                            <asp:Label ID="Label11" runat="server" Text="Mã học sinh:"></asp:Label>
                                            <asp:Label ID="Label15" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td style="width: 150px">
                                            <asp:DropDownList ID="DdlHocSinhThem" runat="server" Width="150px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: text-top; padding-top: 3px;">
                                            <asp:Label ID="Label13" runat="server" Text="Ngày:"></asp:Label>
                                            <asp:Label ID="Label16" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtNgayThem" runat="server" Width="125px"></asp:TextBox>
                                            <asp:Image ID="ImgCalendarNgayThem" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                                                ImageAlign="Middle" />
                                            <ajaxtoolkit:calendarextender id="CalendarExtender3" runat="server" targetcontrolid="TxtNgayThem"
                                                popupbuttonid="ImgCalendarNgayThem" popupposition="Right">
                                        </ajaxtoolkit:calendarextender>
                                            <ajaxtoolkit:maskededitextender id="MaskedEditExtender3" runat="server" targetcontrolid="TxtNgayThem"
                                                masktype="Date" mask="99/99/9999" errortooltipenabled="true">
                                        </ajaxtoolkit:maskededitextender>
                                            <br />
                                            <asp:Label ID="LblErrorNgayThem" runat="server" Text="Ngày không được để trống" CssClass="error hide_error"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="LblErrorMaHocSinhThem" runat="server" Text="Mã học sinh không được để trống"
                                                CssClass="error hide_error"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="padding: 5px 0px 5px 0px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="Label6" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
                                        <asp:Label ID="Label100" runat="server" Text="Thêm tiếp sau khi lưu"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DdlNganhHocThem" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="DdlKhoiLopThem" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="DdlHocSinhThem" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <div style="width: 170px; margin: 0px auto 0px auto;">
            <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/Images/button_yes.png"
                OnClick="BtnSaveAdd_Click" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/Images/button_no.png"
                OnClientClick="return popopAdd_CancelSave_Click();" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup" Width="550px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="sửa thông báo"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <table style="padding: 5px 7px 10px 7px;">
            <tr>
                <td>
                    <div class="inputBorder">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 60px; vertical-align: top; padding-top: 3px;">
                                    <asp:Label ID="Label12" runat="server" Text="Tiêu đề:"></asp:Label>&nbsp;
                                    <asp:Label ID="Label17" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td style="width: auto;" colspan="3">
                                    <asp:TextBox ID="TxtTieuDeSua" runat="server" CssClass="input_textbox"></asp:TextBox>
                                    <asp:Label ID="LblErrorTieuDeSua" runat="server" Text="Tiêu đề không được để trống"
                                        CssClass="error hide_error">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 60px; vertical-align: top; padding-top: 3px;">
                                    <asp:Label ID="Label18" runat="server" Text="Nội dung:"></asp:Label>
                                </td>
                                <td style="width: auto;" colspan="3">
                                    <asp:TextBox ID="TxtNoiDungSua" runat="server" TextMode="MultiLine" CssClass="input_textbox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: text-top; padding-top: 3px;">
                                    <asp:Label ID="Label19" runat="server" Text="Ngành:"></asp:Label>
                                </td>
                                <td style="width: 145px;" class="readOnlyTextBox">
                                    <asp:Label ID="LblNganhHocSua" runat="server"></asp:Label>
                                </td>
                                <td style="vertical-align: text-top; padding-top: 3px; padding-left: 40px">
                                    <asp:Label ID="Label20" runat="server" Text="Khối:"></asp:Label>
                                </td>
                                <td style="width: 145px;" class="readOnlyTextBox">
                                    <asp:Label ID="LblKhoiSua" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: text-top; padding-top: 3px;">
                                    <asp:Label ID="Label21" runat="server" Text="Lớp:"></asp:Label>
                                </td>
                                <td class="readOnlyTextBox">
                                    <asp:Label ID="LblLopSua" runat="server"></asp:Label>
                                </td>
                                <td style="width: 100px; vertical-align: text-top; padding-top: 3px; padding-left: 40px">
                                    <asp:Label ID="Label22" runat="server" Text="Mã học sinh:"></asp:Label>
                                    <asp:Label ID="Label23" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td class="readOnlyTextBox">
                                    <asp:Label ID="LblMaHocSinhSua" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: text-top; padding-top: 3px;">
                                    <asp:Label ID="Label24" runat="server" Text="Ngày:"></asp:Label>
                                    <asp:Label ID="Label25" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtNgaySua" runat="server" Width="125px"></asp:TextBox>
                                    <asp:Image ID="ImgCalendarNgaySua" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                                        ImageAlign="Middle" />
                                    <ajaxtoolkit:calendarextender id="CalendarExtender4" runat="server" targetcontrolid="TxtNgaySua"
                                        popupbuttonid="ImgCalendarNgaySua" popupposition="Right">
                                        </ajaxtoolkit:calendarextender>
                                    <ajaxtoolkit:maskededitextender id="MaskedEditExtender4" runat="server" targetcontrolid="TxtNgaySua"
                                        masktype="Date" mask="99/99/9999" errortooltipenabled="true">
                                        </ajaxtoolkit:maskededitextender>
                                    <br />
                                    <asp:Label ID="LblErrorNgaySua" runat="server" Text="Ngày không được để trống" CssClass="error hide_error"></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:Label ID="LblErrorMaHocSinhSua" runat="server" Text="Mã học sinh không được để trống"
                                        CssClass="error hide_error"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="padding: 5px 0px 5px 0px;">
                        <tr>
                            <td>
                                <asp:Label ID="Label26" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                <asp:Label ID="Label27" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div style="width: 170px; margin: 0px auto 0px auto;">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/Images/button_yes.png"
                OnClick="BtnSaveEdit_Click" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/Images/button_no.png"
                OnClientClick="return popopEdit_CancelSave_Click();" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup" Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa lớp học" CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa lớp học này không?"></asp:Label>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_yes.png"
                OnClick="BtnOKDeleteItem_Click" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_no.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" />
        </div>
    </asp:Panel>
</asp:Content>
