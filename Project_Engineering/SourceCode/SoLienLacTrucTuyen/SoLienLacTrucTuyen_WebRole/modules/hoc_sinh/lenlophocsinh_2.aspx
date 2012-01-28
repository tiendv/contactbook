<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="lenlophocsinh_2.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ChangeStudentGradeSavePage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
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
                        <td class="middle">
                            Lớp
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px; text-align: center">
                            <%# (MainDataPager.CurrentIndex - 1) * MainDataPager.PageSize + Container.ItemIndex + 1 %>
                            <asp:HiddenField ID="HdfRptMaHocSinh" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "StudentId")%>' />
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
                        </td>
                        <td style="height: 40px;">
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "ClassName")%>
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
    <div style="width: 170px; margin: 0px auto 0px auto; padding: 5px 0px 5px 0px">
        <asp:ImageButton ID="BtnSave" runat="server" OnClick="BtnSave_Click" ValidationGroup="SaveChange"
            ImageUrl="~/Styles/buttons/button_save.png" CssClass="SaveButton" />&nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" OnClick="BtnCancel_Click"
            ImageUrl="~/Styles/buttons/button_cancel.png" CssClass="CancelButton" />
    </div>
</asp:Content>
