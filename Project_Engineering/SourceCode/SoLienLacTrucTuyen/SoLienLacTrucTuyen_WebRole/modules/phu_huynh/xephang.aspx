<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="xephang.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.StudentRatingPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
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
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div style="padding: 5px 10px 5px 10px; font-size: 14px; font-weight: bold">
            <asp:Label ID="LblOrderNo" runat="server" Text="Học sinh đứng hạng " ForeColor="Violet">
            </asp:Label>
        </div>
        <table class="repeater">
            <asp:Repeater ID="RptRating" runat="server">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            Hạng
                        </td>                        
                        <td>
                            Học sinh
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
                        <td style="width: 120px">
                            Danh hiệu
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>                
                    <tr class='<%#((string)DataBinder.Eval(Container.DataItem, "StudentCode") == LoggedInStudent.StudentCode) ? "emphasizedRow" : ""%>
                        <%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%> '>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "StudentFullName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "StringAverageMark")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "LearningAptitudeName")%>
                        </td>
                        <td style="height: 40px; vertical-align: middle">
                            <%#DataBinder.Eval(Container.DataItem, "ConductName")%>
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
        <div style="float: right; margin-top: -35px; padding-right: 30px;">
            <cc1:DataPager ID="MainDataPager" runat="server" ViewStateMode="Enabled" OnCommand="MainDataPager_Command"/>
        </div>
    </div>
</asp:Content>
