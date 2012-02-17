﻿<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="diemhocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.SearchStudentMarkPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divSearch">
        <div id="divSearchCriteria">
            <asp:UpdatePanel ID="UPDropdownlists" runat="server">
                <ContentTemplate>
                    <table class="search">
                        <tr style="height: 27px">
                            <td>
                                Năm học:
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="DdlNamHoc" runat="server" Width="120px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNamHoc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Ngành:
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="DdlNganh" runat="server" Width="120px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNganh_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Khối:
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="120px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlKhoiLop_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Lớp:
                            </td>
                            <td style="width: 150px">
                                <asp:DropDownList ID="DdlLopHoc" runat="server" Width="120px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlLopHoc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height: 27px">
                            <td>
                                Học kỳ:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlHocKy" runat="server" Width="120px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlHocKy_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Môn học:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlMonHoc" runat="server" Width="120px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Loại điểm:
                            </td>
                            <td style="width: 120px;">
                                <asp:DropDownList ID="DdlLoaiDiem" runat="server" Width="120px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Trạng thái:
                            </td>
                            <td style="width: 120px;">
                                <asp:DropDownList ID="DdlStatus" runat="server" Width="120px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height: 27px">
                            <td>
                                <asp:RadioButton ID="RBtnMonth" runat="server" GroupName="GroupTime" Checked="true" />
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
                                <asp:DropDownList ID="DdlWeeks" runat="server" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="width: 30px; margin: 0px auto 0px auto">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm điểm học sinh" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/buttons/button_add.png"
                ToolTip="Thêm điểm mới" OnClick="BtnAdd_Click" CssClass="BtnAdd" />
            <asp:ImageButton ID="BtnEdit" runat="server" OnClick="BtnEdit_Click" ImageUrl="~/Styles/buttons/button_edit.png"
                ToolTip="Sửa điểm" CssClass="BtnEdit" />
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
                    <asp:LinkButton ID="LlkBtnStudentCode" runat="server">Mã học sinh</asp:LinkButton>
                </td>
                <td id="tdHoTenHocSinh" runat="server" style="width: 150px">
                    <asp:LinkButton ID="LlkBtnStudentFullName" runat="server">Họ tên</asp:LinkButton>
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
                <td id="thSelectAll" runat="server" class="icon" style="height: 40px;">
                    <asp:CheckBox ID="CkbxSelectAll" runat="server" CssClass="selectAll" />
                </td>
            </tr>
            <asp:Repeater ID="RptDiemMonHoc" runat="server" OnItemDataBound="RptDiemMonHoc_ItemDataBound">
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                            <asp:HiddenField ID="HdfStudentId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaHocSinh")%>' />
                        </td>
                        <td>
                            <asp:HyperLink ID="HlkStudentCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "MaHocSinhHienThi")%>'></asp:HyperLink>
                        </td>
                        <td>
                            <asp:HyperLink ID="HlkStudentFullName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TenHocSinh")%>'></asp:HyperLink>
                        </td>
                        <asp:Repeater ID="RptDiemTheoLoaiDiem" runat="server">
                            <ItemTemplate>
                                <td style="height: 40px">
                                    <asp:HiddenField ID="HdfMarkTypeId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MarkTypeId")%>' />
                                    <asp:HiddenField ID="HdfMarkTypeName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MarkTypeName")%>' />
                                    <%#DataBinder.Eval(Container.DataItem, "StringDiems")%>
                                </td>
                            </ItemTemplate>
                        </asp:Repeater>
                        <td style="text-align: right">
                            <%#((double)DataBinder.Eval(Container.DataItem, "DiemTrungBinh") != -1) ? DataBinder.Eval(Container.DataItem, "DiemTrungBinh") : ""%>
                        </td>
                        <td id="tdSelect" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
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
