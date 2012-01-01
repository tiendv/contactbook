<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="supplierlist.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.SuppliersPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function popopConfirmDelete_Cancel_Click() {
                $find('<%=MPEDelete.ClientID%>').hide();
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
            <asp:UpdatePanel ID="UPDropdownlists" runat="server">
                <ContentTemplate>
                    <table class="search">
                        <tr>
                            <td>
                                Tỉnh/Thành:
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="DdlProvinces" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlProvinces_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Huyện/Quận:
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="DdlDistricts" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Trường:
                            </td>
                            <td>
                                <asp:TextBox ID="TxtSchoolName" runat="server" Width="250px"></asp:TextBox>
                                <asp:TextBoxWatermarkExtender ID="SchoolNameWatermark" runat="server" TargetControlID="TxtSchoolName"
                                    WatermarkText="Tất cả">
                                </asp:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="width: 30px; margin: 0px auto 0px auto; padding: 10px">
            <asp:ImageButton ID="BtnSearch" runat="server" OnClick="BtnSearch_Click" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm trường học" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" OnClick="BtnAdd_Click" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm trường học mới" CssClass="BtnAdd" />
            <asp:ImageButton ID="BtnEdit" runat="server" OnClick="BtnEdit_Click" ImageUrl="~/Styles/buttons/button_edit.png"
                ToolTip="Sửa trường học" CssClass="BtnEdit" />
            <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Styles/buttons/button_delete.png"
                ToolTip="Xóa trường học" CssClass="BtnDelete" />
            <asp:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnDelete"
                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
            </asp:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:Repeater ID="RptSchools" runat="server">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td>
                            <asp:LinkButton ID="LkBtnSchoolName" runat="server">Trường</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LkBtnProvinceName" runat="server">Tỉnh/Thành</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LkBtnDistrictName" runat="server">Huyện/Quận</asp:LinkButton>
                        </td>
                        <td>
                            Email
                        </td>
                        <td>
                            Điện thoại
                        </td>
                        <td>
                            Trạng thái
                        </td>
                        <td class="icon">
                            <asp:CheckBox ID="CkbxSelectAll" runat="server" CssClass="selectAll" />
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptSchoolId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SchoolId")%>' />
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "SchoolName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "ProvinceName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "DistrictName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "Email")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "Phone")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "Status")%>
                        </td>
                        <td class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
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
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa trường Học" CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa trường học đã chọn hay không?"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 15px 0px 5px 0px;">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnDelete_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopConfirmDelete_Cancel_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
