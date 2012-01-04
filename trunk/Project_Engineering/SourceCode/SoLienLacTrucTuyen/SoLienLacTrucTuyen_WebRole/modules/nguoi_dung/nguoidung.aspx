<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="nguoidung.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.NguoiDung" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <div style="padding-left: 50px;">
        <div>
            <div style="padding: 10px;">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Images/user.png" Style="float: left;
                    height: 50px; width: 50px" />
                <div style="width: 85%; float: left; padding-left: 10px;">
                    <asp:HyperLink ID="HlkUsers" runat="server" Style="font-size: 15px; font-weight: bold; text-decoration: none"
                        NavigateUrl="~/modules/nguoi_dung/danhsachnguoidung.aspx">Người dùng</asp:HyperLink>
                    <br />
                    Quản lí danh sách người dùng
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
        <div>
            <div style="padding: 10px;">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/Images/user_group.png" Style="float: left;
                    height: 50px; width: 50px" />
                <div style="width: 85%; float: left; padding-left: 10px;">
                    <asp:HyperLink ID="HlkRoles" runat="server" Style="font-size: 15px; font-weight: bold; text-decoration: none"
                        NavigateUrl="~/modules/nguoi_dung/nhomnguoidung.aspx">Nhóm người dùng</asp:HyperLink>
                    <br />
                    Quản lí danh sách nhóm người dùng
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
        <div>
            <div style="padding: 10px;">
                <asp:Image ID="Image3" runat="server" ImageUrl="~/Styles/Images/user_group_authorization.png"
                    Style="float: left; height: 62px; width: 50px" />
                <div style="width: 85%; float: left; padding-left: 10px;">
                    <asp:HyperLink ID="HyperLink1" runat="server" Style="font-size: 15px; font-weight: bold; text-decoration: none"
                        NavigateUrl="~/modules/nguoi_dung/nhomnguoidung.aspx">Phân quyền</asp:HyperLink>
                    <br />
                    Quản lí phân quyền truy cập vào hệ thống cho nhóm người dùng
                </div>
                <div style="clear: both">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
