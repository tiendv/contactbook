using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public static class AppConstant
    {
        public const string SCHOOL = "SCHOOL";

        public const string PAGEPATH_USERS = "/modules/nguoi_dung/danhsachnguoidung.aspx";
        public const string PAGEPATH_EDITUSER = "/modules/nguoi_dung/suanguoidung.aspx";
        public const string PAGEPATH_STUDENTS = "/modules/hoc_sinh/danhsachhocsinh.aspx";
        public const string PAGEPATH_PARENTSCOMMENTS = "/modules/y_kien/ykien.aspx";
        public const string PAGEPATH_STUDENTINFOR = "/modules/hoc_sinh/thongtincanhan.aspx";
        public const string PAGEPATH_STUDENTEDIT = "/modules/hoc_sinh/suahocsinh.aspx";
        public const string PAGEPATH_ADDCOMMENT = "/modules/phu_huynh/XXX.aspx";
        public const string PAGEPATH_HOMEPAGE = "/modules/trang_chu/trangchu.aspx";
        public const string PAGEPATH_PRINTSTUDENTS = "/modules/hoc_sinh/indanhsachhocsinh.aspx";

        public const string QUERY_STUDENT = "std";
        public const string QUERY_PARENTSCOMMENT = "id";

        public const string VIEWSTATE_CLASSID = "ClassId";
        public const string VIEWSTATE_STUDENTID = "StudentId";
        public const string VIEWSTATE_ROLE = "Role";
        public const string VIEWSTATE_SELECTED_YEAR = "Year";
        public const string VIEWSTATE_SELECTED_CLASS = "Class";
        public const string VIEWSTATE_SELECTED_FACULTY = "Faculty";
        public const string VIEWSTATE_SELECTED_GRADE = "Grade";
        public const string VIEWSTATE_SELECTED_STUDENTCODE = "StudentName";
        public const string VIEWSTATE_SELECTED_STUDENTNAME = "StudenCode";
        public const string VIEWSTATE_STUDENTCLASS_ID = "StudentClassId";
        public const string VIEWSTATE_PREV_PAGE = "PrevPage";

        public const string SESSION_MEMBERSHIP_STUDENT = "MembershipStudent";
        public const string SESSION_SELECTEDPARENTSFUNCTION = "SelectedParentFunction";
        public const string SESSION_STUDENT = "Student";
        public const string SESSION_SELECTED_YEAR = "Year";
        public const string SESSION_SELECTED_CLASS = "Class";
        public const string SESSION_SELECTED_FACULTY = "Faculty";
        public const string SESSION_SELECTED_GRADE = "Grade";
        public const string SESSION_SELECTED_STUDENTCODE = "StudentName";
        public const string SESSION_SELECTED_STUDENTNAME = "StudenCode";
        public const string SESSION_STUDENTCLASS = "StudenClass";
        public const string SESSION_PREV_PAGE = "PrevPage";

        public const string CSSSTYLE_DISPLAY_NONE = "none";
        public const string CSSSTYLE_DISPLAY_BLOCK = "block";

        public const string IMAGESOURCE_SEARCH_DISABLE = "~/Styles/Images/button_search_with_text_disable.png";
        public const string IMAGESOURCE_ADD_DISABLE = "~/Styles/Images/button_add_with_text_disable.png";
        public const string IMAGESOURCE_DELETE_DISABLE = "~/Styles/Images/button_delete_disable.png";

        public const string RESOURCE_SEARCH_NOINFO = "LblSearchResultText";
        public const string RESOURCE_SEARCH_NOMATCH = "LblSearchResultSearchText";

        public const string FILENAME_MAINRESOURCE = "MainResource";

        public const string ROLE = "Role";
        public const string TEACHER = "Teacher";
        public const string UNDERSCORE = "_";

        public const string COMMAND_EDIT = "CommandEdit";
        
    }
}