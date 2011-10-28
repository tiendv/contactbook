<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true" CodeBehind="ThoiKhoaBieu_User.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ThoiKhoaBieu_User" %>
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
            <table class="search">
                <tr>                            
                    <td style="width:60px;">
                        <asp:Label ID="Label1" runat="server" Text="Năm học:"></asp:Label>
                    </td>
                    <td style="width:200px;">
                        <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td style="width:45px;">
                        <asp:Label ID="Label7" runat="server" Text="Học kỳ:"></asp:Label>
                    </td>
                    <td style="width:200px;">
                        <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>                            
                </tr>
            </table>
        </td>        
        <td style="width:80px; padding:5px 0px 0px 50px; vertical-align:top;">
            <asp:ImageButton ID="BtnSearch" runat="server" 
                ImageUrl="~/Styles/Images/button_search_with_text.png" ToolTip="Tìm kiếm thời khóa biểu"
                onclick="BtnSearch_Click"/>
        </td>            
    </tr>
</table>
<table class="table_data">    
    <tr>
        <td>
            <table class="repeater">                
                <asp:Repeater ID="RptMonHocTKB" runat="server" 
                    onitemdatabound="RptMonHocTKB_ItemDataBound">
                    <HeaderTemplate>
                        <tr class="header">
                            <td class="left_2" style="width:78px;">
                                <asp:Label ID="Label6" runat="server" Text="Buổi" style="float:right; vertical-align:top"></asp:Label>
                                <br />
                                <asp:Label ID="Label5" runat="server" Text="Ngày" style="float:left; vertical-align:bottom"></asp:Label>                                
                            </td>                            
                            <td class="middle">
                                Sáng
                            </td>
                            <td class="middle">
                                Chiều
                            </td>
                        </tr>
                    </HeaderTemplate>                        
                    <ItemTemplate>                            
                        <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                            <td style="height:40px;">                                
                                <asp:HiddenField ID="HdfRptMaNamHoc" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaNamHoc")%>'/>
                                <asp:HiddenField ID="HdfRptMaHocKy" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaHocKy")%>'/>
                                <asp:HiddenField ID="HdfRptMaThu" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaThu")%>'/>
                                <asp:HiddenField ID="HdfRptMaLopHoc" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaLopHoc")%>'/>
                                <asp:Label ID="LblTenBuoi" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "TenThu")%>'></asp:Label>
                            </td>
                            <td style="height:40px; padding:0px">
                                <asp:Label ID="LblNghiSang" runat="server" Text="(Nghỉ)" Visible="false" style="padding-left:5px" ></asp:Label>
                                <table width="100%">
                                    <asp:Repeater ID="RptMonHocTKBBuoiSang" runat="server">                                    
                                        <ItemTemplate>
                                            <tr class='<%#((Container.ItemIndex + 1) % 2 != 0) ? "innerOddRow" : "innerEvenRow"%>'>
                                                <td style="height:40px; border-style:none; padding:0px 5px 0px 5px">
                                                    <asp:Label ID="LblTenMonHocTKBBuoiSang" runat="server"
                                                        Text='<%#DataBinder.Eval(Container.DataItem, "TenMonHoc")%>'>
                                                    </asp:Label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>                                        
                                </table>        
                            </td>
                            <td style="height:40px; padding:0px">
                                <asp:Label ID="LblNghiChieu" runat="server" Text="(Nghỉ)" Visible="false" style="padding-left:5px" ></asp:Label>
                                <table width="100%">
                                    <asp:Repeater ID="RptMonHocTKBBuoiChieu" runat="server">                                    
                                        <ItemTemplate>
                                            <tr class='<%#((Container.ItemIndex + 1) % 2 != 0) ? "innerOddRow" : "innerEvenRow"%>'>
                                                <td style="height:40px; border-style:none; padding:0px 5px 0px 5px">
                                                    <asp:Label ID="LblTenMonHocTKBBuoiChieu" runat="server"
                                                        Text='<%#DataBinder.Eval(Container.DataItem, "TenMonHoc")%>'>
                                                    </asp:Label>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>                                        
                                </table>        
                            </td>                            
                        </tr>
                    </ItemTemplate>                        
                    <FooterTemplate>
                        <tr>
                            <td colspan="7" class="footer">
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
</table>
</asp:Content>
