<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="danhmuc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.CategoryManagerPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div style="padding-left: 50px;">
        <asp:Panel ID="PnlYear" runat="server" Width="200px" Style="padding: 10px 40px 40px 0px;
            float: left">
            <asp:HyperLink ID="HlkYear" runat="server" Style="font-size: 15px; font-weight: bold;
                text-decoration: none" NavigateUrl="~/modules/danh_muc/danhmucnamhoc.aspx">Năm học</asp:HyperLink>
            <br />
            Quản lí năm học
        </asp:Panel>
        <asp:Panel ID="PnlFaculty" runat="server" Width="200px" Style="padding: 10px 40px 40px 0px;
            float: left">
            <asp:HyperLink ID="HlkFaculty" runat="server" Style="font-size: 15px; font-weight: bold;
                text-decoration: none" NavigateUrl="~/modules/danh_muc/danhmucnganhhoc.aspx">Ngành học</asp:HyperLink>
            <br />
            Quản lí ngành học
        </asp:Panel>
        <asp:Panel ID="PnlGrade" runat="server" Width="200px" Style="padding: 10px 40px 40px 0px;
            float: left">
            <asp:HyperLink ID="HlkGrade" runat="server" Style="font-size: 15px; font-weight: bold;
                text-decoration: none" NavigateUrl="~/modules/danh_muc/danhmuckhoilop.aspx">Khối lớp</asp:HyperLink>
            <br />
            Quản lí khối lớp
        </asp:Panel>
        <asp:Panel ID="PnlSubject" runat="server" Width="200px" Style="padding: 10px 40px 40px 0px;
            float: left">
            <asp:HyperLink ID="HlkSubject" runat="server" Style="font-size: 15px; font-weight: bold;
                text-decoration: none" NavigateUrl="~/modules/danh_muc/danhmucmonhoc.aspx">Môn học</asp:HyperLink>
            <br />
            Quản lí môn học
        </asp:Panel>
        <asp:Panel ID="PnlTeachingPeriod" runat="server" Width="200px" Style="padding: 10px 40px 40px 0px;
            float: left">
            <asp:HyperLink ID="HlkTeachingPeriod" runat="server" Style="font-size: 15px; font-weight: bold;
                text-decoration: none" NavigateUrl="~/modules/danh_muc/danhmuctiet.aspx">Tiết học</asp:HyperLink>
            <br />
            Quản lí tiết học
        </asp:Panel>
        <asp:Panel ID="PnlMarkType" runat="server" Width="200px" Style="padding: 10px 40px 40px 0px;
            float: left">
            <asp:HyperLink ID="HlkMarkType" runat="server" Style="font-size: 15px; font-weight: bold;
                text-decoration: none" NavigateUrl="~/modules/danh_muc/danhmucloaidiem.aspx">Loại điểm</asp:HyperLink>
            <br />
            Quản lí loại điểm
        </asp:Panel>
        <asp:Panel ID="PnlLearningAptitude" runat="server" Width="200px" Style="padding: 10px 40px 40px 0px;
            float: left">
            <asp:HyperLink ID="HlkLearningAptitude" runat="server" Style="font-size: 15px; font-weight: bold;
                text-decoration: none" NavigateUrl="~/modules/danh_muc/danhmuchocluc.aspx">Học lực</asp:HyperLink>
            <br />
            Quản lí học lực
        </asp:Panel>
        <asp:Panel ID="PnlConduct" runat="server" Width="200px" Style="padding: 10px 40px 40px 0px;
            float: left">
            <asp:HyperLink ID="HlkConduct" runat="server" Style="font-size: 15px; font-weight: bold;
                text-decoration: none" NavigateUrl="~/modules/danh_muc/danhmuchanhkiem.aspx">Hạnh kiểm</asp:HyperLink>
            <br />
            Quản lí hạnh kiểm
        </asp:Panel>
        <asp:Panel ID="PnlLearningResult" runat="server" Width="200px" Style="padding: 10px 40px 40px 0px;
            float: left">
            <asp:HyperLink ID="HlkLearningResult" runat="server" Style="font-size: 15px; font-weight: bold;
                text-decoration: none" NavigateUrl="~/modules/danh_muc/danhmucdanhhieu.aspx">Danh hiệu</asp:HyperLink>
            <br />
            Quản lí danh hiệu
        </asp:Panel>
        <asp:Panel ID="PnlTeacher" runat="server" Width="200px" Style="padding: 10px 40px 40px 0px;
            float: left">
            <asp:HyperLink ID="HlkTeacher" runat="server" Style="font-size: 15px; font-weight: bold;
                text-decoration: none" NavigateUrl="~/modules/danh_muc/giao_vien/danhsachgiaovien.aspx">Giáo viên</asp:HyperLink>
            <br />
            Quản lí giáo viên
        </asp:Panel>
    </div>
</asp:Content>
