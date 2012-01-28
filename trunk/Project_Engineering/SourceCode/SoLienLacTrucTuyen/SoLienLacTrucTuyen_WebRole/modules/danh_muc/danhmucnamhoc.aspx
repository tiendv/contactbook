<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhmucnamhoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.CategoryYearPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function popopConfirmDelete_Cancel_Click() {
                $find('<%=MPEDelete.ClientID%>').hide();
                return false;
            }

            function popopInfoInUse_Close() {
                $find('<%=MPEInfoInUse.ClientID%>').hide();
                return false;
            }             

            $('document').ready(function () {
                $('.BtnDelete').click(function (event) {
                    $find('<%=MPEDelete.ClientID%>').hide();
                });
            });
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            <asp:Label ID="Label9" runat="server" Text="Tên năm học:"></asp:Label>&nbsp;
            <asp:TextBox ID="TxtSearchYear" runat="server" Width="150px"></asp:TextBox>&nbsp;&nbsp;
            <ajaxToolkit:TextBoxWatermarkExtender ID="YearWatermark" runat="server" TargetControlID="TxtSearchYear"
                WatermarkText="Tất cả">
            </ajaxToolkit:TextBoxWatermarkExtender>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm năm học" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" OnClick="BtnAdd_Click" ImageUrl="~/Styles/buttons/button_add.png"
                ToolTip="Thêm năm học mới" CssClass="BtnAdd" />
            <asp:ImageButton ID="BtnEdit" runat="server" OnClick="BtnEdit_Click" ImageUrl="~/Styles/buttons/button_edit.png"
                ToolTip="Sửa năm học" CssClass="BtnEdit" />
            <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Styles/buttons/button_delete.png"
                ToolTip="Xóa năm học" CssClass="BtnDelete" />
            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnDelete"
                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:Repeater ID="RptYears" runat="server" OnItemDataBound="RptYears_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LnkYear" runat="server">Năm học</asp:LinkButton>
                        </td>
                        <td>
                            Thời gian học kì 1
                        </td>
                        <td>
                            Thời gian học kì 2
                        </td>
                        <td id="thSelectAll" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelectAll" runat="server" CssClass="selectAll" />
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptYearId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "YearId")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="LblYearName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "YearName")%>'></asp:Label>
                        </td>
                        <td>
                            Từ ngày
                            <asp:Label ID="LblFirstTermBeginDate" runat="server" Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "FirstTermBeginDate")).ToShortDateString()%>'></asp:Label>
                            đến ngày
                            <asp:Label ID="LblFirstTermEndDate" runat="server" Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "FirstTermEndDate")).ToShortDateString()%>'></asp:Label>
                        </td>
                        <td>
                            Từ ngày
                            <asp:Label ID="LblSecondTermBeginDate" runat="server" Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "SecondTermBeginDate")).ToShortDateString()%>'></asp:Label>
                            đến ngày
                            <asp:Label ID="LblSecondTermEndDate" runat="server" Text='<%#((DateTime)DataBinder.Eval(Container.DataItem, "SecondTermEndDate")).ToShortDateString()%>'></asp:Label>
                        </td>
                        <td id="tdSelect" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="5" class="footer ui-corner-bl ui-corner-br">
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
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa năm học" CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa năm học đã chọn hay không?"></asp:Label>
                <br />
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopConfirmDelete_Cancel_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:ImageButton ID="BtnFakedInUse" runat="server" style="display: none"/>
    <ajaxToolkit:ModalPopupExtender ID="MPEInfoInUse" runat="server" TargetControlID="BtnFakedInUse"
        PopupControlID="PnlInfoInUse" BackgroundCssClass="modalBackground" CancelControlID="imgPnlInfoInUse"
        PopupDragHandleControlID="PnlInfoInUseDrag">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="PnlInfoInUse" runat="server" CssClass="popup ui-corner-all" Width="350px">
        <asp:Panel ID="PnlInfoInUseDrag" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="Label1" runat="server" Text="Thông tin đang được sử dụng" CssClass="popup_header_title"></asp:Label>
            <img id="imgPnlInfoInUse" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="Label2" runat="server" Text="Một vài năm học không thể xóa vì thông tin đang được sử dụng"></asp:Label>
                <br />
            </div>
        </div>
        <div style="width: 85px; margin: 0px auto 0px auto; padding-bottom: 5px;">            
            <asp:ImageButton ID="BtnClose" runat="server" ImageUrl="~/Styles/buttons/button_close.png"
                OnClientClick="return popopInfoInUse_Close();" CssClass="BtnClose" />
        </div>
    </asp:Panel>
</asp:Content>
