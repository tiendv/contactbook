<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="suadanhhieu.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.LearningResultModifyPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            function popopConfirmDelete_CancelDelete_Click() {
                $find('<%=MPEDelete.ClientID%>').hide();
                return false;
            }

            function validateLearningResultNameAdd(ctrl, args) {
                var hfOutput = $get('<%=hfOutputAdd.ClientID%>');
                var LearningResultName = $.trim(args.Value);
                $.ajax({
                    type: "POST",
                    url: "/Modules/Danh_Muc/DanhMucServicePage.aspx/CheckExistLearningResultName",
                    data: "{'LearningResultName':'" + LearningResultName + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (serverResponseData) {
                        if (serverResponseData.d == true) {
                            $get('<%=hfOutputAdd.ClientID%>').value = 'false';
                        } else {
                            $get('<%=hfOutputAdd.ClientID%>').value = 'true';
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        hfOutput.value = 'false';
                    }
                });

                if ($get('<%=hfOutputAdd.ClientID%>').value == 'true') {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            }
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            Tên danh hiệu:
            <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
            <asp:TextBox ID="TxtLearningResultName" runat="server"></asp:TextBox>
            <asp:HiddenField ID="hfOutputAdd" runat="server" Value="true" />
            <asp:RequiredFieldValidator ID="LearningResultNameRequiredAdd" runat="server" ControlToValidate="TxtLearningResultName"
                ValidationGroup="AddLearningResult" ErrorMessage="Tên danh hiệu không được để trống"
                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="LearningResultNameValidatorAdd" runat="server" ControlToValidate="TxtLearningResultName"
                ValidationGroup="AddLearningResult" 
                ErrorMessage="Danh hiệu đã tồn tại" Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="~/Styles/buttons/button_add.png"
                OnClick="BtnAdd_Click" CssClass="BtnAdd" />
            <asp:ImageButton ID="BtnDelete" runat="server" ImageUrl="~/Styles/buttons/button_delete.png"
                ToolTip="Xóa chi tiết danh hiệu" CssClass="BtnDelete" />
            <ajaxToolkit:ModalPopupExtender ID="MPEDelete" runat="server" TargetControlID="BtnDelete"
                PopupControlID="PnlPopupConfirmDelete" BackgroundCssClass="modalBackground" CancelControlID="imgClosePopupConfirmDelete"
                PopupDragHandleControlID="PnlDragPopupConfirmDelete">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"
                Text="Chưa có thông tin chi tiết danh hiệu"></asp:Label>
            <asp:Label ID="LblError" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <table class="repeater">
            <asp:Repeater ID="RptDetailLearningResult" runat="server" OnItemDataBound="RptDetailLearningResult_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl orderNo">
                            STT
                        </td>
                        <td>
                            Học lực
                        </td>
                        <td>
                            Hạnh kiểm
                        </td>
                        <td id="thSelectAll" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelectAll" runat="server" CssClass="selectAll" />
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%#(Container.ItemIndex + 1)%>
                        </td>
                        <td style="height: 40px;">
                            <asp:HiddenField ID="HdfLearningAptitudeId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "LearningAptitudeId")%>' />
                            <asp:DropDownList ID="DDLLearningAptitudes" runat="server" Width="200px">
                            </asp:DropDownList>
                            <br />
                            <asp:RequiredFieldValidator ID="LearningAptitudeRequired" runat="server" ControlToValidate="DDLLearningAptitudes"
                                ValidationGroup="AddLearningResult" ErrorMessage="Học lực không được để trống"
                                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:HiddenField ID="HdfConductId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ConductId")%>' />
                            <asp:DropDownList ID="DDLConducts" runat="server" Width="200px">
                            </asp:DropDownList>
                            <br />
                            <asp:RequiredFieldValidator ID="ConductRequired" runat="server" ControlToValidate="DDLConducts"
                                ValidationGroup="AddLearningResult" ErrorMessage="Hạnh kiểm không được để trống"
                                Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                        <td id="tdSelect" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div style="padding: 5px 7px 5px 7px;">
        <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
        <asp:Label ID="Label4" runat="server" Text=":Thông tin bắt buộc nhập"></asp:Label>
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
        <asp:ImageButton ID="BtnSaveAdd" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="AddLearningResult" CssClass="SaveButton" />&nbsp;
        <asp:ImageButton ID="BtnCancelAdd" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
    <asp:Panel ID="PnlPopupConfirmDelete" runat="server" CssClass="popup ui-corner-all"
        Width="350px">
        <asp:Panel ID="PnlDragPopupConfirmDelete" runat="server" CssClass="popup_header ui-corner-top">
            <asp:Label ID="LblPopupConfirmDeleteTitle" runat="server" Text="Xóa chi tiết danh hiệu"
                CssClass="popup_header_title"></asp:Label>
            <img id="imgClosePopupConfirmDelete" class="button_close" src="../../Styles/buttons/popup_button_close.png"
                alt="close" />
        </asp:Panel>
        <div style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Icons/icon-warning.png"
                Style="float: left;" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:Label ID="LblConfirmDelete" runat="server" Text="Bạn có chắc xóa chi tiết danh hiệu đã chọn không?"></asp:Label>
            </div>
        </div>
       <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 5px 0px; clear: both">
            <asp:ImageButton ID="BtnOKDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
                OnClick="BtnOKDeleteItem_Click" CssClass="SaveButton" />&nbsp;
            <asp:ImageButton ID="BtnCancelDeleteItem" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                OnClientClick="return popopConfirmDelete_CancelDelete_Click();" CssClass="CancelButton" />
        </div>
    </asp:Panel>
</asp:Content>
