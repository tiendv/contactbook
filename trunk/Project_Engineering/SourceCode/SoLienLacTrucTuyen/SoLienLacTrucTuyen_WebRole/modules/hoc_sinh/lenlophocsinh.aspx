<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="lenlophocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ChangeStudentGradeSeletePage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <asp:Panel ID="PnlLinkToCategory" runat="server" CssClass="loginBox ui-corner-all" Visible="false"
        Style="width: 90%; padding: 20px; margin: 0px auto 0px auto">
        Vui lòng thêm năm học mới ở menu
        <asp:HyperLink ID="HyperLink1" runat="server" Text=" danh mục năm học "></asp:HyperLink>
        trước khi thực hiện chức năng này
    </asp:Panel>
    <asp:Panel ID="PnlData" runat="server">
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
                                    <asp:Label ID="LblYear" runat="server" Width="145px" CssClass="readOnlyTextBox"></asp:Label>
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
                <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="~/Styles/buttons/button_search.png"
                    ToolTip="Tìm kiếm học sinh" OnClick="BtnSearch_Click" CssClass="BtnSearch" />
            </div>
        </div>
        <div class="table_data ui-corner-all">
            <div>
                <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
            </div>
            <table class="repeater">
                <asp:Repeater ID="RptHocSinh" runat="server">
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
                                Học lực
                            </td>
                            <td class="middle">
                                Hạnh kiểm
                            </td>
                            <td class="icon">
                                <asp:CheckBox ID="CkbxSelectAll" runat="server" CssClass="selectAll" />
                            </td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                            <td style="height: 40px; text-align: center">
                                <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                                <asp:HiddenField ID="HdfRptStudentId" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "StudentId")%>' />
                                <asp:HiddenField ID="HdfRptClassName" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ClassName")%>' />
                            </td>
                            <td style="height: 40px;">
                                <asp:LinkButton ID="LbtnStudentCode" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "StudentCode")%>'
                                    Style="text-decoration: underline; color: Blue; cursor: pointer;" CommandName="CmdDetailItem"
                                    CommandArgument='<%#DataBinder.Eval(Container.DataItem, "StudentId")%>'>                            
                                </asp:LinkButton>
                            </td>
                            <td style="height: 40px;">
                                <asp:Label ID="LblFullName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "FullName")%>'></asp:Label>
                            </td>
                            <td style="height: 40px;">
                            </td>
                            <td style="height: 40px;">
                            </td>
                            <td class="icon">
                                <asp:CheckBox ID="CkbxSelect" runat="server" CssClass="select" />
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
        <div style="width: 85px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
            <asp:ImageButton ID="BtnNext" runat="server" OnClick="BtnNext_Click" ValidationGroup="SaveChange"
                ImageUrl="~/Styles/buttons/button_next_step.png" CssClass="StepNextButton" />&nbsp;
        </div>
    </asp:Panel>
</asp:Content>
