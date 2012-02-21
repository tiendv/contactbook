<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="duyetdiemhocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ApproveStudentMarkPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divSearch">
        <div id="divSearchCriteria">
            <asp:UpdatePanel ID="UPDropdownlists" runat="server">
                <ContentTemplate>
                    <table class="search">
                        <tr style="height: 27px">
                            <td>
                                Năm học:
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="DdlNamHoc" runat="server" Width="120px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNamHoc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Ngành:
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="DdlNganh" runat="server" Width="120px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlNganh_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Khối:
                            </td>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="120px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlKhoiLop_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Lớp:
                            </td>
                            <td style="width: 150px">
                                <asp:DropDownList ID="DdlLopHoc" runat="server" Width="120px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlLopHoc_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height: 27px">
                            <td>
                                Học kỳ:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlHocKy" runat="server" Width="120px" AutoPostBack="true"
                                    OnSelectedIndexChanged="DdlHocKy_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Môn học:
                            </td>
                            <td>
                                <asp:DropDownList ID="DdlMonHoc" runat="server" Width="120px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Loại điểm:
                            </td>
                            <td style="width: 120px;">
                                <asp:DropDownList ID="DdlLoaiDiem" runat="server" Width="120px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Trạng thái:
                            </td>
                            <td style="width: 120px;">
                                <asp:DropDownList ID="DdlStatus" runat="server" Width="120px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height: 27px">
                            <td>
                                <asp:RadioButton ID="RBtnMonth" runat="server" GroupName="GroupTime" Checked="true" />
                                Tháng:
                            </td>
                            <td>
                                <asp:DropDownList ID="DddMonths" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RadioButton ID="RBtnWeek" runat="server" GroupName="GroupTime" Checked="false" />
                                Tuần:
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="DdlWeeks" runat="server" Width="200px">
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
    <div class="table_data ui-corner-all">
        <div class="add">
            <asp:ImageButton ID="BtnApprove" runat="server" ImageUrl="~/Styles/buttons/button_confirm.png"
                OnClick="BtnApprove_Click" ToolTip="Xác nhận điểm" CssClass="BtnConfirm" />
            <asp:ImageButton ID="BtnUnApprove" runat="server" OnClick="BtnUnApprove_Click" ImageUrl="~/Styles/buttons/button_cancel.png"
                ToolTip="Hủy xác nhận điểm" CssClass="BtnCancel" />
        </div>
        <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"
            Text="Không có thông tin điểm học sinh">
        </asp:Label>
        <asp:Panel ID="PnlApprovalMarkStatus" runat="server" Style="font-weight: bold">
            <span style="clear: both">
                <br />
            </span>
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Images/alert.png" />
            <asp:Label ID="Label1" runat="server" CssClass="Title">Bạn có
                <asp:Label ID="LblApprovalMarkStatus" runat="server" CssClass="alertNumber"></asp:Label>
                điểm chưa phê duyệt </asp:Label>
        </asp:Panel>
        <table class="repeater">
            <asp:Repeater ID="RptDiemMonHoc" runat="server">
                <HeaderTemplate>
                    <tr class="header">
                        <td class="ui-corner-tl" style="width: 40px">
                            STT
                        </td>
                        <td>
                            Học sinh
                        </td>
                        <td>
                            Môn học
                        </td>
                        <td>
                            Loại điểm
                        </td>
                        <td>
                            Giá trị
                        </td>
                        <td>
                            Ngày tạo
                        </td>
                        <td>
                            Trạng thái
                        </td>
                        <td>
                            Ghi chú
                        </td>
                        <td id="thSelectAll" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelectAll" runat="server" CssClass="selectAll" />
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1%>
                            <asp:HiddenField ID="HdfDetailTermSubjectMarkId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "DetailTermSubjectMarkId")%>' />
                        </td>
                        <td>
                            <span style="float: left">
                                <%#DataBinder.Eval(Container.DataItem, "StudentName")%></span><span style="float: right">
                                    <%#DataBinder.Eval(Container.DataItem, "StudentCode")%></span>
                        </td>
                        <td>
                            <%#DataBinder.Eval(Container.DataItem, "SubjectName")%>
                        </td>
                        <td>
                            <%#DataBinder.Eval(Container.DataItem, "MarkTypeName")%>
                        </td>
                        <td>
                            <%#DataBinder.Eval(Container.DataItem, "MarkValue")%>
                        </td>
                        <td>
                            <%# ((DateTime)DataBinder.Eval(Container.DataItem, "Date")).ToShortDateString()%>
                        </td>
                        <td>
                            <asp:HiddenField ID="HdfStatus" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "Approved")%>' />
                            <%#DataBinder.Eval(Container.DataItem, "ApprovedStatus")%>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtNote" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Note")%>'
                                Style="width: 95%; height: 25px"></asp:TextBox>
                        </td>
                        <td id="tdSelect" runat="server" class="icon" style="height: 40px;">
                            <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
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
</asp:Content>
