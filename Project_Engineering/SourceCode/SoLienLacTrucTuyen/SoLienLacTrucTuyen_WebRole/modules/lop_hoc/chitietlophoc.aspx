<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="chitietlophoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ChiTietLopHoc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Styles/Images/button_edit_with_text.png"
        OnClick="BtnEdit_Click" Visible="false" />
    <table style="padding: 5px 7px 10px 7px;" cellspacing="5px">
        <tr>
            <td>
                <table style="width: 100%;" class="inputBorder">
                    <tr>
                        <td style="width: 50px; vertical-align: top; padding-top: 3px;">
                            <asp:Label ID="Label18" runat="server" Text="Tên:"></asp:Label>&nbsp;
                        </td>
                        <td style="width: auto;" class="readOnlyTextBox input_textbox">
                            <asp:Label ID="LblTenLopHocChiTiet" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: text-top; padding-top: 3px;">
                            <asp:Label ID="Label21" runat="server" Text="Ngành:"></asp:Label>
                        </td>
                        <td style="width: auto;" class="readOnlyTextBox input_textbox">
                            <asp:Label ID="LblTenNganhHocChiTiet" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: text-top; padding-top: 3px;">
                            <asp:Label ID="Label22" runat="server" Text="Khối:"></asp:Label>
                        </td>
                        <td style="width: auto;" class="readOnlyTextBox input_textbox">
                            <asp:Label ID="LblTenKhoiLopChiTiet" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: text-top; padding-top: 3px;">
                            <asp:Label ID="Label19" runat="server" Text="GVCN:"></asp:Label>
                        </td>
                        <td style="width: auto;" class="readOnlyTextBox input_textbox">
                            <asp:Label ID="LblTenGVCNChiTiet" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: text-top; padding-top: 3px;">
                            <asp:Label ID="Label20" runat="server" Text="Sỉ số:"></asp:Label>
                        </td>
                        <td style="width: auto;" class="readOnlyTextBox input_textbox">
                            <asp:Label ID="LblSiSoChiTiet" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/Images/button_back.png"
        OnClick="BtnBackPrevPage_Click" />
</asp:Content>
