<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="giaovienchunhiem.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.FormerTeacherListPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScripts">
        <script language="javascript" type="text/javascript">
            function popopConfirmDelete_CancelDelete_Click() {
                $find('<%=MPEDelete.ClientID%>').hide();
                return false;
            }
        </script>
    </div>
    <asp:UpdatePanel ID="UdPnlSearchCriteria" runat="server">
        <ContentTemplate>
            <div id="divSearch">
                <div id="divSearchCriteria">
                    <table class="search">
                        <tr>
                            <td style="width: 60px;">
                                Năm học:
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNamHoc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 35px;">
                                Khối:
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlKhoiLop_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 70px;">
                                Tên giáo viên:
                            </td>
                            <td style="width: 200px;">
                                <asp:TextBox ID="TxtTenGVCN" runat="server" Style="width: 150px;"></asp:TextBox>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="TenGVCNWatermark" runat="server" TargetControlID="TxtTenGVCN"
                                    WatermarkText="Tất cả">
                                </ajaxToolkit:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ngành:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlNganh" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNganh_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Lớp:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlLopHoc" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 85px;">
                                Mã giáo viên:
                            </td>
                            <td style="width: 200px;">
                                <asp:TextBox ID="TxtMaGVCN" runat="server" Style="width: 150px;"></asp:TextBox>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="MaGVCNWatermark" runat="server" TargetControlID="TxtMaGVCN"
                                    WatermarkText="Tất cả">
                                </ajaxToolkit:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divButtonSearch">
                    <asp:ImageButton ID="BtnSearch" runat="server" OnClick="BtnSearch_Click" ImageUrl="~/Styles/buttons/button_search.png"
                        Style="margin: 10px 0px 0px 0px" ToolTip="Tìm kiếm giáo viên chủ nhiệm" CssClass="BtnSearch" />
                </div>
                <br />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="BtnSearch" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" OnClick="BtnAdd_Click" ImageUrl="~/Styles/buttons/button_add.png"
                ToolTip="Thêm giáo viên chủ nhiệm mới" CssClass="BtnAdd" />
            <asp:ImageButton ID="BtnEdit" runat="server" OnClick="BtnEdit_Click" ImageUrl="~/Styles/buttons/button_edit.png"
                ToolTip="Sửa giáo viên chủ nhiệm" CssClass="BtnEdit" />
            <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Styles/buttons/button_delete.png"
                ToolTip="Xóa giáo viên chủ nhiệm" CssClass="BtnDelete" />
            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnDelete"
                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfMaGVCN" runat="server" />
            <asp:HiddenField ID="HdfClassId" runat="server" />
            <asp:HiddenField ID="HdfRptGVCNMPEDelete" runat="server" />
            <asp:Repeater ID="RptGVCN" runat="server" OnItemDataBound="RptFormerTeachers_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton1" runat="server">Giáo viên chủ nhiệm</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton2" runat="server">Lớp</asp:LinkButton>
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
                            <asp:HiddenField ID="HdfRptMaGVCN" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaGVCN")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:HiddenField ID="HdfUserId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "UserId")%>' />
                            <asp:HyperLink ID="HlkTenGVCN" runat="server" Target="_blank"><%#DataBinder.Eval(Container.DataItem, "TenGiaoVien")%></asp:HyperLink>
                        </td>
                        <td style="height: 40px;">
                            <asp:HyperLink ID="HlkClassName" runat="server" Target="_blank"><%#DataBinder.Eval(Container.DataItem, "ClassName")%></asp:HyperLink>
                        </td>
                        <td id="tdSelect" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
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
        <div style="float: right; margin-top: -35px; padding-right: 30px;">
            <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="pager_Command" ViewStateMode="Enabled" />
        </div>
    </div>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa giáo viên chủ nhiệm"
                CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa giáo viên chủ nhiệm đã chọn không?"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" OnClick="BtnOKDeleteItem_Click"
                ImageUrl="~/Styles/buttons/button_save.png" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
