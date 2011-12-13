using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;
using SoLienLacTrucTuyen;
using System.Text.RegularExpressions;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class DanhMucMonHoc : BaseContentPage
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

        #region Repeater event handlers
        protected void RptMonHoc_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (accessibilities.Contains(AccessibilityEnum.Modify))
            {
                // Do something
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thEdit").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item ||
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdEdit").Visible = false;
                }

                PnlPopupEdit.Visible = false;
            }

            if (accessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        TabularSubject tabularSubject = (TabularSubject)e.Item.DataItem;
                        Category_Subject subject = subjectBL.GetSubject(tabularSubject.SubjectName, tabularSubject.FacultyName, tabularSubject.GradeName);
                        if (!subjectBL.IsDeletable(subject))
                        {
                            ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                            btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                            btnDeleteItem.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                if (e.Item.ItemType == ListItemType.Header)
                {
                    e.Item.FindControl("thDelete").Visible = false;
                }

                if (e.Item.ItemType == ListItemType.Item ||
                    e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("tdDelete").Visible = false;
                }

                this.PnlPopupConfirmDelete.Visible = false;
            }
        }

        protected void RptMonHoc_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            this.HfdSelectedSubjectName.Value = (string)e.CommandArgument;
            string strSubjectName = (string)e.CommandArgument;
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        // Set confirm text and show dialog
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa môn học <b>" + strSubjectName + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        // Save current SubjectId to global
                        HiddenField hdfRptSubjectId = (HiddenField)e.Item.FindControl("HdfRptSubjectId");
                        this.HdfSubjectId.Value = hdfRptSubjectId.Value;

                        // Save modal popup ClientID
                        this.HdfRptMonHocMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        string facultyName = ((Label)e.Item.FindControl("LblFacultyName")).Text;
                        string gradeName = ((Label)e.Item.FindControl("LblGradeName")).Text;

                        Category_Subject subject = subjectBL.GetSubject(strSubjectName, facultyName, gradeName);

                        TxtSubjectNameSua.Text = subject.SubjectName;
                        LblFacultyNameSua.Text = subject.Category_Faculty.FacultyName;
                        HdfFacultyIdSua.Value = subject.FacultyId.ToString();
                        LblGradeNameSua.Text = subject.Category_Grade.GradeName;
                        HdfGradeIdSua.Value = subject.GradeId.ToString();
                        TxtMarkRatioSua.Text = subject.MarkRatio.ToString();

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfSubjectId.Value = subject.SubjectId.ToString();
                        this.HdfRptMonHocMPEEdit.Value = mPEEdit.ClientID;

                        this.HdfFacultyName.Value = subject.Category_Faculty.FacultyName;
                        this.HdfGradeName.Value = subject.Category_Grade.GradeName;

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
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

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            ModalPopupExtender modalPopupEdit = new ModalPopupExtender();
            foreach (RepeaterItem rptItem in RptMonHoc.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    modalPopupEdit = (ModalPopupExtender)rptItem.FindControl("MPEEdit");
                    if (modalPopupEdit.ClientID == HdfRptMonHocMPEEdit.Value)
                    {
                        break;
                    }
                }
            }

            if (!Page.IsValid)
            {
                return;
            }

            int SubjectId = Int32.Parse(this.HdfSubjectId.Value);
            string editedSubjectName = this.HfdSelectedSubjectName.Value;
            string facultyName = this.HdfFacultyName.Value;
            string gradeName = this.HdfGradeName.Value;

            string newSubjectName = this.TxtSubjectNameSua.Text.Trim();
            double newMarkRatio = double.Parse(this.TxtMarkRatioSua.Text.Trim());
            Category_Subject editedSubject = subjectBL.GetSubject(editedSubjectName, facultyName, gradeName);

            if (newSubjectName == "")
            {
                SubjectNameRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {
                if (subjectBL.SubjectNameExists(editedSubject, newSubjectName))
                {
                    SubjectNameValidatorEdit.IsValid = false;
                    modalPopupEdit.Show();
                    return;
                }
            }

            subjectBL.UpdateSubject(editedSubject, newSubjectName, newMarkRatio);
            BindRepeater();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            string subjectName = this.HfdSelectedSubjectName.Value;
            string facultyName = this.HdfFacultyName.Value;
            string gradeName = this.HdfGradeName.Value;

            Category_Subject subject = subjectBL.GetSubject(subjectName, facultyName, gradeName);
            subjectBL.DeleteSubject(subject);

            isSearch = false;
            BindRepeater();
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
            if (accessibilities.Contains(AccessibilityEnum.Add))
            {
                BtnAdd.Enabled = true;
                BtnAdd.ImageUrl = "~/Styles/Images/button_add_with_text.png";
                PnlPopupAdd.Visible = true;
            }
            else
            {
                BtnAdd.Visible = false;
                PnlPopupAdd.Visible = false;
            }
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
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptMonHoc.Visible = bDisplayData;
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
            RptMonHoc.DataSource = lTabularSubjects;
            RptMonHoc.DataBind();
        }

        private void BindDropDownLists()
        {
            BindDDLFaculties();
            BindDDLGrades();
        }

        private void BindDDLGrades()
        {
            GradeBL gradeBL = new GradeBL(UserSchool);

            List<Category_Grade> lGrades = gradeBL.GetListGrades();
            DdlKhoiLop.DataSource = lGrades;
            DdlKhoiLop.DataValueField = "GradeName";
            DdlKhoiLop.DataTextField = "GradeName";
            DdlKhoiLop.DataBind();
            if (lGrades.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "Tất cả"));
            }

            DdlKhoiLopThem.DataSource = lGrades;
            DdlKhoiLopThem.DataValueField = "GradeName";
            DdlKhoiLopThem.DataTextField = "GradeName";
            DdlKhoiLopThem.DataBind();
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
    }
}