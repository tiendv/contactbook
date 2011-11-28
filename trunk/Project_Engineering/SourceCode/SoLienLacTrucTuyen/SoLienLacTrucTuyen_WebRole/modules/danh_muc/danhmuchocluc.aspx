<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true" CodeBehind="danhmuchocluc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DanhMucHocLuc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="DataPager" namespace="SoLienLacTrucTuyen.DataPager" tagprefix="cc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">    
    <script type="text/javascript">
        $(document).ready(function () {

            
        });
    </script>
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            $find('<%=MPEAdd.ClientID%>').add_showing(function () {
                $get('<%=TxtTenHocLucThem.ClientID%>').value = "";
                $get('<%=TxtTenHocLucThem.ClientID%>').focus();

                $get('<%=LblErrorTenHocLucThem.ClientID%>').style.display = 'none';
                $get('<%=LblErrorTenHocLucThem.ClientID%>').style.color = '#FF0000';

                $get('<%=TxtDTBTuThem.ClientID%>').value = "";

                $get('<%=TxtDTBDenThem.ClientID%>').value = "";

                $get('<%=LblErrorDTBThem.ClientID%>').style.display = 'none';
                $get('<%=LblErrorDTBThem.ClientID%>').style.color = '#FF0000';
            });
        }

        function popopAdd_CancelSave_Click() {
            $find('<%=MPEAdd.ClientID%>').hide();
            return false;
        }

        function popopConfirmDelete_CancelDelete_Click() {
            var mPEDeleteID = $get('<%=HdfRptHocLucMPEDelete.ClientID%>').value;
            $find(mPEDeleteID).hide();
            return false;
        }

        function popopEdit_Cancel_Click() {
            var mPEEditID = $get('<%=HdfRptHocLucMPEEdit.ClientID%>').value;
            $find(mPEEditID).hide();
            return false;
        }  
    </script>    
    <table class="search">
        <tr>
            <td>
                <asp:Label ID="LblNganhTitle" runat="server" Text="Tên học lực:"></asp:Label>&nbsp;
            </td>
            <td>
                <asp:TextBox ID="TxtSearchHocLuc" runat="server" Width="150px"></asp:TextBox>&nbsp;
            </td>
            <td>
                <asp:ImageButton ID="BtnSearch" runat="server" 
                    ImageUrl="~/Styles/Images/button_search_with_text.png" ToolTip="Tìm kiếm học lực"
                    onclick="BtnSearch_Click"/>
            </td>
        </tr>        
    </table>
    <table class="table_data">
        <tr class="add">
            <td>                        
                <asp:ImageButton ID="BtnAdd" runat="server" 
                    ImageUrl="~/Styles/Images/button_add_with_text.png" 
                    ToolTip="Thêm học lực mới"/>
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
                <asp:Label ID="LblSearchResult" runat="server" style="font-size:15px; font-weight:bold;"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table class="repeater">   
                    <asp:HiddenField ID="HdfEditedTenHanhKiem" runat="server" />
                    <asp:HiddenField ID="HdfMaHocLuc" runat="server"/>
                    <asp:HiddenField ID="HdfRptHocLucMPEDelete" runat="server"/>
                    <asp:HiddenField ID="HdfRptHocLucMPEEdit" runat="server"/>
                    <asp:Repeater ID="RptHocLuc" runat="server" 
                        onitemcommand="RptHocLuc_ItemCommand" 
                        onitemdatabound="RptHocLuc_ItemDataBound">
                        <HeaderTemplate>
                            <tr class="header">
                                <td class="left orderNo">
                                    STT
                                </td>
                                <td class="middle" style="width:400px">
                                    học lực
                                </td>
                                <td class="middle">
                                    DTB Đầu
                                </td>
                                <td class="middle">
                                    DTB Cuối
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
                                    <%# (PagerMain.CurrentIndex - 1) * PagerMain.PageSize + Container.ItemIndex + 1 %>
                                    <asp:HiddenField ID="HdfRptMaHocLuc" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaHocLuc")%>'/>
                                    <asp:HiddenField ID="HdfRptTenHocLuc" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "TenHocLuc")%>'/>
                                </td>
                                <td style="height:40px;">
                                    <%#DataBinder.Eval(Container.DataItem, "TenHocLuc")%>
                                </td>
                                <td style="height:40px; text-align:right">
                                    <%#DataBinder.Eval(Container.DataItem, "DTBDau")%>
                                </td>
                                <td style="height:40px; text-align:right">
                                    <%#DataBinder.Eval(Container.DataItem, "DTBCuoi")%>
                                </td>
                                <td class="icon" style="height:40px;">
                                    <asp:ImageButton ID="BtnFakeEditItem" runat="server" style="display:none;"/>
                                    <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png" 
                                        CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "MaHocLuc")%>' />
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
                                        CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "TenHocLuc")%>' />
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
                                <td colspan="6" class="footer">
<%--                                    <div class="left"></div>
                                    <div class="right"></div>
                                    <div class="middle">                                        
                                    </div>--%>
                                </td>
                            </tr>
                        </FooterTemplate>
                    </asp:Repeater>                    
                </table>                                                
                <div style="float:right; margin-top:-35px; padding-right:30px;">
                    <cc1:DataPager ID="PagerMain" runat="server" OfClause="/" PageClause="TRANG"
                        OnCommand="PagerMain_Command" PageSize="10" ViewStateMode="Enabled" LastClause=">>" GenerateHiddenHyperlinks="False" CompactModePageCount="3" GenerateFirstLastSection="True" GenerateGoToSection="False" FirstClause="<<" BackToFirstClause="Trở về trang đầu" BackToPageClause="Trở về trang" GoToLastClause="Đến trang cuối" NextToPageClause="Đến trang" ShowResultClause="Hiển thị kết quả" ToClause="đến" />
                </div>
            </td>            
        </tr>
    </table>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup" Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa học lực" CssClass="popup_header_title" ></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/Images/popup_button_close.png" alt="close"/>
        </asp:Panel>
        <div style="padding:10px;">               
            <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa học lực này không?"></asp:Label>         
        </div>
        <div style="width:170px; margin:0px auto 0px auto; padding-bottom:5px;">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_yes.png" OnClick="BtnOKDeleteItem_Click"/> &nbsp;                        
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" 
                ImageUrl="~/Styles/Images/button_no.png" OnClientClick="return popopConfirmDelete_CancelDelete_Click();"/>
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup" Width="400px">
        <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header">
            <asp:Label ID="LblPnlPopupAddTitle" runat="server" CssClass="popup_header_title" Text="Thêm học lực"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png" alt="close"/>
        </asp:Panel>
        <table style="padding:5px 7px 10px 7px;">
            <tr>
                <td>
                    <table class="inputBorder" style="width:100%;">
                        <tr>
                            <td style="width:80px; vertical-align:top; padding-top:3px;">
                                <asp:Label ID="Label6" runat="server" Text="Tên:"></asp:Label>&nbsp;
                                <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width:auto;" colspan="4">
                                <asp:TextBox ID="TxtTenHocLucThem" runat="server" CssClass="input_textbox"></asp:TextBox>
                                <asp:Label ID="LblErrorTenHocLucThem" runat="server" 
                                    Text="Tên học lực không được để trống" CssClass="error hide_error">
                                </asp:Label>
                                <asp:RequiredFieldValidator ID="TenHocLucRequiredAdd" runat="server" 
                                    ControlToValidate="TxtTenHocLucThem" Display="Dynamic" 
                                    ErrorMessage="Tên học lực không được để trống" ForeColor="Red" 
                                    ValidationGroup="AddHanhKiem"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="TenHocLucValidatorAdd" runat="server" 
                                    ControlToValidate="TxtTenHocLucThem" Display="Dynamic" 
                                    ErrorMessage="Học lực đã tồn tại" ForeColor="Red" ValidationGroup="AddHanhKiem"></asp:CustomValidator>
                            </td>                                        
                        </tr>
                        <tr>
                            <td style="vertical-align:text-top; padding-top:3px;">
                                <asp:Label ID="Label8" runat="server" Text="Hệ số điểm:"></asp:Label>
                            </td>
                            <td style="width:30px;">
                                <asp:Label ID="Label9" runat="server" Text="Từ:"></asp:Label>
                                <asp:Label ID="Label13" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width:70px; padding-right:20px; float:right">
                                <asp:TextBox ID="TxtDTBTuThem" runat="server" CssClass="input_textbox" style="text-align:right"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MEE_TxtDTBTu" runat="server"
                                    TargetControlID="TxtDTBTuThem" MaskType="Number" Mask="9.9">                                    
                                </ajaxToolkit:MaskedEditExtender>                                
                                <asp:Label ID="LblErrorDTBThem" runat="server" Text="Khoảng điểm trung bình không được để trống" CssClass="error hide_error">
                                </asp:Label>
                            </td>
                            <td style="width:50px;">
                                <asp:Label ID="Label14" runat="server" Text="Đến:"></asp:Label>
                                <asp:Label ID="Label15" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width:70px; float:right; padding-right:5px">
                                <asp:TextBox ID="TxtDTBDenThem" runat="server" CssClass="input_textbox" style="text-align:right"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MEE_TxtDTBDenThem" runat="server"
                                    TargetControlID="TxtDTBDenThem" MaskType="Number" Mask="9.9">                                    
                                </ajaxToolkit:MaskedEditExtender>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="padding:5px 0px 5px 0px;">
                <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        <asp:Label ID="Label4" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>            
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
                        <asp:Label ID="Label1" runat="server" Text="Thêm tiếp sau khi lưu"></asp:Label>                       
                    </td>
                </tr>
            </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width:170px; margin:0px auto 0px auto;">
                        <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/Images/button_yes.png" 
                            OnClick="BtnSaveAdd_Click"/> &nbsp;
                        <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/Images/button_no.png" 
                            OnClientClick="return popopAdd_CancelSave_Click();"/>
                    </div>
                </td>
            </tr>
        </table>              
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup" Width="400px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title" Text="Sửa học lực"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png" alt="close"/>
        </asp:Panel>
        <table style="padding:5px 7px 10px 7px;">
            <tr>
                <td>
                    <div class="inputBorder">
                        <table style="width:100%;">
                            <tr>
                                <td style="width:80px; vertical-align:top; padding-top:3px;">
                                    <asp:Label ID="Label2" runat="server" Text="Tên:"></asp:Label>&nbsp;
                                    <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td style="width:auto;" colspan="4" >
                                    <asp:TextBox ID="TxtSuaTenHocLuc" runat="server" CssClass="input_textbox"></asp:TextBox>                                            
                                    <asp:Label ID="TxtErrorSuaTenHocLuc" runat="server" 
                                        Text="Tên học lực không được để trống" ForeColor="red"
                                        style=" display:none;">
                                    </asp:Label>
                                    <asp:RequiredFieldValidator ID="TenHocLucRequiredEdit" runat="server" 
                                        ControlToValidate="TxtTenHocLucThem" Display="Dynamic" 
                                        ErrorMessage="Tên học lực không được để trống" ForeColor="Red" 
                                        ValidationGroup="AddHanhKiem"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="TenHocLucValidatorEdit" runat="server" 
                                        ControlToValidate="TxtTenHocLucThem" Display="Dynamic" 
                                        ErrorMessage="Học lực đã tồn tại" ForeColor="Red" ValidationGroup="AddHanhKiem"></asp:CustomValidator>
                                </td>                                        
                            </tr>
                            <tr>
                                <td style="vertical-align:text-top; padding-top:3px;">
                                    <asp:Label ID="Label10" runat="server" Text="Hệ số điểm:"></asp:Label>
                                </td>
 <%--                               <td style="width:auto;">
                                    <asp:TextBox ID="TxtHeSoDiemHocLucSua" runat="server" CssClass="input_textbox" style="font-family:arial; text-align:right">
                                    </asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MEE_TxtHeSoDiemHocLucSua" runat="server"
                                        TargetControlID="TxtHeSoDiemHocLucSua"
                                        MaskType="Number"
                                        Mask="9">                                    
                                    </ajaxToolkit:MaskedEditExtender>                       
                                    <ajaxToolkit:TextBoxWatermarkExtender ID="TWTE_TxtHeSoDiemHocLucSua" runat="server" 
                                        Enabled="True" TargetControlID="TxtHeSoDiemHocLucSua"
                                        WatermarkText="Nhập số thập phân tròn chẵn"
                                        WatermarkCssClass="input_textbox textBoxWaterMask">
                                    </ajaxToolkit:TextBoxWatermarkExtender>
                                    <asp:Label ID="TxtErrorHeSoDiemHocLucSua" runat="server" 
                                        Text="Hệ số điểm không được để trống" CssClass="error hide_error">
                                    </asp:Label>
                                </td>--%>
                            <td style="width:30px;">
                                <asp:Label ID="Label16" runat="server" Text="Từ:"></asp:Label>
                                <asp:Label ID="Label17" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width:60px; padding-right:10px; float:right">
                                <asp:TextBox ID="TxtDTBTuSua" runat="server" CssClass="input_textbox" style="text-align:right"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender3" runat="server"
                                    TargetControlID="TxtDTBTuSua" MaskType="Number" Mask="9.9">                                    
                                </ajaxToolkit:MaskedEditExtender>                                
                                <asp:Label ID="Label18" runat="server" Text="Khoảng điểm trung bình không được để trống" CssClass="error hide_error">
                                </asp:Label>
                            </td>
                            <td style="width:50px;">
                                <asp:Label ID="Label19" runat="server" Text="Đến:"></asp:Label>
                                <asp:Label ID="Label20" runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </td>
                            <td style="width:60px; float:right; padding-right:10px">
                                <asp:TextBox ID="TxtDTBDenSua" runat="server" CssClass="input_textbox" style="text-align:right"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" runat="server"
                                    TargetControlID="TxtDTBDenSua" MaskType="Number" Mask="9.9">                                    
                                </ajaxToolkit:MaskedEditExtender>
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
                        <asp:Label ID="Label11" runat="server" Text="*" ForeColor="Red"></asp:Label>
                        <asp:Label ID="Label12" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>            
                    </td>
                </tr>
            </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width:170px; margin:0px auto 0px auto;">
                        <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/Images/button_yes.png"
                            OnClick="BtnSaveEdit_Click"/> &nbsp;
                        <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/Images/button_no.png" 
                            OnClientClick="return popopEdit_Cancel_Click();"/>
                    </div>
                </td>
            </tr>
        </table>              
    </asp:Panel>
</asp:Content>
