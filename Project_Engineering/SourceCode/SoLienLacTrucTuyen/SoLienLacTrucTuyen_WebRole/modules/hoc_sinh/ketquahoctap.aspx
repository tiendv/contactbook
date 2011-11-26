<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="ketquahoctap.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.StudentStudyingResultPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
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
            </tr>
            <asp:Repeater ID="RptKetQuaDiem" runat="server" OnItemDataBound="RptKetQuaDiem_ItemDataBound">
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
            <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="MainDataPager_Command"
                ViewStateMode="Enabled" />
        </div>
        <table class="repeater">
            <asp:HiddenField ID="RptMPEHanhKiem" runat="server" />
            <asp:HiddenField ID="HdfMaDanhHieuHSHK" runat="server" />
            <asp:Repeater ID="RptDanhHieu" runat="server" OnItemDataBound="RptDanhHieu_ItemDataBound">
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
                            <asp:Label ID="LblFakeEdit" runat="server"></asp:Label>
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
            <cc1:DataPager ID="DataPagerDanhHieu" runat="server" ViewStateMode="Enabled" />
        </div>
    </div>    
    <div style="padding: 5px; vertical-align: middle;">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
    <asp:HiddenField ID="HdfMaHocSinh" runat="server" />
</asp:Content>
