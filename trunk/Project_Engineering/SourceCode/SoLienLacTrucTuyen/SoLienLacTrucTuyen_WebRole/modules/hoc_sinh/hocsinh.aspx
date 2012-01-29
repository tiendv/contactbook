<%@ Page Title="" Language="C#" MasterPageFile="~/modules/Site.Master" AutoEventWireup="true"
    CodeBehind="hocsinh.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.StudentManagementPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div style="padding-left: 50px;">
        <asp:Panel ID="PnlStudentList" runat="server" Style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Images/student.png" Style="float: left;
                height: 49px; width: 50px" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:HyperLink ID="HlkStudentList" runat="server" Style="font-size: 15px; font-weight: bold;
                    text-decoration: none" NavigateUrl="~/modules/hoc_sinh/danhsachhocsinh.aspx">Danh sách học sinh</asp:HyperLink>
                <br />
                Quản lí thông tin cá nhân, tình hình nghỉ học, hoạt động của học sinh
            </div>
            <div style="clear: both">
            </div>
        </asp:Panel>
        <asp:Panel ID="PnlStudentMark" runat="server" Style="padding: 10px;">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/Images/studying_result.png" Style="float: left;
                height: 47px; width: 49px" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:HyperLink ID="HlkStudentMark" runat="server" Style="font-size: 15px; font-weight: bold;
                    text-decoration: none" NavigateUrl="~/modules/hoc_sinh/diemhocsinh.aspx">Điểm học sinh</asp:HyperLink>
                <br />
                Quản lí điểm của học sinh
            </div>
            <div style="clear: both">
            </div>
        </asp:Panel>
        <asp:Panel ID="PnlStudentConduct" runat="server" Style="padding: 10px;">
            <asp:Image ID="Image3" runat="server" ImageUrl="~/Styles/Images/user.png" Style="float: left;
                height: 49px; width: 50px" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:HyperLink ID="HlkStudentConduct" runat="server" Style="font-size: 15px; font-weight: bold;
                    text-decoration: none" NavigateUrl="~/modules/hoc_sinh/hanhkiemhocsinh.aspx">Hạnh kiểm học sinh</asp:HyperLink>
                <br />
                Quản lí hạnh kiểm của học sinh
            </div>
            <div style="clear: both">
            </div>
        </asp:Panel>
        <asp:Panel ID="PnlChangeGrade" runat="server" Style="padding: 10px;">
            <asp:Image ID="Image4" runat="server" ImageUrl="~/Styles/Images/user.png" Style="float: left;
                height: 49px; width: 50px" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:HyperLink ID="HlkChangeGrade" runat="server" Style="font-size: 15px; font-weight: bold;
                    text-decoration: none" NavigateUrl="~/modules/hoc_sinh/lenlophocsinh.aspx">Lên lớp học sinh</asp:HyperLink>
                <br />
                Lên lớp cho học sinh
            </div>
            <div style="clear: both">
            </div>
        </asp:Panel>
    </div>
</asp:Content>
