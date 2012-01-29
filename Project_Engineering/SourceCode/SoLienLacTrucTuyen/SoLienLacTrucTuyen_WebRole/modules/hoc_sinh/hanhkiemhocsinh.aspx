<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="hanhkiemhocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ViewStudentConductPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divSearch">
        <div id="divSearchCriteria">
            <asp:UpdatePanel ID="UPDropdownlists" runat="server">
                <ContentTemplate>
                    <table class="search">
                        <tr>
                            <td>
                                Năm học:
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNamHoc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Ngành:
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="DdlNganh" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNganh_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Khối:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlKhoiLop_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Lớp:
                            </td>
                            <td style="width: 180px">
                                <asp:DropDownList ID="DdlLopHoc" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Học kỳ:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divButtonSearch" style="margin: 3px 0px 0px 10px">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm thông tin hạnh kiểm học sinh" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnEdit" runat="server" OnClick="BtnEdit_Click" ImageUrl="~/Styles/buttons/button_danhgia.png"
                ToolTip="Đánh giá hạnh kiểm học sinh" CssClass="BtnDanhGia" />
        </div>
        <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"
            Text="Không có thông tin hạnh kiểm của học sinh">
        </asp:Label>
        <table class="repeater">
            <asp:Repeater ID="RptHanhKiemHocSinh" runat="server" OnItemDataBound="RptHanhKiemHocSinh_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl" style="width: 40px">
                            STT
                        </td>
                        <td style="width: 85px">
                            <asp:LinkButton ID="LlkBtnMaHocSinh" runat="server">Mã học sinh</asp:LinkButton>
                        </td>
                        <td style="width: 150px">
                            <asp:LinkButton ID="LlkBtnHoTenHocSinh" runat="server">Họ tên</asp:LinkButton>
                        </td>
                        <td>
                            Ngày nghỉ
                        </td>
                        <td>
                            Hạnh kiểm
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                            <asp:HiddenField ID="HdfMaHocSinh" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "StudentID")%>' />
                        </td>
                        <td>
                            <asp:HyperLink ID="HlkMaHocSinhHienThi" runat="server"><%#DataBinder.Eval(Container.DataItem, "StudentCode")%></asp:HyperLink>
                        </td>
                        <td>
                            <asp:HyperLink ID="HlkHoTenHocSinh" runat="server"><%#DataBinder.Eval(Container.DataItem, "StudentName")%></asp:HyperLink>
                        </td>
                        <td>
                            Tổng cộng:
                            <%#DataBinder.Eval(Container.DataItem, "TotalOfAbsentDays")%>
                            <span style='<%#((int)DataBinder.Eval(Container.DataItem, "TotalOfAbsentDays") != 0) ? "": "display:none"%>'>
                                , có phép:
                                <%#DataBinder.Eval(Container.DataItem, "TotalOfAskedAbsentDays")%>
                                , không phép:
                                <%#DataBinder.Eval(Container.DataItem, "TotalOfUnaskedAbsentDays")%>
                            </span>
                        </td>
                        <td style="text-align: center">
                            <%#DataBinder.Eval(Container.DataItem, "ConductName")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="20" class="footer ui-corner-bl ui-corner-br">
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
</asp:Content>
