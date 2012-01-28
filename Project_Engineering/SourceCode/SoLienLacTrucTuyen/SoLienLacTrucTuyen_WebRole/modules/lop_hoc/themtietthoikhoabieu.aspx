<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="themtietthoikhoabieu.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.AddDetailSchedulePage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScripts">
        <script type="text/javascript">
            function popopMonHoc_Cancel_Click() {
                $find('<%=MPEMonHoc.ClientID%>').hide();
                return false;
            }

            function popopGiaoVien_Cancel_Click() {
                $find('<%=MPEGiaoVien.ClientID%>').hide();
                return false;
            }
        </script>
        <script type="text/javascript">
            $(document).ready(function () {
                $(this).find(".radioMonHoc input[type='radio']").change(function () {
                    if ($(this).is(':checked')) {
                        $(this).parents('td').parents('tr').siblings().each(function () {
                            $(this).find(".radioMonHoc input[type='radio']").each(function () {
                                $(this).attr('checked', false);
                            });
                        });
                    }
                });

                $(this).find(".radioGiaoVien input[type='radio']").change(function () {
                    if ($(this).is(':checked')) {
                        $(this).parents('td').parents('tr').siblings().each(function () {
                            $(this).find(".radioGiaoVien input[type='radio']").each(function () {
                                $(this).attr('checked', false);
                            });
                        });
                    }
                });
            });
        </script>
    </div>
    <div style="width: 500px; padding: 10px;" class="loginBox ui-corner-all">
        <table>
            <tr style="height: 27px">
                <td style="width: 70px;">
                    Lớp:
                </td>
                <td>
                    <asp:Label ID="LblTenLop" runat="server"></asp:Label>
                </td>
            </tr>
            <tr style="height: 27px">
                <td>
                    Năm học:
                </td>
                <td>
                    <asp:Label ID="LblNamHoc" runat="server"></asp:Label>
                </td>
            </tr>
            <tr style="height: 27px">
                <td>
                    Học kỳ:
                </td>
                <td>
                    <asp:Label ID="LblHocKy" runat="server"></asp:Label>
                </td>
            </tr>
            <tr style="height: 27px">
                <td>
                    Thứ:
                </td>
                <td>
                    <asp:Label ID="LblThu" runat="server"></asp:Label>
                </td>
            </tr>
            <tr style="height: 27px">
                <td>
                    Tiết:
                </td>
                <td>
                    <asp:Label ID="LblTiet" runat="server"></asp:Label>
                </td>
            </tr>
            <tr style="height: 27px">
                <td>
                    Môn học:
                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="LblMonHoc" runat="server" Text="(Chưa xác định)" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="BtnFakeAddItem" runat="server" Style="display: none;" />
                    <asp:ImageButton ID="BtnAddItem" runat="server" OnClick="BtnOpenPopupMonHoc_Click"
                        ImageUrl="~/Styles/Icons/icon_add.png" />
                    <ajaxToolkit:ModalPopupExtender ID="MPEMonHoc" runat="server" TargetControlID="BtnFakeAddItem"
                        PopupControlID="PnlPopupMonHoc" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupMonHoc"
                        PopupDragHandleControlID="PnlDragPopupMonHoc">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Label ID="LblMonHocError" runat="server" Text="Chưa chọn môn học" ForeColor="Red"
                        Visible="false"></asp:Label>
                </td>
            </tr>
            <tr style="height: 27px">
                <td>
                    Giáo viên:
                    <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td style="width: 250px">
                    <asp:Label ID="LblGiaoVien" runat="server" Text="(Chưa xác định)" Font-Bold="true"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton ID="BtnFakeSelectGiaoVien" runat="server" Style="display: none;" />
                    <asp:ImageButton ID="BtnSelectGiaoVien" runat="server" OnClick="BtnOpenPopupGiaoVien_Click"
                        ImageUrl="~/Styles/Icons/icon_add.png" />
                    <ajaxToolkit:ModalPopupExtender ID="MPEGiaoVien" runat="server" TargetControlID="BtnFakeSelectGiaoVien"
                        PopupControlID="PnlPopupGiaoVien" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupGiaoVien"
                        PopupDragHandleControlID="PnlDragPopupGiaoVien">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Label ID="LblGiaoVienError" runat="server" Text="Chưa chọn giáo viên" ForeColor="Red"
                        Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <asp:Panel ID="PnlPopupMonHoc" runat="server" CssClass="popup ui-corner-all" Width="510px">
        <asp:Panel ID="PnlDragPopupMonHoc" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupMonHocTitle" runat="server" CssClass="popup_header_title"
                Text="Chọn môn học cho tiết học"></asp:Label>
            <img id="ImgClosePopupMonHoc" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div id="divSearchMonHoc" style="padding: 10px 7px 0px 7px;">
            <table class="search" style="width: 100%">
                <tr>
                    <td style="width: 60px; height: 25px">
                        Ngành:
                    </td>
                    <td>
                        <asp:DropDownList ID="DdlNganh" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Khối:
                    </td>
                    <td>
                        <asp:DropDownList ID="DdlKhoi" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Môn học:
                    </td>
                    <td>
                        <asp:TextBox ID="TxtMonHoc" runat="server"></asp:TextBox>
                        <ajaxToolkit:TextBoxWatermarkExtender ID="MonHocWatermark" runat="server" TargetControlID="TxtMonHoc"
                            WatermarkText="Tất cả">
                        </ajaxToolkit:TextBoxWatermarkExtender>
                    </td>
                    <td colspan="2">
                        <asp:ImageButton ID="BtnSearchMonHoc" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                            ToolTip="Tìm kiếm môn học" OnClick="BtnSearchMonHoc_Click" CssClass="BtnSearch" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="table_data ui-corner-all" style="border-style: none; width: 100%;">
            <div>
                <asp:Label ID="LblSearchResultMonHoc" runat="server" Text="Không tìm thấy môn học"
                    Style="font-size: 15px; font-weight: bold;"></asp:Label>
            </div>
            <table class="repeater ui-corner-all" style="width: 95%;">
                <asp:HiddenField ID="HdfSubjectId" runat="server" Value="0" />
                <asp:Repeater ID="RptMonHoc" runat="server">
                    <HeaderTemplate>
                        <tr class="header">
                            <td class="ui-corner-tl orderNo">
                                STT
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton2" runat="server">Môn học</asp:LinkButton>
                            </td>
                            <td class="icon ui-corner-tr">
                                Chọn
                            </td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                            <td style="height: 40px; text-align: center">
                                <%# (DataPageMonHoc.CurrentIndex - 1) * DataPageMonHoc.PageSize + Container.ItemIndex + 1%>
                                <asp:HiddenField ID="HdfRptSubjectId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SubjectId")%>' />
                                <asp:HiddenField ID="HdfRptSubjectName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "SubjectName")%>' />
                            </td>
                            <td style="height: 40px;">
                                <asp:Label ID="LblSubjectName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "SubjectName")%>'></asp:Label>
                            </td>
                            <td style="height: 40px; text-align: right">
                                <asp:RadioButton ID="RBtnSelect" runat="server" CssClass="radioMonHoc" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr>
                            <td colspan="3" class="footer ui-corner-bl ui-corner-br">
                            </td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
            <div style="float: right; margin-top: -35px; padding-right: 30px;">
                <cc1:DataPager ID="DataPageMonHoc" runat="server" OnCommand="DataPagerMonHoc_Command"
                    ViewStateMode="Enabled" LastClause=">>" FirstClause="<<" ToClause="đến" />
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveMonHoc" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnSaveMonHoc_Click" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopMonHoc_Cancel_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupGiaoVien" runat="server" CssClass="popup ui-corner-all" Width="550px">
        <asp:Panel ID="PnlDragPopupGiaoVien" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupGiaoVienTitle" runat="server" CssClass="popup_header_title"
                Text="Chọn giáo viên cho tiết học"></asp:Label>
            <img id="ImgClosePopupGiaoVien" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div id="divSearchGiaoVien" style="padding: 10px 7px 0px 7px;">
            <table class="search" style="width: 100%">
                <tr>
                    <td>
                        Mã giáo viên:
                        <asp:TextBox ID="TxtSearchUserId" runat="server" Width="150px"></asp:TextBox>
                        <ajaxToolkit:TextBoxWatermarkExtender ID="UserIdWatermark" runat="server" TargetControlID="TxtSearchUserId"
                            WatermarkText="Tất cả">
                        </ajaxToolkit:TextBoxWatermarkExtender>
                    </td>
                    <td>
                        Tên giáo viên:
                        <asp:TextBox ID="TxtSearchTenGiaoVien" runat="server" Width="150px"></asp:TextBox>
                        <ajaxToolkit:TextBoxWatermarkExtender ID="TenGiaoVienWatermark" runat="server" TargetControlID="TxtSearchTenGiaoVien"
                            WatermarkText="Tất cả">
                        </ajaxToolkit:TextBoxWatermarkExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ImageButton ID="BtnSearchGiaoVien" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                            ToolTip="Tìm kiếm giáo viên" OnClick="BtnSearchGiaoVien_Click" CssClass="BtnSearch" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="table_data ui-corner-all" style="border-style: none; width: 100%;">
            <div>
                <asp:Label ID="LblSearchResultGiaoVien" runat="server" Text="Không tìm thấy giáo viên"
                    Style="font-size: 15px; font-weight: bold;"></asp:Label>
            </div>
            <table class="repeater ui-corner-all" style="width: 95%;">
                <asp:HiddenField ID="HdfUserId" runat="server" Value="0" />
                <asp:Repeater ID="RptGiaoVien" runat="server">
                    <HeaderTemplate>
                        <tr class="header">
                            <td class="ui-corner-tl orderNo">
                                STT
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton1" runat="server">Mã giáo viên</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="LinkButton2" runat="server">Tên giáo viên</asp:LinkButton>
                            </td>
                            <td class="icon ui-corner-tr">
                                Chọn
                            </td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                            <td style="height: 40px; text-align: center">
                                <%# (DataPageGiaoVien.CurrentIndex - 1) * DataPageGiaoVien.PageSize + Container.ItemIndex + 1%>
                                <asp:HiddenField ID="HdfRptUserId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "UserId")%>' />
                                <asp:HiddenField ID="HdfRptTenGiaoVien" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "HoTen")%>' />
                            </td>
                            <td style="height: 40px;">
                                <%#DataBinder.Eval(Container.DataItem, "MaHienThiGiaoVien")%>
                            </td>
                            <td style="height: 40px;">
                                <asp:Label ID="LblTenGiaoVien" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "HoTen")%>'></asp:Label>
                            </td>
                            <td style="height: 40px; text-align: right">
                                <asp:RadioButton ID="RBtnSelect" runat="server" CssClass="radioGiaoVien" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr>
                            <td colspan="4" class="footer ui-corner-bl ui-corner-br">
                            </td>
                        </tr>
                    </FooterTemplate>
                </asp:Repeater>
            </table>
            <div style="float: right; margin-top: -35px; padding-right: 30px;">
                <cc1:DataPager ID="DataPageGiaoVien" runat="server" OnCommand="DataPagerGiaoVien_Command"
                    ViewStateMode="Enabled" PageSize="8"/>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnSaveGiaoVien" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnSaveGiaoVien_Click" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopGiaoVien_Cancel_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
        <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
            OnClick="BtnSaveAdd_Click" ValidationGroup="AddTietHocTKB" CssClass="SaveButton" />&nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
            OnClick="BtnCancelAdd_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
