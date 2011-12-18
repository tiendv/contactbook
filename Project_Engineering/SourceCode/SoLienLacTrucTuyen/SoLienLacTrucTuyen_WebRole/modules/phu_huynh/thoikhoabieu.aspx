<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="ThoiKhoaBieu.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.SchedulePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divSearch">
        <div id="divSearchCriteria">
            <table class="search">
                <tr>
                    <td style="width: 60px;">
                        Năm học:
                    </td>
                    <td style="width: 200px;">
                        <asp:DropDownList ID="DdlYear" runat="server" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="DdlYear_SelectedIndexChanged">
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
        <table class="ui-corner-all" style="width: 100%">
            <asp:Repeater ID="RptMonHocTKB" runat="server" OnItemDataBound="RptMonHocTKB_ItemDataBound">
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="padding: 10px">
                            <asp:HiddenField ID="HdfRptYearId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "YearId")%>' />
                            <asp:HiddenField ID="HdfRptTermId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "TermId")%>' />
                            <asp:HiddenField ID="HdfRptDayInWeekId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "DayInWeekId")%>' />
                            <asp:HiddenField ID="HdfRptClassId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ClassId")%>' />
                            <asp:Label ID="LblSessionName" runat="server" Font-Bold="true" Font-Size="15px" Text='<%#DataBinder.Eval(Container.DataItem, "DayInWeekName")%>'></asp:Label>
                            <asp:Panel ID="PnlAllDaySchedule" runat="server" Style="width: 100%; padding: 5px 0px 5px 60px">
                                <asp:Label ID="Label3" runat="server" Text="(Nghỉ)" Style="height: 27px; padding: 0px 0px 0px 30px"></asp:Label>
                            </asp:Panel>
                            <%--Thời khóa biểu buổi sáng--%>
                            <asp:Panel ID="PnlMorningSchedule" runat="server" Style="width: 100%; padding: 5px 0px 5px 60px">
                                <asp:Label ID="Label1" runat="server" Text="Buổi sáng:" Font-Size="14px"></asp:Label>
                                <br />
                                <asp:Label ID="LblNghiSang" runat="server" Text="(Nghỉ)" Style="height: 27px; padding: 0px 0px 0px 30px"
                                    Visible="false"></asp:Label>
                                <table width="100%">
                                    <asp:Repeater ID="RptMonHocBuoiSang" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td style="height: 27px; padding: 0px 0px 0px 30px">
                                                    <%#DataBinder.Eval(Container.DataItem, "StringDetailTeachingPeriod")%>&nbsp;-&nbsp;
                                                    <b>
                                                        <%#DataBinder.Eval(Container.DataItem, "SubjectName")%></b> (GV:
                                                    <%#DataBinder.Eval(Container.DataItem, "TeacherName")%>)
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="PnlEveningSchedule" runat="server" Style="width: 100%; padding: 5px 0px 5px 60px">
                                <asp:Label ID="Label2" runat="server" Text="Buổi chiều:" Font-Size="14px"></asp:Label>
                                <br />
                                <asp:Label ID="LblNghiChieu" runat="server" Text="(Nghỉ)" Style="height: 27px; padding: 0px 0px 0px 30px"
                                    Visible="false"></asp:Label>
                                <table width="100%">
                                    <asp:Repeater ID="RptMonHocBuoiChieu" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td style="height: 27px; padding: 0px 5px 0px 30px">
                                                    <%#DataBinder.Eval(Container.DataItem, "StringDetailTeachingPeriod")%>&nbsp;-&nbsp;
                                                    <b>
                                                        <%#DataBinder.Eval(Container.DataItem, "SubjectName")%></b> (GV:
                                                    <%#DataBinder.Eval(Container.DataItem, "TeacherName")%>)
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
</asp:Content>
