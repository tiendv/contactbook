<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="quanlilophoc.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.QuanLiLopHoc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div style="padding-left: 50px;">
        <asp:Panel ID="PnlClass" runat="server" Style="padding: 10px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Images/XXX.png" Style="float: left;
                height: 50px; width: 50px" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:HyperLink ID="HlkClass" runat="server" Style="font-size: 15px; font-weight: bold;
                    text-decoration: none" NavigateUrl="~/modules/lop_hoc/danhsachlop.aspx">Lớp học</asp:HyperLink>
                <br />
                Quản lí danh sách lớp
            </div>
            <div style="clear: both">
            </div>
        </asp:Panel>
        <asp:Panel ID="PnlFormerTeacher" runat="server" Style="padding: 10px;">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/Images/user.png" Style="float: left;
                height: 50px; width: 50px" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:HyperLink ID="HlkFormerTeacher" runat="server" Style="font-size: 15px; font-weight: bold;
                    text-decoration: none" NavigateUrl="~/modules/lop_hoc/giaovienchunhiem.aspx">Giáo viên chủ nhiệm</asp:HyperLink>
                <br />
                Quản lí phân công giáo viên chủ nhiệm
            </div>
            <div style="clear: both">
            </div>
        </asp:Panel>
        <asp:Panel ID="PnlSchedule" runat="server" Style="padding: 10px;">
            <asp:Image ID="Image3" runat="server" ImageUrl="~/Styles/Images/schedule.png" Style="float: left;
                height: 49px; width: 50px" />
            <div style="width: 85%; float: left; padding-left: 10px;">
                <asp:HyperLink ID="HlkSchedule" runat="server" Style="font-size: 15px; font-weight: bold;
                    text-decoration: none" NavigateUrl="~/modules/lop_hoc/thoikhoabieu.aspx">Thời khóa biểu</asp:HyperLink>
                <br />
                Quản lí sắp xếp thời khóa biểu
            </div>
            <div style="clear: both">
            </div>
        </asp:Panel>
    </div>
</asp:Content>
