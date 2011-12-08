<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="LoiNhanKhan.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ModuleParents.MessageForParentsPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScripts">
        <script type="text/javascript">
            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptLoiNhanKhanMPEDelete.ClientID%>').value;
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
                            PopupButtonID="ImgCalendarTuNgay" PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="TxtTuNgay"
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
                        <asp:TextBox ID="TxtDenNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarDenNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Top" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="TxtDenNgay"
                            PopupButtonID="ImgCalendarDenNgay" PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="TxtDenNgay"
                            MaskType="Date" Mask="99/99/9999" ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm lời nhắn" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfMaLoiNhanKhan" runat="server" />
            <asp:HiddenField ID="HdfRptLoiNhanKhanMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptLoiNhanKhanMPEDetail" runat="server" />
            <asp:Repeater ID="RptLoiNhanKhan" runat="server" OnItemCommand="RptLoiNhanKhan_ItemCommand"
                OnItemDataBound="RptLoiNhanKhan_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td class="middle" style="width: 20%">
                            Lời nhắn
                        </td>
                        <td class="middle">
                            Ngày
                        </td>
                        <td class="middle" style="width: 100px">
                            Xác nhận của phụ huynh
                        </td>
                        <td class="icon">
                            Xác nhận
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptMaLoiNhanKhan" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MessageId")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label28" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Title")%>'></asp:Label>
                            <%--<asp:Label ID="LblTieuDe" runat="server" style="display:none"></asp:Label>
                                <asp:LinkButton ID="LbtnTieuDe" runat="server"
                                    Text='<%#DataBinder.Eval(Container.DataItem, "TieuDe")%>'
                                    style="text-decoration:underline; color:Blue;cursor:pointer;"
                                    CommandName="CmdDetailItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaLoiNhanKhan")%>'>
                                </asp:LinkButton>--%>
                            <%--<ajaxToolkit:ModalPopupExtender ID="MPEDetail" runat="server"                                         
                                    TargetControlID="LblClassName"
                                    PopupControlID="PnlPopupDetail"
                                    BackgroundCssClass="modalBackground"
                                    CancelControlID="ImgClosePopupDetail"
                                    PopupDragHandleControlID="PnlDragPopupDetail">
                                </ajaxToolkit:ModalPopupExtender>--%>
                        </td>
                        <td style="height: 40px;">
                            <%#((DateTime)DataBinder.Eval(Container.DataItem, "Date")).ToShortDateString()%>
                        </td>
                        <td style="height: 40px;">
                            <%#((bool)DataBinder.Eval(Container.DataItem, "IsConfirmed") == true) ? "Có" : "Không"%>
                        </td>
                        <td class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Icons/icon_apply.png"
                                CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MessageId")%>' />
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
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa lời nhắn khẩn"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa lời nhắn khẩn này không?"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_yes.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="YesButton" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_no.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="NoButton" />
        </div>
    </asp:Panel>
</asp:Content>
