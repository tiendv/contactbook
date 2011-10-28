<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="suathoikhoabieu_orginal.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.SuaThoiKhoaBieuPage_orginal" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScripts">
        <script type="text/javascript">
            $(document).ready(function () {
                $('.ckbxsSang').find(':checkbox').each(function () {
                    $(this).change(function () {
                        var $maNganhHoc = $(this).parent().parent().find('.lblMaNganhHoc').text();
                        if ($(this).is(':checked')) {
                            $.ajax({
                                type: "POST",
                                url: "/Modules/Lop_Hoc/LopHocServicePage.aspx/AddCheckedMonHocTKBSang",
                                data: "{'maMonHoc':'" + $maNganhHoc + "'}",
                                contentType: "application/json; charset=utf-8",
                                success: function (serverResponseData) {
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    alert('Error!');
                                }
                            });
                        }
                        else {
                            $.ajax({
                                type: "POST",
                                url: "/Modules/Lop_Hoc/LopHocServicePage.aspx/RemoveCheckedMonHocTKBSang",
                                data: "{'maMonHoc':'" + $maNganhHoc + "'}",
                                contentType: "application/json; charset=utf-8",
                                success: function (serverResponseData) {
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    alert('Error!');
                                }
                            });
                        }
                    });
                });

                $('.ckbxsChieu').find(':checkbox').each(function () {
                    $(this).change(function () {
                        var $maNganhHoc = $(this).parent().parent().find('.lblMaNganhHoc').text();
                        if ($(this).is(':checked')) {
                            $.ajax({
                                type: "POST",
                                url: "/Modules/Lop_Hoc/LopHocServicePage.aspx/AddCheckedMonHocTKBChieu",
                                data: "{'maMonHoc':'" + $maNganhHoc + "'}",
                                contentType: "application/json; charset=utf-8",
                                success: function (serverResponseData) {
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    alert('Error!');
                                }
                            });
                        }
                        else {
                            $.ajax({
                                type: "POST",
                                url: "/Modules/Lop_Hoc/LopHocServicePage.aspx/RemoveCheckedMonHocTKBChieu",
                                data: "{'maMonHoc':'" + $maNganhHoc + "'}",
                                contentType: "application/json; charset=utf-8",
                                success: function (serverResponseData) {
                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    alert('Error!');
                                }
                            });
                        }
                    });
                });
            });
        </script>
        <script language="javascript" type="text/javascript">
            function pageLoad() {
            }        
        </script>
    </div>
    <asp:Label ID="Label1" runat="server" Text="CHỌN MÔN HỌC THỜI KHÓA BIỂU" CssClass="subTitle"></asp:Label>
    <div style="width: 100%; margin-left: 50px; font-weight: bold; font-size: 14px;">
        Năm học:
        <asp:Label ID="LblTenNamHoc" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp; Học kỳ:
        <asp:Label ID="LblTenHocKy" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp; Thứ:
        <asp:Label ID="LblTenThu" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp; Lớp:
        <asp:Label ID="LblTenLopHoc" runat="server"></asp:Label>
    </div>
    <div class="table_data ui-corner-all">
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"
                Text="Chưa có thông tin môn học">
            </asp:Label>
        </div>
        <table class="repeater">
            <tr>
                <td>
                    <asp:HiddenField ID="HdfMaNamHoc" runat="server" />
                    <asp:HiddenField ID="HdfMaHocKy" runat="server" />
                    <asp:HiddenField ID="HdfMaThu" runat="server" />
                    <asp:HiddenField ID="HdfMaLopHoc" runat="server" />
                    <table class="repeater">
                        <asp:Repeater ID="RptMonHoc" runat="server" OnItemDataBound="RptMonHoc_ItemDataBound">
                            <HeaderTemplate>
                                <tr class="header">
                                    <td class="left orderNo">
                                        STT
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" runat="server">Môn học</asp:LinkButton>                                        
                                    </td>
                                    <td style="vertical-align: middle; text-align: center; width: 78px;">
                                        Sáng
                                    </td>
                                    <td style="vertical-align: middle; text-align: center; width: 78px;">
                                        Chiều
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                                    <td style="height: 40px; text-align: center">
                                        <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                                        <asp:HiddenField ID="HdfRptMaMonHoc" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaMonHoc")%>' />
                                    </td>
                                    <td style="height: 40px;">
                                        <%#DataBinder.Eval(Container.DataItem, "TenMonHoc")%>
                                    </td>
                                    <td class="icon ckbxsSang" style="height: 40px;">
                                        <asp:Label ID="Label3" runat="server" class="lblMaNganhHoc" Style="display: none"
                                            Text='<%#DataBinder.Eval(Container.DataItem, "MaMonHoc")%>'></asp:Label>
                                        <asp:Label ID="LblSelectedMaNganhHocSang" runat="server" Text="0" Style="display: none"
                                            class="LblSelectedMaNganhHoc">
                                        </asp:Label>
                                        <asp:CheckBox ID="CkbxSang" runat="server" class="ckbxSang" />
                                    </td>
                                    <td class="icon ckbxsChieu" style="height: 40px;">
                                        <asp:Label ID="Label5" runat="server" class="lblMaNganhHoc" Style="display: none"
                                            Text='<%#DataBinder.Eval(Container.DataItem, "MaMonHoc")%>'></asp:Label>
                                        <asp:Label ID="LblSelectedMaNganhHocChieu" runat="server" EnableViewState="true"
                                            Text="0" Style="display: none" class="LblSelectedMaNganhHoc">
                                        </asp:Label>
                                        <asp:CheckBox ID="CkbxChieu" runat="server" class="ckbxChieu" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr>
                                    <td colspan="7" class="footer">
                                    </td>
                                </tr>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                    <div style="float: right; margin-top: -35px; padding-right: 30px; display: none">
                        <cc1:DataPager ID="MainDataPager" runat="server" OfClause="/" PageClause="TRANG" OnCommand="pager_Command"
                            PageSize="100" ViewStateMode="Enabled" LastClause=">>" GenerateHiddenHyperlinks="False"
                            CompactModePageCount="3" GenerateFirstLastSection="True" GenerateGoToSection="False"
                            FirstClause="<<" BackToFirstClause="Trở về trang đầu" BackToPageClause="Trở về trang"
                            GoToLastClause="Đến trang cuối" NextToPageClause="Đến trang" ShowResultClause="Hiển thị kết quả"
                            ToClause="đến" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 170px; margin: 10px auto 0px auto;">
        <asp:ImageButton ID="BtnSave" runat="server" OnClick="BtnSave_Click" ImageUrl="~/Styles/Images/button_save.png"
            CssClass="SaveButton" />
        &nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" OnClick="BtnCancel_Click" ImageUrl="~/Styles/Images/button_cancel.png"
            CssClass="CancelButton" />
    </div>
</asp:Content>
