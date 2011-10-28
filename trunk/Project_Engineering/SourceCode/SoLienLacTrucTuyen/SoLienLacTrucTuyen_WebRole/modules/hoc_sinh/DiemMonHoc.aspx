<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true" CodeBehind="DiemMonHoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DiemMonHoc" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
<div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=BtnSaveAdd.ClientID%>').click(function (event) {
                var diem = $.trim($('#<%=TxtDiemThem.ClientID%>').val());
                var $errorDiem = $('#<%=LblErrorDiemThem.ClientID%>');
                if (diem.length == 0) {
                    $errorDiem.show();
                    $errorDiem.text('Điểm không được để trống');
                    event.preventDefault();
                }
            });            
        });
    </script>

    <script language="javascript" type="text/javascript">
        function popopAdd_CancelSave_Click() {
            $find('<%=MPEAdd.ClientID%>').hide();
            return false;
        }

        function popopEdit_CancelSave_Click() {
            var mPEEditID = $get('<%=HdfRptLopHocMPEEdit.ClientID%>').value;
            $find(mPEEditID).hide();
            return false;
        }

        function popopConfirmDelete_CancelDelete_Click() {
            var mPEDeleteID = $get('<%=HdfRptLopHocMPEDelete.ClientID%>').value;
            $find(mPEDeleteID).hide();
            return false;
        }

    </script>
</div>
<div style="padding:5px 10px 10px 10px;">
    <table style="font-weight:bold; font-size:14px; color:Purple">
        <tr>
            <td style="width:300px">
                <asp:Label ID="LblMaHocSinh" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                <asp:Label ID="LblHoVaTen" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="LblNamHoc" runat="server" Text="Label"></asp:Label>
            </td>
            <td>
                <asp:Label ID="LblHocKy" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
    </table>
</div>
<div style="padding:5px 10px 5px 10px; font-size:15px; font-weight:bold; text-transform:uppercase">
    <asp:Label ID="LblBangDiemMon" runat="server" ForeColor="Violet"></asp:Label>
</div>
<table class="table_data">
    <tr class="add">
        <td>                        
            <asp:ImageButton ID="BtnAdd" runat="server" 
                ImageUrl="~/Styles/Images/button_add_with_text.png" 
                ToolTip="Thêm lớp học mới"/>
            <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server"
                TargetControlID="BtnAdd"
                PopupControlID="PnlPopupAdd"
                BackgroundCssClass="modalBackground"                                
                CancelControlID="ImgClosePopupAdd"                      
                PopupDragHandleControlID="PnlDragPopupAdd">
            </ajaxToolkit:ModalPopupExtender>                        
        </td>                
    </tr>
    <tr>
        <td>
            <asp:Label ID="LblSearchResult" runat="server" style="font-size:15px; font-weight:bold;"
                Text="Chưa có thông tin điểm">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <table class="repeater">   
                <asp:HiddenField ID="HdfMaDiemMonHK" runat="server"/>
                <asp:HiddenField ID="HdfMaChiTietDiem" runat="server"/>
                <asp:HiddenField ID="HdfRptLopHocMPEDelete" runat="server"/>
                <asp:HiddenField ID="HdfRptLopHocMPEEdit" runat="server"/>
                <asp:Repeater ID="RptDiemMonHoc" runat="server" 
                    onitemcommand="RptDiemMonHoc_ItemCommand" onitemdatabound="RptDiemMonHoc_ItemDataBound">
                    <HeaderTemplate>
                        <tr class="header">
                            <td class="left orderNo">
                                STT
                            </td>
                            <td class="middle">
                                Loại điểm
                            </td>
                            <td class="middle" style="width:100px;">
                                Điểm
                            </td>                            
                            <td class="middle icon">
                                Sửa
                            </td>
                            <td class="right icon">
                                Xóa
                            </td>
                        </tr>
                    </HeaderTemplate>                        
                    <ItemTemplate>                            
                        <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                            <td style="height:40px; text-align:center">
                                <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                                <asp:HiddenField ID="HdfRptMaChiTietDiem" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaChiTietDiem")%>'/>
                            </td>                            
                            <td style="height:40px;">
                                <%#DataBinder.Eval(Container.DataItem, "TenLoaiDiem")%>
                            </td>
                            <td style="height:40px; text-align:right;">
                                <%#DataBinder.Eval(Container.DataItem, "Diem")%>
                            </td>                           
                            <td class="icon" style="height:40px;">
                                <asp:ImageButton ID="BtnFakeEditItem" runat="server" style="display:none;"/>
                                <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png" 
                                    CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaChiTietDiem")%>' />
                                <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server"                                         
                                    TargetControlID="BtnFakeEditItem"
                                    PopupControlID="PnlPopupEdit"    
                                    BackgroundCssClass="modalBackground"
                                    CancelControlID="ImgClosePopupEdit"
                                    PopupDragHandleControlID="PnlDragPopupEdit">
                                </ajaxToolkit:ModalPopupExtender>
                            </td>
                            <td class="icon" style="height:40px;">
                                <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" style="display:none;"/>
                                <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                    CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaChiTietDiem")%>' />
                                <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server"                                         
                                    TargetControlID="BtnFakeDeleteItem"
                                    PopupControlID="PnlPopupConfirmDelete"                                        
                                    BackgroundCssClass="modalBackground"
                                    CancelControlID="imgClosePopupConfirmDelete"
                                    PopupDragHandleControlID="PnlDragPopupConfirmDelete">
                                </ajaxToolkit:ModalPopupExtender>
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
            <div style="float:left; margin-top:-30px; margin-left:10px; text-transform:uppercase; font-size:15px; font-weight:bold">
                <asp:Label ID="LblDiemTB" runat="server"></asp:Label>                
            </div>                                          
            <div style="float:right; margin-top:-35px; padding-right:30px;">                    
                <cc1:DataPager ID="MainDataPager" runat="server" OfClause="/" PageClause="TRANG"
                    OnCommand="pager_Command" PageSize="10" ViewStateMode="Enabled" LastClause=">>" GenerateHiddenHyperlinks="False" CompactModePageCount="3" GenerateFirstLastSection="True" GenerateGoToSection="False" FirstClause="<<" BackToFirstClause="Trở về trang đầu" BackToPageClause="Trở về trang" GoToLastClause="Đến trang cuối" NextToPageClause="Đến trang" ShowResultClause="Hiển thị kết quả" ToClause="đến" />
            </div>
        </td>            
    </tr>
</table>
<div style="padding:5px; vertical-align:middle;">
    <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back.png" OnClick="BtnBackPrevPage_Click"/>    
    <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back_text.png" OnClick="BtnBackPrevPage_Click"/>
</div>
<asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup" Width="350px">
    <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header">
        <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa điểm môn học" CssClass="popup_header_title" ></asp:Label>
        <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/Images/popup_button_close.png" alt="close"/>
    </asp:Panel>
    <div style="padding:10px;">               
        <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa điểm này không?"></asp:Label>         
    </div>
    <div style="width:170px; margin:0px auto 0px auto; padding-bottom:5px;">
        <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_yes.png" OnClick="BtnOKDeleteItem_Click"/> &nbsp;                        
        <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" 
            ImageUrl="~/Styles/Images/button_no.png" OnClientClick="return popopConfirmDelete_CancelDelete_Click();"/>
    </div>
</asp:Panel>
<asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup" Width="310px">
    <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header">
        <asp:Label ID="LblPnlPopupAddTitle" runat="server" CssClass="popup_header_title" Text="Thêm điểm môn học"></asp:Label>
        <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png" alt="close"/>
    </asp:Panel>
    <table style="padding:5px 7px 10px 7px;">
        <tr>
            <td>
                <div class="inputBorder">
                    <table style="width:100%;">                        
                        <tr>
                            <td style="width:100px; vertical-align:text-top; padding-top:3px;">
                                <asp:Label ID="Label8" runat="server" Text="Loại điểm:"></asp:Label>
                            </td>
                            <td style="width:auto;">
                                <asp:DropDownList ID="DdlLoaiDiemThem" runat="server" Width="200px"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top; padding-top:3px;">
                                <asp:Label ID="Label9" runat="server" Text="Điểm:"></asp:Label>&nbsp;
                                <asp:Label ID="Label10" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width:auto;">
                                <asp:TextBox ID="TxtDiemThem" runat="server" CssClass="input_textbox" style="text-align:right"></asp:TextBox>                                
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                    TargetControlID="TxtDiemThem" FilterType="Custom, Numbers" ValidChars=".">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:Label ID="LblErrorDiemThem" runat="server" Text="Điểm không được để trống" CssClass="error hide_error">
                                </asp:Label>
                            </td>                
                        </tr>
                    </table>                        
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table style="padding:5px 0px 5px 0px;">
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Label ID="Label5" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>            
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
                    <asp:Label ID="Label6" runat="server" Text="Thêm tiếp sau khi lưu"></asp:Label>                       
                </td>
            </tr>
        </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="width:170px; margin:0px auto 0px auto;">
                    <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/Images/button_yes.png" OnClick="BtnSaveAdd_Click"/> &nbsp;
                    <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/Images/button_no.png" OnClientClick="return popopAdd_CancelSave_Click();"/>
                </div>
            </td>
        </tr>
    </table>              
</asp:Panel>
<asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup" Width="310px">
    <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header">
        <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title" Text="Sửa điểm môn học"></asp:Label>
        <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png" alt="close"/>
    </asp:Panel>
    <table style="padding:5px 7px 10px 7px;">
        <tr>
            <td>
                <div class="inputBorder">
                    <table style="width:100%;">                        
                        <tr>
                            <td style="width:100px; vertical-align:text-top; padding-top:3px;">
                                <asp:Label ID="Label1" runat="server" Text="Loại điểm:"></asp:Label>
                            </td>
                            <td  style="width:auto;" class="input_textbox readOnlyTextBox">
                                <asp:Label ID="LblLoaiDiemSua" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top; padding-top:3px;">
                                <asp:Label ID="Label2" runat="server" Text="Điểm:"></asp:Label>&nbsp;
                                <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width:auto;">
                                <asp:TextBox ID="TxtDiemSua" runat="server" CssClass="input_textbox" style="text-align:right"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                    TargetControlID="TxtDiemSua" FilterType="Custom, Numbers" ValidChars=".">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:Label ID="LblErrorDiemSua" runat="server" Text="Điểm không được để trống" CssClass="error hide_error">
                                </asp:Label>
                            </td>                
                        </tr>
                    </table>                        
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <table style="padding:5px 0px 5px 0px;">
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Label ID="Label11" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>
                </td>
            </tr>
        </table>
            </td>
        </tr>
        <tr>
            <td>
                <div style="width:170px; margin:0px auto 0px auto;">
                    <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/Images/button_yes.png" OnClick="BtnSaveEdit_Click"/> &nbsp;
                    <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/Images/button_no.png" OnClientClick="return popopEdit_CancelSave_Click();"/>
                </div>
            </td>
        </tr>
    </table>              
</asp:Panel>
<div>
    <asp:HiddenField ID="HdfSTab" runat="server" />
</div>
</asp:Content>
