<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="suadiemhocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ModifyStudentMarkPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            $(document).ready(function () {
                $('.MarkTextBox').keydown(function (event) {
                    // Allow only backspace and delete
                    if (event.keyCode == 46 || event.keyCode == 8) { // delete  & backspace 
                        // let it happen, don't do anything
                    }
                    else {
                        // Ensure that it is a number and stop the keypress
                        if ((event.keyCode < 48 || event.keyCode > 57) // differ from "0" -> "9"
                        && (event.keyCode < 96 || event.keyCode > 105) // differ from "numpad 0" -> "numpad 9"
                        && event.keyCode != 9 // differ from "TAB"
                        && event.keyCode != 32 && event.keyCode != 190 // differ "Esc" from and "."
                        && event.keyCode != 37 && event.keyCode != 39) { // differ from "left arrow " and "right arrow"
                            event.preventDefault();
                        }
                    }
                });
            });
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            <table class="search">
                <tr>
                    <td>
                        Họ tên học sinh:
                    </td>
                    <td style="width: 180px;">
                        <asp:Label ID="LblStudentName" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        Mã học sinh:
                    </td>
                    <td>
                        <asp:Label ID="LblStudentCode" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Môn học:
                    </td>
                    <td>
                        <asp:Label ID="LblSubjectName" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        Loại điểm:
                    </td>
                    <td style="width: 150px;">
                        <asp:Label ID="LblMarkTypeName" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Học kì:
                    </td>
                    <td style="width: 180px;">
                        <asp:Label ID="LblTermName" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        Lớp:
                    </td>
                    <td style="width: 180px">
                        <asp:Label ID="LblClassName" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>
                        Năm học:
                    </td>
                    <td style="width: 180px;">
                        <asp:Label ID="LblYearName" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <table class="repeater">
            <tr class="header">
                <td class="ui-corner-tl">
                    Loại điểm
                </td>
                <td colspan="10">
                    Điểm
                </td>
            </tr>
            <asp:Repeater ID="RptDiemMonHoc" runat="server" OnItemDataBound="RptDiemMonHoc_ItemDataBound">
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px">
                            <asp:HiddenField ID="HdfMarkTypeId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MarkTypeId")%>' />
                            <%#DataBinder.Eval(Container.DataItem, "MarkTypeName")%>
                        </td>
                        <td>
                            <asp:Repeater ID="RptDiemTheoLoaiDiem" runat="server">
                                <ItemTemplate>
                                    <asp:HiddenField ID="HdfDetailTermSubjectMarkId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "DetailTermSubjectMarkId")%>' />
                                    <asp:HiddenField ID="HdfOldMarkValue" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MarkValue")%>' />
                                    <asp:TextBox ID="TxtMarkValue" runat="server" CssClass="MarkTextBox" Text='<%#DataBinder.Eval(Container.DataItem, "MarkValue")%>'></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="MarkValueRegularExpression" runat="server" ErrorMessage="Giá trị điểm không hợp lệ"
                                        ControlToValidate="TxtMarkValue" Display="Dynamic" ForeColor="Red" ValidationExpression="^(10|[0-9]|0[0-9])$"
                                        ValidationGroup="ModifyMark">
                                    </asp:RegularExpressionValidator>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="20" class="footer ui-corner-bl ui-corner-br">
                        </td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
        <div style="float: right; margin-top: -35px; padding-right: 30px;">
            <cc1:DataPager ID="MainDataPager" runat="server" ViewStateMode="Enabled" Visible="false" />
        </div>
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
        <asp:ImageButton ID="BtnSave" runat="server" OnClick="BtnSave_Click" ValidationGroup="ModifyMark"
            ImageUrl="~/Styles/buttons/button_save.png" CssClass="SaveButton" />&nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" OnClick="BtnCancel_Click" ImageUrl="~/Styles/buttons/button_cancel.png"
            CssClass="CancelButton" />
    </div>
</asp:Content>
