<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="sapxepthoikhoabieu.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ScheduleArrangementPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScripts">
        <script type="text/javascript">
            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }
        </script>
    </div>
    <asp:Label ID="LblTitle" runat="server" Font-Bold="true"></asp:Label>
    <asp:Panel ID="PnlLinkToCategory" runat="server" CssClass="loginBox ui-corner-all"
        Visible="false" Style="width: 90%; padding: 20px; margin: 0px auto 0px auto">
        Vui lòng thêm tiết học mới ở menu
        <asp:HyperLink ID="HyperLink1" runat="server" Text=" danh mục tiết học "></asp:HyperLink>
        trước khi thực hiện chức năng này
    </asp:Panel>
    <asp:Panel ID="PnlSchedule" runat="server">
        <div class="table_data ui-corner-all">
            <table class="repeater" style="width: 100%">
                <asp:HiddenField ID="HdfMPEDelete" runat="server" />
                <asp:HiddenField ID="HdfSelectedScheduleId" runat="server" />
                <asp:Repeater ID="RptSchedule" runat="server" OnItemDataBound="RptSchedule_ItemDataBound">
                    <HeaderTemplate>
                        <tr class="header">
                            <td style="width: 80px; text-align: center" class="ui-corner-tl">
                                Thứ
                            </td>
                            <td>
                                Tiết
                            </td>
                            <%--<td>
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
                        </td>--%>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                            <td style="width: 80px; text-align: center">
                                <asp:HiddenField ID="HdfDayInWeekId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "DayInWeekId")%>' />
                                <%#DataBinder.Eval(Container.DataItem, "DayInWeekName")%>
                            </td>
                            <td style="padding: 0px">
                                <table class="repeater">
                                    <asp:HiddenField ID="HdfSubjectIdTKB" runat="server" />
                                    <asp:Repeater ID="RptDailySchedule" runat="server" OnItemDataBound="RptDailySchedule_ItemDataBound"
                                        OnItemCommand="RptDailySchedule_ItemCommand">
                                        <ItemTemplate>
                                            <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                                                <td style="height: 40px; width: 200px">
                                                    <asp:HiddenField ID="HdfRptScheduleId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ScheduleId")%>' />
                                                    <asp:HiddenField ID="HdfDayInWeekId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "DayInWeekId")%>' />
                                                    <%#DataBinder.Eval(Container.DataItem, "StringDetailTeachingPeriod")%>
                                                </td>
                                                <td style="height: 40px; width: 200px">
                                                    <asp:Label ID="LblNghi" runat="server" Text="Nghỉ" Visible="false"></asp:Label>
                                                    <asp:HiddenField ID="HdfOrginalSubjectId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SubjectId")%>' />
                                                    <asp:HiddenField ID="HdfSubjectId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SubjectId")%>' />
                                                    <%#DataBinder.Eval(Container.DataItem, "SubjectName")%>
                                                </td>
                                                <td style="height: 40px;">
                                                    <asp:HiddenField ID="HdfOrginalUserId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "UserId")%>' />
                                                    <asp:HiddenField ID="HdfUserId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "UserId")%>' />
                                                    <%#DataBinder.Eval(Container.DataItem, "TeacherName")%>
                                                </td>
                                                <td id="tdAdd" runat="server" class="icon" style="height: 40px;">
                                                    <asp:ImageButton ID="BtnAddItem" runat="server" ImageUrl="~/Styles/Icons/icon_add.png"
                                                        CommandName="CmdAddItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TeachingPeriodId")%>' />
                                                </td>
                                                <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                                                    <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/buttons/icon_edit.png"
                                                        CommandName="CmdEditItem" CommandArgument='<%#string.Format("{0}:{1}", DataBinder.Eval(Container.DataItem, "ScheduleId"), DataBinder.Eval(Container.DataItem, "TempScheduleId"))%>' />
                                                        
                                                </td>
                                                <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                                                    <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                                                    <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/buttons/icon_delete.png"
                                                        CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ScheduleId")%>' />
                                                    <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnFakeDeleteItem"
                                                        PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                                                        PopupDragHandleControlID="PnlDragPopupConfirmDelete">
                                                    </ajaxToolkit:ModalPopupExtender>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
            Width="350px">
            <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
                <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa thời khóa biểu"></asp:Label>
                <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                    alt="close" />
            </asp:Panel>
            <div style="padding: 10px;">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                    Style="float: left;" />
                <div style="width: 85%; float: left; padding-left: 10px;">
                    <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa thông tin thời khóa biểu này không?"></asp:Label>
                </div>
            </div>
            <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
                <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                    OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />
                &nbsp;&nbsp;
                <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                    OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
            </div>
        </asp:Panel>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="SaveButton" runat="server" OnClick="BtnSave_Click" ImageUrl="~/Styles/buttons/button_save.png"
                ValidationGroup="SaveSchedule" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancel" runat="server" OnClick="BtnCancel_Click" ImageUrl="~/Styles/buttons/button_cancel.png"
                CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
