﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="ThoiKhoaBieu.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.SchedulePage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divSearch">
        <div id="divSearchCriteria">
            <table class="search">
                <tr>
                    <td style="width: 60px;">
                        Năm học:
                    </td>
                    <td style="width: 200px;">
                        <asp:DropDownList ID="DdlYear" runat="server" Width="150px" AutoPostBack="true"
                            OnSelectedIndexChanged="DdlYear_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Học kỳ:
                    </td>
                    <td style="width: 200px;">
                        <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px" AutoPostBack="true"
                            OnSelectedIndexChanged="DdlTerm_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>                    
                </tr>
                <tr>
                    <td>
                        Lớp:
                    </td>
                    <td>
                        <asp:Label ID="LblClassName" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"
                Text="Không có thông tin thời khóa biểu"></asp:Label>
        </div>
        <table class="repeater">
            <asp:Repeater ID="RptMonHocTKB" runat="server" OnItemDataBound="RptMonHocTKB_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="left_2 ui-corner-tl" style="width: 78px;">
                            <asp:Label ID="Label6" runat="server" Text="Buổi" Style="float: right; vertical-align: top"></asp:Label>
                            <br />
                            <asp:Label ID="Label5" runat="server" Text="Ngày" Style="float: left; vertical-align: bottom"></asp:Label>
                        </td>
                        <td>
                            Sáng
                        </td>
                        <td>
                            Chiều
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; width: 10%; vertical-align: middle;">
                            <asp:HiddenField ID="HdfRptYearId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "YearId")%>' />
                            <asp:HiddenField ID="HdfRptTermId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "TermId")%>' />
                            <asp:HiddenField ID="HdfRptDayInWeekId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "DayInWeekId")%>' />
                            <asp:HiddenField ID="HdfRptClassId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ClassId")%>' />
                            <asp:Label ID="LblSessionName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "DayInWeekName")%>'></asp:Label>
                        </td>
                        <%--Thời khóa biểu buổi sáng--%>
                        <td style="height: 40px; width: 41%; padding: 0px">
                            <asp:Label ID="LblNghiSang" runat="server" Text="(Nghỉ)" Visible="false" Style="padding-left: 5px"></asp:Label>
                            <table width="100%">
                                <asp:Repeater ID="RptMonHocBuoiSang" runat="server">
                                    <ItemTemplate>
                                        <tr class='<%#((Container.ItemIndex + 1) % 2 != 0) ? "innerOddRow" : "innerEvenRow"%>'>
                                            <td style="height: 40px; width: 37%; border-style: none; border-bottom-style: solid;
                                                border-right-style: solid; padding: 0px 5px 0px 5px">
                                                <%#DataBinder.Eval(Container.DataItem, "StringDetailTeachingPeriod")%>
                                            </td>
                                            <td style="height: 40px; border-style: none; border-bottom-style: solid; padding: 0px 5px 0px 5px">
                                                <b>
                                                    <%#DataBinder.Eval(Container.DataItem, "SubjectName")%></b>
                                                <br />
                                                (<%#DataBinder.Eval(Container.DataItem, "TeacherName")%>)
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                        <%--Thời khóa biểu buổi chiều--%>
                        <td style="height: 40px; width: 41%; padding: 0px">
                            <asp:Label ID="LblNghiChieu" runat="server" Text="(Nghỉ)" Visible="false" Style="padding-left: 5px"></asp:Label>
                            <table width="100%">
                                <asp:Repeater ID="RptMonHocBuoiChieu" runat="server">
                                    <ItemTemplate>
                                        <tr class='<%#((Container.ItemIndex + 1) % 2 != 0) ? "innerOddRow" : "innerEvenRow"%>'>
                                            <td style="height: 40px; width: 40%; border-style: none; border-bottom-style: solid;
                                                border-right-style: solid; padding: 0px 5px 0px 5px">
                                                <%#DataBinder.Eval(Container.DataItem, "StringDetailTeachingPeriod")%>
                                            </td>
                                            <td style="height: 40px; border-style: none; border-bottom-style: solid; padding: 0px 5px 0px 5px">
                                                <b>
                                                    <%#DataBinder.Eval(Container.DataItem, "SubjectName")%></b>
                                                <br />
                                                (<%#DataBinder.Eval(Container.DataItem, "TeacherName")%>)
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
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
    </div>
</asp:Content>