<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true" CodeBehind="TinhHinhNghiHoc_User.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.TinhHinhNghiHoc_User" %>
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

        function popopConfirm_CancelSave_Click() {
            var mPEConfirmID = $get('<%=HdfRptNgayNghiMPEConfirm.ClientID%>').value;
            $find(mPEConfirmID).hide();
            return false;
        }
    </script>
</div>
<asp:HiddenField ID="HdfMaHocSinh" runat="server" />
<table>
    <tr>
        <td>
            <table class="search">
                <tr>
                    <td style="width:60px;">
                        <asp:Label ID="Label23" runat="server" Text="Năm học:"></asp:Label>
                    </td>
                    <td style="width:250px;">
                        <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td style="width:63px;">
                        <asp:Label ID="Label24" runat="server" Text="Học kỳ:"></asp:Label>
                    </td>
                    <td style="width:200px;">
                        <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>            
                </tr>  
                <tr>
                    <td>
                        <asp:Label ID="Label26" runat="server" Text="Từ ngày:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtTuNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarTuNgay" runat="server" 
                            ImageUrl="~/Styles/Images/calendar.png" ImageAlign="Middle"/>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
                            TargetControlID="TxtTuNgay" PopupButtonID="ImgCalendarTuNgay" 
                            PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedConfirmExtender1" runat="server" 
                            TargetControlID="TxtTuNgay" MaskType="Date" Mask="99/99/9999"
                            ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label27" runat="server" Text="Đến ngày:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TxtDenNgay" runat="server" Width="125px"></asp:TextBox>
                        <asp:Image ID="ImgCalendarDenNgay" runat="server" 
                            ImageUrl="~/Styles/Images/calendar.png" ImageAlign="Middle"/>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
                            TargetControlID="TxtDenNgay" PopupButtonID="ImgCalendarDenNgay" 
                            PopupPosition="Right">
                        </ajaxToolkit:CalendarExtender>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedConfirmExtender2" runat="server" 
                            TargetControlID="TxtDenNgay" MaskType="Date" Mask="99/99/9999"
                            ErrorTooltipEnabled="true">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
            </table>
        </td>
        <td style="width:80px">
            <asp:ImageButton ID="BtnSearch" runat="server" 
                ImageUrl="~/Styles/Images/button_search_with_text.png" ToolTip="Tìm kiếm ngày nghỉ học" OnClick="BtnSearch_Click"/>
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
                <asp:HiddenField ID="HdfMaNgayNghiHoc" runat="server"/>
                <asp:HiddenField ID="HdfRptNgayNghiMPEConfirm" runat="server"/>
                <asp:Repeater ID="RptNgayNghi" runat="server" onitemcommand="RptNgayNghi_ItemCommand" onitemdatabound="RptNgayNghi_ItemDataBound">
                    <HeaderTemplate>
                        <tr class="header">
                            <td class="left orderNo">
                                STT
                            </td>
                            <td class="middle" style="width:100px">
                                Ngày
                            </td>
                            <td class="middle" style="width:100px">
                                Buổi
                            </td>
                            <td class="middle" style="width:100px">
                                Có phép
                            </td>
                            <td class="middle">
                                Lý do
                            </td>
                            <td class="middle" style="width:150px">
                                Phụ huynh xác nhận
                            </td>
                            <td class="right" style="width:78px">
                                Xác nhận
                            </td>
                        </tr>
                    </HeaderTemplate>                        
                    <ItemTemplate>                            
                        <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                            <td style="height:40px; text-align:center">
                                <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                                <asp:HiddenField ID="HdfRptMaNgayNghiHoc" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaNgayNghiHoc")%>'/>
                            </td>
                            <td style="height:40px;">
                                <asp:Label ID="Label40" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Ngay")%>'></asp:Label>
                            </td>
                            <td style="height:40px;">
                                <asp:Label ID="Label41" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Buoi")%>'></asp:Label>
                            </td>
                            <td style="height:40px;">
                                <asp:Label ID="Label42" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "XinPhep")%>'></asp:Label>
                            </td>
                            <td style="height:40px;">
                                <asp:Label ID="Label43" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "LyDo")%>'></asp:Label>
                            </td>
                            <td style="height:40px;">
                                <asp:Label ID="Label44" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "XacNhan")%>'></asp:Label>
                            </td>
                            <td class="icon" style="height:40px;">
                                <asp:ImageButton ID="BtnFakeConfirmItem" runat="server" style="display:none;"/>
                                <asp:ImageButton ID="BtnConfirmItem" runat="server" ImageUrl="~/Styles/Images/button_confirm.png" 
                                    CommandName="CmdConfirmItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaNgayNghiHoc")%>'/>
                                <ajaxToolkit:ModalPopupExtender ID="MPEConfirm" runat="server"                                         
                                    TargetControlID="BtnFakeConfirmItem"
                                    PopupControlID="PnlPopupConfirm"    
                                    BackgroundCssClass="modalBackground"
                                    CancelControlID="ImgClosePopupConfirm"
                                    PopupDragHandleControlID="PnlDragPopupConfirm">
                                </ajaxToolkit:ModalPopupExtender>
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

<asp:Panel ID="PnlPopupConfirm" runat="server" CssClass="popup" Width="300px">
    <asp:Panel ID="PnlDragPopupConfirm" runat="server" CssClass="popup_header">
        <asp:Label ID="LblPnlPopupConfirmTitle" runat="server" CssClass="popup_header_title" Text="Xác nhận ngày nghỉ học"></asp:Label>
        <img id="ImgClosePopupConfirm" class="button_close" src="../../Styles/Images/popup_button_close.png" alt="close"/>
    </asp:Panel>
    <div style="padding:5px 7px 10px 7px;">
        <asp:Label ID="LblConfirm" runat="server"></asp:Label>
    </div>
    <div style="width:170px; margin:0px auto 0px auto;">
        <asp:ImageButton ID="BtnConfirm" runat="server"
            ImageUrl="~/Styles/Images/button_yes.png" OnClick="BtnConfirm_Click"/> &nbsp;
        <asp:ImageButton ID="BtnCancelConfirm" runat="server" 
            ImageUrl="~/Styles/Images/button_no.png" OnClientClick="return popopConfirm_CancelSave_Click();"/>
    </div>          
</asp:Panel>
</asp:Content>

        