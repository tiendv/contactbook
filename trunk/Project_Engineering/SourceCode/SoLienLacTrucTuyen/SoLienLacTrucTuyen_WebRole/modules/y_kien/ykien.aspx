<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="ykien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ParentsCommentPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script type="text/javascript">
            $(document).ready(function () {
                $(this).find(".radio input[type='radio']").change(function () {
                    if ($(this).is(':checked')) {
                        $(this).parents('td').parents('tr').siblings().each(function () {
                            $(this).find(".radio input[type='radio']").each(function () {
                                $(this).attr('checked', false);
                            });
                        });
                    }
                });
            });
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            <table class="search">
                <tr>
                    <td>
                        Năm học:
                    </td>
                    <td style="width: 200px;">
                        <asp:DropDownList ID="DdlYears" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Từ ngày:
                    </td>
                    <td style="width: 200px;">
                        <asp:TextBox ID="TxtBeginDate" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarBeginDate" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Top" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtBeginDate"
                            PopupButtonID="ImgCalendarBeginDate" PopupPosition="Right" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="TxtBeginDate"
                            MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Xác nhận:
                    </td>
                    <td style="width: 180px">
                        <asp:DropDownList ID="DdlXacNhan" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Đến ngày:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtEndDate" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarEndDate" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Top" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtEndDate"
                            PopupButtonID="ImgCalendarEndDate" PopupPosition="Right" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="TxtEndDate"
                            MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divButtonSearch" style="margin: 5px 0px 0px 0px">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm ý kiến phụ huynh" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnFeedback" runat="server" ImageUrl="~/Styles/buttons/button_feedback.png"
                OnClick="BtnFeedback_Click" ToolTip="Phản hồi ý kiến" CssClass="ButtonFeedback" />
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <asp:Panel ID="PnlCommentStatus" runat="server" Style="font-weight: bold">
            <span style="clear: both">
                <br />
            </span>
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Images/alert.png" />
            Bạn có
            <asp:Label ID="LblCommentStatus" runat="server" CssClass="alertNumber"></asp:Label>
            góp ý chưa phản hồi
        </asp:Panel>
        <table class="repeater">
            <asp:HiddenField ID="HdfMaLoiNhanKhan" runat="server" />
            <asp:Repeater ID="RptLoiNhanKhan" runat="server" OnItemCommand="RptLoiNhanKhan_ItemCommand"
                OnItemDataBound="RptLoiNhanKhan_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td>
                            Ý kiến của phụ huynh
                        </td>
                        <td style="width: 100px">
                            Ngày
                        </td>
                        <td style="width: 100px">
                            Tình trạng
                        </td>
                        <td id="thSelectAll" runat="server" class="icon" style="height: 40px;">
                            Chọn
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptMaLoiNhanKhan" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "CommentId")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:LinkButton ID="LbtnCommentTitle" runat="server" Style="text-decoration: underline;
                                color: Blue; cursor: pointer;" CommandName="CmdDetailItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "CommentId")%>'>
                            <%#DataBinder.Eval(Container.DataItem, "Title")%>
                            </asp:LinkButton>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "Date")%>
                        </td>
                        <td style="height: 40px;">
                            <asp:HiddenField ID="HdfCommentStatusId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "CommentStatusId")%>' />
                            <%#DataBinder.Eval(Container.DataItem, "CommentStatusName")%>
                        </td>
                        <td id="tdSelect" runat="server" class="icon" style="height: 40px;">
                            <%--<asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />--%>
                            <asp:RadioButton ID="RBtnSelect" runat="server" CssClass="radio" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="8" class="footer ui-corner-bl ui-corner-br">
                        </td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div style="float: right; margin-top: -45px; padding-right: 30px;">
        <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="MainDataPager_Command"
            ViewStateMode="Enabled" />
    </div>
</asp:Content>
