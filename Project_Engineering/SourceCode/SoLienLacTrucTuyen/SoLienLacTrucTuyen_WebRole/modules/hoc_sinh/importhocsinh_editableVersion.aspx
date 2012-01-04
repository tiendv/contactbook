<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="importhocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.ImportStudentPage" %>

<%@ Register Assembly="DataPager" Namespace="SoLienLacTrucTuyen.DataPager" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="padding: 10px 0px 10px 0px">
                Năm học:&nbsp;
                <asp:Label ID="LblYear" runat="server" Width="150px" CssClass="readOnlyTextBox"></asp:Label>
                <asp:HiddenField ID="HdfYearId" runat="server" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Ngành:
                <asp:DropDownList ID="DdlNganh" runat="server" Width="150px" AutoPostBack="true"
                    OnSelectedIndexChanged="DdlNganh_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Khối:
                <asp:DropDownList ID="DdlKhoiLop" runat="server" Width="150px" AutoPostBack="true"
                    OnSelectedIndexChanged="DdlKhoiLop_SelectedIndexChanged">
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Lớp:
                <asp:DropDownList ID="DdlLopHoc" runat="server" Width="150px">
                </asp:DropDownList>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    Download template import tại đây
    <asp:LinkButton ID="LkBtnDownload" runat="server" OnClick="LkBtnDownload_Click" Text='Tại đây'>
    </asp:LinkButton>
    <br />
    <br />
    <div style="vertical-align: middle">
        Chọn file cần import
        <asp:FileUpload ID="FileUpload1" runat="server"></asp:FileUpload>&nbsp;&nbsp;&nbsp;
        <asp:ImageButton ID="BtnUpload" runat="server" ImageUrl="~/Styles/buttons/button_upload.png"
            OnClick="BtnUpload_Click" CssClass="UploadButton" ValidationGroup="ImportFile" />
    </div>
    <asp:RequiredFieldValidator ID="RequiredClass" runat="server" ErrorMessage="Chưa chọn lớp học"
        ControlToValidate="DdlLopHoc" ValidationGroup="ImportFile" Display="Dynamic"
        ForeColor="Red">
    </asp:RequiredFieldValidator>
    <asp:Label ID="lbError" runat="server" ForeColor="Red" Text="Label"></asp:Label>
    <br />
    <div class="table_data ui-corner-all">
        <div>
            <asp:Label ID="LblSearchResult" runat="server" Style="font-size: 15px; font-weight: bold;"></asp:Label>
        </div>
        <table class="repeater">
            <asp:Repeater ID="RptHocSinh" runat="server" OnItemDataBound="RptHocSinh_ItemDataBound">
                <HeaderTemplate>
                    <tr class="header">
                        <td style="width: 90px;" class="ui-corner-tl">
                            Mã học sinh
                        </td>
                        <td>
                            Thông tin học sinh
                        </td>
                        <td>
                            Địa chỉ
                        </td>
                        <td>
                            Cha
                        </td>
                        <td>
                            Mẹ
                        </td>
                        <td>
                            Người đỡ đầu
                        </td>
                        <td id="thDelete" runat="server" class="icon">
                            <asp:CheckBox ID="CkBxAll" runat="server" />
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="RepeaterRow" runat="server" class='<%#((Container.ItemIndex + 1) % 2 == 0) ? "oddRow" : "evenRow"%>'>
                        <td style="height: 40px;">
                            <asp:TextBox ID="TextBox1" runat="server" Style="height: 90%; width: 95%" Text='<%#DataBinder.Eval(Container.DataItem, "StudentCode")%>'></asp:TextBox>
                        </td>
                        <td style="height: 40px;">
                            Họ tên:
                            <asp:TextBox ID="TextBox2" runat="server" Style="width: 95%" Text='<%#DataBinder.Eval(Container.DataItem, "FullName")%>'></asp:TextBox>
                            <br />
                            Giới tính:
                            <%#DataBinder.Eval(Container.DataItem, "StringGender")%>
                            <br />
                            Ngày sinh:
                            <asp:TextBox ID="TextBox4" runat="server" Style="width: 95%" Text='<%#DataBinder.Eval(Container.DataItem, "StringDateOfBirth")%>'></asp:TextBox>
                            <br />
                            Nơi sinh:
                            <asp:TextBox ID="TextBox3" runat="server" Style="width: 95%" Text='<%#DataBinder.Eval(Container.DataItem, "BirthPlace")%>'></asp:TextBox>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "Address")%>
                        </td>
                        <td style="height: 40px;">
                            Họ tên:
                            <asp:TextBox ID="TextBox5" runat="server" Style="width: 95%" Text='<%#DataBinder.Eval(Container.DataItem, "FatherName")%>'></asp:TextBox>
                            <br />
                            Ngày sinh:
                            <asp:TextBox ID="TextBox6" runat="server" Style="width: 95%" Text='<%#DataBinder.Eval(Container.DataItem, "StringFatherDateOfBirth")%>'></asp:TextBox>
                            <br />
                            Nghề nghiệp:
                            <asp:TextBox ID="TextBox7" runat="server" Style="width: 95%" Text='<%#DataBinder.Eval(Container.DataItem, "FatherJob")%>'></asp:TextBox>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "MotherName")%>
                        </td>
                        <td style="height: 40px;">
                            <%#DataBinder.Eval(Container.DataItem, "PatronName")%>
                        </td>
                        <td style="height: 40px; text-align: center">
                            <asp:CheckBox ID="CkBxSelect" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div style="width: 170px; margin: 0px auto 0px auto;">
        <asp:ImageButton ID="BtnSave" runat="server" ImageUrl="~/Styles/Images/button_save.png"
            OnClick="BtnSave_Click" CssClass="SaveButton" ValidationGroup="ImportFile" />
        &nbsp;
        <asp:ImageButton ID="BtnCancel" runat="server" ImageUrl="~/Styles/Images/button_cancel.png"
            OnClick="BtnCancel_Click" CssClass="CancelButton" />
    </div>
</asp:Content>
