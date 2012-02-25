<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="ykien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.ParentsCommentPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function popopConfirmDelete_CancelDelete_Click() {
                $find('<%=MPEDelete.ClientID%>').hide();
                return false;
            }

            function popopInfoInUse_Close() {
                $find('<%=MPEInfoInUse.ClientID%>').hide();
                return false;
            }
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
                        <br />
                        <asp:RequiredFieldValidator ID="BeginDateRequired" runat="server" ValidationGroup="Search"
                            Display="Dynamic" ForeColor="Red" ControlToValidate="TxtBeginDate" ErrorMessage="Ngày bắt đầu không được để trống">
                        </asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="BeginDateValidator" runat="server" Display="Dynamic" ForeColor="Red"
                            ControlToValidate="TxtBeginDate" ErrorMessage="Ngày bắt đầu không hợp lệ" ValidationGroup="Search"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Tình trạng:
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
                        <br />
                        <asp:RequiredFieldValidator ID="EndDateRequired" runat="server" ValidationGroup="Search"
                            Display="Dynamic" ForeColor="Red" ControlToValidate="TxtEndDate" ErrorMessage="Ngày kết thúc không được để trống">
                        </asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="EndDateValidator" runat="server" Display="Dynamic" ForeColor="Red"
                            ControlToValidate="TxtEndDate" ErrorMessage="Ngày kết thúc không hợp lệ" ValidationGroup="Search"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" Style="margin: 5px 0px 0px 0px" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm ý kiến phụ huynh" OnClick="BtnSearch_Click" CssClass="BtnSearch"
                ValidationGroup="Search" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" OnClick="BtnAdd_Click" ImageUrl="~/Styles/buttons/button_add.png"
                ToolTip="Thêm lời góp ý mới" CssClass="BtnAdd" />
            <asp:ImageButton ID="BtnEdit" runat="server" OnClick="BtnEdit_Click" ImageUrl="~/Styles/buttons/button_edit.png"
                ToolTip="Sửa góp ý" CssClass="BtnEdit" />
            <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Styles/buttons/button_delete.png"
                ToolTip="Xóa góp ý" CssClass="BtnDelete" />
            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnDelete"
                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <asp:Panel ID="PnlCommentStatus" runat="server" Style="font-weight: bold">
            <span style="clear: both">
                <br />
            </span>Bạn có
            <asp:Label ID="LblCommentStatus" runat="server" CssClass="alertNumber"></asp:Label>
            phản hồi chưa xem
        </asp:Panel>
        <table class="repeater">
            <asp:HiddenField ID="HdfCommentId" runat="server" />
            <asp:HiddenField ID="HdfRptCommentMPEDelete" runat="server" />
            <asp:Repeater ID="RptLoiNhanKhan" runat="server" OnItemDataBound="RptLoiNhanKhan_ItemDataBound"
                OnItemCommand="RptLoiNhanKhan_ItemCommand">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td>
                            Góp ý của phụ huynh
                        </td>
                        <td style="width: 120px">
                            Thời gian góp ý
                        </td>
                        <td style="width: 210px;">
                            Tình trạng
                        </td>
                        <td id="thSelectAll" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelectAll" runat="server" CssClass="selectAll" />
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
                            <asp:Image ID="ImgConfirmed" runat="server" ImageUrl="~/Styles/Icons/orange_arrow_down.png"
                                Visible='<%#((int)DataBinder.Eval(Container.DataItem, "CommentStatusId") != 1)%>' />
                            <asp:Image ID="ImgUnconfirmed" runat="server" ImageUrl="~/Styles/Icons/green_arrow_up.png"
                                Visible='<%#((int)DataBinder.Eval(Container.DataItem, "CommentStatusId") == 1)%>' />
                            <asp:LinkButton ID="LbtnCommentTitle" runat="server" Style="text-decoration: underline;
                                color: Blue; cursor: pointer;" CommandName="CmdDetailItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "CommentId")%>'
                                Font-Bold='<%#((int)DataBinder.Eval(Container.DataItem, "CommentStatusId") == 2)%>'>
                            <%#DataBinder.Eval(Container.DataItem, "Title")%>
                            </asp:LinkButton>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "Date")%>
                        </td>
                        <td style="height: 40px; text-align: left">
                            <%#DataBinder.Eval(Container.DataItem, "CommentStatusName")%>
                        </td>
                        <td id="tdSelect" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" Visible='<%#((int)DataBinder.Eval(Container.DataItem, "CommentStatusId") == 1)%>' />
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
        &nbsp;&nbsp;&nbsp;<asp:Image ID="ImgUnconfirmedNote" runat="server" ImageUrl="~/Styles/Icons/green_arrow_up.png" />:
        Chưa phản hồi<br />
        &nbsp;&nbsp;&nbsp;<asp:Image ID="ImgConfirmedNote" runat="server" ImageUrl="~/Styles/Icons/orange_arrow_down.png" />:
        Đã phản hồi
    </asp:Panel>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa góp ý" CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa góp ý đã chọn không?"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px;">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:ImageButton ID="BtnFakedInUse" runat="server" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="MPEInfoInUse" runat="server" TargetControlID="BtnFakedInUse"
        PopupControlID="PnlInfoInUse" BackgroundCssClass="modalBackground" CancelControlID="imgPnlInfoInUse"
        PopupDragHandleControlID="PnlInfoInUseDrag">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="PnlInfoInUse" runat="server" CssClass="popup ui-corner-all" Width="350px">
        <asp:Panel ID="PnlInfoInUseDrag" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="Label4" runat="server" Text="Thông tin đang được sử dụng" CssClass="popup_header_title"></asp:Label>
            <img id="imgPnlInfoInUse" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="Label6" runat="server" Text="Một vài góp ý không thể sửa hay xóa vì thông tin đang được phản hồi"></asp:Label>
                <br />
            </div>
        </div>
        <div style="width: 85px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="BtnClose" runat="server" ImageUrl="~/Styles/buttons/button_close.png"
                OnClientClick="return popopInfoInUse_Close();" CssClass="BtnClose" />
        </div>
    </asp:Panel>
</asp:Content>
