using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public static class AppConstant
    {
        public const string SCHOOL = "SCHOOL";
        public const string SESSION_CURRENT_YEAR = "CurrentYear";

        #region Page paths
        // Home page
        public const string PAGEPATH_HOMEPAGE = "/modules/trang_chu/trangchu.aspx"; 
        
        // User 
        public const string PAGEPATH_USER_LIST = "/modules/nguoi_dung/danhsachnguoidung.aspx";
        public const string PAGEPATH_USER_ADD = "/modules/nguoi_dung/themnguoidung.aspx";
        public const string PAGEPATH_USER_EDIT = "/modules/nguoi_dung/suanguoidung.aspx";
        public const string PAGEPATH_USER_DETAIL = "/modules/nguoi_dung/chitietnguoidung.aspx";

        // Student
        public const string PAGEPATH_STUDENT_LIST = "/modules/hoc_sinh/danhsachhocsinh.aspx";
        public const string PAGEPATH_STUDENT_INFOR = "/modules/hoc_sinh/thongtincanhan.aspx";
        public const string PAGEPATH_STUDENT_EDIT = "/modules/hoc_sinh/suahocsinh.aspx";
        public const string PAGEPATH_STUDENT_PRINT = "/modules/hoc_sinh/indanhsachhocsinh.aspx";
        public const string PAGEPATH_STUDENT_CHANGEGRADE_SELECT = "/modules/hoc_sinh/lenlophocsinh.aspx";
        public const string PAGEPATH_STUDENT_CHANGEGRADE_SAVE = "/modules/hoc_sinh/lenlophocsinh_2.aspx";
        public const string PAGEPATH_STUDENT_ADDMARK = "/modules/hoc_sinh/themdiemhocsinh.aspx";
        public const string PAGEPATH_STUDENT_MARK = "/modules/hoc_sinh/diemhocsinh.aspx";
        public const string PAGEPATH_STUDENT_EDITMARK = "/modules/hoc_sinh/suadiemhocsinh.aspx";

        // Message
        public const string PAGEPATH_MESSAGE_LIST = "/modules/loi_nhan/thongbao.aspx";
        public const string PAGEPATH_MESSAGE_ADD = "/modules/loi_nhan/themthongbao.aspx";
        public const string PAGEPATH_MESSAGE_MODIFY = "/modules/loi_nhan/suathongbao.aspx";
        public const string PAGEPATH_MESSAGE_DETAIL = "/modules/loi_nhan/chitietthongbao.aspx";
        
        // Comment
        public const string PAGEPATH_PARENTS_HOMEPAGE = "/modules/phu_huynh/trangchu.aspx"; 
        public const string PAGEPATH_COMMENT_LIST = "/modules/y_kien/ykien.aspx";
        public const string PAGEPATH_COMMENT_DETAIL = "/modules/y_kien/chitietykien.aspx";
        public const string PAGEPATH_COMMENT_FEEDBACK = "/modules/y_kien/phanhoiykien.aspx";        
        public const string PAGEPATH_PARENTS_COMMENT_ADD = "/modules/phu_huynh/themykien.aspx";
        public const string PAGEPATH_PARENTS_COMMENT_EDIT = "/modules/phu_huynh/suaykien.aspx";
        public const string PAGEPATH_PARENTS_COMMENT_DETAIL = "/modules/phu_huynh/chitietykien.aspx";
        public const string PAGEPATH_PARENTS_COMMENT_LIST = "/modules/phu_huynh/ykien.aspx";
        public const string PAGEPATH_PARENTS_MESSAGE_LIST = "/modules/phu_huynh/thongbao.aspx";        
        public const string PAGEPATH_PARENTS_MESSAGE_DETAIL = "/modules/phu_huynh/chitietthongbao.aspx";

        public const string PAGEPATH_CONFIGURATION_DETAIL = "/modules/cau_hinh/cauhinh.aspx";
        public const string PAGEPATH_CONFIGURATION_MODIFY = "/modules/cau_hinh/suacauhinh.aspx";

        public const string PAGEPATH_PRINTTEACHERS = "/modules/danh_muc/giao_vien/indanhsachgiaovien.aspx";
        public const string PAGEPATH_PRINTCLASSES = "/modules/lop_hoc/indanhsachlophoc.aspx";
        public const string PAGEPATH_SCHEDULE = "/modules/lop_hoc/thoikhoabieu.aspx";
        public const string PAGEPATH_SCHEDULE_AGGRANGE = "/modules/lop_hoc/sapxepthoikhoabieu.aspx";
        public const string PAGEPATH_SCHEDULE_ADD = "/modules/lop_hoc/themtietthoikhoabieu.aspx";
        public const string PAGEPATH_SCHEDULE_MODIFY = "/modules/lop_hoc/suatietthoikhoabieu.aspx";
        public const string PAGEPATH_PRINTTERM = "/modules/lop_hoc/inthoikhoabieu.aspx";
        public const string PAGEPATH_SIGNIN = "/Account/DangNhap.aspx";
        public const string PAGEPATH_SCHOOLLIST = "/modules/supplier/supplierlist.aspx";
        public const string PAGEPATH_ADDSCHOOL = "/modules/supplier/addsupplier.aspx";
        public const string PAGEPATH_MODIFYSCHOOL = "/modules/supplier/modifysupplier.aspx";
        public const string PAGEPATH_SCHOOL_DETAIL = "/modules/supplier/detailschool.aspx";

        public const string PAGEPATH_CATEGORY_YEARLIST = "/modules/danh_muc/danhmucnamhoc.aspx";
        public const string PAGEPATH_CATEGORY_ADDYEAR = "/modules/danh_muc/themnamhoc.aspx";
        public const string PAGEPATH_CATEGORY_MODIFYYEAR = "/modules/danh_muc/suanamhoc.aspx";

        public const string PAGEPATH_HANDLER_STUDENTPHOTOLOADER = "/modules/hoc_sinh/StudentPhotoLoadingHandler.ashx";
        public const string PAGEPATH_HANDLER_SCHOOLPHOTOLOADER = "/modules/supplier/PhotoLoadingHandler.ashx";

        // Class
        public const string PAGEPATH_CLASS_LIST = "/modules/lop_hoc/danhsachlop.aspx";
        public const string PAGEPATH_CLASS_DETAIL = "/modules/lop_hoc/chitietlophoc.aspx";
        public const string PAGEPATH_FORMERTEACHER_LIST = "/modules/lop_hoc/giaovienchunhiem.aspx";
        public const string PAGEPATH_FORMERTEACHER_MODIFY = "/modules/lop_hoc/suagiaovienchunhiem.aspx";

        // Teacher
        public const string PAGEPATH_TEACHER_LIST = "/modules/danh_muc/giao_vien/danhsachgiaovien.aspx";
        public const string PAGEPATH_TEACHER_EDIT = "/modules/danh_muc/giao_vien/suagiaovien.aspx";
        public const string PAGEPATH_TEACHER_DETAIL = "/modules/danh_muc/giao_vien/chitietgiaovien.aspx";

        public const string PAGEPATH_CATEGORY_LEARNINGRESULT_LIST = "/modules/danh_muc/danhmucdanhhieu.aspx";
        public const string PAGEPATH_CATEGORY_LEARNINGRESULT_ADD = "/modules/danh_muc/themdanhhieu.aspx";
        public const string PAGEPATH_CATEGORY_LEARNINGRESULT_MODIFY = "/modules/danh_muc/suadanhhieu.aspx";

        public const string PAGEPATH_STUDENT_CONDUCT_LIST = "/modules/hoc_sinh/hanhkiemhocsinh.aspx";
        public const string PAGEPATH_STUDENT_CONDUCT_MODIFY = "/modules/hoc_sinh/danhgiahanhkiemhocsinh.aspx";
        #endregion

        public const string QUERY_STUDENT = "std";
        public const string QUERY_PARENTSCOMMENT = "id";

        public const string VIEWSTATE_CLASSID = "ClassId";
        public const string VIEWSTATE_EMAIL = "Email";
        public const string VIEWSTATE_GENERATEDPASSWORD = "GeneratedPassword";
        public const string VIEWSTATE_MESSAGEID = "MessageId";
        public const string VIEWSTATE_PARENTSCOMMENTID = "ParentCommentId";
        public const string VIEWSTATE_PREV_PAGE = "PrevPage";
        public const string VIEWSTATE_PREVIOUSPAGE = "PrevPage";
        public const string VIEWSTATE_ROLE = "Role";
        public const string VIEWSTATE_SELECTED_CLASSID = "ClassID";
        public const string VIEWSTATE_SELECTED_CLASSNAME = "ClassName";
        public const string VIEWSTATE_SELECTED_CONFIRMSTATUS = "ConfirmStatus";
        public const string VIEWSTATE_SELECTED_DAYINWEEK = "DayInWeek";
        public const string VIEWSTATE_SELECTED_DISTRICTID = "District";
        public const string VIEWSTATE_SELECTED_FACULTYID = "FacultyId";
        public const string VIEWSTATE_SELECTED_FACULTYNAME = "FacultyName";
        public const string VIEWSTATE_SELECTED_FROMDATE = "FromDate";
        public const string VIEWSTATE_SELECTED_GRADEID = "GradeId";
        public const string VIEWSTATE_SELECTED_GRADENAME = "GradeName";
        public const string VIEWSTATE_SELECTED_LEARNINGRESULTID = "LearningResultId";
        public const string VIEWSTATE_SELECTED_LEARNINGRESULTNAME = "LearningResultName";
        public const string VIEWSTATE_SELECTED_MARKTYPEID = "MarkTypeID";
        public const string VIEWSTATE_SELECTED_MARKTYPENAME = "MarkTypeName";
        public const string VIEWSTATE_SELECTED_PROVINCEID = "Province";
        public const string VIEWSTATE_SELECTED_ROLEID = "RoleID";
        public const string VIEWSTATE_SELECTED_SCHEDULEID = "ScheduleId";
        public const string VIEWSTATE_SEARCHED_SCHOOLNAME = "SearchedSchoolName";
        public const string VIEWSTATE_SELECTED_STUDENTCODE = "StudentName";
        public const string VIEWSTATE_SELECTED_STUDENTID = "StudentId";
        public const string VIEWSTATE_SELECTED_STUDENTNAME = "StudenCode";
        public const string VIEWSTATE_SELECTED_SUBJECTID = "SubjectID";
        public const string VIEWSTATE_SELECTED_SUBJECTNAME = "SubjectName";
        public const string VIEWSTATE_SELECTED_TEACHERID = "TeacherID";
        public const string VIEWSTATE_SELECTED_TEACHERNAME = "TeacherName";
        public const string VIEWSTATE_SELECTED_TEACHINGPERIOD = "TeachingPeriod";
        public const string VIEWSTATE_SELECTED_TERMID = "TermID";
        public const string VIEWSTATE_SELECTED_TERMNAME = "TermName";
        public const string VIEWSTATE_SELECTED_TODATE = "ToDate";
        public const string VIEWSTATE_SELECTED_USERID = "UserID";
        public const string VIEWSTATE_SELECTED_YEARID = "YearId";
        public const string VIEWSTATE_SELECTED_YEARNAME = "YearName";
        public const string VIEWSTATE_SCHOOLID = "SchoolID";
        public const string VIEWSTATE_SCHOOLNAME = "SchoolName";
        public const string VIEWSTATE_STUDENTCLASS_ID = "StudentClassId";
        public const string VIEWSTATE_STUDENTID = "StudentId";
        public const string VIEWSTATE_USER = "User";
        public const string VIEWSTATE_USER_ID = "UserId";
        public const string VIEWSTATE_USER_ISDELETABLE = "IsDeletable";
        public const string VIEWSTATE_USER_ISTEACHER = "IsTeacher";

        public const string SESSION_PAGEPATH = "PagePath";
        public const string SESSION_MEMBERSHIP_STUDENT = "MembershipStudent";
        public const string SESSION_SELECTEDPARENTSFUNCTION = "SelectedParentFunction";
        public const string SESSION_STUDENT = "Student";        
        public const string SESSION_SUPPLIEDPARENTSAUTHORIZATIONS = "SuppliedParentsAuthorizations";
        public const string SESSION_SELECTEDPARENTSFUNCTIONS = "SelectedParentsFunctions";           
        public const string SESSION_SELECTED_YEAR = "Year";
        public const string SESSION_SELECTED_YEARNAME = "YearName";
        public const string SESSION_SELECTED_CLASS = "Class";
        public const string SESSION_SELECTED_TERM = "Term";
        public const string SESSION_SELECTED_TEACHINGPERIOD = "TeachingPeriod";
        public const string SESSION_SELECTED_DAYINWEEK = "DayInWeek";
        public const string SESSION_SELECTED_FACULTY = "Faculty";
        public const string SESSION_SELECTED_GRADE = "Grade";
        public const string SESSION_SELECTED_STUDENT = "Student";
        public const string SESSION_SELECTED_STUDENTCODE = "StudentName";
        public const string SESSION_SELECTED_STUDENTNAME = "StudenCode";
        public const string SESSION_SELECTED_PROVINCE = "Province";
        public const string SESSION_SELECTED_DISTRICT = "District";
        public const string SESSION_SELECTED_SCHOOLNAME = "SchoolName";
        public const string SESSION_SELECTED_SCHOOL = "School";
        public const string SESSION_SELECTED_MARKTYPES = "MarkTypes";
        public const string SESSION_SELECTED_MARKTYPE = "MarkType";
        public const string SESSION_SELECTED_SUBJECT = "Subject";
        public const string SESSION_SELECTED_FORMERTEACHER = "FormerTeacher";

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
        public const string SESSION_FILEIMPORTEDSTUDENTS = "FileImportedStudents";        
        public const string SESSION_CHANGEGRADE_STUDENTS = "ChangeGradeStudents";

        public const string SESSION_SCHEDULE = "ScheduleId";
        public const string SESSION_WEEKLYSCHEDULE = "Schedule";
        public const string SESSION_SCHEDULE_EVENT_ADD = "AddSchedule";
        public const string SESSION_SCHEDULE_EVENT_MODIFY = "ModifySchedule";

        public const string SESSION_PREVIOUSPAGE = "PrevPage";

        public const string SESSION_SELECTED_LEARNINGRESULT = "LearningResult";
        public const string SESSION_SELECTEDDETAILLEARNINGRESULTS = "SelectedDetailLearningResults";
        public const string SESSION_LOGEDIN_ROLES = "LogedRoles";
        public const string SESSION_LOGEDIN_USER = "LogedUser";
        public const string SESSION_LOGEDIN_USER_IS_FORMERTEACHER = "LogedUserIsFormerTeacher";
        public const string SESSION_LOGEDIN_USER_IS_SUBJECTTEACHER = "LogedUserIsSubjectTeacher";

        #region Session Teacher
        public const string SESSION_TEACHERID = "TeacherID";
        public const string SESSION_TEACHERNAME = "TeacherName";
        #endregion
        public const string CSSSTYLE_DISPLAY_NONE = "none";
        public const string CSSSTYLE_DISPLAY_BLOCK = "block";

        public const string IMAGESOURCE_BUTTON_ARRANGE = "~/Styles/buttons/button_arrange.png";
        public const string IMAGESOURCE_BUTTON_ARRANGE_DISABLE = "~/Styles/buttons/button_arrange_disable.png";
        public const string IMAGESOURCE_BUTTON_SAVE = "~/Styles/buttons/button_save.png";
        public const string IMAGESOURCE_BUTTON_SAVE_DISABLE = "~/Styles/buttons/button_save_disable.png";
        public const string IMAGESOURCE_BUTTON_SEARCH = "~/Styles/buttons/button_search.png";
        public const string IMAGESOURCE_BUTTON_SEARCH_DISABLE = "~/Styles/buttons/button_search_disable.png";
        public const string IMAGESOURCE_BUTTON_ADD = "~/Styles/buttons/button_add.png";
        public const string IMAGESOURCE_BUTTON_ADD_DISABLE = "~/Styles/buttons/button_add_disable.png";
        public const string IMAGESOURCE_BUTTON_IMPORT = "~/Styles/buttons/button_import.png";
        public const string IMAGESOURCE_BUTTON_IMPORT_DISABLE = "~/Styles/buttons/button_import_disable.png";
        public const string IMAGESOURCE_DELETE_DISABLE = "~/Styles/buttons/button_delete_disable.png";
        public const string IMAGESOURCE_BUTTON_MODIFY = "~/Styles/buttons/button_edit.png";        
        public const string IMAGESOURCE_BUTTON_MODIFY_DISABLED = "~/Styles/buttons/button_edit_disable.png";
        public const string IMAGESOURCE_BUTTON_DELETE = "~/Styles/buttons/button_delete.png"; 
        public const string IMAGESOURCE_BUTTON_DELETE_DISABLED = "~/Styles/buttons/button_delete_disable.png";
        public const string IMAGESOURCE_BUTTON_DANHGIA = "~/Styles/buttons/button_danhgia.png";
        public const string IMAGESOURCE_BUTTON_DANHGIA_DISABLE = "~/Styles/buttons/button_danhgia_disable.png"; 
        public const string IMAGESOURCE_BUTTON_EXPORT = "~/Styles/buttons/button_export.png";
        public const string IMAGESOURCE_BUTTON_FEEDBACK = "~/Styles/buttons/button_feedback.png";
        public const string IMAGESOURCE_BUTTON_FEEDBACK_DISABLED = "~/Styles/buttons/button_feedback_disable.png";
        public const string IMAGESOURCE_BUTTON_EXPORT_DISABLED = "~/Styles/buttons/button_export_disable.png";
        public const string IMAGESOURCE_BUTTON_NEXT = "~/Styles/buttons/button_next_2.png";
        public const string IMAGESOURCE_BUTTON_NEXT_DISABLED = "~/Styles/buttons/button_next_2_disable.png";

        public const string RESOURCE_SEARCH_NOINFO = "LblSearchResultText";
        public const string RESOURCE_SEARCH_NOMATCH = "LblSearchResultSearchText";

        public const string FILENAME_MAINRESOURCE = "MainResource";

        public const string ROLE = "Role";
        public const string TEACHER = "Teacher";
        public const string UNDERSCORE = "_";
        public const char UNDERSCORE_CHAR = '_';
        public const string USERPARENT_PREFIX = "PH";
        public const string COMMAND_EDIT = "CommandEdit";

        public const string STRING_BLANK = "";
        public const string STRING_ZERO = "0";
        public const string STRING_MALE = "Nam";
        public const string STRING_FEMALE = "Nữ";
        public const string STRING_UNDEFINED = "(Không xác định)";
    }
}