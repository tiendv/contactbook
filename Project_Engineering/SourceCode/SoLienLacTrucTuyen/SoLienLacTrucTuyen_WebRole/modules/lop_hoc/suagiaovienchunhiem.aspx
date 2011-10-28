<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="suagiaovienchunhiem.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.SuaGiaoVienChuNhiemPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script type="text/javascript">
            $(document).ready(function () {
                $(this).find(".radio input[type='radio']").change(function () {
                    if ($(this).is(':checked')) {
                        $(this).parents('td').parents('tr').siblings().each(function () {
                            $(this).find(".radio input[type='radio']").each(function () {
                                $(this).attr('checked', false);
                            });
                        });
                    }
                });
            });
        </script>
    </div>
    <div id="divSearch">
        <div>
            Lớp:
            <asp:Label ID="LblLopHoc" runat="server" Font-Bold="true"></asp:Label>
            <br />
            Giáo viên chủ nhiệm hiện hành:
            <asp:Label ID="LblCurrentGiaoVienChuNhiem" runat="server" Font-Bold="true"></asp:Label>
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div style="padding: 0px 0px 5px 0px; text-align: center">
            <asp:Label ID="LblTitleTeacherList" runat="server" Font-Bold="true" ForeColor="Blue"
                Font-Size="15px"></asp:Label>
        </div>
        <div id="div1" style="padding: 10px 0px 30px 0px;">
            <div style="float: left;">
                Mã giáo viên:&nbsp;
                <asp:TextBox ID="TxtSearchMaHienThiGiaoVien" runat="server" Width="150px"></asp:TextBox>
                <ajaxToolkit:TextBoxWatermarkExtender ID="MaGiaoVienWatermark" runat="server" TargetControlID="TxtSearchMaHienThiGiaoVien"
                    WatermarkText="Tất cả">
                </ajaxToolkit:TextBoxWatermarkExtender>
                &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; Tên giáo viên:
                <asp:TextBox ID="TxtSearchTenGiaoVien" runat="server" Width="150px"></asp:TextBox>
                <ajaxToolkit:TextBoxWatermarkExtender ID="TenGiaoVienWatermark" runat="server" TargetControlID="TxtSearchTenGiaoVien"
                    WatermarkText="Tất cả">
                </ajaxToolkit:TextBoxWatermarkExtender>
                &nbsp;&nbsp;
            </div>
            <div style="float: left; width: 80px; margin-top: -5px;">
                <asp:ImageButton ID="BtnSearch" runat="server" CssClass="BtnSearch" ImageUrl="~/Styles/Images/button_search_with_text.png"
                    ToolTip="Tìm kiếm giáo viên" OnClick="BtnSearch_Click" />
            </div>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfMaGiaoVien" runat="server" />
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
                            <asp:LinkButton ID="LkBtnHoTen" runat="server" CommandName="SortByHoTen">Họ tên giáo viên</asp:LinkButton>
                        </td>
                        <td style="width: 130px">
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandName="SortByNgaySinh">Ngày sinh</asp:LinkButton>
                        </td>
                        <td style="width: 80px">
                            <asp:LinkButton ID="LinkButton2" runat="server" CommandName="SortByGioiTinh">Giới tính</asp:LinkButton>
                        </td>
                        <td class="icon">
                            Chọn
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptMaGiaoVien" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaGiaoVien")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:HyperLink ID="HlkMaGiaoVien" runat="server" Target="_blank"><%#DataBinder.Eval(Container.DataItem, "MaHienThiGiaoVien")%></asp:HyperLink>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "HoTen")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "StringNgaySinh")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "StringGioiTinh")%>
                        </td>
                        <td class="icon">
                            <asp:RadioButton ID="RBtnSelect" runat="server" CssClass="radio" />
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
            <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="DataPager_Command" PageSize="10"
                ViewStateMode="Enabled" PageClause="Trang" OfClause="/" FirstClause="<<" BackToFirstClause="Trở về trang đầu"
                LastClause=">>" GoToLastClause="Đến trang cuối" PreviousClause="<" NextClause=">"
                CompactModePageCount="3" GenerateFirstLastSection="True" BackToPageClause="Trở về trang"
                NextToPageClause="Đến trang" ShowResultClause="Hiển thị kết quả" ToClause="đến" />
        </div>
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 0px 0px">
        <asp:ImageButton ID="BtnSave" runat="server" OnClick="BtnSave_Click" ValidationGroup="EditGiaoVienChuNhiem"
            ImageUrl="~/Styles/Images/button_save.png" CssClass="SaveButton" />
        &nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" OnClick="BtnCancel_Click" ImageUrl="~/Styles/Images/button_cancel.png"
            CssClass="CancelButton" />
    </div>
</asp:Content>
