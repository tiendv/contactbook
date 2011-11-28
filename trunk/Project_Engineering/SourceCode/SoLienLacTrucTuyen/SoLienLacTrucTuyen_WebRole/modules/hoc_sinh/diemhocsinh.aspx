﻿<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="diemhocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.StudentMarkPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript">
        <script language="javascript" type="text/javascript">
            $(document).ready(function () {
            });
        </script>
        <script language="javascript" type="text/javascript">
            $(document).ready(function () {
                $('.MarkTextBox').keydown(function (event) {
                    // Allow only backspace and delete
                    if (event.keyCode == 46 || event.keyCode == 8) {
                        // let it happen, don't do anything
                    }
                    else {
                        // Ensure that it is a number and stop the keypress
                        if ((event.keyCode < 48 || event.keyCode > 57)
                        && (event.keyCode < 96 || event.keyCode > 105)
                        && event.keyCode != 9 && event.keyCode != 188
                        && event.keyCode != 32 && event.keyCode != 190
                        && event.keyCode != 37 && event.keyCode != 39) {
                            event.preventDefault();
                        }
                    }
                });
            });

            function validateMarks(ctrl, args) {
                var markTypeCode = $('#' + ctrl.id).next().text();
                var marks = $.trim(args.Value);
                if (marks == '' || marks.length == 0) {
                    args.IsValid = true;
                }
                else {
                    $.ajax({
                        type: "POST",
                        url: "/modules/hoc_sinh/HocSinhServicePage.aspx/ValidateMark",
                        data: "{'marks':'" + marks + "', 'markTypeCode':'" + markTypeCode + "'}",
                        contentType: "application/json; charset=utf-8",
                        success: function (serverResponseData) {
                            if (serverResponseData.d == true) {
                                args.IsValid = false;
                            } else {
                                args.IsValid = true;
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            alert('Error');
                        }
                    });
                }
            }
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
                            <td style="width: 55px;">
                                Lớp:
                            </td>
                            <td style="width: 180px">
                                <asp:DropDownList ID="DdlLopHoc" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlLopHoc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Loại điểm:
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="DdlLoaiDiem" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Học kỳ:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlHocKy" runat="server" Width="150px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlHocKy_SelectedIndexChanged">
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
                            <td>
                                Môn học:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlMonHoc" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="width: 30px; margin: 0px auto 0px auto">
            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/Images/button_search_with_text.png"
                ToolTip="Tìm kiếm học sinh" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
        </div>
    </div>
    <div class="table_data ui-corner-all">
        <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"
            Text="Không có thông tin kết quả học tập">
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
                <asp:Repeater ID="RptLoaiDiem" runat="server">
                    <ItemTemplate>
                        <td class="middle">
                            <%#DataBinder.Eval(Container.DataItem, "TenLoaiDiem")%>
                        </td>
                    </ItemTemplate>
                </asp:Repeater>
                <td id="tdDTB" runat="server" style="width: 50px">
                    ĐTB
                </td>
            </tr>
            <asp:Repeater ID="RptDiemMonHoc" runat="server" OnItemDataBound="RptDiemMonHoc_ItemDataBound">
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                            <asp:HiddenField ID="HdfMaHocSinh" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaHocSinh")%>' />
                            <asp:HiddenField ID="HdfMaDiemHK" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaDiemHK")%>' />
                        </td>
                        <td>
                            <asp:HyperLink ID="HlkMaHocSinhHienThi" runat="server"><%#DataBinder.Eval(Container.DataItem, "MaHocSinhHienThi")%></asp:HyperLink>
                        </td>
                        <td>
                            <asp:HyperLink ID="HlkHoTenHocSinh" runat="server"><%#DataBinder.Eval(Container.DataItem, "TenHocSinh")%></asp:HyperLink>
                        </td>
                        <asp:Repeater ID="RptDiemTheoLoaiDiem" runat="server">
                            <ItemTemplate>
                                <td style="height: 40px">
                                    <asp:HiddenField ID="HdfMaLoaiDiem" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MaLoaiDiem")%>' />
                                    <asp:HiddenField ID="HdfTenLoaiDiem" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "TenLoaiDiem")%>' />
                                    <asp:HiddenField ID="HdfDiem" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "StringDiems")%>' />
                                    <asp:TextBox ID="TxtDiems" runat="server" CssClass="MarkTextBox" Style="width: 98%;
                                        height: 25px" Text='<%#DataBinder.Eval(Container.DataItem, "StringDiems")%>'></asp:TextBox>
                                    <asp:HiddenField ID="HdfIsChange" runat="server" Value="false" />
                                    <asp:CustomValidator ID="DiemsValidator" runat="server" ControlToValidate="TxtDiems"
                                        ValidationGroup="AddDiemHocSinh" ErrorMessage="Điểm không hợp lệ"
                                        Display="Dynamic" ForeColor="Red"></asp:CustomValidator>
                                    <span class="SpanMaLoaiDiem" style="display: none">
                                        <%#DataBinder.Eval(Container.DataItem, "MaLoaiDiem")%></span>
                                </td>
                            </ItemTemplate>
                        </asp:Repeater>
                        <td style="text-align: right">
                            <%#((double)DataBinder.Eval(Container.DataItem, "DiemTrungBinh") != -1) ? DataBinder.Eval(Container.DataItem, "DiemTrungBinh") : "Chưa xác định"%>
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
        <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/Styles/Images/button_save.png"
            OnClick="BtnSave_Click" ValidationGroup="AddDiemHocSinh" CssClass="SaveButton" />&nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>