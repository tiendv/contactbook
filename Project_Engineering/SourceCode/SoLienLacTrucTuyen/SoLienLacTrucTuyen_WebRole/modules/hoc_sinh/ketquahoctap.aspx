﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="ketquahoctap.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.StudentStudyingResultPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div>
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
                            <%#DataBinder.Eval(Container.DataItem, "MarkTypeName")%>
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
                            <%#DataBinder.Eval(Container.DataItem, "SubjectName")%>
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
            <asp:HiddenField ID="HdfLearningResultIdHSHK" runat="server" />
            <asp:Repeater ID="RptDanhHieu" runat="server">
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
                            <asp:HiddenField ID="HdfRptLearningResultIdHSHK" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "LearningResultIdHSHK")%>' />
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TermName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "StrDiemTB")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "LearningAptitudeName")%>
                        </td>
                        <td style="height: 40px; vertical-align: middle">
                            <asp:HiddenField ID="HiddenField1" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ConductId")%>' />
                            <%#DataBinder.Eval(Container.DataItem, "ConductName")%>
                            <asp:Label ID="LblFakeEdit" runat="server"></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "LearningResultName")%>
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
