<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="diemhocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.SearchStudentMarkPage" %>

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
                            <td style="width: 180px;">
                                <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlKhoiLop_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Lớp:
                            </td>
                            <td style="width: 180px">
                                <asp:DropDownList ID="DdlLopHoc" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlLopHoc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Môn học:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlMonHoc" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Loại điểm:
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="DdlLoaiDiem" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:RadioButton ID="RBtnTerm" runat="server" GroupName="GroupTime" Checked="true" />
                                Học kỳ:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlHocKy_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RadioButton ID="RBtnMonth" runat="server" GroupName="GroupTime" Checked="false" />
                                Tháng:
                            </td>
                            <td>
                                <asp:DropDownList ID="DddMonths" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RadioButton ID="RBtnWeek" runat="server" GroupName="GroupTime" Checked="false" />
                                Tuần:
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="DddWeeks" runat="server" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="width: 30px; margin: 0px auto 0px auto">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm điểm học sinh" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">        
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/buttons/button_add.png"
                ToolTip="Thêm điểm mới" OnClick="BtnAdd_Click" CssClass="BtnAdd" />            
        </div>
        <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"
            Text="Không có thông tin điểm học sinh">
        </asp:Label>        
        <table class="repeater">
            <tr class="header">
                <td id="tdSTT" runat="server" class="ui-corner-tl" style="width: 40px">
                    STT
                </td>
                <td id="tdMaHocSinh" runat="server" style="width: 85px">
                    <asp:LinkButton ID="LlkBtnMaHocSinh" runat="server">Mã học sinh</asp:LinkButton>
                </td>
                <td id="tdHoTenHocSinh" runat="server" style="width: 150px">
                    <asp:LinkButton ID="LlkBtnHoTenHocSinh" runat="server">Họ tên</asp:LinkButton>
                </td>
                <asp:Repeater ID="RptLoaiDiem" runat="server">
                    <ItemTemplate>
                        <td class="middle">
                            <%#DataBinder.Eval(Container.DataItem, "MarkTypeName")%>
                        </td>
                    </ItemTemplate>
                </asp:Repeater>
                <td id="tdDTB" runat="server" style="width: 50px">
                    ĐTB
                </td>
            </tr>
            <asp:Repeater ID="RptDiemMonHoc" runat="server" OnItemDataBound="RptDiemMonHoc_ItemDataBound">
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                        </td>
                        <td>
                            <asp:HyperLink ID="HlkMaHocSinhHienThi" runat="server"><%#DataBinder.Eval(Container.DataItem, "MaHocSinhHienThi")%></asp:HyperLink>
                        </td>
                        <td>
                            <asp:HyperLink ID="HlkHoTenHocSinh" runat="server"><%#DataBinder.Eval(Container.DataItem, "TenHocSinh")%></asp:HyperLink>
                        </td>
                        <asp:Repeater ID="RptDiemTheoLoaiDiem" runat="server">
                            <ItemTemplate>
                                <td style="height: 40px">
                                    <%#DataBinder.Eval(Container.DataItem, "StringDiems")%>
                                </td>
                            </ItemTemplate>
                        </asp:Repeater>
                        <td style="text-align: right">
                            <%#((double)DataBinder.Eval(Container.DataItem, "DiemTrungBinh") != -1) ? DataBinder.Eval(Container.DataItem, "DiemTrungBinh") : "Chưa xác định"%>
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
