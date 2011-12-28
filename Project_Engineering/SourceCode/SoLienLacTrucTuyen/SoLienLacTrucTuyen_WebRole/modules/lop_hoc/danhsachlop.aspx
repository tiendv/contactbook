<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhsachlop.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ClassesPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScripts">
        <script language="javascript" type="text/javascript">
            function pageLoad() {
                var modalPopupAdd = $find('<%=MPEAdd.ClientID%>');
                if (modalPopupAdd != null) {
                    modalPopupAdd.add_showing(function () {
                        $get('<%=TxtClassNameThem.ClientID%>').value = "";
                        var DdlNganh = $get('<%=DdlNganh.ClientID%>');
                        DdlNganh.options[DdlNganh.selectedIndex].value = DdlNganh.options[0].value;
                        var DdlKhoiLop = $get('<%=DdlKhoiLop.ClientID%>');
                        DdlKhoiLop.options[DdlKhoiLop.selectedIndex].value = DdlKhoiLop.options[0].value;
                    });
                }
            }

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

            function validateClassNameAdd(ctrl, args) {
                var hfOutput = $get('<%=hfOutputAdd.ClientID%>');
                var ddlNamHoc = $get('<%=DdlNamHocThem.ClientID%>');
                var YearId = ddlNamHoc.options[ddlNamHoc.selectedIndex].value;
                var ClassName = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Lop_Hoc/LopHocServicePage.aspx/LopHocExists",
                    data: "{'ClassName':'" + ClassName + "','YearId':'" + YearId + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            $get('<%=hfOutputAdd.ClientID%>').value = 'false';
                        } else {
                            $get('<%=hfOutputAdd.ClientID%>').value = 'true';
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert('Error');
                        $get('<%=hfOutputAdd.ClientID%>').value = 'false';
                    }
                });

                if ($get('<%=hfOutputAdd.ClientID%>').value == 'true') {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            }
            function fncOpen() {
                var pageId = '<%=  Page.ClientID %>';
                __doPostBack(pageId, "myargs");
                window.showModalDialog("/modules/hoc_sinh/indanhsachhocsinh.aspx", null, "dialogWidth:1000px; dialogHeight:1000px; center:yes");
            }  
            function validateClassNameEdit(ctrl, args) {
                var hfOutput = $get('<%=hfOutputEdit.ClientID%>');
                var ClassId = $get('<%=HdfClassId.ClientID%>').value;
                var ClassName = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Lop_Hoc/LopHocServicePage.aspx/LopHocExists",
                    data: "{'ClassId':'" + ClassId + "','ClassName':'" + ClassName + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            $get('<%=hfOutputEdit.ClientID%>').value = 'false';
                        } else {
                            $get('<%=hfOutputEdit.ClientID%>').value = 'true';
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert('Error');
                        $get('<%=hfOutputEdit.ClientID%>').value = 'false';
                    }
                });

                if ($get('<%=hfOutputEdit.ClientID%>').value == 'true') {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            }
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="search">
                        <tr>
                            <td style="width: 60px; height: 27px">
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
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" OnClick="BtnSearch_Click" ImageUrl="~/Styles/Images/button_search_with_text.png"
                Style="margin: 5px 0px 0px 0px" ToolTip="kiếm lớp học" CssClass="BtnSearch" />
            <asp:ImageButton ID="BtnPrint" runat="server" ImageUrl="~/Styles/Images/button_print.png"
                ToolTip="In danh sách học sinh" OnClientClick="fncOpen();" />
        </div>

        <br />
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/Images/button_add_with_text.png"
                ToolTip="Thêm lớp học mới" CssClass="BtnAdd" />
            <ajaxToolkit:ModalPopupExtender ID="MPEAdd" runat="server" TargetControlID="BtnAdd"
                PopupControlID="PnlPopupAdd" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupAdd"
                PopupDragHandleControlID="PnlDragPopupAdd">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:HiddenField ID="HdfSltClassName" runat="server" />
            <asp:HiddenField ID="HdfClassId" runat="server" />
            <asp:HiddenField ID="HdfRptLopHocMPEDelete" runat="server" />
            <asp:HiddenField ID="HdfRptLopHocMPEEdit" runat="server" />
            <asp:HiddenField ID="HdfRptLopHocMPEDetailGVCN" runat="server" />
            <asp:Repeater ID="RptLopHoc" runat="server" OnItemCommand="RptLopHoc_ItemCommand"
                OnItemDataBound="RptLopHoc_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header ui-corner-right ui-corner-left">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td style="width: 20%">
                            <asp:LinkButton ID="LkBtnLop" runat="server">Lớp</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LkBtnNganh" runat="server">Ngành</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LkBtnKhoi" runat="server">Khối</asp:LinkButton>
                        </td>
                        <td style="width: 150px">
                            GVCN
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
                            <asp:HiddenField ID="HdfRptClassId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ClassId")%>' />
                            <asp:HiddenField ID="HdfRptClassName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ClassName")%>' />
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="LblClassName" runat="server" Style="display: none"></asp:Label>
                            <asp:LinkButton ID="LbtnClassName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ClassName")%>'
                                Style="text-decoration: underline; color: Blue; cursor: pointer;" CommandName="CmdDetailItem"
                                CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ClassId")%>'>
                                    '<%#DataBinder.Eval(Container.DataItem, "ClassName")%>'
                            </asp:LinkButton>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "FacultyName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "GradeName")%>
                        </td>
                        <td style="height: 40px;">
                            <asp:Label ID="LblTenGVCN" runat="server" Style="display: none"></asp:Label>
                            <asp:HyperLink ID="HlkHomeRoomTeacher" runat="server" Target="_blank"
                                ><%#DataBinder.Eval(Container.DataItem, "TenGVCN")%></asp:HyperLink>
                        </td>
                        <td id="tdEdit" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeEditItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnEditItem" runat="server" ImageUrl="~/Styles/Images/button_edit.png"
                                CommandName="CmdEditItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ClassId")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEEdit" runat="server" TargetControlID="BtnFakeEditItem"
                                PopupControlID="PnlPopupEdit" BackgroundCssClass="modalBackground" CancelControlID="ImgClosePopupEdit"
                                PopupDragHandleControlID="PnlDragPopupEdit">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                        <td id="tdDelete" runat="server" class="icon" style="height: 40px;">
                            <asp:ImageButton ID="BtnFakeDeleteItem" runat="server" Style="display: none;" />
                            <asp:ImageButton ID="BtnDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_delete.png"
                                CommandName="CmdDeleteItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "ClassName")%>' />
                            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnFakeDeleteItem"
                                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
                            </ajaxToolkit:ModalPopupExtender>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="7" class="footer ui-corner-bl ui-corner-br">
                        </td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div style="float: right; margin-top: -45px; padding-right: 30px;">
        <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="pager_Command" ViewStateMode="Enabled" />
    </div>

    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa lớp học"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_yes.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="YesButton" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/Images/button_no.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="NoButton" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupAdd" runat="server" CssClass="popup ui-corner-all" Width="300px">
        <asp:Panel ID="PnlDragPopupAdd" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupAddTitle" runat="server" CssClass="popup_header_title"
                Text="Thêm lớp học"></asp:Label>
            <img id="ImgClosePopupAdd" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table style="width: 100%;" class="inputBorder">
                <tr>
                    <td style="width: 200px; vertical-align: top; padding-top: 3px;">
                        Tên:
                        <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtClassNameThem" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ClassNameRequiredAdd" runat="server" ControlToValidate="TxtClassNameThem"
                            ValidationGroup="AddLopHoc" ErrorMessage="Tên lớp học không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="ClassNameValidatorAdd" runat="server" ControlToValidate="TxtClassNameThem"
                            ValidationGroup="AddLopHoc" ErrorMessage="Tên lớp học đã tồn tại"
                            Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                        <asp:HiddenField ID="hfOutputAdd" runat="server" Value="true" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Ngành:
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlNganhHocThem" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Khối:
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlKhoiLopThem" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Năm học:
                    </td>
                    <td style="width: auto;">
                        <asp:DropDownList ID="DdlNamHocThem" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <div style="padding: 10px 0px 0px 0px;">
                <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                :Thông tin bắt buộc nhập<br />
                <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
                Thêm tiếp sau khi lưu
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 10px 0px">
            <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                CssClass="SaveButton" ValidationGroup="AddLopHoc" OnClick="BtnSaveAdd_Click" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                CssClass="CancelButton" OnClientClick="return popopAdd_CancelSave_Click();" />
        </div>
    </asp:Panel>
    <asp:Panel ID="PnlPopupEdit" runat="server" CssClass="popup ui-corner-all" Width="300px">
        <asp:Panel ID="PnlDragPopupEdit" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPnlPopupEditTitle" runat="server" CssClass="popup_header_title"
                Text="Sửa lớp học"></asp:Label>
            <img id="ImgClosePopupEdit" class="button_close" src="../../Styles/Images/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 5px 7px 10px 7px;">
            <table style="width: 100%;" class="inputBorder">
                <tr>
                    <td style="width: 200px; vertical-align: top; padding-top: 3px;">
                        Tên:
                        <asp:Label ID="Label9" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    </td>
                    <td style="width: auto;">
                        <asp:TextBox ID="TxtClassNameSua" runat="server" CssClass="input_textbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ClassNameRequiredEdit" runat="server" ControlToValidate="TxtClassNameSua"
                            ValidationGroup="EditLopHoc" ErrorMessage="Tên lớp học không được để trống" Display="Dynamic"
                            ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="ClassNameValidatorEdit" runat="server" ControlToValidate="TxtClassNameSua"
                            ValidationGroup="EditLopHoc"
                            ErrorMessage="Tên lớp học đã tồn tại" Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                        <asp:HiddenField ID="hfOutputEdit" runat="server" Value="true" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Ngành:
                    </td>
                    <td style="width: auto;">
                        <asp:Label ID="LblNganhHocSua" runat="server" Width="200px" CssClass="readOnlyTextBox"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Khối:
                    </td>
                    <td style="width: auto;">
                        <asp:Label ID="LblKhoiLopSua" runat="server" Width="200px" CssClass="readOnlyTextBox"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: text-top; padding-top: 3px;">
                        Năm học:
                    </td>
                    <td style="width: auto;">
                        <asp:Label ID="LblNamHocSua" runat="server" Width="200px" CssClass="readOnlyTextBox"></asp:Label>
                    </td>
                </tr>
            </table>
            <div style="padding: 10px 0px 0px 0px;">
                <asp:Label ID="Label13" runat="server" Text="*" ForeColor="Red"></asp:Label>
                <asp:Label ID="Label15" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>
            </div>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 10px 0px">
            <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/Images/button_save.png"
                CssClass="SaveButton" ValidationGroup="EditLopHoc" OnClick="BtnSaveEdit_Click" />
            &nbsp;&nbsp;
            <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
                CssClass="CancelButton" OnClientClick="return popopEdit_CancelSave_Click();" />
        </div>
    </asp:Panel>
</asp:Content>
