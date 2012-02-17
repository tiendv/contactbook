<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="thoikhoabieu.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.SchedulePage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScripts">
        <script language="javascript" type="text/javascript">
            function fncOpen() {
                var pageId = '<%=  Page.ClientID %>';
                __doPostBack(pageId, "myargs");
                window.showModalDialog("/modules/hoc_sinh/indanhsachhocsinh.aspx", null, "dialogWidth:1000px; dialogHeight:1000px; center:yes");
            }
        </script>
        <script language="javascript" type="text/javascript">
            $(document).ready(function () {
                $(".ButtonEdit").hover(function () {
                    if ($(".ButtonEdit").is(':disabled') == false) {
                        $(this).attr("src", "/Styles/buttons/button_edit_hover.png");
                    }
                }, function () {
                    if ($(".ButtonEdit").is(':disabled') == false) {
                        $(this).attr("src", "/Styles/buttons/button_edit.png");
                    }
                });
            });	
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            <asp:UpdatePanel ID="UPDropdownlists" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="search">
                        <tr>
                            <td style="width: 60px;">
                                Năm học:
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNamHoc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Ngành:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlNganh" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNganh_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Lớp:
                            </td>
                            <td style="width: 200px">
                                <asp:DropDownList ID="DdlLopHoc" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 35px;">
                                Học kỳ:
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 35px;">
                                Khối:
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlKhoiLop_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divButtonSearch" style="margin: 3px 0px 0px 0px">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm thời khóa biểu" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnPrint" runat="server" ImageUrl="~/Styles/buttons/button_export.png"
                ToolTip="Báo cáo" OnClientClick="fncOpen();" CssClass="BtnExport" />
            <asp:ImageButton ID="BtnEdit" runat="server" OnClick="BtnEdit_Click" ImageUrl="~/Styles/buttons/button_arrange.png"
                ToolTip="Sắp xếp thời khóa biểu" CssClass="BtnArrage" />
        </div>
        <div>
            <asp:Label ID="LblErrorResult" runat="server" Style="font-size: 15px; font-weight: bold;"
                Text="Không có thông tin thời khóa biểu"></asp:Label>
        </div>
        <asp:Label ID="LblTitle" runat="server" Font-Bold="true" ForeColor="blue"></asp:Label>
        <table id="tbHeader" runat="server" class="repeater" style="width: 100%">
            <tr class="header">
                <td class=" ui-corner-tl">
                    Buổi
                </td>
                <td style="width: 100px">
                    Tiết
                </td>
                <td style="width: 100px">
                    Thứ 2
                </td>
                <td style="width: 100px">
                    Thứ 3
                </td>
                <td style="width: 100px">
                    Thứ 4
                </td>
                <td style="width: 100px">
                    Thứ 5
                </td>
                <td style="width: 100px">
                    Thứ 6
                </td>
                <td style="width: 100px">
                    Thứ 7
                </td>
            </tr>
        </table>
        <table class="repeater" style="width: 100%">
            <asp:Repeater ID="RptSchedule" runat="server" OnItemDataBound="RptSchedule_ItemDataBound">
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="text-align: center">
                            <asp:Label ID="LblSessionName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SessionName")%>'></asp:Label>
                        </td>
                        <td style="width: 784px; padding: 0px; margin: 0px">
                            <table class="repeater" style="width: 100%; padding: 0px; margin: 0px">
                                <asp:Repeater ID="RptSessionSchedule" runat="server" OnItemDataBound="RptSessionSchedule_ItemDataBound">
                                    <ItemTemplate>
                                        <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                                            <td style="height: 40px; width: 100px; text-align: center">
                                                <asp:Label ID="LblTeachingPeriodName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TeachingPeriodName")%>'></asp:Label>
                                            </td>
                                            <asp:Repeater ID="RptTeachingPeriodSchedule" runat="server">
                                                <ItemTemplate>
                                                    <td style="width: 100px; padding: 0px 5px 0px 5px">
                                                        <%#DataBinder.Eval(Container.DataItem, "SubjectName")%>
                                                    </td>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div style="padding: 5px 0px 0px 30px">
        <ul>
            <asp:Repeater ID="RptTeachingPeriod" runat="server">
                <HeaderTemplate>
                    <span style="text-decoration: underline; font-weight: bold">Chú thích: </span>
                </HeaderTemplate>
                <ItemTemplate>
                    <li style="padding: 5px; margin: 0px 0px 10px 0px"><span>
                        <%#DataBinder.Eval(Container.DataItem, "TeachingPeriodName")%>:
                        <%#DataBinder.Eval(Container.DataItem, "StringBeginTime")%>
                        -
                        <%#DataBinder.Eval(Container.DataItem, "StringEndTime")%>
                        (<%#DataBinder.Eval(Container.DataItem, "SessionName")%>) </span></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</asp:Content>
