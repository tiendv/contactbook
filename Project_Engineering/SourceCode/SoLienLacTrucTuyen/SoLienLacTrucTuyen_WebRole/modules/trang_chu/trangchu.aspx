﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Modules/Site.Master" AutoEventWireup="true"
    CodeBehind="TrangChu.aspx.cs" Inherits="SoLienLacTrucTuyen_WebRole.Modules.HomePage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_Main" runat="server">
    <asp:Label ID="LblGreetingHead" runat="server"></asp:Label>
    <asp:Label ID="LblGreetingTail" runat="server"></asp:Label>
    <asp:LoginView ID="LoginView1" runat="server">
        <RoleGroups>
            <asp:RoleGroup>
                <ContentTemplate>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 33%">
                                <div>
                                    <div style="padding: 10px;">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Images/studying_result.png"
                                            Style="float: left; height: 47px; width: 49px" />
                                        <div style="width: 85%; float: left; padding-left: 10px;">
                                            <asp:HyperLink ID="HlkUsers" runat="server" Style="font-size: 15px; font-weight: bold;
                                                text-decoration: none" NavigateUrl="~/modules/phu_huynh/ketquahoctap.aspx">Kết quả học tập</asp:HyperLink>
                                            <br />
                                            <asp:Label ID="LblHasNewMarkStt" runat="server"></asp:Label>
                                            <br />
                                            <asp:Label ID="LblHasNewFinalMarkStt" runat="server"></asp:Label>
                                            <div style="clear: both">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td style="width: 33%">
                                <div>
                                    <div style="padding: 10px;">
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/Images/absent.png" Style="float: left;
                                            height: 50px; width: 50px" />
                                        <div style="width: 85%; float: left; padding-left: 10px;">
                                            <asp:HyperLink ID="HlkRoles" runat="server" Style="font-size: 15px; font-weight: bold;
                                                text-decoration: none" NavigateUrl="~/modules/phu_huynh/ngaynghihoc.aspx">Ngày nghỉ học</asp:HyperLink>
                                            <br />
                                            <asp:Label ID="LblUnconfirmAbsent" runat="server"></asp:Label>
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
                                            <asp:HyperLink ID="HyperLink1" runat="server" Style="font-size: 15px; font-weight: bold;
                                                text-decoration: none" NavigateUrl="~/modules/phu_huynh/hoatdong.aspx">Hoạt động</asp:HyperLink>
                                            <br />
                                            <asp:Label ID="LblActivityStt" runat="server"></asp:Label>
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
                                        <asp:HyperLink ID="HyperLink2" runat="server" Style="font-size: 15px; font-weight: bold;
                                            text-decoration: none" NavigateUrl="~/modules/phu_huynh/thoikhoabieu.aspx">Thời khóa biểu</asp:HyperLink>
                                        <br />
                                        <asp:Label ID="LblScheduleStt" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Styles/Images/message.png" Style="float: left;
                                        height: 50px; width: 50px" />
                                    <div style="width: 85%; float: left; padding-left: 10px;">
                                        <asp:HyperLink ID="HyperLink3" runat="server" Style="font-size: 15px; font-weight: bold;
                                            text-decoration: none" NavigateUrl="~/modules/phu_huynh/thongbao.aspx">Thông báo</asp:HyperLink>
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
                                        <asp:HyperLink ID="HyperLink4" runat="server" Style="font-size: 15px; font-weight: bold;
                                            text-decoration: none" NavigateUrl="~/modules/phu_huynh/ykien.aspx">Góp ý</asp:HyperLink>
                                        <br />
                                        <asp:Label ID="LblCommentStatus" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:RoleGroup>
            <asp:RoleGroup>
            </asp:RoleGroup>
            <asp:RoleGroup>
                <ContentTemplate>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 33%">
                                <div style="padding: 10px;">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/Images/school.png" Style="float: left;
                                        height: 48px; width: 48px" />
                                    <div style="width: 85%; float: left; padding-left: 10px;">
                                        <asp:HyperLink ID="HlkSchools" runat="server" Style="font-size: 15px; text-decoration: none;
                                            font-weight: bold" NavigateUrl="~/modules/supplier/supplierlist.aspx">
                                            Quản lí trường học</asp:HyperLink>
                                        <br />
                                        Quản lí các trường học đăng kí sử dụng hệ thống sổ liên lạc trực tuyến
                                        <div style="clear: both">
                                        </div>
                                    </div>
                                </div>
                            </td>
                    </table>
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
</asp:Content>
