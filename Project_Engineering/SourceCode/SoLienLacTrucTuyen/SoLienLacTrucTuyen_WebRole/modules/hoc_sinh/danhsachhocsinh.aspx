<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhsachhocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.StudentsPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function popopConfirmDelete_CancelDelete_Click() {
                var mPEDeleteID = $get('<%=HdfRptHocSinhMPEDelete.ClientID%>').value;
                $find(mPEDeleteID).hide();
                return false;
            }        
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table style="padding-bottom: 10px">
                        <tr>
                            <td style="width: 60px; height: 25px">
                                Năm học:
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNamHoc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 35px;">
                                Khối:
                            </td>
                            <td style="width: 200px;">
                                <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlKhoiLop_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 80px;">
                                Mã học sinh:
                            </td>
                            <td style="width: 200px;">
                                <asp:TextBox ID="TxtMaHocSinh" runat="server" Style="width: 150px;"></asp:TextBox>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="MaHocSinhWatermark" runat="server" TargetControlID="TxtMaHocSinh"
                                    WatermarkText="Tất cả">
                                </ajaxToolkit:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ngành:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlNganh" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNganh_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Lớp:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlLopHoc" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 70px;">
                                Tên học sinh:
                            </td>
                            <td style="width: 200px;">
                                <asp:TextBox ID="TxtTenHocSinh" runat="server" Style="width: 150px;"></asp:TextBox>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="TenHocSinhWatermark" runat="server" TargetControlID="TxtTenHocSinh"
                                    WatermarkText="Tất cả">
                                </ajaxToolkit:TextBoxWatermarkExtender>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm học sinh" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnPrint" runat="server" ImageUrl="~/Styles/Images/button_print.png"
                ToolTip="In danh sách học sinh" OnClick="BtnPrint_Click" />
            <asp:ImageButton ID="BtnImport" runat="server" ImageUrl="~/Styles/buttons/button_import.png"
                ToolTip="Import học sinh" OnClick="BtnImport_Click" />
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm học sinh mới" OnClick="BtnAdd_Click" CssClass="BtnAdd" />
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfMaHocSinh" runat="server" />
            <asp:HiddenField ID="HdfClassId" runat="server" />
            <asp:HiddenField ID="HdfRptHocSinhMPEDelete" runat="server" />
            <asp:Repeater ID="RptHocSinh" runat="server" OnItemCommand="RptHocSinh_ItemCommand"
                OnItemDataBound="RptHocSinh_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td class="middle" style="width: 100px;">
                            <asp:LinkButton ID="LinkButton1" runat="server">Mã học sinh</asp:LinkButton>
                        </td>
                        <td class="middle" style="width: 200px;">
                            <asp:LinkButton ID="LinkButton2" runat="server">Tên học sinh</asp:LinkButton>
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LinkButton3" runat="server">Ngành</asp:LinkButton>
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LinkButton4" runat="server">Khối</asp:LinkButton>
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LinkButton5" runat="server">Lớp</asp:LinkButton>
                        </td>
                        <td id="thEdit" runat="server" class="icon">
                            Sửa
                        </td>
                        <td id="thDelete" runat="server" class="icon">
                            Xóa
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptMaHocSinh" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "StudentId")%>' />
                            <asp:HiddenField ID="HdfRptTenHocSinh" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "FullName")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:LinkButton ID="LbtnStudentCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "StudentCode")%>'
                                Style="text-decoration: underline; color: Blue; cursor: pointer;" CommandName="CmdDetailItem"
                                CommandArgument='<%#DataBinder.Eval(Container.DataItem, "StudentId")%>'>
                            '<%#DataBinder.Eval(Container.DataItem, "FullName")%>'
                            </asp:LinkButton>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "FullName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "FacultyName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "GradeName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "ClassName")%>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "StudentId")%>' />
                            <asp:HiddenField ID="HdfClassId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ClassId")%>' />
                        </td>
                        <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                ToolTip="Xóa học sinh" CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "FullName")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnFakeDeleteItem"
                                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
                            </ajaxToolkit:ModalPopupExtender>
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
            <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="pager_Command" ViewStateMode="Enabled" />
        </div>
    </div>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa học sinh" CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" Width="32px" Height="32px" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 20px 0px 5px 0px">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
