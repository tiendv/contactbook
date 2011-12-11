<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="LoiNhanKhan.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ModuleParents.MessageForParentsPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScripts">
        <script type="text/javascript">
            function popopConfirmMessage_CancelConfirm_Click() {
                var mPEConfirmID = $get('<%=HdfRptLoiNhanKhanMPEConfirm.ClientID%>').value;
                $find(mPEConfirmID).hide();
                return false;
            }

            function popopCancelConfirmMessage_Cancel_Click() {
                var mPECancelConfirmID = $get('<%=HdfRptLoiNhanKhanMPECancelConfirm.ClientID%>').value;
                $find(mPECancelConfirmID).hide();
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
            <asp:HiddenField ID="HdfRptLoiNhanKhanMPECancelConfirm" runat="server" />
            <asp:HiddenField ID="HdfRptLoiNhanKhanMPEConfirm" runat="server" />
            <asp:HiddenField ID="HdfRptLoiNhanKhanMPEDetail" runat="server" />
            <asp:Repeater ID="RptLoiNhanKhan" runat="server" OnItemCommand="RptLoiNhanKhan_ItemCommand"
                OnItemDataBound="RptLoiNhanKhan_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td>
                            Lời nhắn
                        </td>
                        <td style="width: 200px">
                            Ngày
                        </td>
                        <td style="width: 100px">
                            Xác nhận
                        </td>
                        <td style="width: 100px">
                            Hủy xác nhận
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
                            <%--<asp:Label ID="Label28" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Title")%>'></asp:Label>--%>
                            <asp:Label ID="LblTitle" runat="server" style="display:none"></asp:Label>
                            <asp:LinkButton ID="LbtnTieuDe" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Title")%>'
                                Style="text-decoration: underline; color: Blue; cursor: pointer;" CommandName="CmdDetailItem"
                                CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MessageId")%>'>
                            </asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="MPEDetail" runat="server" TargetControlID="LblTitle"
                                PopupControlID="PnlPopupDetail" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupDetail"
                                PopupDragHandleControlID="PnlDragPopupDetail">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td style="height: 40px;">
                            <%#((DateTime)DataBinder.Eval(Container.DataItem, "Date")).ToShortDateString()%>
                        </td>
                        <td style="height: 40px; text-align: center">
                            <asp:ImageButton ID="BtnFakeConfirmItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnConfirmItem" runat="server" ImageUrl="~/Styles/Icons/icon_apply.png"
                                CommandName="CmdConfirmItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "Title")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEConfirm" runat="server" TargetControlID="BtnFakeConfirmItem"
                                PopupControlID="PnlPopupConfirmMessage" BackgroundCssClass="modalBackground"
                                CancelControlID="imgClosePopupConfirmMessage" PopupDragHandleControlID="PnlDragPopupConfirmMessage">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td style="height: 40px; text-align: center">
                            <asp:ImageButton ID="BtnFakeCancelConfirmItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnCancelConfirmItem" runat="server" ImageUrl="~/Styles/Icons/icon_delete.png"
                                CommandName="CmdCancelConfirmItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "Title")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPECancelConfirm" runat="server" TargetControlID="BtnFakeCancelConfirmItem"
                                PopupControlID="PnlPopupCancelConfirmMessage" BackgroundCssClass="modalBackground"
                                CancelControlID="imgClosePopupCancelConfirmMessage" PopupDragHandleControlID="PnlDragPopupCancelConfirmMessage">
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
    <asp:Panel ID="PnlPopupConfirmMessage" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmMessage" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmMessageTitle" runat="server" Text="Xác nhận lời nhắn khẩn"></asp:Label>
            <img id="imgClosePopupConfirmMessage" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmMessage" runat="server"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKConfirmItem" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnOKConfirmItem_Click" CssClass="SaveButton" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelConfirmItem" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopConfirmMessage_CancelConfirm_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupCancelConfirmMessage" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupCancelConfirmMessage" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupCancelConfirmMessageTitle" runat="server" Text="Hủy xác nhận lời nhắn khẩn"></asp:Label>
            <img id="imgClosePopupCancelConfirmMessage" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblCancelConfirmMessage" runat="server"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKCancelConfirmItem" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnCancelConfirmItem_Click" CssClass="SaveButton" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopCancelConfirmMessage_Cancel_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupDetail" runat="server" CssClass="popup ui-corner-all" Width="550px">
        <asp:Panel ID="PnlDragPopupDetail" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupDetailTitle" runat="server" Text="Chi tiết lời nhắn khẩn"></asp:Label>
            <img id="ImgClosePopupDetail" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table style="width: 100%" class="inputBorder">
                <tr>
                    <td style="width: 60px; vertical-align: top; padding-top: 3px;">
                        Tiêu đề:
                    </td>
                    <td style="width: auto;" colspan="3" class="readOnlyTextBox input_textbox">
                        <asp:Label ID="LblTitle" runat="server" Style="width: 99%"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 60px; vertical-align: top; padding-top: 3px;" >
                        Nội dung:
                    </td>
                    <td style="width: auto;" colspan="3" class="readOnlyTextBox input_textbox">
                    <asp:Label ID="LblContent" runat="server" Style="width: 99%"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <%--<div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 10px 0px">
            <asp:ImageButton ID="BtnSaveAdd" runat="server" OnClick="BtnSaveAdd_Click" ValidationGroup="AddLoiNhanKhan"
                ImageUrl="~/Styles/Images/button_save.png" CssClass="SaveButton" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" OnClientClick="return popopDetail_CancelSave_Click();"
                ImageUrl="~/Styles/Images/button_cancel.png" CssClass="CancelButton" />
        </div>--%>
    </asp:Panel>
</asp:Content>
