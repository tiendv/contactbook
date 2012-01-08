﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.BusinessEntity;
using EContactBook.DataAccess;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class AddStudentMarkPage : BaseContentPage
    {
        #region Fields
        private StudyingResultBL studyingResultBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
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

            studyingResultBL = new StudyingResultBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                BindRptMarkTypes();
                BindRptStudentMarks();
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
            BindDDLMarkTypes();
        }

        protected void DdlHocKy_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLMonHoc();
        }

        protected void DdlLopHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLMonHoc();
        }
        #endregion

        #region Methods
        private void BindDropDownLists()
        {
            BindDDLNamHoc();
            BindDDLHocKy();
            BindDDLNganhHoc();
            BindDDLGrades();
            BindDDLClasses();
            BindDDLMonHoc();
            BindDDLMarkTypes();
        }

        private void BindDDLNamHoc()
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

        private void BindDDLHocKy()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Term> lstHocKy = systemConfigBL.GetListTerms();
            DdlHocKy.DataSource = lstHocKy;
            DdlHocKy.DataValueField = "TermId";
            DdlHocKy.DataTextField = "TermName";
            DdlHocKy.DataBind();
        }

        private void BindDDLNganhHoc()
        {
            FacultyBL facultyBL = new FacultyBL(UserSchool);
            List<Category_Faculty> faculties = facultyBL.GetFaculties();
            DdlNganh.DataSource = faculties;
            DdlNganh.DataValueField = "FacultyId";
            DdlNganh.DataTextField = "FacultyName";
            DdlNganh.DataBind();
            if (faculties.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "0"));
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

        private void BindDDLMarkTypes()
        {
            if (DdlKhoiLop.Items.Count != 0)
            {
                Category_Grade grade = new Category_Grade();
                grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);

                MarkTypeBL markTypeBL = new MarkTypeBL(UserSchool);
                List<Category_MarkType> markTypes = markTypeBL.GetListMarkTypes(grade);
                DdlLoaiDiem.DataSource = markTypes;
                DdlLoaiDiem.DataValueField = "MarkTypeName";
                DdlLoaiDiem.DataTextField = "MarkTypeName";
                DdlLoaiDiem.DataBind();

                if (markTypes.Count > 1)
                {
                    DdlLoaiDiem.Items.Insert(0, new ListItem("Tất cả", ""));
                }
            }
        }

        private void BindDDLClasses()
        {
            Configuration_Year year = null;
            Category_Faculty faculty = null;
            Category_Grade grade = null;

            if (DdlNamHoc.Items.Count == 0 || DdlNganh.Items.Count == 0 || DdlKhoiLop.Items.Count == 0)
            {
                //BtnSearch.ImageUrl = "~/Styles/Images/button_search_with_text_disable.png";
                //BtnSearch.Enabled = false;

                //BtnAdd.ImageUrl = "~/Styles/Images/button_add_with_text_disable.png";
                //BtnAdd.Enabled = false;

                //PnlPopupConfirmDelete.Visible = false;
                //RptHocSinh.Visible = false;
                //LblSearchResult.Visible = true;
                //LblSearchResult.Text = "Chưa có thông tin HocSinh";

                //MainDataPager.ItemCount = 0;
                //MainDataPager.Visible = false;

                return;
            }

            year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);

            try
            {
                if (DdlNganh.SelectedIndex > 0)
                {
                    faculty = new Category_Faculty();
                    faculty.FacultyId = Int32.Parse(DdlNganh.SelectedValue);
                }
            }
            catch (Exception) { }

            try
            {
                if (DdlKhoiLop.SelectedIndex > 0)
                {
                    grade = new Category_Grade();
                    grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                }
            }
            catch (Exception) { }

            ClassBL lopHocBL = new ClassBL(UserSchool);
            List<Class_Class> lstLop = lopHocBL.GetListClasses(year, faculty, grade);
            DdlLopHoc.DataSource = lstLop;
            DdlLopHoc.DataValueField = "ClassId";
            DdlLopHoc.DataTextField = "ClassName";
            DdlLopHoc.DataBind();
        }

        private void BindDDLMonHoc()
        {
            Class_Class Class = null;
            Configuration_Term term = null;
            ScheduleBL scheduleBL = new ScheduleBL(UserSchool);

            if (DdlLopHoc.Items.Count == 0)
            {
                DdlMonHoc.DataSource = null;
            }
            else
            {
                Class = new Class_Class();
                Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
                term = new Configuration_Term();
                term.TermId = Int32.Parse(DdlHocKy.SelectedValue);

                List<Category_Subject> scheduledSubjects = scheduleBL.GetScheduledSubjects(Class, term);
                DdlMonHoc.DataSource = scheduledSubjects;
                DdlMonHoc.DataValueField = "SubjectId";
                DdlMonHoc.DataTextField = "SubjectName";
                DdlMonHoc.DataBind();
            }
        }

        private void BindRptMarkTypes()
        {
            if (DdlKhoiLop.Items.Count != 0)
            {
                Category_Grade grade = new Category_Grade();
                grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);
                MarkTypeBL markTypeBL = new MarkTypeBL(UserSchool);
                List<Category_MarkType> markTypes = new List<Category_MarkType>();

                if (DdlLoaiDiem.Items.Count != 0)
                {
                    if (DdlLoaiDiem.SelectedIndex == 0)
                    {
                        markTypes = markTypeBL.GetListMarkTypes(grade);
                    }
                    else
                    {
                        string markTypeName = DdlLoaiDiem.SelectedValue;
                        markTypes.Add(markTypeBL.GetMarkType(grade, markTypeName));
                    }
                }

                this.RptLoaiDiem.DataSource = markTypes;
                this.RptLoaiDiem.DataBind();
            }
        }

        private void BindRptStudentMarks()
        {
            // declare variables
            Class_Class Class = null;
            MarkTypeBL markTypeBL = new MarkTypeBL(UserSchool);
            List<Category_MarkType> markTypes = new List<Category_MarkType>();            
            List<TabularStudentMark> tabularStudentMarks = new List<TabularStudentMark>();
            StudentBL studentBL = new StudentBL(UserSchool);

            // case: there is no Class or schedule subject or marktype
            if (DdlLopHoc.Items.Count == 0 || DdlMonHoc.Items.Count == 0 || DdlLoaiDiem.Items.Count == 0)
            {
                // do not display 
                ProcDisplayGUI(false);
                return;
            }

            // init object against user selections
            Class = new Class_Class();
            Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);            

            List<Student_Student> students = studentBL.GetStudents(Class);

            // bind to repeater and datapager
            this.RptDiemMonHoc.DataSource = students;
            this.RptDiemMonHoc.DataBind();
            MainDataPager.ItemCount = students.Count;

            // display information
            bool bDisplayData = (students.Count != 0) ? true : false;
            ProcDisplayGUI(bDisplayData);
        }

        private void ProcDisplayGUI(bool bDisplayData)
        {
            RptDiemMonHoc.Visible = bDisplayData;
            MainDataPager.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            RptLoaiDiem.Visible = bDisplayData;
            tdSTT.Visible = bDisplayData;
            tdMaHocSinh.Visible = bDisplayData;
            tdHoTenHocSinh.Visible = bDisplayData;

            BtnSave.Visible = bDisplayData;
            BtnCancel.Visible = bDisplayData;
        }

        private void RedirectToPrevPage()
        {
            Response.Redirect(AppConstant.PAGEPATH_STUDENT_MARK);
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            BindRptMarkTypes();
            BindRptStudentMarks();
        }

        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            Dictionary<Student_Student, List<MarkValueAndTypePair>> dicEnteredStudentMarks = new Dictionary<Student_Student, List<MarkValueAndTypePair>>();
            List<MarkValueAndTypePair> markValueAndTypePairs;
            Student_Student student = null;            
            Category_MarkType markType = null;
            HiddenField hdfMarkTypeId = null;
            TextBox txtDiems = null;
            HiddenField hdfStudentId = null;            
            Class_Class Class = new Class_Class();
            Configuration_Term term = new Configuration_Term();
            Category_Subject subject = new Category_Subject();
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            term.TermId = Int32.Parse(DdlHocKy.SelectedValue);
            Class.ClassId = Int32.Parse(DdlLopHoc.SelectedValue);
            subject.SubjectId = Int32.Parse(DdlMonHoc.SelectedValue);

            foreach (RepeaterItem rptItemStudentMark in RptDiemMonHoc.Items)
            {
                if (rptItemStudentMark.ItemType == ListItemType.Item || rptItemStudentMark.ItemType == ListItemType.AlternatingItem)
                {
                    // get student object
                    student = new Student_Student();
                    hdfStudentId = (HiddenField)rptItemStudentMark.FindControl("HdfMaHocSinh");
                    student.StudentId = Int32.Parse(hdfStudentId.Value);

                    markValueAndTypePairs = new List<MarkValueAndTypePair>();
                    Repeater rptStudentSpecificDetailMark = (Repeater)rptItemStudentMark.FindControl("RptDiemTheoLoaiDiem");
                    foreach (RepeaterItem rptItemDiem in rptStudentSpecificDetailMark.Items)
                    {
                        if (rptItemDiem.ItemType == ListItemType.Item || rptItemDiem.ItemType == ListItemType.AlternatingItem)
                        {
                            // get markType object
                            hdfMarkTypeId = (HiddenField)rptItemDiem.FindControl("HdfMarkTypeId");
                            markType = new Category_MarkType();
                            markType.MarkTypeId = Int32.Parse(hdfMarkTypeId.Value);

                            // get mark input
                            txtDiems = (TextBox)rptItemDiem.FindControl("TxtDiems");
                            StudyingResultBL.ADDINGMARKERROR error = studyingResultBL.ValidateMark(student, year, term, subject, markType, txtDiems.Text.Trim());
                            if (StudyingResultBL.ADDINGMARKERROR.NOERROR == error) // in case mark is valid
                            {
                                // get mark values
                                if (!CheckUntils.IsNullOrBlank(txtDiems.Text))
                                {
                                    string[] strMarks = txtDiems.Text.Trim().Split(new char[] { ',' });
                                    foreach (string strMark in strMarks)
                                    {
                                        double dMark = double.Parse(strMark.Trim());
                                        markValueAndTypePairs.Add(new MarkValueAndTypePair
                                        {
                                            MarkTypeId = Int32.Parse(hdfMarkTypeId.Value),
                                            GiaTri = dMark
                                        });
                                    }
                                }
                                else
                                {
                                    markValueAndTypePairs.Add(new MarkValueAndTypePair
                                    {
                                        MarkTypeId = Int32.Parse(hdfMarkTypeId.Value),
                                        GiaTri = -1
                                    });
                                }
                            }
                            else // in case mark is not valid
                            {
                                CustomValidator diemsValidator = (CustomValidator)rptItemDiem.FindControl("DiemsValidator");
                                diemsValidator.IsValid = false;
                                switch (error)
                                {
                                    case StudyingResultBL.ADDINGMARKERROR.NOTANUMBER:
                                        {
                                            diemsValidator.ErrorMessage = "Điểm phải là giá trị số";
                                            break;
                                        }
                                    case StudyingResultBL.ADDINGMARKERROR.EXCEEDVALUETEN:
                                        {
                                            diemsValidator.ErrorMessage = "Giá trị điểm không được vượt quá 10";
                                            break;
                                        }
                                    case StudyingResultBL.ADDINGMARKERROR.EXCEEDQUANTITY:
                                        {
                                            diemsValidator.ErrorMessage = "Số lượng điểm không được vượt quá số lượng qui định";
                                            break;
                                        }
                                    default:
                                        break;
                                }
                                return;
                            }
                        }
                    }

                    

                    dicEnteredStudentMarks.Add(student, markValueAndTypePairs);
                }
            }

            foreach (KeyValuePair<Student_Student, List<MarkValueAndTypePair>> pair in dicEnteredStudentMarks)
            {
                studyingResultBL.AddDetailedMark(pair.Key, Class, term, subject, pair.Value);
            }

            RedirectToPrevPage();
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            RedirectToPrevPage();
        }
        #endregion

        #region Repeater event handlers
        protected void RptDiemMonHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (DdlKhoiLop.Items.Count != 0)
            {
                Category_Grade grade = new Category_Grade();
                grade.GradeId = Int32.Parse(DdlKhoiLop.SelectedValue);

                MarkTypeBL markTypeBL = new MarkTypeBL(UserSchool);
                List<Category_MarkType> markTypes = new List<Category_MarkType>();
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (DdlLoaiDiem.SelectedIndex == 0)
                    {
                        markTypes = markTypeBL.GetListMarkTypes(grade);
                    }
                    else
                    {
                        string markTypeName = DdlLoaiDiem.SelectedValue;
                        markTypes.Add(markTypeBL.GetMarkType(grade, markTypeName));
                    }

                    Repeater rptMarkTypeBasedMarks = (Repeater)e.Item.FindControl("RptDiemTheoLoaiDiem");
                    rptMarkTypeBasedMarks.DataSource = markTypes;
                    rptMarkTypeBasedMarks.DataBind();
                }
            }
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currnetPageIndx;
            BindRptStudentMarks();
        }
        #endregion
    }
}