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

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ImportStudentPage : BaseContentPage
    {
        #region Fields
        private StudentBL studentBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            //base.Page_Load(sender, e);
            
            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            lbError.Text = string.Empty;
            UserSchool = (School_School)Session[AppConstant.SCHOOL];
            UserBL userBL = new UserBL(UserSchool);
            RoleBL roleBL = new RoleBL(UserSchool);
            AuthorizationBL authorizationBL = new AuthorizationBL(UserSchool);

            string pageUrl = Page.Request.Path;
            List<aspnet_Role> roles = userBL.GetRoles(User.Identity.Name);
            aspnet_Role role = roles[0];

            Site masterPage = (Site)Page.Master;
            masterPage.UserName = User.Identity.Name;
            masterPage.UserRole = role;
            masterPage.PageUrl = pageUrl;
            masterPage.UserSchool = UserSchool;

            studentBL = new StudentBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
            }
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNamHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }

        protected void DdlNganh_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }

        protected void DdlKhoiLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLClasses();
        }
        protected void lbtDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string strURL = "Book2.xls";
                WebClient req = new WebClient();
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + Server.MapPath(strURL) + "\"");
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
            #region
            string filename = Path.GetFileName(FileUpload1.FileName);
            FileUpload1.SaveAs(Server.MapPath("~/upload/") + filename);
            filename = Server.MapPath("~/upload/") + filename;
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
            // Validate data : follow this task            
            for (int i = 2; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i][0].ToString() == string.Empty
                    || ds.Tables[0].Rows[i][1].ToString() == string.Empty)
                {
                    lbError.Text = "Mã số học sinh và tên học sinh ko được để trống !";
                    return;
                }
                else if (ds.Tables[0].Rows[i][3].ToString() == string.Empty)
                {
                    lbError.Text = "Ngày sinh của học sinh "+ ds.Tables[0].Rows[i][0].ToString()+" không được để trống !";
                    return;                
                }
                else if (ds.Tables[0].Rows[i][2].ToString() == string.Empty)
                {
                    lbError.Text = "Giới tính của học sinh " + ds.Tables[0].Rows[i][0].ToString() + " không được để trống !";
                    return;
                }
                else if ((ds.Tables[0].Rows[i][7].ToString() == string.Empty
                        || ds.Tables[0].Rows[i][8].ToString() == string.Empty
                        || ds.Tables[0].Rows[i][9].ToString() == string.Empty)
                     && (ds.Tables[0].Rows[i][10].ToString() == string.Empty
                        || ds.Tables[0].Rows[i][11].ToString() == string.Empty
                        || ds.Tables[0].Rows[i][12].ToString() == string.Empty)
                     && (ds.Tables[0].Rows[i][13].ToString() == string.Empty
                        || ds.Tables[0].Rows[i][14].ToString() == string.Empty
                        || ds.Tables[0].Rows[i][15].ToString() == string.Empty))
                {
                    lbError.Text = "Học sinh " + ds.Tables[0].Rows[i][0].ToString() + " phải có ít nhất thông tin đầy đủ của cha/mẹ hoặc người đỡ đầu !";
                    return;                
                }
                try
                {
                    DateTime.Parse(ds.Tables[0].Rows[i][3].ToString());
                    if (ds.Tables[0].Rows[i][8].ToString() != string.Empty)
                        DateTime.Parse(ds.Tables[0].Rows[i][8].ToString());
                    if (ds.Tables[0].Rows[i][11].ToString() != string.Empty)
                        DateTime.Parse(ds.Tables[0].Rows[i][11].ToString());
                    if (ds.Tables[0].Rows[i][14].ToString() != string.Empty)
                        DateTime.Parse(ds.Tables[0].Rows[i][14].ToString());
                }
                catch (Exception ex)
                {
                    lbError.Text = "Dữ liệu ngày tháng của học sinh "+ds.Tables[0].Rows[i][0].ToString()+" không đúng định dạng";
                    return;
                }

                DataRow[] dr = ds.Tables[0].Select(ds.Tables[0].Columns[0].ColumnName + "='" + ds.Tables[0].Rows[i][0].ToString() + "'");
                if (dr.Length > 1)
                {
                    lbError.Text = "Mã số của các học sinh không được trùng nhau !";
                    return;                
                }
            }
            // Insert Data
            #region
            // Thu tu cac cot trong excel           
            // 0.[StudentCode]
            // 1.[FullName]            
            // 2.[Gender]
            // 3.[StudentBirthday]
            // 4.[Birthplace]
            // 5.[Address]
            // 6.[ContactPhone]
            // 7.[Photo]
            // 8.[FatherName]
            // 9.[FatherBirthday]
            // 10.[FatherJob]
            // 11.[MotherName]
            // 12.[MotherBirthday]
            // 13.[MotherJob]
            // 14.[PatronName]
            // 15.[PatronBirthday]
            // 16.[PatronJob]      
            #endregion
            Class_Class Class = new Class_Class();
            Class.ClassId = Int32.Parse(this.DdlLopHoc.SelectedValue);

            for (int i = 2; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i][0].ToString() != string.Empty)
                {

                    studentBL.InsertStudent(Class,
                                            ds.Tables[0].Rows[i][0].ToString(),
                                            ds.Tables[0].Rows[i][1].ToString(),
                                            ds.Tables[0].Rows[i][2].ToString()=="Nam"?true:false,
                                            ds.Tables[0].Rows[i][3].ToString()== string.Empty?DateTime.MinValue:DateTime.Parse(ds.Tables[0].Rows[i][3].ToString()),
                                            ds.Tables[0].Rows[i][4].ToString(),
                                            ds.Tables[0].Rows[i][5].ToString(),
                                            ds.Tables[0].Rows[i][6].ToString(),
                                            ds.Tables[0].Rows[i][7].ToString(),
                                            ds.Tables[0].Rows[i][9].ToString(),
                                            ds.Tables[0].Rows[i][8].ToString()== string.Empty?DateTime.MinValue:DateTime.Parse(ds.Tables[0].Rows[i][8].ToString()),
                                            ds.Tables[0].Rows[i][10].ToString(),
                                            ds.Tables[0].Rows[i][12].ToString(),
                                            DateTime.Parse(ds.Tables[0].Rows[i][11].ToString()),
                                            ds.Tables[0].Rows[i][13].ToString(),
                                            ds.Tables[0].Rows[i][15].ToString(),
                                            ds.Tables[0].Rows[i][14].ToString() == string.Empty?DateTime.MinValue:DateTime.Parse(ds.Tables[0].Rows[i][14].ToString())
                                            );
                }
            }
            
            #endregion
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(AppConstant.PAGEPATH_STUDENTS);
        }

        #endregion

        #region Methods
        private void BindDropDownLists()
        {
            BindDDLYears();
            BindDDLFaculties();
            BindDDLGrades();
            BindDDLClasses();
        }

        private void BindDDLYears()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> years = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();

            if (DdlNamHoc.Items.Count != 0)
            {
                SystemConfigBL cauHinhBL = new SystemConfigBL(UserSchool);
                DdlNamHoc.SelectedValue = cauHinhBL.GetLastedYear().ToString();
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
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            ClassBL lopHocBL = new ClassBL(UserSchool);

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

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

            List<Class_Class> lstLop = lopHocBL.GetListClasses(year, faculty, grade);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();

        }

        private DateTime? ToDateTime(string str)
        {
            if (str != "")
            {
                string[] arrDateTime = str.Split('/');
                return new DateTime(Int32.Parse(arrDateTime[2]),
                    Int32.Parse(arrDateTime[1]), Int32.Parse(arrDateTime[0]));
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}