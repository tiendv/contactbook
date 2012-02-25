﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="thongbao.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.MessageForParentsPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divSearch">
        <div id="divSearchCriteria">
            <table class="search">
                <tr>
                    <td>
                        Năm học:
                    </td>
                    <td style="width: 200px;">
                        <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Từ ngày:
                    </td>
                    <td style="width: 200px;">
                        <asp:TextBox ID="TxtTuNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarTuNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Top" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TxtTuNgay"
                            PopupButtonID="ImgCalendarTuNgay" PopupPosition="Right" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="TxtTuNgay"
                            MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                        <br />
                        <asp:RequiredFieldValidator ID="BeginDateRequired" runat="server" ValidationGroup="Search"
                            Display="Dynamic" ForeColor="Red" ControlToValidate="TxtTuNgay" ErrorMessage="Ngày bắt đầu không được để trống">
                        </asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="BeginDateValidator" runat="server" Display="Dynamic" ForeColor="Red"
                            ControlToValidate="TxtTuNgay" ErrorMessage="Ngày bắt đầu không hợp lệ" ValidationGroup="Search"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Trạng thái:
                    </td>
                    <td style="width: 180px">
                        <asp:DropDownList ID="DdlXacNhan" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Đến ngày:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtDenNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarDenNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Top" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtDenNgay"
                            PopupButtonID="ImgCalendarDenNgay" PopupPosition="Right" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="TxtDenNgay"
                            MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                        <br />
                        <asp:RequiredFieldValidator ID="EndDateRequired" runat="server" ValidationGroup="Search"
                            Display="Dynamic" ForeColor="Red" ControlToValidate="TxtDenNgay" ErrorMessage="Ngày kết thúc không được để trống">
                        </asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="EndDateValidator" runat="server" Display="Dynamic" ForeColor="Red"
                            ControlToValidate="TxtDenNgay" ErrorMessage="Ngày kết thúc không hợp lệ" ValidationGroup="Search"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" Style="margin: 5px 0px 0px 0px" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm thông báo" OnClick="BtnSearch_Click" CssClass="BtnSearch"  ValidationGroup="Search"/>
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <asp:Panel ID="PnlMessageStatus" runat="server" Style="font-weight: bold; padding: 0px 0px 10px 0px">
            <span style="clear: both">
                <br />
            </span>Bạn có
            <asp:Label ID="LblMessageStatus" runat="server" CssClass="alertNumber"></asp:Label>
            thông báo mới
            <br />
            Bạn còn
            <asp:Label ID="LblUnconfirmedMessageStatus" runat="server" CssClass="alertNumber"></asp:Label>
            thông báo chưa xác nhận
        </asp:Panel>
        <table class="repeater">
            <asp:HiddenField ID="HdfMaLoiNhanKhan" runat="server" />
            <asp:Repeater ID="RptLoiNhanKhan" runat="server" OnItemCommand="RptLoiNhanKhan_ItemCommand"
                OnItemDataBound="RptLoiNhanKhan_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl">
                            Thông báo
                        </td>
                        <td style="width: 150px">
                            <asp:LinkButton ID="LkBtnDate" runat="server" CommandName="SortDate">Thời gian</asp:LinkButton>
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%> <%#((int)DataBinder.Eval(Container.DataItem, "MessageStatusId") < 3) ? "hover" : ""%> '>
                        <td style="height: 40px; padding: 0px 0px 0px 10px">
                            <asp:HiddenField ID="HdfRptMaLoiNhanKhan" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MessageId")%>' />
                            <asp:Image ID="ImgAlreadyReadMsg" runat="server" Height="21" Width="25" ImageUrl="~/Styles/Images/already_read_message.png" />
                            <asp:Image ID="ImgWarning" runat="server" Height="19" Width="19" ImageUrl="~/Styles/Images/warning.png" />
                            <asp:Image ID="ImgUnreadMsg" runat="server" Height="16" Width="25" ImageUrl="~/Styles/Images/unread_message.png" />
                            <asp:LinkButton ID="LkbtnTitle" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Title")%>'
                                Style="text-decoration: none; color: Blue; cursor: pointer;" CommandName="CmdDetailItem"
                                CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MessageId")%>'>
                            </asp:LinkButton>
                        </td>
                        <td style="height: 40px; text-align: left">
                            <%#(DataBinder.Eval(Container.DataItem, "StringDate"))%>
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
    <asp:Panel ID="PnlNote" runat="server" Style="padding: 10px">
        Chú thích:<br />
        &nbsp;&nbsp;&nbsp;<asp:Image ID="ImgUnreadMsgNote" runat="server" Height="16" Width="25"
            ImageUrl="~/Styles/Images/unread_message.png" />: Thông báo chưa đọc<br />
        &nbsp;&nbsp;&nbsp;<asp:Image ID="ImgAlreadyReadMsg" runat="server" Height="21" Width="25"
            ImageUrl="~/Styles/Images/already_read_message.png" />
        <asp:Image ID="ImgWarning" runat="server" Height="19" Width="19" ImageUrl="~/Styles/Images/warning.png" />:
        Thông báo đã đọc và chưa xác nhận<br />
        &nbsp;&nbsp;&nbsp;<asp:Image ID="Image2" runat="server" Height="21" Width="25" ImageUrl="~/Styles/Images/already_read_message.png" />:
        Thông báo đã đọc và đã xác nhận<br />
    </asp:Panel>
</asp:Content>
