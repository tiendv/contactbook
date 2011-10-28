<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="ketquahoctap.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.KetQuaHocTapPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script type="text/javascript">
            function popopHanhKiem_Cancel_Click() {
                var mPEHanhKiem = $get('<%=RptMPEHanhKiem.ClientID%>').value;
                $find(mPEHanhKiem).hide();
                return false;
            }
        </script>
        <script type="text/javascript">
            $(document).ready(function () {
                $('.rbtnHanhKiem').find(':radio').each(function () {
                    $(this).change(function () {
                        var $maHanhKiem = $(this).val();
                        if ($(this).is(':checked')) {
                            $.ajax({
                                type: "POST",
                                url: "/modules/hoc_sinh/hocsinhservicepage.aspx/CheckedHanhKiem",
                                data: "{'radioButtonName':'" + $maHanhKiem + "'}",
                                contentType: "application/json; charset=utf-8",
                                success: function (serverResponseData) {
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    alert('Error!');
                                }
                            });
                        }
                        else {

                        }
                    });
                });
            });
        </script>
    </div>
    <div>
        <asp:HyperLink ID="HlkThongTinCaNhan" runat="server" CssClass="tabHeader">THÔNG TIN CÁ NHÂN</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label2" runat="server" Text="KẾT QUẢ HỌC TẬP" CssClass="tabHeader"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="HlkNgayNghiHoc" runat="server" CssClass="tabHeader">NGÀY NGHỈ HỌC</asp:HyperLink>&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="HlkHoatDong" runat="server" CssClass="tabHeader">HOẠT ĐỘNG</asp:HyperLink>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            Năm học:&nbsp;
            <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px">
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp; Học kì:&nbsp;
            <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px">
            </asp:DropDownList>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div style="padding: 5px 10px 5px 10px; font-size: 15px; font-weight: bold">
            <asp:Label ID="Label14" runat="server" Text="BẢNG ĐIỂM MÔN HỌC" ForeColor="Violet">
            </asp:Label>
            <br />
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"
                Text="Chưa có thông tin kết quả học tập">
            </asp:Label>
        </div>
        <table class="repeater">
            <tr class="header">
                <td id="tdKQHocTapSTT" runat="server" class="ui-corner-tl orderNo">
                    STT
                </td>
                <td id="tdKQHocTapMonHoc" runat="server" style="width: 20%">
                    <asp:LinkButton ID="LkBtnMonHoc" runat="server">Môn học</asp:LinkButton>
                </td>
                <asp:Repeater ID="RptLoaiDiem" runat="server">
                    <ItemTemplate>
                        <td class="middle">
                            <%#DataBinder.Eval(Container.DataItem, "TenLoaiDiem")%>
                        </td>
                    </ItemTemplate>
                </asp:Repeater>
                <td id="tdKQHocTapDTB" runat="server" style="width: 70px">
                    Điểm trung bình
                </td>
                <td id="tdEdit" runat="server" class="icon">
                    Sửa
                </td>
            </tr>
            <asp:Repeater ID="RptKetQuaDiem" runat="server" OnItemDataBound="RptKetQuaDiem_ItemDataBound"
                OnItemCommand="RptKetQuaDiem_ItemCommand">
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                        </td>
                        <td>
                            <%#DataBinder.Eval(Container.DataItem, "TenMonHoc")%>
                            <asp:HiddenField ID="HdfMaDiemMonHK" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaDiemMonHK")%>' />
                        </td>
                        <asp:Repeater ID="RptDiemTheoMonHoc" runat="server">
                            <ItemTemplate>
                                <td style="height: 40px;">
                                    <%#DataBinder.Eval(Container.DataItem, "Diems")%>
                                </td>
                            </ItemTemplate>
                        </asp:Repeater>
                        <td>
                            <asp:Label ID="Label16" runat="server" Style="float: right" Text='<%#DataBinder.Eval(Container.DataItem, "StrDiemTB")%>'></asp:Label>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdDetailItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaDiemMonHK")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="9" class="footer ui-corner-bl ui-corner-br">
                        </td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
        <div style="float: right; margin-top: -45px; padding-right: 30px;">
            <cc1:DataPager ID="MainDataPager" runat="server" OfClause="/" PageClause="TRANG"
                OnCommand="MainDataPager_Command" PageSize="10" ViewStateMode="Enabled" LastClause=">>"
                GenerateHiddenHyperlinks="False" CompactModePageCount="3" GenerateFirstLastSection="True"
                GenerateGoToSection="False" FirstClause="<<" BackToFirstClause="Trở về trang đầu"
                BackToPageClause="Trở về trang" GoToLastClause="Đến trang cuối" NextToPageClause="Đến trang"
                ShowResultClause="Hiển thị kết quả" ToClause="đến" />
        </div>
        <table class="repeater">
            <asp:HiddenField ID="RptMPEHanhKiem" runat="server" />
            <asp:HiddenField ID="HdfMaDanhHieuHSHK" runat="server" />
            <asp:Repeater ID="RptDanhHieu" runat="server" OnItemCommand="RptDanhHieu_ItemCommand"
                OnItemDataBound="RptDanhHieu_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td class="middle">
                            Học kỳ
                        </td>
                        <td class="middle" style="width: 120px">
                            Điểm trung bình
                        </td>
                        <td class="middle">
                            Học lực
                        </td>
                        <td class="middle">
                            Hạnh kiểm
                        </td>
                        <td style="width: 78px">
                            Danh hiệu
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (DataPagerDanhHieu.CurrentIndex - 1) * DataPagerDanhHieu.PageSize + Container.ItemIndex + 1%>
                            <asp:HiddenField ID="HdfRptMaDanhHieuHSHK" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaDanhHieuHSHK")%>' />
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TenHocKy")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "StrDiemTB")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TenHocLuc")%>
                        </td>
                        <td style="height: 40px; vertical-align: middle">
                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaHanhKiem")%>' />
                            <%#DataBinder.Eval(Container.DataItem, "TenHanhKiem")%>
                            <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Styles/Images/button_edit_with_text.png"
                                Style="float: right;" CommandName="OpenPopupHanhKiem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaHanhKiem")%>'
                                class='<%#(Container.ItemIndex == 2) ? "hidden" : "visible"%>' />
                            <asp:Label ID="LblFakeEdit" runat="server"></asp:Label>
                            <ajaxToolkit:ModalPopupExtender ID="MPEHanhKiem" runat="server" TargetControlID="LblFakeEdit"
                                PopupControlID="PnlPopupHanhKiem" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupHanhKiem"
                                PopupDragHandleControlID="PnlDragPopupHanhKiem">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TenDanhHieu")%>
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
        <div style="float: right; margin-top: -35px; padding-right: 30px; display: none">
            <cc1:DataPager ID="DataPagerDanhHieu" runat="server" OfClause="/" PageClause="TRANG"
                PageSize="10" ViewStateMode="Enabled" LastClause=">>" GenerateHiddenHyperlinks="False"
                CompactModePageCount="3" GenerateFirstLastSection="True" GenerateGoToSection="False"
                FirstClause="<<" BackToFirstClause="Trở về trang đầu" BackToPageClause="Trở về trang"
                GoToLastClause="Đến trang cuối" NextToPageClause="Đến trang" ShowResultClause="Hiển thị kết quả"
                ToClause="đến" />
        </div>
    </div>
    <asp:Panel ID="PnlPopupHanhKiem" runat="server" CssClass="popup ui-corner-all" Width="200px">
        <asp:Panel ID="PnlDragPopupHanhKiem" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupHanhKiemTitle" runat="server" CssClass="popup_header_title"
                Text="Xếp loại hạnh kiểm"></asp:Label>
            <img id="ImgClosePopupHanhKiem" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div id="PnlPopupHanhKiem_DivListHanhKiem" runat="server">
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="BtnSave" runat="server" OnClick="popopHanhKiem_Save_Click" ImageUrl="~/Styles/Images/button_save.png"
                CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancel" runat="server" OnClientClick="return popopHanhKiem_Cancel_Click();"
                ImageUrl="~/Styles/Images/button_cancel.png" CssClass="Button" />
        </div>
    </asp:Panel>
    <div style="padding: 5px; vertical-align: middle;">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
    <asp:HiddenField ID="HdfMaHocSinh" runat="server" />
</asp:Content>
