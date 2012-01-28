<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="themthongbao.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.AddMessagePage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            $(document).ready(function () {
                $(".messageAll input[type='checkbox']").attr('checked', 'true');
                $(".messageAll input[type='checkbox']").attr("disabled", "disabled");

                $(".message input[type='checkbox']").each(function () {
                    $(this).attr('checked', 'true');
                    $(this).attr("disabled", "disabled");
                    $(this).parents('td').parents('tr').each(function () {
                        $(this).find('td').addClass('hover');
                    });
                });

                $(".RbtnSelectAll input[type='radio']").each(function () {
                    if ($(this).is(':checked')) {
                        $(".messageAll input[type='checkbox']").attr('checked', 'true');
                        $(".messageAll input[type='checkbox']").attr("disabled", "disabled");

                        $(".message input[type='checkbox']").each(function () {
                            $(this).attr('checked', 'true');
                            $(this).attr("disabled", "disabled");
                            $(this).parents('td').parents('tr').each(function () {
                                $(this).find('td').addClass('hover');
                            });
                        });
                    } else {
                        $(".messageAll input[type='checkbox']").attr('checked', 'false');
                        $(".messageAll input[type='checkbox']").removeAttr('disabled');

                        $(".message input[type='checkbox']").each(function () {
                            $(this).attr('checked', 'false');
                            $(this).removeAttr('disabled');

                            $(this).parents('td').parents('tr').each(function () {
                                $(this).find('td').removeClass('hover');
                            });
                        });
                    }
                });


                $('.RbtnSelectAll').click(function () {
                    $(".messageAll input[type='checkbox']").attr('checked', 'true');
                    $(".messageAll input[type='checkbox']").attr("disabled", "disabled");

                    $(".message input[type='checkbox']").each(function () {
                        $(this).attr('checked', 'true');
                        $(this).attr("disabled", "disabled");
                        $(this).parents('td').parents('tr').each(function () {
                            $(this).find('td').addClass('hover');
                        });
                    });
                });

                $('.RbtnSelectSome').click(function () {
                    $(".messageAll input[type='checkbox']").attr('checked', 'false');
                    $(".messageAll input[type='checkbox']").removeAttr('disabled');

                    $(".message input[type='checkbox']").each(function () {
                        $(this).attr('checked', 'false');
                        $(this).removeAttr('disabled');
                    });
                });
            });
        </script>
    </div>
    <asp:Label ID="Label14" runat="server" Text="NỘI DUNG THÔNG BÁO" ForeColor="Violet"
        Font-Bold="true"></asp:Label>
    <table style="width: 100%; padding: 10px; border: 2px solid #0000C8; background-color: #F8FDFD;"
        class="inputBorder ui-corner-all">
        <tr>
            <td style="width: 60px; vertical-align: top; padding-top: 3px;">
                Tiêu đề:
                <asp:Label ID="Label102" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td style="width: auto;" colspan="3">
                <asp:TextBox ID="TxtTitle" runat="server" Style="width: 99%; font-weight: bold" MaxLength="100"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="TitleRequired" runat="server" ControlToValidate="TxtTitle"
                    ValidationGroup="AddMessage" ErrorMessage="Tiêu đề không được để trống" Display="Dynamic"
                    ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 80px; vertical-align: top; padding-top: 3px;">
                Nội dung:
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
            <td style="width: auto;" colspan="3">
                <asp:TextBox ID="TxtContent" runat="server" TextMode="MultiLine" MaxLength="500"
                    Style="width: 99%; height: 100px; font-family: Arial"></asp:TextBox><br />
                <asp:RequiredFieldValidator ID="ContentRequired" runat="server" ControlToValidate="TxtContent"
                    ValidationGroup="AddMessage" ErrorMessage="Nội dung không được để trống" Display="Dynamic"
                    ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="padding: 10px 0px 0px 0px;">
                    <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    :Thông tin bắt buộc nhập<br />
                    <asp:CheckBox ID="CkbAddAfterSave" runat="server" />
                    Thêm tiếp sau khi lưu
                </div>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <asp:Label ID="Label2" runat="server" Text="DANH SÁCH HỌC SINH NHẬN" ForeColor="Violet"
        Font-Bold="true"></asp:Label>
    <div class="table_data ui-corner-all">
        <div id="divSearch">
            <div id="divSearchCriteria">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="padding-bottom: 10px">
                            <tr>
                                <td style="width: 60px; height: 25px">
                                    Năm học:
                                </td>
                                <td style="width: 180px;">
                                    <asp:Label ID="LblYear" runat="server" Width="150px" CssClass="readOnlyTextBox"></asp:Label>
                                </td>
                                <td>
                                    Ngành:
                                </td>
                                <td style="width: 180px;">
                                    <asp:DropDownList ID="DdlNganh" runat="server" Width="150px" AutoPostBack="true"
                                        OnSelectedIndexChanged="DdlNganh_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Khối:
                                </td>
                                <td style="width: 180px;">
                                    <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="150px" AutoPostBack="true"
                                        OnSelectedIndexChanged="DdlKhoiLop_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Lớp:
                                </td>
                                <td style="width: 180px">
                                    <asp:DropDownList ID="DdlLopHoc" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div style="width: 30px; margin: 0px auto 0px auto">
                <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                    ToolTip="Tìm kiếm điểm học sinh" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
            </div>
        </div>
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <asp:Panel ID="PnlStudentSelection" runat="server">
            <asp:RadioButton ID="RbtnSelectAll" runat="server" CssClass="RbtnSelectAll" GroupName="SelectStudent"
                Checked="true" Text="Tất cả học sinh từ kết quả tìm kiếm" />
            &nbsp;&nbsp;&nbsp;
            <asp:RadioButton ID="RbtnSelectSome" runat="server" CssClass="RbtnSelectSome" GroupName="SelectStudent"
                Text="Tùy chọn" />
        </asp:Panel>
        <table style="margin: 20px 0px 0px 0px" class="repeater">
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
                            <asp:LinkButton ID="LinkButton5" runat="server">Lớp</asp:LinkButton>
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LinkButton3" runat="server">Ngành</asp:LinkButton>
                        </td>
                        <td class="middle">
                            <asp:LinkButton ID="LinkButton4" runat="server">Khối</asp:LinkButton>
                        </td>
                        <td class="icon">
                            <asp:CheckBox ID="CkbxSelectAll" runat="server" CssClass="selectAll messageAll" />
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptMaHocSinh" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "StudentId")%>' />
                            <asp:HiddenField ID="HdfRptStudentInClassId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "StudentInClassId")%>' />
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
                            <%#DataBinder.Eval(Container.DataItem, "ClassName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "FacultyName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "GradeName")%>
                        </td>
                        <td class="icon">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select message" />
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
            <cc1:DataPager ID="MainDataPager" runat="server" PageSize="15" OnCommand="pager_Command"
                ViewStateMode="Enabled" />
        </div>
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 15px 0px 5px 0px">
        <asp:ImageButton ID="BtnSaveEdit" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="AddMessage" CssClass="SaveButton" />&nbsp;
        <asp:ImageButton ID="BtnCancelEdit" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
