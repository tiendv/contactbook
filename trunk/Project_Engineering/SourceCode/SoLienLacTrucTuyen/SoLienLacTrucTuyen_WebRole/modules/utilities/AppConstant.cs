using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public static class AppConstant
    {
        public const string SCHOOL = "SCHOOL";

        public const string PAGEPATH_HOMEPAGE = "/modules/trang_chu/trangchu.aspx"; 
        
        public const string PAGEPATH_USERS = "/modules/nguoi_dung/danhsachnguoidung.aspx";
        public const string PAGEPATH_ADDUSER = "/modules/nguoi_dung/themnguoidung.aspx";
        public const string PAGEPATH_EDITUSER = "/modules/nguoi_dung/suanguoidung.aspx";
        public const string PAGEPATH_DETAILUSER = "/modules/nguoi_dung/chitietnguoidung.aspx";

        public const string PAGEPATH_STUDENTS = "/modules/hoc_sinh/danhsachhocsinh.aspx";
        public const string PAGEPATH_STUDENTINFOR = "/modules/hoc_sinh/thongtincanhan.aspx";
        public const string PAGEPATH_STUDENTEDIT = "/modules/hoc_sinh/suahocsinh.aspx";
        public const string PAGEPATH_PRINTSTUDENTS = "/modules/hoc_sinh/indanhsachhocsinh.aspx";
        public const string PAGEPATH_CHANGEGRADE_SELECT = "/modules/hoc_sinh/lenlophocsinh.aspx";
        public const string PAGEPATH_CHANGEGRADE_SAVE = "/modules/hoc_sinh/lenlophocsinh_2.aspx";

        public const string PAGEPATH_STUDENT_ADDMARK = "/modules/hoc_sinh/themdiemhocsinh.aspx";
        public const string PAGEPATH_STUDENT_MARK = "/modules/hoc_sinh/diemhocsinh.aspx";

        public const string PAGEPATH_MESSAGES = "/modules/loi_nhan/thongbao.aspx";
        public const string PAGEPATH_ADDMESSAGES = "/modules/loi_nhan/themthongbao.aspx";
        
        public const string PAGEPATH_PARENTSCOMMENTS = "/modules/y_kien/ykien.aspx";
        public const string PAGEPATH_DETAILEDPARENTSCOMMENTS = "/modules/y_kien/chitietykien.aspx";
        public const string PAGEPATH_FEEDBACKPARENTSCOMMENTS = "/modules/y_kien/phanhoiykien.aspx";
        
        public const string PAGEPATH_ADDCOMMENT = "/modules/phu_huynh/themykien.aspx";
        public const string PAGEPATH_EDITCOMMENT = "/modules/phu_huynh/suaykien.aspx";
        public const string PAGEPATH_DETAILEDCOMMENT = "/modules/phu_huynh/chitietykien.aspx";
        public const string PAGEPATH_COMMENTS = "/modules/phu_huynh/ykien.aspx";
        public const string PAGEPATH_PARENTS_MESSAGE = "/modules/phu_huynh/thongbao.aspx";
        public const string PAGEPATH_PARENTS_HOMEPAGE = "/modules/phu_huynh/trangchu.aspx";
        public const string PAGEPATH_PARENTS_DETAILEDMESSAGE = "/modules/phu_huynh/chitietthongbao.aspx";        
        
        public const string PAGEPATH_PRINTTEACHERS = "/modules/danh_muc/giao_vien/indanhsachgiaovien.aspx";
        public const string PAGEPATH_PRINTCLASSES = "/modules/lop_hoc/indanhsachlophoc.aspx";
        public const string PAGEPATH_SCHEDULE = "/modules/lop_hoc/thoikhoabieu.aspx";
        public const string PAGEPATH_SCHEDULEAGGRANGEMENT = "/modules/lop_hoc/suathoikhoabieu.aspx";
        public const string PAGEPATH_PRINTTERM = "/modules/lop_hoc/inthoikhoabieu.aspx";
        public const string PAGEPATH_SIGNIN = "/Account/DangNhap.aspx";
        public const string PAGEPATH_SCHOOLLIST = "/modules/supplier/supplierlist.aspx";
        public const string PAGEPATH_ADDSCHOOL = "/modules/supplier/addsupplier.aspx";
        public const string PAGEPATH_MODIFYSCHOOL = "/modules/supplier/modifysupplier.aspx";

        public const string QUERY_STUDENT = "std";
        public const string QUERY_PARENTSCOMMENT = "id";

        public const string VIEWSTATE_CLASSID = "ClassId";
        public const string VIEWSTATE_STUDENTID = "StudentId";
        public const string VIEWSTATE_USER = "User";
        public const string VIEWSTATE_ROLE = "Role";
        public const string VIEWSTATE_SELECTED_YEAR = "Year";
        public const string VIEWSTATE_SELECTED_YEARNAME = "YearName";
        public const string VIEWSTATE_SELECTED_CLASS = "Class";
        public const string VIEWSTATE_SELECTED_TERM = "Term";
        public const string VIEWSTATE_SELECTED_FACULTY = "Faculty";
        public const string VIEWSTATE_SELECTED_GRADE = "Grade";
        public const string VIEWSTATE_SELECTED_STUDENTCODE = "StudentName";
        public const string VIEWSTATE_SELECTED_STUDENTNAME = "StudenCode";
        public const string VIEWSTATE_STUDENTCLASS_ID = "StudentClassId";
        public const string VIEWSTATE_PREV_PAGE = "PrevPage";
        public const string VIEWSTATE_PARENTSCOMMENTID = "ParentCommentId";
        public const string VIEWSTATE_MESSAGEID = "MessageId";
        public const string VIEWSTATE_SELECTED_TEACHERNAME = "TeacherName";
        public const string VIEWSTATE_SELECTED_TEACHERID = "TeacherID";
        public const string VIEWSTATE_SELECTED_FROMDATE = "FromDate";
        public const string VIEWSTATE_SELECTED_TODATE = "ToDate";
        public const string VIEWSTATE_SELECTED_CONFIRMSTATUS = "ConfirmStatus";
        public const string VIEWSTATE_SELECTED_PROVINCEID = "Province";
        public const string VIEWSTATE_SELECTED_DISTRICTID = "District";
        public const string VIEWSTATE_SELECTED_SCHOOLNAME = "SchoolName";

        public const string SESSION_PAGEPATH = "PagePath";
        public const string SESSION_MEMBERSHIP_STUDENT = "MembershipStudent";
        public const string SESSION_SELECTEDPARENTSFUNCTION = "SelectedParentFunction";
        public const string SESSION_STUDENT = "Student";        
        public const string SESSION_SUPPLIEDPARENTSAUTHORIZATIONS = "SuppliedParentsAuthorizations";
        public const string SESSION_SELECTEDPARENTSFUNCTIONS = "SelectedParentsFunctions";           
        public const string SESSION_SELECTED_YEAR = "Year";
        public const string SESSION_SELECTED_CLASS = "Class";
        public const string SESSION_SELECTED_TERM = "Term";
        public const string SESSION_SELECTED_FACULTY = "Faculty";
        public const string SESSION_SELECTED_GRADE = "Grade";
        public const string SESSION_SELECTED_STUDENTCODE = "StudentName";
        public const string SESSION_SELECTED_STUDENTNAME = "StudenCode";
        public const string SESSION_SELECTED_PROVINCE = "Province";
        public const string SESSION_SELECTED_DISTRICT = "District";
        public const string SESSION_SELECTED_SCHOOLNAME = "SchoolName";

        public const string SESSION_STUDENTCLASS = "StudenClass";
        public const string SESSION_PREV_PAGE = "PrevPage";
        public const string SESSION_PARENTSCOMMENTID = "ParentCommentId";
        public const string SESSION_MESSAGE = "Message";
        public const string SESSION_SELECTED_FROMDATE = "FromDate";
        public const string SESSION_SELECTED_TODATE = "ToDate";
        public const string SESSION_SELECTED_CONFIRMSTATUS = "ConfirmStatus";
        public const string SESSION_SELECTED_USER = "User";
        public const string SESSION_DAILYSCHEDULES = "DailySchedules";
        public const string SESSION_IMPORTEDSTUDENTS = "ImportedStudents";
        public const string SESSION_CHANGEGRADE_STUDENTS = "ChangeGradeStudents";    

        #region Session Teacher
        public const string SESSION_TEACHERID = "TeacherID";
        public const string SESSION_TEACHERNAME = "TeacherName";
        #endregion
        public const string CSSSTYLE_DISPLAY_NONE = "none";
        public const string CSSSTYLE_DISPLAY_BLOCK = "block";

        public const string IMAGESOURCE_BUTTON_SAVE = "~/Styles/buttons/button_save.png";
        public const string IMAGESOURCE_BUTTON_SAVE_DISABLE = "~/Styles/buttons/button_save_disable.png";
        public const string IMAGESOURCE_BUTTON_SEARCH = "~/Styles/buttons/button_search.png";
        public const string IMAGESOURCE_BUTTON_SEARCH_DISABLE = "~/Styles/buttons/button_search_disable.png";
        public const string IMAGESOURCE_BUTTON_ADD = "~/Styles/buttons/button_add.png";
        public const string IMAGESOURCE_BUTTON_ADD_DISABLE = "~/Styles/buttons/button_add_disable.png";
        public const string IMAGESOURCE_BUTTON_IMPORT = "~/Styles/buttons/button_import.png";
        public const string IMAGESOURCE_BUTTON_IMPORT_DISABLE = "~/Styles/buttons/button_import_disable.png";
        public const string IMAGESOURCE_DELETE_DISABLE = "~/Styles/Images/button_delete_disable.png";
        public const string IMAGESOURCE_BUTTON_MODIFY = "~/Styles/buttons/button_edit.png";        
        public const string IMAGESOURCE_BUTTON_MODIFY_DISABLED = "~/Styles/buttons/button_edit_disable.png";
        public const string IMAGESOURCE_BUTTON_DELETE = "~/Styles/buttons/button_delete.png"; 
        public const string IMAGESOURCE_BUTTON_DELETE_DISABLED = "~/Styles/buttons/button_delete_disable.png";
        public const string IMAGESOURCE_BUTTON_EXPORT = "~/Styles/buttons/button_export.png";
        public const string IMAGESOURCE_BUTTON_EXPORT_DISABLED = "~/Styles/buttons/button_export_disable.png";

        public const string RESOURCE_SEARCH_NOINFO = "LblSearchResultText";
        public const string RESOURCE_SEARCH_NOMATCH = "LblSearchResultSearchText";

        public const string FILENAME_MAINRESOURCE = "MainResource";

        public const string ROLE = "Role";
        public const string TEACHER = "Teacher";
        public const string UNDERSCORE = "_";
        public const char UNDERSCORE_CHAR = '_';
        public const string USERPARENT_PREFIX = "PH";
        public const string STRING_MALE = "Nam";
        public const string STRING_FEMALE = "Nữ";

        public const string COMMAND_EDIT = "CommandEdit";


    }
}