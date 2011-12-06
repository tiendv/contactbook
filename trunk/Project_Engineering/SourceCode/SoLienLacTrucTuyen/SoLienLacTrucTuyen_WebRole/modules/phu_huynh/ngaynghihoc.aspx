<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="ngaynghihoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ModuleParents.StudentAbsentPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">        
    <asp:HiddenField ID="HdfMaHocSinh" runat="server" />
    <div id="divSearch">
        <div id="divSearchCriteria">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="search">
                        <tr>
                            <td style="width: 60px;">
                                Năm học:
                            </td>
                            <td style="width: 250px;">
                                <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 63px;">
                                Học kỳ:
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Từ ngày:
                            </td>
                            <td>
                                <asp:TextBox ID="TxtTuNgay" runat="server" Width="125px"></asp:TextBox>
                                <asp:Image ID="ImgCalendarTuNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                                    ImageAlign="Middle" />
                                <ajaxtoolkit:calendarextender id="CalendarExtender1" runat="server" targetcontrolid="TxtTuNgay"
                                    popupbuttonid="ImgCalendarTuNgay" popupposition="Right">
                                </ajaxtoolkit:calendarextender>
                                <ajaxtoolkit:maskededitextender id="MaskedEditExtender1" runat="server" targetcontrolid="TxtTuNgay"
                                    masktype="Date" mask="99/99/9999" errortooltipenabled="true">
                                </ajaxtoolkit:maskededitextender>
                            </td>
                            <td>
                                Đến ngày:
                            </td>
                            <td>
                                <asp:TextBox ID="TxtDenNgay" runat="server" Width="125px"></asp:TextBox>
                                <asp:Image ID="ImgCalendarDenNgay" runat="server" ImageUrl="~/Styles/Images/calendar.png"
                                    ImageAlign="Middle" />
                                <ajaxtoolkit:calendarextender id="CalendarExtender2" runat="server" targetcontrolid="TxtDenNgay"
                                    popupbuttonid="ImgCalendarDenNgay" popupposition="Right">
                                </ajaxtoolkit:calendarextender>
                                <ajaxtoolkit:maskededitextender id="MaskedEditExtender2" runat="server" targetcontrolid="TxtDenNgay"
                                    masktype="Date" mask="99/99/9999" errortooltipenabled="true">
                                </ajaxtoolkit:maskededitextender>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm ngày nghỉ học" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">        
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfMaNgayNghiHoc" runat="server" />
            <asp:HiddenField ID="HdfRptNgayNghiMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptNgayNghiMPEEdit" runat="server" />
            <asp:Repeater ID="RptNgayNghi" runat="server" OnItemCommand="RptNgayNghi_ItemCommand"
                OnItemDataBound="RptNgayNghi_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td class="middle" style="width: 20%">
                            <asp:LinkButton ID="LkBtnNgay" runat="server">Ngày</asp:LinkButton>
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LkBtnBuoi" runat="server">Buổi</asp:LinkButton>
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LkBtnCoPhep" runat="server">Có phép</asp:LinkButton>
                        </td>
                        <td class="middle" style="width: 150px">
                            Lý do
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LkBtnXacNhan" runat="server">Phụ huynh xác nhận</asp:LinkButton>
                        </td>
                        <td id="thEdit" runat="server" class="icon">
                            Xác nhận
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                            <asp:HiddenField ID="HdfRptMaNgayNghiHoc" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaNgayNghiHoc")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label40" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Ngay")%>'></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label41" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Buoi")%>'></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label42" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "XinPhep")%>'></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label43" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "LyDo")%>'></asp:Label>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="Label44" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "XacNhan")%>'></asp:Label>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeEditItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaNgayNghiHoc")%>' />
                            <ajaxtoolkit:modalpopupextender id="MPEEdit" runat="server" targetcontrolid="BtnFakeEditItem"
                                popupcontrolid="PnlPopupConfirm" backgroundcssclass="modalBackground" cancelcontrolid="ImgClosePopupEdit"
                                popupdraghandlecontrolid="PnlDragPopupConfirm">
                            </ajaxtoolkit:modalpopupextender>
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
            <cc1:datapager id="MainDataPager" runat="server" oncommand="MainDataPager_Command" viewstatemode="Enabled"/>
        </div>
    </div>    
    <asp:Panel ID="PnlPopupConfirm" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirm" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmTitle" runat="server" Text="Xác nhận ngày nghỉ học"
                CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirm" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding-bottom: 5px;">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
