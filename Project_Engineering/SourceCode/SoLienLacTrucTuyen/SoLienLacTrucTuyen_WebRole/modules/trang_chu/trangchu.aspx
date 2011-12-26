<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="TrangChu.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.TrangChu" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    Chào mừng phụ huynh đến với hệ thống sổ liên lạc trực tuyến EContactBook của trường &nbsp;
    <asp:Label ID="LblSchoolName" runat="server"></asp:Label>
    <asp:LoginView ID="LoginView1" runat="server">
        <RoleGroups>
            <asp:RoleGroup>
                <ContentTemplate>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 33%">
                                <div>
                                    <div style="padding: 10px;">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Images/user.png" Style="float: left;
                                            height: 50px; width: 50px" />
                                        <div style="width: 85%; float: left; padding-left: 10px;">
                                            <asp:HyperLink ID="HlkUsers" runat="server" Style="font-size: 15px" NavigateUrl="~/modules/nguoi_dung/danhsachnguoidung.aspx">Kết quả học tập</asp:HyperLink>
                                            <br />
                                            <div style="clear: both">
                                            </div>
                                        </div>
                                    </div>
                            </td>
                            <td style="width: 33%">
                                <div>
                                    <div style="padding: 10px;">
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/Images/user_group.png" Style="float: left;
                                            height: 50px; width: 50px" />
                                        <div style="width: 85%; float: left; padding-left: 10px;">
                                            <asp:HyperLink ID="HlkRoles" runat="server" Style="font-size: 15px" NavigateUrl="~/modules/nguoi_dung/phanquyen.aspx">Ngày nghỉ học</asp:HyperLink>
                                            <br />
                                            Ngày nghỉ học
                                        </div>
                                        <div style="clear: both">
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td style="width: 33%">
                                <div>
                                    <div style="padding: 10px;">
                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/Styles/Images/activity.png" Style="float: left;
                                            height: 50px; width: 50px" />
                                        <div style="width: 85%; float: left; padding-left: 10px;">
                                            <asp:HyperLink ID="HyperLink1" runat="server" Style="font-size: 15px" NavigateUrl="~/modules/nguoi_dung/nhomnguoidung.aspx">Hoạt động</asp:HyperLink>
                                            <br />
                                            Hoạt động
                                        </div>
                                        <div style="clear: both">
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Styles/Images/schedule.png" Style="float: left;
                                        height: 50px; width: 50px" />
                                    <div style="width: 85%; float: left; padding: 5px 0px 0px 10px;">
                                        <asp:HyperLink ID="HyperLink2" runat="server" Style="font-size: 15px;" NavigateUrl="~/modules/phu_huynh/thoikhoabieu.aspx">Thời khóa biểu</asp:HyperLink>
                                        <br />
                                        <asp:Label ID="Label1" runat="server" Text="Thời khóa biểu thay đổi"></asp:Label>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Styles/Images/message.png" Style="float: left;
                                        height: 50px; width: 50px" />
                                    <div style="width: 85%; float: left; padding-left: 10px;">
                                        <asp:HyperLink ID="HyperLink3" runat="server" Style="font-size: 15px" NavigateUrl="~/modules/phu_huynh/loinhankhan.aspx">Thông báo</asp:HyperLink>
                                        <br />
                                        <asp:Label ID="LblNewMessageStatus" runat="server"></asp:Label><br />
                                        <asp:Label ID="LblUnconfirmedMessageStatus" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div style="padding: 10px;">
                                    <asp:Image ID="Image6" runat="server" ImageUrl="~/Styles/Images/FeedbackIcon.png"
                                        Style="float: left; height: 50px; width: 71px" />
                                    <div style="width: 85%; float: left; padding-left: 10px;">
                                        <asp:HyperLink ID="HyperLink4" runat="server" Style="font-size: 15px" NavigateUrl="~/modules/phu_huynh/ykien.aspx">Góp ý</asp:HyperLink>
                                        <br />
                                        <asp:Label ID="LblCommentStatus" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:RoleGroup>
            <asp:RoleGroup></asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
</asp:Content>
