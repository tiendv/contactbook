<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true" CodeBehind="KetQuaHocTap_User.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.KetQuaHocTap_User" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
<div>
    <script type="text/javascript">
        $(document).ready(function () {
            });            
    </script>

    <script language="javascript" type="text/javascript">
        function pageLoad() {         
        }
    </script>
</div>
<table>
    <tr>
        <td>
            <asp:Label ID="Label9" runat="server" Text="Năm học:"></asp:Label>&nbsp;
            <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px"></asp:DropDownList>
        </td>
        <td style="padding-left:50px">
            <asp:Label ID="Label15" runat="server" Text="Năm học:"></asp:Label>&nbsp;
            <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px"></asp:DropDownList>
        </td>
        <td style="padding-left:50px">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                OnClick="BtnSearch_Click"/>
        </td>
    </tr>
</table>
<table class="table_data">
    <tr>
        <td>
            <div style="padding:5px 10px 5px 10px; font-size:15px; font-weight:bold">
                <asp:Label ID="Label14" runat="server" Text="BẢNG ĐIỂM MÔN HỌC" ForeColor="Violet">
                </asp:Label>
                <br />
                <asp:Label ID="LblSearchResult" runat="server" style="font-size:15px; font-weight:bold;"
                    Text="Chưa có thông tin">
                </asp:Label>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <table class="repeater">                                
                <tr class="header">
                    <td class="left orderNo" style="width:40px">
                        STT
                    </td>
                    <td class="middle" style="width:20%">
                        Môn học
                    </td>
                    <asp:Repeater ID="RptTenLoaiDiem" runat="server">
                    <ItemTemplate>
                        <td class="middle">
                            <%#DataBinder.Eval(Container.DataItem, "TenLoaiDiem")%>
                        </td>
                    </ItemTemplate>
                    </asp:Repeater>
                    <td class="right" style="width:78px">
                        Điểm trung bình
                    </td>
                </tr>
                <asp:Repeater ID="RptKetQuaDiem" runat="server" OnItemDataBound="RptKetQuaDiem_ItemDataBound">
                    <ItemTemplate>
                        <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                            <td style="height:40px; text-align:center">
                                <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%> 
                            </td>
                            <td>
                                <asp:Label ID="LblTenMonHoc" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TenMonHoc")%>'></asp:Label>
                                <asp:HiddenField ID="RptKetQuaDiem_HdfMaDiemMonHK" runat="server" 
                                                    Value='<%#DataBinder.Eval(Container.DataItem, "MaDiemMonHK")%>'/>
                            </td>
                            <asp:Repeater ID="RptKetQuaDiem_RptDiemMonHoc" runat="server">
                            <ItemTemplate>
                            <td style="height:40px;">
                                <%#DataBinder.Eval(Container.DataItem, "Diems")%>
                            </td>
                            </ItemTemplate>
                            </asp:Repeater>
                            <td>
                                <asp:Label ID="Label16" runat="server" style="float:right" Text='<%#DataBinder.Eval(Container.DataItem, "StrDiemTB")%>'></asp:Label>                                                
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr>
                            <td colspan="9" class="footer">
                                <div class="left"></div>
                                <div class="right"></div>
                                <div class="middle">                                        
                                </div>
                            </td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <div style="float:right; margin-top:-45px; padding-right:30px;">                    
                <cc1:DataPager ID="MainDataPager" runat="server" OfClause="/" PageClause="TRANG"
                    OnCommand="MainDataPager_Command" PageSize="10" ViewStateMode="Enabled" LastClause=">>" GenerateHiddenHyperlinks="False" CompactModePageCount="3" GenerateFirstLastSection="True" GenerateGoToSection="False" FirstClause="<<" BackToFirstClause="Trở về trang đầu" BackToPageClause="Trở về trang" GoToLastClause="Đến trang cuối" NextToPageClause="Đến trang" ShowResultClause="Hiển thị kết quả" ToClause="đến" />
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <table class="repeater">
                <asp:Repeater ID="RptDanhHieu" runat="server" onitemdatabound="RptDanhHieu_ItemDataBound">
                <HeaderTemplate>
                <tr class="header">
                    <td class="left orderNo" style="width:40px">
                        STT
                    </td>
                    <td class="middle">
                        Học kỳ
                    </td>
                    <td class="middle" style="width:120px">
                        Điểm trung bình
                    </td>
                    <td class="middle">
                        Học lực
                    </td>
                    <td class="middle">
                        Hạnh kiểm
                    </td>
                    <td class="right" style="width:78px">
                        Danh hiệu
                    </td>
                </tr>
                </HeaderTemplate>                        
                <ItemTemplate>                            
                <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                    <td style="height:40px; text-align:center">
                        <%# (DataPagerDanhHieu.CurrentIndex - 1) * DataPagerDanhHieu.PageSize + Container.ItemIndex + 1%>
                        <asp:HiddenField ID="HdfRptMaDanhHieuHSHK" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaDanhHieuHSHK")%>'/>
                    </td>
                    <td style="height:40px;">
                        <asp:Label ID="Label28" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TenHocKy")%>'></asp:Label>
                    </td>
                    <td style="height:40px;">
                        <asp:Label ID="Label79" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "StrDiemTB")%>'></asp:Label>
                    </td>
                    <td style="height:40px;">
                        <asp:Label ID="Label80" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TenHocLuc")%>'></asp:Label>
                    </td>
                    <td style="height:40px; vertical-align:middle">
                        <asp:Label ID="Label81" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TenHanhKiem")%>'></asp:Label>
                    </td>
                    <td style="height:40px;">                            
                        <asp:Label ID="Label82" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TenDanhHieu")%>'></asp:Label>
                    </td>
                </tr>
                </ItemTemplate>                        
                <FooterTemplate>
                <tr>
                    <td colspan="6" class="footer">
                        <div class="left"></div>
                        <div class="right"></div>
                        <div class="middle">                                        
                        </div>
                    </td>
                </tr>
                </FooterTemplate>
                </asp:Repeater>                    
            </table>                                                
            <div style="float:right; margin-top:-35px; padding-right:30px; display:none">
                <cc1:DataPager ID="DataPagerDanhHieu" runat="server" OfClause="/" PageClause="TRANG"
                PageSize="10" ViewStateMode="Enabled" LastClause=">>" GenerateHiddenHyperlinks="False" CompactModePageCount="3" GenerateFirstLastSection="True" GenerateGoToSection="False" FirstClause="<<" BackToFirstClause="Trở về trang đầu" BackToPageClause="Trở về trang" GoToLastClause="Đến trang cuối" NextToPageClause="Đến trang" ShowResultClause="Hiển thị kết quả" ToClause="đến" />
            </div>
        </td>
    </tr>
</table>                
</asp:Content>
