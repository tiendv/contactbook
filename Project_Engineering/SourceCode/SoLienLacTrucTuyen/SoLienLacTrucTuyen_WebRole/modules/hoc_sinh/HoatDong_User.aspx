<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true" CodeBehind="HoatDong_User.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.HoatDong_User" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
<div>
    <script type="text/javascript">
    </script>
</div>     
<asp:HiddenField ID="HdfMaHocSinh" runat="server" />   
<table>
    <tr>
        <td>
            <table class="search">
                <tr>
                    <td style="width:60px;">
                        <asp:Label ID="Label54" runat="server" Text="Năm học:"></asp:Label>
                    </td>
                    <td style="width:250px;">
                        <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td style="width:63px;">
                        <asp:Label ID="Label55" runat="server" Text="Học kỳ:"></asp:Label>
                    </td>
                    <td style="width:200px;">
                        <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>            
                </tr>  
                <tr>
                    <td>
                        <asp:Label ID="Label56" runat="server" Text="Từ ngày:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtTuNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarTuNgay" runat="server" 
                            ImageUrl="~/Styles/Images/calendar.png" ImageAlign="Middle"/>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" 
                            TargetControlID="TxtTuNgay" PopupButtonID="ImgCalendarTuNgay" 
                            PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender5" runat="server" 
                            TargetControlID="TxtTuNgay" MaskType="Date" Mask="99/99/9999"
                            ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label57" runat="server" Text="Đến ngày:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtDenNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarDenNgay" runat="server" 
                            ImageUrl="~/Styles/Images/calendar.png" ImageAlign="Middle"/>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender6" runat="server" 
                            TargetControlID="TxtDenNgay" PopupButtonID="ImgCalendarDenNgay" 
                            PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender6" runat="server" 
                            TargetControlID="TxtDenNgay" MaskType="Date" Mask="99/99/9999"
                            ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
            </table>
        </td>
        <td style="width:80px">
            <asp:ImageButton ID="BtnSearch" runat="server" 
                ImageUrl="~/Styles/Images/button_search_with_text.png" ToolTip="Tìm kiếm hoạt động" OnClick="BtnSearch_Click"/>
        </td>
    </tr>
</table>
<table class="table_data">    
    <tr>
        <td>
            <asp:Label ID="LblSearchResult" runat="server" style="font-size:15px; font-weight:bold;"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <table class="repeater">                
                <asp:HiddenField ID="HdfMaHoatDong" runat="server"/>
                <asp:HiddenField ID="HdfRptHoatDongMPEEdit" runat="server"/>
                <asp:Repeater ID="RptHoatDong" runat="server" 
                    onitemcommand="RptHoatDong_ItemCommand" 
                    onitemdatabound="RptHoatDong_ItemDataBound">
                    <HeaderTemplate>
                        <tr class="header">
                            <td class="left orderNo">
                                STT
                            </td>
                            <td class="middle">
                                Hoạt động
                            </td>
                            <td class="middle" style="width:100px">
                                Ngày
                            </td>
                            <td class="right" style="width:78px">
                                Thái độ tham gia
                            </td>
                        </tr>
                    </HeaderTemplate>                        
                    <ItemTemplate>                            
                        <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                            <td style="height:40px; text-align:center">
                                <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                                <asp:HiddenField ID="HdfRptMaHoatDong" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaHoatDong")%>'/>
                            </td>
                            <td style="height:40px;">
                                <asp:Label ID="LblTenHoatDong" runat="server" style="display:none"></asp:Label>
                                <asp:LinkButton ID="LbtnTenHoatDong" runat="server"
                                    Text='<%#DataBinder.Eval(Container.DataItem, "TenHoatDong")%>'
                                    style="text-decoration:underline; color:Blue;cursor:pointer;"
                                    CommandName="CmdDetailItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaHoatDong")%>'>
                                </asp:LinkButton>                                    
                                <ajaxToolkit:ModalPopupExtender ID="MPEDetail" runat="server"                                         
                                    TargetControlID="LblTenHoatDong"
                                    PopupControlID="PnlPopupDetail"
                                    BackgroundCssClass="modalBackground"
                                    CancelControlID="ImgClosePopupDetail"
                                    PopupDragHandleControlID="PnlDragPopupDetail">
                                </ajaxToolkit:ModalPopupExtender>
                            </td>
                            <td style="height:40px;">
                                <asp:Label ID="Label41" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "StrNgay")%>'></asp:Label>
                            </td>
                            <td style="height:40px;">
                                <asp:Label ID="Label42" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ThaiDoThamGia")%>'></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>                        
                    <FooterTemplate>
                        <tr>
                            <td colspan="8" class="footer">
                                <div class="left"></div>
                                <div class="right"></div>
                                <div class="middle">                                        
                                </div>
                            </td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>                    
            </table>                                                
            <div style="float:right; margin-top:-35px; padding-right:30px;">                    
                <cc1:DataPager ID="MainDataPager" runat="server" OfClause="/" PageClause="TRANG"
                    OnCommand="MainDataPager_Command" PageSize="10" ViewStateMode="Enabled" LastClause=">>" GenerateHiddenHyperlinks="False" CompactModePageCount="3" GenerateFirstLastSection="True" GenerateGoToSection="False" FirstClause="<<" BackToFirstClause="Trở về trang đầu" BackToPageClause="Trở về trang" GoToLastClause="Đến trang cuối" NextToPageClause="Đến trang" ShowResultClause="Hiển thị kết quả" ToClause="đến" />
            </div>
        </td>            
    </tr>
</table>                
<asp:Panel ID="PnlPopupDetail" runat="server" CssClass="popup" Width="400px">
    <asp:Panel ID="PnlDragPopupDetail" runat="server" CssClass="popup_header">
        <asp:Label ID="LblPnlPopupDetailTitle" runat="server" CssClass="popup_header_title" Text="Chi tiết hoạt động"></asp:Label>
        <img id="ImgClosePopupDetail" class="button_close" src="../../Styles/Images/popup_button_close.png" alt="close"/>
    </asp:Panel>
    <table style="padding:5px 7px 10px 7px;">
        <tr>
            <td>
                <div class="inputBorder">
                    <table style="width:100%;">
                        <tr>
                            <td style="width:105px; vertical-align:top; padding-top:3px">
                                <asp:Label ID="Label68" runat="server" Text="Tiêu đề:"></asp:Label>
                            </td>
                            <td class="readOnlyTextBox">
                                <asp:Label ID="LblTieuDe" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top; padding-top:3px">
                                <asp:Label ID="Label70" runat="server" Text="Mô tả:"></asp:Label>
                            </td>
                            <td class="readOnlyTextBox">
                                <asp:Label ID="LblMoTa" runat="server" Text="Label"></asp:Label>
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top; padding-top:3px">
                                <asp:Label ID="Label71" runat="server" Text="Cấp độ:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label72" runat="server" Text="Cá nhân" style="font-style:italic; font-weight:bold"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:text-top; padding-top:3px;">
                                <asp:Label ID="Label73" runat="server" Text="Học kỳ:"></asp:Label>
                            </td>
                            <td style="width:auto;" class="readOnlyTextBox">
                                <asp:Label ID="LblHocKy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top; padding-top:3px;">
                                <asp:Label ID="Label69" runat="server" Text="Ngày:"></asp:Label>
                            </td>
                            <td style="width:auto;" class="readOnlyTextBox">                                                
                                <asp:Label ID="LblNgay" runat="server"></asp:Label>
                            </td>                                        
                        </tr>                                        
                        <tr>
                            <td style="vertical-align:text-top; padding-top:3px;">
                                <asp:Label ID="Label75" runat="server" Text="Thái độ tham gia:"></asp:Label>
                            </td>
                            <td style="width:auto;" class="readOnlyTextBox">
                                <asp:Label ID="LblThaiDoThamGia" runat="server" Text="Thái độ tham gia:"></asp:Label>
                            </td>
                        </tr>           
                    </table>                        
                </div>
            </td>
        </tr>        
    </table>              
</asp:Panel>
</asp:Content>
