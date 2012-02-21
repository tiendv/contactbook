<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="thongtincanhan.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.TeacherPersonalInfomationPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">    
    <div style="width: 90%; padding: 10px; clear: both" class="loginBox ui-corner-all">
        <table style="width: 400px; float: left">
            <tr>
                <td style="width: 90px; height: 27px;">
                    Mã giáo viên:
                </td>
                <td>
                    <asp:Label ID="LblUserIdHienThi" runat="server" Width="293px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 27px;">
                    Họ tên:
                </td>
                <td>
                    <asp:Label ID="LblTenGiaoVien" runat="server" Width="293px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 27px;">
                    Ngày sinh:
                </td>
                <td>
                    <asp:Label ID="LblNgaySinh" runat="server" Width="293px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 27px;">
                    Giới tính:
                </td>
                <td>
                    <asp:Label ID="LblGioiTinh" runat="server" Width="293px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 27px;">
                    Địa chỉ:
                </td>
                <td>
                    <asp:Label ID="LblDiaChi" runat="server" Width="293px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 27px;">
                    Điện thoại:
                </td>
                <td>
                    <asp:Label ID="LblDienThoai" runat="server" Width="293px"></asp:Label>
                </td>
            </tr>
        </table>
        <div style="width: 100px; text-align: center; vertical-align: top; float: right">
            <asp:Image ID="ImgHinhAnh" runat="server" Width="90px" Height="120px" ImageUrl="~/Styles/Images/avatar.png"
                AlternateText="Hình ảnh" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black" />
        </div>
        <div style="clear: both">
        </div>
    </div>
    <div class="table_data ui-corner-all" style="width: 90%; margin: 0px auto 0px auto">
        HOẠT ĐỘNG CHỦ NHIỆM<br />
        <br />
        <div>
            <asp:Label ID="LblSearchResultChuNhiem" runat="server" Text="Không có thông tin chủ nhiệm"
                Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:Repeater ID="RptChuNhiem" runat="server">
                <HeaderTemplate>
                    <tr class="header ui-corner-right ui-corner-left">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td>
                            <asp:LinkButton ID="LkBtnNamHoc" runat="server">Năm học</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton1" runat="server">Lớp học</asp:LinkButton>
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (DataPagerChuNhiem.CurrentIndex - 1) * DataPagerChuNhiem.PageSize + Container.ItemIndex + 1%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "YearName")%>
                        </td>
                        <td style="height: 40px;">
                            <asp:HyperLink ID="HyperLink1" runat="server"><%#DataBinder.Eval(Container.DataItem, "ClassName")%></asp:HyperLink>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="3" class="footer ui-corner-bl ui-corner-br">
                        </td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
        <div style="float: right; margin-top: -35px; padding-right: 30px;">
            <cc1:DataPager ID="DataPagerChuNhiem" runat="server" OnCommand="DataPagerChuNhiem_Command"
                ViewStateMode="Enabled" />
        </div>
    </div>
    <br />
    <div class="table_data ui-corner-all" style="width: 90%; margin: 0px auto 0px auto">
        HOẠT ĐỘNG GIẢNG DẠY<br />
        <br />
        <div>
            <asp:Label ID="LblSearchResultGiangDay" runat="server" Text="Không có thông tin giảng dạy"
                Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:Repeater ID="RptGiangDay" runat="server">
                <HeaderTemplate>
                    <tr class="header ui-corner-right ui-corner-left">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td>
                            <asp:LinkButton ID="LkBtnNamHoc" runat="server">Năm học</asp:LinkButton>
                        </td>
                        <td>
                            Học kỳ
                        </td>
                        <td>
                            Thứ
                        </td>
                        <td>
                            Tiết
                        </td>
                        <td>
                            Lớp học
                        </td>
                        <td>
                            Môn học
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (DataPagerChuNhiem.CurrentIndex - 1) * DataPagerChuNhiem.PageSize + Container.ItemIndex + 1%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "YearName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TermName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "DayInWeekName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TeachingPeriodName")%>
                        </td>
                        <td style="height: 40px;">
                            <asp:HyperLink ID="HyperLink1" runat="server"><%#DataBinder.Eval(Container.DataItem, "ClassName")%></asp:HyperLink>
                        </td>
                        <td style="height: 40px;">
                            <asp:HyperLink ID="HyperLink2" runat="server"><%#DataBinder.Eval(Container.DataItem, "SubjectName")%></asp:HyperLink>
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
            <cc1:DataPager ID="DataPagerGiangDay" runat="server" OnCommand="DataPagerGiangDay_Command"
                ViewStateMode="Enabled" />
        </div>
    </div>
</asp:Content>
