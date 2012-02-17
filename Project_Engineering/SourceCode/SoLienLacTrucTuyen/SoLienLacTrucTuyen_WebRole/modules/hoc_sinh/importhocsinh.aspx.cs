using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using System.IO;
using System.Data.OleDb;
using System.Net;
using System.Web.Security;
using EContactBook.BusinessEntity;
using System.Text;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ImportStudentPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        string filename = string.Empty;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            // Dont remove this code
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                return;
            }
            
            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                lbError.Text = string.Empty;
                BindDropDownLists();
                if (ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] == null || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
                {
                    Response.Redirect(AppConstant.PAGEPATH_STUDENT_LIST);
                }
                BtnSave.Enabled = false;
                BtnSave.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SAVE_DISABLE;
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptHocSinh_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }

        protected void LkBtnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string strURL = "~/upload/ListStudent.xls";
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "Attachment;Filename=\"" + Server.MapPath(strURL) + "\"");
                byte[] data = req.DownloadData(Server.MapPath(strURL));
                response.BinaryWrite(data);
                response.End();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            UserBL userBL = new UserBL(UserSchool);
            List<TabularImportedStudent> importedStudents;
            string strGeneratedPassword;
            string strUserName;
            aspnet_User userParents = null;

            if(CheckSessionKey(AppConstant.SESSION_IMPORTEDSTUDENTS))
            {
                importedStudents = (List<TabularImportedStudent>)GetSession(AppConstant.SESSION_IMPORTEDSTUDENTS);
                Class_Class Class = new Class_Class();
                Class.ClassId = Int32.Parse(this.DdlLopHoc.SelectedValue);
                
                foreach (TabularImportedStudent importedStudent in importedStudents)
                {
                    studentBL.InsertStudent(Class, importedStudent.StudentCode, importedStudent.FullName, importedStudent.Gender,
                        importedStudent.DateOfBirth, importedStudent.BirthPlace, importedStudent.Address,
                        importedStudent.Phone, null, importedStudent.FatherName, importedStudent.FatherJob, importedStudent.FatherDateOfBirth,
                        importedStudent.MotherName, importedStudent.MotherJob, importedStudent.MotherDateOfBirth,
                        importedStudent.PatronName, importedStudent.PatronJob, importedStudent.PatronDateOfBirth);

                    // create new user parents
                    //strGeneratedPassword = Membership.GeneratePassword(Membership.Provider.MinRequiredPasswordLength,
                    //    Membership.Provider.MinRequiredNonAlphanumericCharacters);
                    strGeneratedPassword = "1qazxsw@";
                    strUserName = UserSchool.SchoolId.ToString() + "_PH" + importedStudent.StudentCode;
                    Membership.CreateUser(strUserName, strGeneratedPassword);                    
                    userParents = new aspnet_User();
                    userParents.UserName = strUserName;
                    userBL.CreateUserParents(userParents);
                }           
            }

            RptHocSinh.DataSource = null;
            RptHocSinh.DataBind();
            RemoveSession(AppConstant.SESSION_IMPORTEDSTUDENTS);
            LblImportSuccess.Visible = true;
            BtnSave.Enabled = false;
            BtnSave.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SAVE_DISABLE;

            if (CheckSessionKey(AppConstant.SESSION_FILEIMPORTEDSTUDENTS))
            {
                string file = (string)GetSession(AppConstant.SESSION_FILEIMPORTEDSTUDENTS);
                //File.Delete(file);
            }
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_STUDENT_LIST);
        }
        
        protected void BtnUpload_Click(object sender, ImageClickEventArgs e)
        {
            System.Threading.Thread.Sleep(5000);
            filename = Path.GetFileName(FileUpload1.FileName);
            if (filename == string.Empty || (!filename.Contains(".xls") && !filename.Contains(".xlsx")))
            {
                lbError.Text = "Vui lòng chọn file định dang Microsoft Excel để tiến hành import!";
                return;
            }

            try
            {
                FileUpload1.SaveAs(Server.MapPath("~/upload/template/") + filename);
            }
            catch (Exception ex)
            {
            }

            filename = Server.MapPath("~/upload/template/") + filename;
            String connString = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filename + ";" + "Extended Properties='Excel 8.0;HDR=No'";
            OleDbConnection oledbConn = new OleDbConnection(connString);

            //Check if the Sheet Exists
            oledbConn.Open();
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [Sheet1$]", oledbConn);
            DataSet ds = new DataSet();
            if (da != null)
            {
                da.Fill(ds);
            }

            DateTime dtStudentDateOfBirth;
            DateTime dtFatherDateOfBirth;
            DateTime dtMotherDateOfBirth;
            DateTime dtPatronDateOfBirth;
            bool bImportError = false;
            bool bLackOfParents = true;
            StringBuilder strBuilder = new StringBuilder();
            List<TabularImportedStudent> tabularImportedStudents = new List<TabularImportedStudent>();
            TabularImportedStudent tabularImportedStudent = null;

            // Loop in imported student informations
            for (int i = 2; i < ds.Tables[0].Rows.Count; i++)
            {
                tabularImportedStudent = new TabularImportedStudent();

                // Mã học sinh (column 0)
                if (CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][0].ToString()))
                {
                    strBuilder.Append("Mã học sinh không được để trống <br/>");
                    tabularImportedStudent.StudentCode = "(trống)";
                }
                else if (studentBL.StudentCodeExists(ds.Tables[0].Rows[i][0].ToString()))
                {
                    tabularImportedStudent.StudentCode = ds.Tables[0].Rows[i][0].ToString();
                    strBuilder.Append("Mã học sinh đã tồn tại <br/>");
                }
                else
                {
                    // valid studentCode
                    tabularImportedStudent.StudentCode = ds.Tables[0].Rows[i][0].ToString();
                }

                // Họ tên học sinh (column 1)
                if (CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][1].ToString()))
                {
                    strBuilder.Append("Họ tên học sinh không được để trống <br/>");
                    tabularImportedStudent.FullName = "(trống)";
                }
                else
                {
                    tabularImportedStudent.FullName = ds.Tables[0].Rows[i][1].ToString();
                }

                // Giới tính (column 2)
                if (CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][2].ToString()))
                {
                    strBuilder.Append("Giới tính của học sinh không được để trống <br/>");
                    tabularImportedStudent.StringGender = "(trống)";
                }
                else if (ds.Tables[0].Rows[i][2].ToString() != "Nam" && ds.Tables[0].Rows[i][2].ToString() != "Nữ")
                {
                    strBuilder.Append("Giới tính của học sinh không hợp lệ <br/>");
                    tabularImportedStudent.StringGender = "(không hợp lệ)";
                }
                else
                {
                    tabularImportedStudent.StringGender = ds.Tables[0].Rows[i][2].ToString();
                    tabularImportedStudent.Gender = tabularImportedStudent.StringGender == AppConstant.STRING_MALE ? true : false;
                }

                // Ngày sinh của học sinh (column 3)
                if (CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][3].ToString()))
                {
                    strBuilder.Append("Ngày sinh của học sinh không được để trống <br/>");
                    tabularImportedStudent.StringDateOfBirth = "(trống)";
                }
                else if (!DateTime.TryParse(ds.Tables[0].Rows[i][3].ToString(), out dtStudentDateOfBirth))
                {
                    strBuilder.Append("Ngày sinh của học sinh không hợp lệ <br/>");
                    tabularImportedStudent.StringDateOfBirth = "(không hợp lệ)";
                }
                else
                {
                    tabularImportedStudent.DateOfBirth = dtStudentDateOfBirth;
                    tabularImportedStudent.StringDateOfBirth = dtStudentDateOfBirth.ToShortDateString();
                }

                // Nơi sinh (column 4)
                tabularImportedStudent.BirthPlace = ds.Tables[0].Rows[i][4].ToString();

                // Địa chỉ liên lạc (column 5)
                if (CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][5].ToString()))
                {
                    strBuilder.Append("Địa chỉ liên lạc không được để trống <br/>");
                }
                else
                {                    
                    tabularImportedStudent.Address = ds.Tables[0].Rows[i][5].ToString();
                }

                // Điện thoại (column 6)
                tabularImportedStudent.Phone = ds.Tables[0].Rows[i][6].ToString();

                // Họ tên bố (column 7)
                // Ngày sinh bố (column 8)
                // Nghề nghiệp bố (column 9)
                if (!CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][7].ToString())
                    && !CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][8].ToString())
                    && !CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][9].ToString()))
                {
                    bLackOfParents = false;
                    tabularImportedStudent.FatherName = ds.Tables[0].Rows[i][7].ToString();
                    tabularImportedStudent.FatherJob = ds.Tables[0].Rows[i][9].ToString();
                    if (!DateTime.TryParse(ds.Tables[0].Rows[i][8].ToString(), out dtFatherDateOfBirth))
                    {
                        strBuilder.Append("Ngày sinh của bố không hợp lệ <br/>");
                        tabularImportedStudent.FatherDateOfBirth = null;
                    }
                    else
                    {
                        tabularImportedStudent.FatherDateOfBirth = dtFatherDateOfBirth;
                        tabularImportedStudent.StringFatherDateOfBirth = dtFatherDateOfBirth.ToShortDateString();
                    }
                }

                if (!CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][10].ToString())
                    && !CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][11].ToString())
                    && !CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][12].ToString()))
                {
                    bLackOfParents = false;
                    tabularImportedStudent.MotherName = ds.Tables[0].Rows[i][10].ToString();
                    tabularImportedStudent.MotherJob = ds.Tables[0].Rows[i][12].ToString();
                    
                    if (!DateTime.TryParse(ds.Tables[0].Rows[i][11].ToString(), out dtMotherDateOfBirth))
                    {
                        strBuilder.Append("Ngày sinh của mẹ không hợp lệ <br/>");
                        tabularImportedStudent.MotherDateOfBirth = null;
                    }
                    else
                    {
                        tabularImportedStudent.MotherDateOfBirth = dtMotherDateOfBirth;
                        tabularImportedStudent.StringMotherDateOfBirth = dtMotherDateOfBirth.ToShortDateString();
                    }
                }

                if (!CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][13].ToString())
                    && !CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][14].ToString())
                    && !CheckUntils.IsNullOrBlank(ds.Tables[0].Rows[i][15].ToString()))
                {
                    bLackOfParents = false;
                    tabularImportedStudent.PatronName = ds.Tables[0].Rows[i][13].ToString();
                    tabularImportedStudent.PatronJob = ds.Tables[0].Rows[i][15].ToString();
                    
                    if (!DateTime.TryParse(ds.Tables[0].Rows[i][14].ToString(), out dtPatronDateOfBirth))
                    {
                        strBuilder.Append("Ngày sinh của người đỡ đầu không hợp lệ <br/>");
                        tabularImportedStudent.PatronDateOfBirth = null;
                    }
                    else
                    {
                        tabularImportedStudent.PatronDateOfBirth = dtPatronDateOfBirth;
                        tabularImportedStudent.StringPatronDateOfBirth = dtPatronDateOfBirth.ToShortDateString();
                    }
                }

                if(bLackOfParents)
                {
                    strBuilder.Append("Phải có ít nhất thông tin đầy đủ của bố hoặc mẹ hoặc người đỡ đầu <br/>");
                }

                DataRow[] dr = ds.Tables[0].Select(ds.Tables[0].Columns[0].ColumnName + "='" + ds.Tables[0].Rows[i][0].ToString() + "'");
                if (dr.Length > 1)
                {
                    strBuilder.Append("Mã số của các học sinh không được trùng nhau <br/>");
                }

                
                tabularImportedStudent.Error = strBuilder.ToString();
                if (CheckUntils.IsNullOrBlank(tabularImportedStudent.Error))
                {
                    tabularImportedStudent.ImportStatus = "Thông tin hợp lệ";
                }
                else
                {
                    tabularImportedStudent.ImportStatus = "Lỗi:<br/>";
                    bImportError = true;
                }
                
                tabularImportedStudents.Add(tabularImportedStudent);

                strBuilder.Clear();
                bLackOfParents = true;
            }

            RptHocSinh.DataSource = tabularImportedStudents;
            RptHocSinh.DataBind();

            if (bImportError)
            {
                BtnSave.Enabled = false;
                BtnSave.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SAVE_DISABLE;
                LblImportError.Visible = true;
            }
            else
            {
                BtnSave.Enabled = true;
                BtnSave.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_SAVE;
                LblImportError.Visible = false;
                AddSession(AppConstant.SESSION_IMPORTEDSTUDENTS, tabularImportedStudents);
                AddSession(AppConstant.SESSION_FILEIMPORTEDSTUDENTS, filename);
            }            
        }
        #endregion

        #region Methods
        private void BindDropDownLists()
        {
            FillYear();
            BindDDLFaculties();
            BindDDLGrades();
            BindDDLClasses();
        }

        private void FillYear()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            Configuration_Year lastedYear = systemConfigBL.GetLastedYear();
            if (lastedYear != null)
            {
                LblYear.Text = lastedYear.YearName;
                ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] = lastedYear.YearId;
            }
        }

        private void BindDDLGrades()
        {
            GradeBL gradeBL = new GradeBL(UserSchool);
            List<Category_Grade> grades = gradeBL.GetListGrades();
            DdlKhoiLop.DataSource = grades;
            DdlKhoiLop.DataValueField = "GradeId";
            DdlKhoiLop.DataTextField = "GradeName";
            DdlKhoiLop.DataBind();
        }

        private void BindDDLFaculties()
        {
            FacultyBL facultyBL = new FacultyBL(UserSchool);
            List<Category_Faculty> faculties = facultyBL.GetFaculties();
            DdlNganh.DataSource = faculties;
            DdlNganh.DataValueField = "FacultyId";
            DdlNganh.DataTextField = "FacultyName";
            DdlNganh.DataBind();
        }

        private void BindDDLClasses()
        {
            if (ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID] == null)
            {
                return;
            }
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            ClassBL lopHocBL = new ClassBL(UserSchool);

            year = new Configuration_Year();
            year.YearId = (int)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARID];

            try
            {
                if (DdlNganh.SelectedIndex >= 0)
                {
                    faculty = new Category_Faculty();
                    faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                }
            }
            catch (Exception) { }

            try
            {
                if (DdlKhoiLop.SelectedIndex >= 0)
                {
                    grade = new Category_Grade();
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }
            catch (Exception) { }

            List<Class_Class> lstLop = lopHocBL.GetClasses(LogedInUser, IsFormerTeacher, IsSubjectTeacher, year, faculty, grade, null);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();

        }
        #endregion
    }
}