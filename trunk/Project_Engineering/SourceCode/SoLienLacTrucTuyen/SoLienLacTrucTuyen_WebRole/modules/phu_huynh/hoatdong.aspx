<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="hoatdong.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.StudentActivityPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
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
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfSltActivityName" runat="server" />
            <asp:HiddenField ID="HdfMaHoatDong" runat="server" />
            <asp:HiddenField ID="HdfRptHoatDongMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptHoatDongMPEEdit" runat="server" />
            <asp:Repeater ID="RptHoatDong" runat="server">
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
        <div style="float: right; margin-top: -35px; padding-right: 30px;">
            <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="MainDataPager_Command"
                ViewStateMode="Enabled" />
        </div>
    </div>
    <asp:HiddenField ID="HdfMaHocSinh" runat="server" />
    <asp:HiddenField ID="HdfTermId" runat="server" />
    <asp:HiddenField ID="HdfTieuDe" runat="server" />
    <asp:HiddenField ID="HdfNgay" runat="server" />
</asp:Content>
