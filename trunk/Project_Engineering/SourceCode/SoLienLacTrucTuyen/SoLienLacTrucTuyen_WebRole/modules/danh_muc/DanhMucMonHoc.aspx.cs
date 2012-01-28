using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;
using AjaxControlToolkit;
using SoLienLacTrucTuyen;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class SubjectsPage : BaseContentPage
    {
        #region Fields
        private SubjectBL subjectBL;
        private bool isSearch;
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

            subjectBL = new SubjectBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownLists();
                isSearch = false;
                BindRepeater();
            }

            ProcPermissions();
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;
            isSearch = true;
            BindRepeater();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            string subjectName = this.TxtSubjectNameThem.Text.Trim();
            string facultyName = this.DdlNganhHocThem.SelectedValue;
            string gradeName = this.DdlKhoiLopThem.SelectedValue;
            string markRatio = this.TxtMarkRatioThem.Text.Trim();

            bool bValidAdd = ValidateForAdd(subjectName, facultyName, gradeName, markRatio);

            if (bValidAdd)
            {
                Category_Faculty faculty = (new FacultyBL(UserSchool)).GetFaculty(facultyName);
                Category_Grade grade = (new GradeBL(UserSchool)).GetGrade(gradeName);

                subjectBL.InsertSubject(subjectName, grade, faculty, Double.Parse(markRatio));

                MainDataPager.CurrentIndex = 1;
                BindRepeater();

                this.TxtSubjectNameThem.Text = "";
                this.TxtMarkRatioThem.Text = "";

                if (this.CkbAddAfterSave.Checked)
                {
                    this.MPEAdd.Show();
                }
                else
                {
                    this.DdlNganhHocThem.SelectedIndex = 0;
                    this.DdlKhoiLopThem.SelectedIndex = 0;
                }
            }
        }

        private bool ValidateForAdd(string subjectName, string facultyName, string gradeName, string markRatio)
        {
            if (!Page.IsValid)
            {
                return false;
            }

            if (subjectName == "")
            {
                SubjectNameRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return false;
            }
            else
            {
                if (subjectBL.SubjectNameExists(subjectName, facultyName, gradeName))
                {
                    SubjectNameValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return false;
                }
            }

            if (!Regex.IsMatch(markRatio, MarkRatioRegExp.ValidationExpression))
            {
                MarkRatioRegExp.IsValid = false;
                MPEAdd.Show();
                return false;
            }

            return true;
        }

        protected void BtnEdit_Click(object sender, ImageClickEventArgs e)
        {
            HiddenField hdfRptSubjectId = null;
            foreach (RepeaterItem item in RptSubject.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox CkbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (CkbxSelect.Checked)
                    {
                        hdfRptSubjectId = (HiddenField)item.FindControl("HdfRptSubjectId");
                        Category_Subject subject = subjectBL.GetSubject(Int32.Parse(hdfRptSubjectId.Value));

                        TxtSubjectNameSua.Text = subject.SubjectName;
                        LblFacultyNameSua.Text = subject.Category_Faculty.FacultyName;
                        HdfFacultyIdSua.Value = subject.FacultyId.ToString();
                        LblGradeNameSua.Text = subject.Category_Grade.GradeName;
                        HdfGradeIdSua.Value = subject.GradeId.ToString();
                        TxtMarkRatioSua.Text = subject.MarkRatio.ToString();
                        HdfSubjectId.Value = subject.SubjectId.ToString();
                        HfdSelectedSubjectName.Value = subject.SubjectName;
                        MPEEdit.Show();
                        return;
                    }
                }
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            int iSubjectId = Int32.Parse(this.HdfSubjectId.Value);
            string editedSubjectName = this.HfdSelectedSubjectName.Value;
            string facultyName = this.HdfFacultyName.Value;
            string gradeName = this.HdfGradeName.Value;

            string newSubjectName = this.TxtSubjectNameSua.Text.Trim();
            double newMarkRatio = double.Parse(this.TxtMarkRatioSua.Text.Trim());
            Category_Subject editedSubject = subjectBL.GetSubject(iSubjectId);

            if (newSubjectName == "")
            {
                SubjectNameRequiredEdit.IsValid = false;
                MPEEdit.Show();
                return;
            }
            else
            {
                if (subjectBL.SubjectNameExists(editedSubject, newSubjectName))
                {
                    SubjectNameValidatorEdit.IsValid = false;
                    MPEEdit.Show();
                    return;
                }
            }

            subjectBL.UpdateSubject(editedSubject, newSubjectName, newMarkRatio);
            BindRepeater();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            bool bInfoInUse = false;
            CheckBox ckbxSelect = null;
            HiddenField hdfRptSubjectId = null;
            Category_Subject subject = null;

            foreach (RepeaterItem item in RptSubject.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    ckbxSelect = (CheckBox)item.FindControl("CkbxSelect");
                    if (ckbxSelect.Checked)
                    {
                        hdfRptSubjectId = (HiddenField)item.FindControl("HdfRptSubjectId");
                        subject = new Category_Subject();
                        subject.SubjectId = Int32.Parse(hdfRptSubjectId.Value);

                        if (subjectBL.IsDeletable(subject))
                        {
                            subjectBL.DeleteSubject(subject);
                        }
                        else
                        {
                            bInfoInUse = true;
                        }
                    }
                }
            }

            isSearch = false;
            BindRepeater();

            if (bInfoInUse)
            {
                MPEInfoInUse.Show();
            }
        }
        #endregion

        #region Pager event handlers
        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRepeater();
        }
        #endregion

        #region Methods
        private void ProcPermissions()
        {
            BtnAdd.Visible = (DdlKhoiLop.Items.Count != 0 && accessibilities.Contains(AccessibilityEnum.Add));
            PnlPopupAdd.Visible = BtnAdd.Visible;
            BtnEdit.Visible = accessibilities.Contains(AccessibilityEnum.Modify);
            BtnDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
            PnlPopupConfirmDelete.Visible = accessibilities.Contains(AccessibilityEnum.Delete);
        }

        private void BindRepeater()
        {
            Category_Faculty faculty = null;
            Category_Grade grade = null; ;
            FacultyBL facultyBL = new FacultyBL(UserSchool);
            GradeBL gradeBL = new GradeBL(UserSchool);
            string subjectName = this.TxtSearchedSubject.Text.Trim();
            double dTotalRecords;

            if (DdlNganh.SelectedIndex > 0)
            {
                string facultyName = DdlNganh.SelectedValue;
                faculty = facultyBL.GetFaculty(facultyName);
            }

            if (DdlKhoiLop.SelectedIndex > 0)
            {
                string gradeName = DdlKhoiLop.SelectedValue;
                grade = gradeBL.GetGrade(gradeName);
            }

            List<TabularSubject> lTabularSubjects = subjectBL.GetListTabularSubjects(faculty, grade, subjectName, MainDataPager.CurrentIndex,
                MainDataPager.PageSize, out dTotalRecords);
            MainDataPager.ItemCount = dTotalRecords;

            // Decrease page current index when delete
            if (lTabularSubjects.Count == 0 && MainDataPager.ItemCount != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRepeater();
                return;
            }

            bool bDisplayData = (lTabularSubjects.Count != 0) ? true : false;
            RptSubject.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin môn học";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy môn học";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
            RptSubject.DataSource = lTabularSubjects;
            RptSubject.DataBind();
        }

        private void BindDropDownLists()
        {
            BindDDLFaculties();
            BindDDLGrades();
        }

        private void BindDDLGrades()
        {
            GradeBL gradeBL = new GradeBL(UserSchool);

            List<Category_Grade> grades = gradeBL.GetListGrades();            
            DdlKhoiLop.DataSource = grades;
            DdlKhoiLop.DataValueField = "GradeName";
            DdlKhoiLop.DataTextField = "GradeName";
            DdlKhoiLop.DataBind();
            if (grades.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "Tất cả"));
            }

            DdlKhoiLopThem.DataSource = grades;
            DdlKhoiLopThem.DataValueField = "GradeName";
            DdlKhoiLopThem.DataTextField = "GradeName";
            DdlKhoiLopThem.DataBind();

            if (grades.Count != 0)
            {
                BtnAdd.Visible = true;
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD;
                PnlPopupAdd.Visible = true;
            }
            else
            {
                BtnAdd.Visible = false;
                BtnAdd.ImageUrl = AppConstant.IMAGESOURCE_BUTTON_ADD_DISABLE;
                PnlPopupAdd.Visible = false;
            }
        }

        private void BindDDLFaculties()
        {
            FacultyBL facultyBL = new FacultyBL(UserSchool);

            List<Category_Faculty> faculties = facultyBL.GetFaculties();
            DdlNganh.DataSource = faculties;
            DdlNganh.DataValueField = "FacultyName";
            DdlNganh.DataTextField = "FacultyName";
            DdlNganh.DataBind();
            if (faculties.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "Tất cả"));
            }

            DdlNganhHocThem.DataSource = faculties;
            DdlNganhHocThem.DataValueField = "FacultyName";
            DdlNganhHocThem.DataTextField = "FacultyName";
            DdlNganhHocThem.DataBind();
        }
        #endregion

        #region Repeater event handlers
        protected void RptSubject_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                e.Item.FindControl("thSelectAll").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.FindControl("tdSelect").Visible = (accessibilities.Contains(AccessibilityEnum.Modify) || accessibilities.Contains(AccessibilityEnum.Delete));
            }
        }
        #endregion
    }
}