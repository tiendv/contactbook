<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="chitietlophoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.DetailedClassPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="~/Styles/buttons/button_edit.png"
        OnClick="BtnEdit_Click" Visible="false" />
    <table style="padding: 5px 7px 10px 7px;" cellspacing="5px" class="detailTable">
        <tr>
            <td>
                <table style="width: 100%;" class="inputBorder">
                    <tr>
                        <td style="width: 135px;">
                            Tên:
                        </td>
                        <td>
                            <asp:Label ID="LblClassName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Ngành:
                        </td>
                        <td>
                            <asp:Label ID="LblFacultyName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Khối:
                        </td>
                        <td>
                            <asp:Label ID="LblGradeName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Sỉ số:
                        </td>
                        <td>
                            <asp:Label ID="LblStudentQuantity" runat="server"></asp:Label> học sinh
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Giáo viên chủ nhiệm:
                        </td>
                        <td>
                            <asp:Label ID="LblFormerTeacherName" runat="server"></asp:Label>
                        </td>
                    </tr>                    
                </table>
            </td>
        </tr>
    </table>
    <div style="padding: 5px; vertical-align: middle;">
        <asp:ImageButton ID="BtnBackPrevPage" runat="server" ImageUrl="~/Styles/buttons/button_back.png"
            OnClick="BtnBackPrevPage_Click" />
        <asp:ImageButton ID="BtnTextBackPrevPage" runat="server" ImageUrl="~/Styles/buttons/button_back_text.png"
            OnClick="BtnBackPrevPage_Click" />
    </div>
</asp:Content>
