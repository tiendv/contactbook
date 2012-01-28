<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="phanquyen.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.AuthenticationPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script type="text/javascript">
            $(document).ready(function () {
                $(".CheckboxAdd input[type='checkbox']").click(function () {
                    if ($(this).is(':checked')) {
                        $(this).parents('td').siblings().each(function () {
                            $(this).find(".CheckboxView input[type='checkbox']").each(function () {
                                $(this).attr('checked', 'true');
                            });
                        });
                    }
                });

                $(".CheckboxModify input[type='checkbox']").click(function () {
                    if ($(this).is(':checked')) {
                        $(this).parents('td').siblings().each(function () {
                            $(this).find(".CheckboxView input[type='checkbox']").each(function () {
                                $(this).attr('checked', 'true');
                            });
                        });
                    }
                });

                $(".CheckboxDelete input[type='checkbox']").click(function () {
                    if ($(this).is(':checked')) {
                        $(this).parents('td').siblings().each(function () {
                            $(this).find(".CheckboxView input[type='checkbox']").each(function () {
                                $(this).attr('checked', 'true');
                            });
                        });
                    }
                });

                $('.CkBxViewAll').click(function () {
                    $(".CheckboxView input[type='checkbox']").each(function () {
                        if ($(this).is(':enabled')) {
                            $(this).attr('checked', $(".CkBxViewAll input[type='checkbox']").is(':checked'));

                            if ($(this).is(':checked')) {
                                $(this).parents('td').siblings().each(function () {
                                    $(this).find(".CheckboxView input[type='checkbox']").each(function () {
                                        $(this).attr('checked', 'true');
                                    });
                                });
                            } 
                        }
                    });
                });

                $('.CkBxAddAll').click(function () {
                    $(".CheckboxAdd input[type='checkbox']").each(function () {
                        if ($(this).is(':enabled')) {
                            $(this).attr('checked', $(".CkBxAddAll input[type='checkbox']").is(':checked'));

                            if ($(this).is(':checked')) {
                                $(this).parents('td').siblings().each(function () {
                                    $(this).find(".CheckboxView input[type='checkbox']").each(function () {
                                        $(this).attr('checked', 'true');
                                    });
                                });
                            }
                        }
                    });
                });

                $('.CkBxModifyAll').click(function () {
                    $(".CheckboxModify input[type='checkbox']").each(function () {
                        if ($(this).is(':enabled')) {
                            $(this).attr('checked', $(".CkBxModifyAll input[type='checkbox']").is(':checked'));

                            if ($(this).is(':checked')) {
                                $(this).parents('td').siblings().each(function () {
                                    $(this).find(".CheckboxView input[type='checkbox']").each(function () {
                                        $(this).attr('checked', 'true');
                                    });
                                });
                            }
                        }
                    });
                });

                $('.CkBxDeleteAll').click(function () {
                    $(".CheckboxDelete input[type='checkbox']").each(function () {
                        if ($(this).is(':enabled')) {
                            $(this).attr('checked', $(".CkBxDeleteAll input[type='checkbox']").is(':checked'));

                            if ($(this).is(':checked')) {
                                $(this).parents('td').siblings().each(function () {
                                    $(this).find(".CheckboxView input[type='checkbox']").each(function () {
                                        $(this).attr('checked', 'true');
                                    });
                                });
                            }
                        }
                    });
                });
            });           
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            Nhóm người dùng: &nbsp;
            <asp:DropDownList ID="DdlRoles" runat="server" Width="200px" />
            &nbsp; &nbsp; &nbsp;
        </div>
        <div id="divButtonSearch">
            <asp:ImageButton ID="BtnSearch" runat="server" OnClick="BtnSearch_Click" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm phân quyền" CssClass="BtnSearch" />
        </div>
        <br />
    </div>
    <div class="table_data ui-corner-all">
        <table class="repeater">
            <asp:HiddenField ID="HdfRoleId" runat="server" />
            <asp:Repeater ID="RptPhanQuyen" runat="server" OnItemDataBound="RptPhanQuyen_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl">
                            <asp:Label ID="Label6" runat="server" Text="Khả năng truy cập" Style="float: right;
                                vertical-align: top;"></asp:Label><br />
                            <asp:Label ID="Label5" runat="server" Text="Chức năng" Style="float: left; vertical-align: bottom"></asp:Label>
                        </td>
                        <td style="width: 90px; text-align: center">
                            Xem<br />
                            <asp:CheckBox ID="CkBxViewAll" runat="server" CssClass="CkBxViewAll" />
                        </td>
                        <td style="width: 90px; text-align: center">
                            Thêm<br />
                            <asp:CheckBox ID="CkBxAddAll" runat="server" CssClass="CkBxAddAll" />
                        </td>
                        <td style="width: 90px; text-align: center">
                            Sửa<br />
                            <asp:CheckBox ID="CkBxModifyAll" runat="server" CssClass="CkBxModifyAll" />
                        </td>
                        <td class="ui-corner-tr" style="width: 90px">
                            Xóa<br />
                            <asp:CheckBox ID="CkBxDeleteAll" runat="server" CssClass="CkBxDeleteAll" />
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td colspan="5" style="height: 35px; padding-left: 10px; font-size: 15px; font-weight: bold;
                            text-transform: uppercase">
                            <asp:Label ID="LblFunctionCategoryName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "FunctionCategoryName")%>'></asp:Label>
                        </td>
                    </tr>
                    <asp:Repeater ID="RptChiTietPhanQuyen" runat="server">
                        <ItemTemplate>
                            <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                                <td style="padding-left: 20px">
                                    <%--<asp:CheckBox ID="CkBxFunctionAll" runat="server" />--%>
                                    <asp:HiddenField ID="HfFunctionId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "FunctionId")%>' />
                                    <asp:Label ID="LblFunctionName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "FunctionName")%>'></asp:Label>
                                </td>
                                <td style="height: 35px; text-align: center">
                                    <asp:CheckBox ID="CkbxView" runat="server" CssClass="CheckboxView" Visible='<%#DataBinder.Eval(Container.DataItem, "ViewDisplay")%>'
                                        Checked='<%#DataBinder.Eval(Container.DataItem, "ViewAccessibility")%>' />
                                </td>
                                <td style="text-align: center">
                                    <asp:CheckBox ID="CkbxAdd" runat="server" CssClass="CheckboxAdd" Visible='<%#DataBinder.Eval(Container.DataItem, "AddDisplay")%>'
                                        Checked='<%#DataBinder.Eval(Container.DataItem, "AddAccessibility")%>' />
                                </td>
                                <td style="text-align: center">
                                    <asp:CheckBox ID="CkbxModify" runat="server" CssClass="CheckboxModify" Visible='<%#DataBinder.Eval(Container.DataItem, "ModifyDisplay")%>'
                                        Checked='<%#DataBinder.Eval(Container.DataItem, "ModifyAccessibility")%>' />
                                </td>
                                <td style="text-align: center">
                                    <asp:CheckBox ID="CkbxDelete" runat="server" CssClass="CheckboxDelete" Visible='<%#DataBinder.Eval(Container.DataItem, "DeleteDisplay")%>'
                                        Checked='<%#DataBinder.Eval(Container.DataItem, "DeleteAccessibility")%>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </ItemTemplate>
                <FooterTemplate>
                    <tr>
                        <td colspan="5" class="footer ui-corner-bl ui-corner-br">
                        </td>
                    </tr>
                </FooterTemplate>
            </asp:Repeater>
        </table>
        <div style="float: right; margin-top: -35px; padding-right: 30px;">
            <cc1:DataPager ID="MainDataPager" runat="server" Visible="false" PageSize="50"/>
        </div>
        <div style="width: 170px; margin: 0px auto 0px auto; padding: 10px 0px 0px 0px">
            <asp:ImageButton ID="BtnSave" runat="server" OnClick="BtnSave_Click" ImageUrl="~/Styles/buttons/button_save.png"
                CssClass="SaveButton" />
            &nbsp;
            <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
                CssClass="CancelButton" />
        </div>
    </div>
</asp:Content>
