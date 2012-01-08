<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhsachgiaovien.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.TeachersPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript" runat="server">
        <script language="javascript" type="text/javascript">
            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptGiaoVienMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }
            function fncOpen() {
                var pageId = '<%=  Page.ClientID %>';
                __doPostBack(pageId, "myargs");
                window.showModalDialog("/modules/hoc_sinh/indanhsachhocsinh.aspx", null, "dialogWidth:1000px; dialogHeight:1000px; center:yes");
            } 
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            Mã giáo viên:&nbsp;
            <asp:TextBox ID="TxtSearchMaHienThiGiaoVien" runat="server" Width="150px"></asp:TextBox>
            <ajaxToolkit:TextBoxWatermarkExtender ID="UserIdWatermark" runat="server" TargetControlID="TxtSearchMaHienThiGiaoVien"
                WatermarkText="Tất cả">
            </ajaxToolkit:TextBoxWatermarkExtender>
            &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; Tên giáo viên:
            <asp:TextBox ID="TxtSearchTenGiaoVien" runat="server" Width="150px"></asp:TextBox>
            <ajaxToolkit:TextBoxWatermarkExtender ID="TenGiaoVienWatermark" runat="server" TargetControlID="TxtSearchTenGiaoVien"
                WatermarkText="Tất cả">
            </ajaxToolkit:TextBoxWatermarkExtender>
            &nbsp;&nbsp;
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" CssClass="BtnSearch" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm giáo viên" OnClick="BtnSearch_Click" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAddGiaoVien" runat="server" CssClass="BtnAdd" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm giáo viên mới" OnClick="BtnAdd_Click" />
            <asp:ImageButton ID="BtnExport" runat="server" ImageUrl="~/Styles/buttons/button_export.png"
                ToolTip="In danh sach học sinh" OnClientClick="fncOpen();" CssClass="BtnExport"
                Visible="true" />
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfTeacherCode" runat="server" />
            <asp:HiddenField ID="HdfUserId" runat="server" />
            <asp:HiddenField ID="HdfRptGiaoVienMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptGiaoVienMPEEdit" runat="server" />
            <asp:Repeater ID="RptGiaoVien" runat="server" OnItemCommand="RptGiaoVien_ItemCommand"
                OnItemDataBound="RptGiaoVien_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header ui-corner-right ui-corner-left">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td style="width: 100px">
                            <asp:LinkButton ID="LkBtnMaHienThi" runat="server" CommandName="SortByMaHienThiGiaoVien"
                                ForeColor="White">Mã giáo viên</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LkBtnHoTen" runat="server" CommandName="SortByHoTen" ForeColor="White">Họ tên giáo viên</asp:LinkButton>
                        </td>
                        <td id="thEdit" runat="server" class="icon">
                            Sửa
                        </td>
                        <%--<td id="thDelete" runat="server" class="icon">
                            Xóa
                        </td>--%>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptUserId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "UserId")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:HyperLink ID="HlkUserId" runat="server"><%#DataBinder.Eval(Container.DataItem, "MaHienThiGiaoVien")%></asp:HyperLink>
                        </td>
                        <td style="height: 40px;">
                            <asp:HyperLink ID="HyperLink1" runat="server"><%#DataBinder.Eval(Container.DataItem, "HoTen")%></asp:HyperLink>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CssClass="EditItemButton" CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "UserId")%>' />
                        </td>
                        <%--<td id="tdDelete" runat="server" class="icon" style="height: 40px">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CssClass="DeleteItemButton" CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaHienThiGiaoVien")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnFakeDeleteItem"
                                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>--%>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="6" class="footer ui-corner-bl ui-corner-br">
                        </td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
        <div style="float: right; margin-top: -35px; padding-right: 30px;">
            <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="DataPager_Command" ViewStateMode="Enabled" />
        </div>
    </div>
    <%--<asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa hạnh kiểm" CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../../Styles/Images/popup_button_close.png"
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
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>--%>
</asp:Content>
