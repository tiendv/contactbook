<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="suathoikhoabieu.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.SuaThoiKhoaBieuPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScripts">
        <script type="text/javascript">
            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptThoiKhoaBieuMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }
        </script>
    </div>
    <asp:Label ID="LblTitle" runat="server" Font-Bold="true"></asp:Label>
    <div id="divSearch">
        <div id="divSearchCriteria">
            Thứ:
            <asp:DropDownList ID="DdlThu" runat="server" Width="150px">
            </asp:DropDownList>
        </div>
        <div id="divButtonSearch" style="margin: -15px 0px 0px 20px">
            <asp:ImageButton ID="BtnSearch" runat="server" OnClick="BtnSearch_Click" ImageUrl="~/Styles/Images/button_search_with_text.png"
                Style="margin: 10px 0px 0px 0px" ToolTip="Tìm kiếm thời khóa biểu" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <table class="repeater">
            <asp:HiddenField ID="HdfSubjectIdTKB" runat="server" />
            <asp:HiddenField ID="HdfRptThoiKhoaBieuMPEDelete" runat="server" />
            <asp:Repeater ID="RptThoiKhoaBieu" runat="server" OnItemDataBound="RptThoiKhoaBieu_ItemDataBound"
                OnItemCommand="RptThoiKhoaBieu_ItemCommand">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="orderNo ui-corner-tl">
                            STT
                        </td>
                        <td>
                            <asp:LinkButton ID="LkBtnTiet" runat="server">Tiết</asp:LinkButton>
                        </td>
                        <td>
                            Môn Học
                        </td>
                        <td>
                            Giáo viên
                        </td>
                        <td class="icon">
                            Thêm
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
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfSubjectIdTKB" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ScheduleId")%>' />
                        </td>
                        <td style="height: 40px; width: 200px">
                            <%#DataBinder.Eval(Container.DataItem, "StringDetailTeachingPeriod")%>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="LblNghi" runat="server" Text="Nghỉ" Visible="false"></asp:Label>
                            <%#DataBinder.Eval(Container.DataItem, "SubjectName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "TeacherName")%>
                        </td>
                        <td id="tdAdd" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnAddItem" runat="server" ImageUrl="~/Styles/Icons/icon_add.png"
                                CommandName="CmdAddItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TeachingPeriodId")%>' />
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ScheduleId")%>' />
                        </td>
                        <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ScheduleId")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnFakeDeleteItem"
                                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="7" class="footer ui-corner-bl ui-corner-br">
                        </td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
        <div style="float: right; margin-top: -45px; padding-right: 30px;">
            <cc1:DataPager ID="MainDataPager" runat="server" OfClause="/" PageClause="TRANG"
                PageSize="10" ViewStateMode="Enabled" LastClause=">>" GenerateHiddenHyperlinks="False"
                CompactModePageCount="3" GenerateFirstLastSection="True" GenerateGoToSection="False"
                FirstClause="<<" BackToFirstClause="Trở về trang đầu" BackToPageClause="Trở về trang"
                GoToLastClause="Đến trang cuối" NextToPageClause="Đến trang" ShowResultClause="Hiển thị kết quả"
                ToClause="đến" Visible="false" />
        </div>
    </div>
    <div style="padding: 5px; vertical-align: middle;">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa lớp học"></asp:Label>
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
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_yes.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="YesButton" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_no.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="NoButton" />
        </div>
    </asp:Panel>
</asp:Content>
