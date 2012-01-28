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
            <asp:ImageButton ID="BtnSearch" runat="server" CssClass="BtnSearch" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm giáo viên" OnClick="BtnSearch_Click" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAddGiaoVien" runat="server" CssClass="BtnAdd" ImageUrl="~/Styles/buttons/button_add.png"
                ToolTip="Thêm giáo viên mới" OnClick="BtnAdd_Click" />
            <asp:ImageButton ID="BtnEdit" runat="server" OnClick="BtnEdit_Click" ImageUrl="~/Styles/buttons/button_edit.png"
                ToolTip="Sửa khối lớp" CssClass="BtnEdit" />
            <asp:ImageButton ID="BtnExport" runat="server" ImageUrl="~/Styles/buttons/button_export.png"
                ToolTip="Báo cáo" OnClientClick="fncOpen();" CssClass="BtnExport" />
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfTeacherCode" runat="server" />
            <asp:HiddenField ID="HdfUserId" runat="server" />
            <asp:HiddenField ID="HdfRptGiaoVienMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptGiaoVienMPEEdit" runat="server" />
            <asp:Repeater ID="RptGiaoVien" runat="server" OnItemDataBound="RptGiaoVien_ItemDataBound" OnItemCommand="RptGiaoVien_ItemCommand">
                <HeaderTemplate>
                    <tr class="header ui-corner-right ui-corner-left">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td style="width: 100px">
                            <asp:LinkButton ID="LkBtnStudentCode" runat="server" CommandName="SortByMaHienThiGiaoVien" ForeColor="White">Mã giáo viên</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LkBtnHoTen" runat="server" CommandName="SortByHoTen" ForeColor="White">Họ tên giáo viên</asp:LinkButton>
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
                            <asp:HiddenField ID="HdfRptUserId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "UserId")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:LinkButton ID="LbBtnTeacherCode" runat="server" CommandName="CmdDetailItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "UserId")%>'><%#DataBinder.Eval(Container.DataItem, "MaHienThiGiaoVien")%></asp:LinkButton>
                        </td>
                        <td style="height: 40px;">
                            <asp:HyperLink ID="HyperLink1" runat="server"><%#DataBinder.Eval(Container.DataItem, "HoTen")%></asp:HyperLink>
                        </td>
                        <td id="tdSelect" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
                        </td>
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
</asp:Content>
