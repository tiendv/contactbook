<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true" CodeBehind="LoiNhanKhan_User.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.LoiNhanKhan_User" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
<div>
    <script type="text/javascript">
        $(document).ready(function () { 
        });
    </script>
    <script type="text/javascript">
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
                    <td style="width:60px;">
                        <asp:Label ID="Label4" runat="server" Text="Xác nhận:"></asp:Label>
                    </td>
                    <td style="width:175px">
                        <asp:DropDownList ID="DdlXacNhan" runat="server" Width="151px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:70px;">
                        <asp:Label ID="Label3" runat="server" Text="Từ ngày:"></asp:Label>
                    </td>
                    <td style="width:200px;">
                        <asp:TextBox ID="TxtTuNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarTuNgay" runat="server" 
                            ImageUrl="~/Styles/Images/calendar.png" ImageAlign="Middle"/>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
                            TargetControlID="TxtTuNgay" PopupButtonID="ImgCalendarTuNgay" 
                            PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" 
                            TargetControlID="TxtTuNgay" MaskType="Date" Mask="99/99/9999"
                            ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>  
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Đến ngày:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtDenNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarDenNgay" runat="server" 
                            ImageUrl="~/Styles/Images/calendar.png" ImageAlign="Middle"/>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
                            TargetControlID="TxtDenNgay" PopupButtonID="ImgCalendarDenNgay" 
                            PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" 
                            TargetControlID="TxtDenNgay" MaskType="Date" Mask="99/99/9999"
                            ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
            </table>
        </td>
        <td style="width:80px; vertical-align:top; padding-top:5px">
            <asp:ImageButton ID="BtnSearch" runat="server" 
                ImageUrl="~/Styles/Images/button_search_with_text.png" ToolTip="Tìm kiếm lời nhắn khẩn" OnClick="BtnSearch_Click"/>
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
                <asp:HiddenField ID="HdfMaLoiNhanKhan" runat="server"/>
                <asp:HiddenField ID="HdfRptLoiNhanKhanMPEDetail" runat="server"/>
                <asp:Repeater ID="RptLoiNhanKhan" runat="server" 
                    onitemcommand="RptLoiNhanKhan_ItemCommand" onitemdatabound="RptLoiNhanKhan_ItemDataBound">
                    <HeaderTemplate>
                        <tr class="header">
                            <td class="left orderNo">
                                STT
                            </td>
                            <td class="middle">
                                Lời nhắn
                            </td>
                            <td class="middle" style="width:100px">
                                Ngày
                            </td>                            
                            <td class="right" style="width:78px">
                                Xác nhận
                            </td>
                        </tr>
                    </HeaderTemplate>                        
                    <ItemTemplate>                            
                        <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                            <td style="height:40px; text-align:center">
                                <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                                <asp:HiddenField ID="HdfRptMaLoiNhanKhan" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaLoiNhanKhan")%>'/>
                            </td>
                            <td style="height:40px;">
                                <asp:Label ID="LblTieuDe" runat="server" style="display:none"></asp:Label>
                                <asp:LinkButton ID="LbtnTieuDe" runat="server"
                                    Text='<%#DataBinder.Eval(Container.DataItem, "TieuDe")%>'
                                    style="text-decoration:underline; color:Blue;cursor:pointer;"
                                    CommandName="CmdDetailItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaLoiNhanKhan")%>'>
                                </asp:LinkButton>                                    
                                <ajaxToolkit:ModalPopupExtender ID="MPEDetail" runat="server"                                         
                                    TargetControlID="LblTieuDe"
                                    PopupControlID="PnlPopupDetail"
                                    BackgroundCssClass="modalBackground"
                                    CancelControlID="ImgClosePopupDetail"
                                    PopupDragHandleControlID="PnlDragPopupDetail">
                                </ajaxToolkit:ModalPopupExtender>
                            </td>
                            <td style="height:40px;">
                                <%#DataBinder.Eval(Container.DataItem, "StrNgay")%>
                            </td>                            
                            <td style="height:40px;">
                                <%#DataBinder.Eval(Container.DataItem, "XacNhan")%>
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
<asp:Panel ID="PnlPopupDetail" runat="server" CssClass="popup" Width="450px">    
    <asp:Panel ID="PnlDragPopupDetail" runat="server" CssClass="popup_header">
        <asp:Label ID="LblPnlPopupDetailTitle" runat="server" CssClass="popup_header_title" Text="Chi tiết lời nhắn khẩn"></asp:Label>
        <img id="ImgClosePopupDetail" class="button_close" src="../../Styles/Images/popup_button_close.png" alt="close"/>
    </asp:Panel>    
    <table style="padding:5px 7px 10px 7px;">
        <tr>
            <td>
                <div class="inputBorder">
                    <table style="width:100%;">
                        <tr>
                            <td style="width:60px; vertical-align:top; padding-top:3px;">
                                <asp:Label ID="Label12" runat="server" Text="Tiêu đề:"></asp:Label>
                            </td>
                            <td style="width:auto;" class="readOnlyTextBox">
                                <asp:Label ID="LblTieuDe" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:60px; vertical-align:top; padding-top:3px;">
                                <asp:Label ID="Label18" runat="server" Text="Nội dung:"></asp:Label>
                            </td>
                            <td style="width:auto; height:40px;" colspan="3" class="readOnlyTextBox">
                                <asp:Label ID="LblNoiDung" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>                                             
                        <tr>
                            <td style="vertical-align:text-top; padding-top:3px;">
                                <asp:Label ID="Label7" runat="server" Text="Ngày:"></asp:Label>
                            </td>
                            <td class="readOnlyTextBox">
                                <asp:Label ID="LblNgay" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:text-top; padding-top:3px;">
                                <asp:Label ID="Label6" runat="server" Text="Xác nhận:"></asp:Label>
                            </td>
                            <td class="readOnlyTextBox">
                                <asp:Label ID="LblXacNhan" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>                    
                </div>
            </td>
        </tr>             
    </table>
    <div style="width:80px; margin:0px auto 0px auto;">
        <asp:ImageButton ID="BtnConfirm" runat="server"
            ImageUrl="~/Styles/Images/button_confirm_with_text.png" OnClick="BtnConfirm_Click"/> &nbsp;
    </div>
</asp:Panel>
</asp:Content>
