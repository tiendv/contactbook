<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="ykien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.ParentsCommentPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptCommentMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
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
                            PopupButtonID="ImgCalendarBeginDate" PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="TxtBeginDate"
                            MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
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
                            PopupButtonID="ImgCalendarEndDate" PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="TxtEndDate"
                            MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm ý kiến phụ huynh" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" OnClick="BtnAdd_Click" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm lới góp ý mới" CssClass="BtnAdd" />
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
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
                        <td style="width: 100px">
                            Ngày góp ý
                        </td>
                        <td style="width: 90px">
                            Tình trạng
                        </td>
                        <td class="icon">
                            Sửa
                        </td>
                        <td class="icon">
                            Xóa
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
                            <asp:Image ID="ImgConfirmed" runat="server" ImageUrl="~/Styles/Icons/orange_arrow_down.png" Visible='<%#((int)DataBinder.Eval(Container.DataItem, "CommentStatusId") != 1)%>' />
                            <asp:Image ID="ImgUnconfirmed" runat="server" ImageUrl="~/Styles/Icons/green_arrow_up.png" Visible='<%#((int)DataBinder.Eval(Container.DataItem, "CommentStatusId") == 1)%>' />
                            <asp:LinkButton ID="LbtnCommentTitle" runat="server" Style="text-decoration: underline;
                                color: Blue; cursor: pointer;" CommandName="CmdDetailItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "CommentId")%>'
                                Font-Bold='<%#((int)DataBinder.Eval(Container.DataItem, "CommentStatusId") == 2)%>'>
                            <%#DataBinder.Eval(Container.DataItem, "Title")%>
                            </asp:LinkButton>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "Date")%>
                        </td>
                        <td style="height: 40px; text-align: center">
                            <%#DataBinder.Eval(Container.DataItem, "CommentStatusName")%>
                        </td>
                        <td style="height: 40px; text-align: center">
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "CommentId")%>' />
                        </td>
                        <td style="height: 40px; text-align: center">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "Title")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnFakeDeleteItem"
                                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
                            </ajaxToolkit:ModalPopupExtender>
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
    <div style="float: right; margin-top: -35px; padding-right: 30px;">
        <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="MainDataPager_Command"
            ViewStateMode="Enabled" />
    </div>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa góp ý" CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px;">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
