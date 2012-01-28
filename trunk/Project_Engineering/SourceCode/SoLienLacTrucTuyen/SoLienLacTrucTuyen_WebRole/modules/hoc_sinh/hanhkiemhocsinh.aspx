<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="hanhkiemhocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ViewStudentConductPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            $(document).ready(function () {
                $(this).find(".radioBehaviour input[type='radio']").change(function () {
                    if ($(this).is(':checked')) {
                        $(this).parent().siblings().each(function () {
                            $(this).find("input[type='radio']").each(function () {
                                $(this).attr('checked', false);
                            });
                        });
                    }
                });
            });
        </script>
    </div>
    <div id="divSearch">
        <div id="divSearchCriteria">
            <asp:UpdatePanel ID="UPDropdownlists" runat="server">
                <ContentTemplate>
                    <table class="search">
                        <tr>
                            <td>
                                Năm học:
                            </td>
                            <td style="width: 180px;">
                                <asp:DropDownList ID="DdlNamHoc" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNamHoc_SelectedIndexChanged">
                                </asp:DropDownList>
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
                            <td>
                                <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlKhoiLop_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Lớp:
                            </td>
                            <td style="width: 180px">
                                <asp:DropDownList ID="DdlLopHoc" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Học kỳ:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divButtonSearch" style="margin: 3px 0px 0px 10px">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                ToolTip="Tìm kiếm thông tin hạnh kiểm học sinh" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"
            Text="Không có thông tin hạnh kiểm của học sinh">
        </asp:Label>
        <table class="repeater">
            <tr class="header">
                <td id="tdSTT" runat="server" class="ui-corner-tl" style="width: 40px">
                    STT
                </td>
                <td id="tdMaHocSinh" runat="server" style="width: 85px">
                    <asp:LinkButton ID="LlkBtnMaHocSinh" runat="server">Mã học sinh</asp:LinkButton>
                </td>
                <td id="tdHoTenHocSinh" runat="server" style="width: 150px">
                    <asp:LinkButton ID="LlkBtnHoTenHocSinh" runat="server">Họ tên</asp:LinkButton>
                </td>
                <td id="tdAbsent" runat="server">
                    Ngày nghỉ
                </td>
                <td id="tdDTB" runat="server">
                    Hạnh kiểm
                </td>
            </tr>
            <asp:Repeater ID="RptHanhKiemHocSinh" runat="server" OnItemDataBound="RptHanhKiemHocSinh_ItemDataBound">
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                            <asp:HiddenField ID="HdfMaHocSinh" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "StudentID")%>' />
                        </td>
                        <td>
                            <asp:HyperLink ID="HlkMaHocSinhHienThi" runat="server"><%#DataBinder.Eval(Container.DataItem, "StudentCode")%></asp:HyperLink>
                        </td>
                        <td>
                            <asp:HyperLink ID="HlkHoTenHocSinh" runat="server"><%#DataBinder.Eval(Container.DataItem, "StudentName")%></asp:HyperLink>
                        </td>
                        <td>
                            Tổng cộng:
                            <%#DataBinder.Eval(Container.DataItem, "TotalOfAbsentDays")%>
                            <span style='<%#((int)DataBinder.Eval(Container.DataItem, "TotalOfAbsentDays") != 0) ? "": "display:none"%>'>
                                , có phép:
                                <%#DataBinder.Eval(Container.DataItem, "TotalOfAskedAbsentDays")%>
                                , không phép:
                                <%#DataBinder.Eval(Container.DataItem, "TotalOfUnaskedAbsentDays")%>
                            </span>
                        </td>
                        <td style="text-align: center">
                            <asp:HiddenField ID="HdfConductIdHocSinh" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ConductId")%>' />
                            <asp:Repeater ID="RptHanhKiem" runat="server" OnItemDataBound="RptHanhKiem_ItemDataBound">
                                <ItemTemplate>
                                    <asp:HiddenField ID="HdfConductId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ConductId")%>' />
                                    <%#DataBinder.Eval(Container.DataItem, "ConductName")%>
                                    <asp:RadioButton ID="RbtnHanhKiem" runat="server" CssClass="radioBehaviour" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
            <cc1:DataPager ID="MainDataPager" runat="server" OnCommand="MainDataPager_Command"
                ViewStateMode="Enabled" />
        </div>
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
        <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/Styles/buttons/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="AddDiemHocSinh" CssClass="SaveButton" />&nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/buttons/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
