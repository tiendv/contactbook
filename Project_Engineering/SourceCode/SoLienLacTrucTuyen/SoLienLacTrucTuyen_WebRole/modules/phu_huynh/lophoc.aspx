<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="lophoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ModuleParents.DetailedClassPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div id="divScript" runat="server">
        <script type="text/javascript">
            $(document).ready(function () {
                alert("This module is implementing");
            });
        </script>
    </div>
    <table style="padding: 5px 7px 10px 7px;" cellspacing="5px">
        <tr>
            <td>
                <table style="width: 100%;" class="inputBorder">
                    <tr>
                        <td style="width: 50px; vertical-align: top; padding-top: 3px;">
                            <asp:Label ID="Label18" runat="server" Text="Tên:"></asp:Label>&nbsp;
                        </td>
                        <td style="width: auto;" class="readOnlyTextBox input_textbox">
                            <asp:Label ID="LblClassNameChiTiet" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: text-top; padding-top: 3px;">
                            <asp:Label ID="Label21" runat="server" Text="Ngành:"></asp:Label>
                        </td>
                        <td style="width: auto;" class="readOnlyTextBox input_textbox">
                            <asp:Label ID="LblFacultyNameChiTiet" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: text-top; padding-top: 3px;">
                            <asp:Label ID="Label22" runat="server" Text="Khối:"></asp:Label>
                        </td>
                        <td style="width: auto;" class="readOnlyTextBox input_textbox">
                            <asp:Label ID="LblGradeNameChiTiet" runat="server"></asp:Label>
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
</asp:Content>
