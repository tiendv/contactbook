<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="suathoikhoabieu.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ScheduleArrangementPage"
    EnableEventValidation="false" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <asp:Label ID="LblTitle" runat="server" Font-Bold="true"></asp:Label>
    <div class="table_data ui-corner-all">
        <table class="repeater">
            <asp:Repeater ID="RptDailySchedule" runat="server" OnItemDataBound="RptDailySchedule_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header ui-corner-right ui-corner-left">
                        <td class="ui-corner-tl" style="width: 10%">
                            Thứ
                        </td>
                        <td style="width: 20%">
                            Tiết
                        </td>
                        <td style="width: 30%">
                            Môn học
                        </td>
                        <td style="width: 30%">
                            Giáo viên
                        </td>
                        <td style="width: 10%">
                            Xóa
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%#DataBinder.Eval(Container.DataItem, "DayInWeekName")%>
                        </td>
                        <td style="height: 40px; padding: 0px" colspan="4">
                            <table style="width: 100%">
                                <asp:Repeater ID="rptTeachingPeriodSchedule" runat="server" OnItemCommand="RptTeachingPeriodSchedule_ItemCommand">
                                    <ItemTemplate>
                                        <tr id="RepeaterRow" runat="server">
                                            <td style="height: 40px; width: 22%">
                                                <%#DataBinder.Eval(Container.DataItem, "StringDetailTeachingPeriod")%>
                                            </td>
                                            <td style="height: 40px; width: 33%">
                                                <%#DataBinder.Eval(Container.DataItem, "SubjectName")%>
                                                <asp:ImageButton ID="BtnFakeSubjectItem" runat="server" Style="display: none;" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                                    CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ClassName")%>' />
                                                <ajaxToolkit:ModalPopupExtender ID="MPEChooseSubject" runat="server" TargetControlID="BtnFakeSubjectItem"
                                                    PopupControlID="PnlPopupMonHoc" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupMonHoc"
                                                    PopupDragHandleControlID="PnlDragPopupMonHoc">
                                                </ajaxToolkit:ModalPopupExtender>
                                            </td>
                                            <td style="height: 40px; width: 33%">
                                                <%#DataBinder.Eval(Container.DataItem, "TeacherName")%>
                                            </td>
                                            <td style="height: 40px; width: 11%">
                                                <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                                    CommandName="CmdDeleteItem" />
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
    <asp:Panel ID="PnlPopupMonHoc" runat="server" CssClass="popup ui-corner-all" Width="510px">
        <asp:Panel ID="PnlDragPopupMonHoc" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupMonHocTitle" runat="server" CssClass="popup_header_title"
                Text="Chọn môn học cho tiết học"></asp:Label>
            <img id="ImgClosePopupMonHoc" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div id="divSearchMonHoc" style="padding: 10px 7px 0px 7px;">
            <table class="search" style="width: 100%">
                <tr>
                    <td style="width: 60px; height: 25px">
                        Ngành:
                    </td>
                    <td>
                        <asp:DropDownList ID="DdlNganh" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Khối:
                    </td>
                    <td>
                        <asp:DropDownList ID="DdlKhoi" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Môn học:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtMonHoc" runat="server"></asp:TextBox>
                        <ajaxToolkit:TextBoxWatermarkExtender ID="MonHocWatermark" runat="server" TargetControlID="TxtMonHoc"
                            WatermarkText="Tất cả">
                        </ajaxToolkit:TextBoxWatermarkExtender>
                    </td>
                    <td colspan="2">
                        <asp:ImageButton ID="BtnSearchMonHoc" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                            ToolTip="Tìm kiếm môn học" OnClick="BtnSearchMonHoc_Click" CssClass="BtnSearch" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="table_data ui-corner-all" style="border-style: none; width: 100%;">
            <div>
                <asp:Label ID="LblSearchResultMonHoc" runat="server" Text="Không tìm thấy môn học"
                    Style="font-size: 15px; font-weight: bold;"></asp:Label>
            </div>
            <table class="repeater ui-corner-all" style="width: 95%;">
                <asp:HiddenField ID="HdfSubjectId" runat="server" Value="0" />
                <asp:Repeater ID="RptMonHoc" runat="server">
                    <HeaderTemplate>
                        <tr class="header">
                            <td class="ui-corner-tl orderNo">
                                STT
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton2" runat="server">Môn học</asp:LinkButton>
                            </td>
                            <td class="icon ui-corner-tr">
                                Chọn
                            </td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                            <td style="height: 40px; text-align: center">
                                <%# (DataPageMonHoc.CurrentIndex - 1) * DataPageMonHoc.PageSize + Container.ItemIndex + 1%>
                                <asp:HiddenField ID="HdfRptSubjectId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SubjectId")%>' />
                                <asp:HiddenField ID="HdfRptSubjectName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SubjectName")%>' />
                            </td>
                            <td style="height: 40px;">
                                <asp:Label ID="LblSubjectName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SubjectName")%>'></asp:Label>
                            </td>
                            <td style="height: 40px; text-align: right">
                                <asp:RadioButton ID="RBtnSelect" runat="server" CssClass="radioMonHoc" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr>
                            <td colspan="3" class="footer ui-corner-bl ui-corner-br">
                            </td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
            <div style="float: right; margin-top: -35px; padding-right: 30px;">
                <cc1:DataPager ID="DataPageMonHoc" runat="server" OnCommand="DataPagerMonHoc_Command"
                    ViewStateMode="Enabled" LastClause=">>" FirstClause="<<" ToClause="đến" />
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveMonHoc" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnSaveMonHoc_Click" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopMonHoc_Cancel_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupGiaoVien" runat="server" CssClass="popup ui-corner-all" Width="550px">
        <asp:Panel ID="PnlDragPopupGiaoVien" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupGiaoVienTitle" runat="server" CssClass="popup_header_title"
                Text="Chọn giáo viên cho tiết học"></asp:Label>
            <img id="ImgClosePopupGiaoVien" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div id="divSearchGiaoVien" style="padding: 10px 7px 0px 7px;">
            <table class="search" style="width: 100%">
                <tr>
                    <td>
                        Mã giáo viên:
                        <asp:TextBox ID="TxtSearchUserId" runat="server" Width="150px"></asp:TextBox>
                        <ajaxToolkit:TextBoxWatermarkExtender ID="UserIdWatermark" runat="server" TargetControlID="TxtSearchUserId"
                            WatermarkText="Tất cả">
                        </ajaxToolkit:TextBoxWatermarkExtender>
                    </td>
                    <td>
                        Tên giáo viên:
                        <asp:TextBox ID="TxtSearchTenGiaoVien" runat="server" Width="150px"></asp:TextBox>
                        <ajaxToolkit:TextBoxWatermarkExtender ID="TenGiaoVienWatermark" runat="server" TargetControlID="TxtSearchTenGiaoVien"
                            WatermarkText="Tất cả">
                        </ajaxToolkit:TextBoxWatermarkExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ImageButton ID="BtnSearchGiaoVien" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                            ToolTip="Tìm kiếm giáo viên" OnClick="BtnSearchGiaoVien_Click" CssClass="BtnSearch" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="table_data ui-corner-all" style="border-style: none; width: 100%;">
            <div>
                <asp:Label ID="LblSearchResultGiaoVien" runat="server" Text="Không tìm thấy giáo viên"
                    Style="font-size: 15px; font-weight: bold;"></asp:Label>
            </div>
            <table class="repeater ui-corner-all" style="width: 95%;">
                <asp:HiddenField ID="HdfUserId" runat="server" Value="0" />
                <asp:Repeater ID="RptGiaoVien" runat="server">
                    <HeaderTemplate>
                        <tr class="header">
                            <td class="ui-corner-tl orderNo">
                                STT
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton1" runat="server">Mã giáo viên</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton2" runat="server">Tên giáo viên</asp:LinkButton>
                            </td>
                            <td class="icon ui-corner-tr">
                                Chọn
                            </td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                            <td style="height: 40px; text-align: center">
                                <%# (DataPageGiaoVien.CurrentIndex - 1) * DataPageGiaoVien.PageSize + Container.ItemIndex + 1%>
                                <asp:HiddenField ID="HdfRptUserId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "UserId")%>' />
                                <asp:HiddenField ID="HdfRptTenGiaoVien" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "HoTen")%>' />
                            </td>
                            <td style="height: 40px;">
                                <%#DataBinder.Eval(Container.DataItem, "MaHienThiGiaoVien")%>
                            </td>
                            <td style="height: 40px;">
                                <asp:Label ID="LblTenGiaoVien" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "HoTen")%>'></asp:Label>
                            </td>
                            <td style="height: 40px; text-align: right">
                                <asp:RadioButton ID="RBtnSelect" runat="server" CssClass="radioGiaoVien" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr>
                            <td colspan="4" class="footer ui-corner-bl ui-corner-br">
                            </td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
            <div style="float: right; margin-top: -35px; padding-right: 30px;">
                <cc1:DataPager ID="DataPageGiaoVien" runat="server" OnCommand="DataPagerGiaoVien_Command"
                    ViewStateMode="Enabled" LastClause=">>" PageSize="8" FirstClause="<<" ToClause="đến" />
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveGiaoVien" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnSaveGiaoVien_Click" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopGiaoVien_Cancel_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
