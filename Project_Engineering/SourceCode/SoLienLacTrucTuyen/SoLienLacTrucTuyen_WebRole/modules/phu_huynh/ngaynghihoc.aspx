<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="ngaynghihoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.ModuleParents.StudentAbsentPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script type="text/javascript">
            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptAbsentMPEConfirm.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }

            function popopUnConfirm_Cancel_Click() {
                var mPEDeleteID = $get('<%=HdfRptAbsentMPEUnConfirm.ClientID%>').value;
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
                    <td style="width: 250px;">
                        <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Từ ngày:
                    </td>
                    <td style="width: 180px;">
                        <asp:TextBox ID="TxtTuNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarTuNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Middle" />
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
                        Học kỳ:
                    </td>
                    <td style="width: 200px;">
                        <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Đến ngày:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtDenNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarDenNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                            ImageAlign="Middle" />
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
        <div id="divButtonSearch" style="padding-top: 5px">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm ngày nghỉ học" OnClick="BtnSearch_Click" CssClass="BtnSearch" ValidationGroup="Search" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfMaNgayNghiHoc" runat="server" />
            <asp:HiddenField ID="HdfRptAbsentMPEConfirm" runat="server" />
            <asp:HiddenField ID="HdfRptAbsentMPEUnConfirm" runat="server" />
            <asp:Repeater ID="RptNgayNghi" runat="server" OnItemCommand="RptNgayNghi_ItemCommand"
                OnItemDataBound="RptNgayNghi_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td style="width: 100px">
                            <asp:LinkButton ID="LkBtnNgay" runat="server">Ngày</asp:LinkButton>
                        </td>
                        <td style="width: 100px">
                            <asp:LinkButton ID="LkBtnBuoi" runat="server">Buổi</asp:LinkButton>
                        </td>
                        <td style="width: 100px">
                            <asp:LinkButton ID="LkBtnCoPhep" runat="server">Có phép</asp:LinkButton>
                        </td>
                        <td>
                            Lý do
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
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                            <asp:HiddenField ID="HdfRptMaNgayNghiHoc" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "AbsentId")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label40" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Date")%>'></asp:Label>
                        </td>
                        <td style="height: 40px; padding:5px 0px 5px 0px">
                            <asp:Label ID="Label41" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Session")%>'></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label42" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "IsAsked")%>'></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label43" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Reason")%>'></asp:Label>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeEditItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnConfirm" runat="server" ImageUrl="~/Styles/Icons/icon_apply.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AbsentId")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEConfirm" runat="server" TargetControlID="BtnFakeEditItem"
                                PopupControlID="PnlPopupConfirm" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirm"
                                PopupDragHandleControlID="PnlDragPopupConfirm">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeUnConfirm" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnUnConfirm" runat="server" ImageUrl="~/Styles/Icons/icon_delete.png"
                                CommandName="CmdUnConfirm" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "AbsentId")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEUnConfirm" runat="server" TargetControlID="BtnFakeUnConfirm"
                                PopupControlID="PnlPopupUnConfirm" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupUnConfirm"
                                PopupDragHandleControlID="PnlDragPopupUnConfirm">
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
        <div style="float: right; margin-top: -35px; padding-right: 30px;">
            <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="MainDataPager_Command"
                ViewStateMode="Enabled" />
        </div>
    </div>
    <asp:Panel ID="PnlPopupConfirm" runat="server" CssClass="popup ui-corner-all" Width="350px">
        <asp:Panel ID="PnlDragPopupConfirm" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmTitle" runat="server" Text="Xác nhận ngày nghỉ học"
                CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirm" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupUnConfirm" runat="server" CssClass="popup ui-corner-all" Width="350px">
        <asp:Panel ID="PnlDragPopupUnConfirm" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupUnConfirmTitle" runat="server" Text="Xác nhận ngày nghỉ học"
                CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupUnConfirm" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblUnConfirm" runat="server"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="BtnOKUnConfirm" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnOKUnConfirm_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelUnConfirmBtn" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopUnConfirm_Cancel_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
