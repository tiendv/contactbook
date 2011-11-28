using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
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
            if (isAccessDenied)
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
            if (lstAccessibilities.Contains(AccessibilityEnum.Modify))
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

            if (lstAccessibilities.Contains(AccessibilityEnum.Delete))
            {
                if (e.Item.ItemType == ListItemType.Item
                    || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    if (e.Item.DataItem != null)
                    {
                        TabularSubject tabularSubject = (TabularSubject)e.Item.DataItem;
                        DanhMuc_MonHoc subject = subjectBL.GetSubject(tabularSubject.TenMonHoc, tabularSubject.TenNganhHoc, tabularSubject.TenKhoiLop);
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

                        // Save current MaMonHoc to global
                        HiddenField hdfRptMaMonHoc = (HiddenField)e.Item.FindControl("HdfRptMaMonHoc");
                        this.HdfMaMonHoc.Value = hdfRptMaMonHoc.Value;

                        // Save modal popup ClientID
                        this.HdfRptMonHocMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        string facultyName = ((Label)e.Item.FindControl("LblFacultyName")).Text;
                        string gradeName = ((Label)e.Item.FindControl("LblGradeName")).Text;

                        DanhMuc_MonHoc subject = subjectBL.GetSubject(strSubjectName, facultyName, gradeName);

                        TxtTenMonHocSua.Text = subject.TenMonHoc;
                        LblTenNganhHocSua.Text = subject.DanhMuc_NganhHoc.TenNganhHoc;
                        HdfMaNganhHocSua.Value = subject.MaNganhHoc.ToString();
                        LblTenKhoiLopSua.Text = subject.DanhMuc_KhoiLop.TenKhoiLop;
                        HdfMaKhoiLopSua.Value = subject.MaKhoiLop.ToString();
                        TxtHeSoDiemSua.Text = subject.HeSoDiem.ToString();

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfMaMonHoc.Value = subject.MaMonHoc.ToString();
                        this.HdfRptMonHocMPEEdit.Value = mPEEdit.ClientID;

                        this.HdfFacultyName.Value = subject.DanhMuc_NganhHoc.TenNganhHoc;
                        this.HdfGradeName.Value = subject.DanhMuc_KhoiLop.TenKhoiLop;

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
            string subjectName = this.TxtTenMonHocThem.Text.Trim();
            string facultyName = this.DdlNganhHocThem.SelectedValue;
            string gradeName = this.DdlKhoiLopThem.SelectedValue;
            string markRatio = this.TxtHeSoDiemThem.Text.Trim();            

            bool bValidAdd = ValidateForAdd(subjectName, facultyName, gradeName, markRatio);

            if (bValidAdd)
            {
                DanhMuc_NganhHoc faculty = (new FacultyBL(UserSchool)).GetFaculty(facultyName);
                DanhMuc_KhoiLop grade = (new GradeBL(UserSchool)).GetGrade(gradeName);

                subjectBL.InsertSubject(subjectName, grade, faculty, Double.Parse(markRatio));

                MainDataPager.CurrentIndex = 1;
                BindRepeater();

                this.TxtTenMonHocThem.Text = "";
                this.TxtHeSoDiemThem.Text = "";

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
                TenMonHocRequiredAdd.IsValid = false;
                MPEAdd.Show();
                return false;
            }
            else
            {
                if (subjectBL.SubjectNameExists(subjectName, facultyName, gradeName))
                {
                    TenMonHocValidatorAdd.IsValid = false;
                    MPEAdd.Show();
                    return false;
                }
            }

            if (!Regex.IsMatch(markRatio, HeSoDiemRegExp.ValidationExpression))
            {
                HeSoDiemRegExp.IsValid = false;
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

            int maMonHoc = Int32.Parse(this.HdfMaMonHoc.Value);
            string editedSubjectName = this.HfdSelectedSubjectName.Value;
            string facultyName = this.HdfFacultyName.Value;
            string gradeName = this.HdfGradeName.Value;

            string newSubjectName = this.TxtTenMonHocSua.Text.Trim();
            double newMarkRatio = double.Parse(this.TxtHeSoDiemSua.Text.Trim());
            DanhMuc_MonHoc editedSubject = subjectBL.GetSubject(editedSubjectName, facultyName, gradeName);

            if (newSubjectName == "")
            {
                TenMonHocRequiredEdit.IsValid = false;
                modalPopupEdit.Show();
                return;
            }
            else
            {   
                if (subjectBL.SubjectNameExists(editedSubject, newSubjectName))
                {
                    TenMonHocValidatorEdit.IsValid = false;
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

            DanhMuc_MonHoc subject = subjectBL.GetSubject(subjectName, facultyName, gradeName);
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
            if (lstAccessibilities.Contains(AccessibilityEnum.Add))
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
            DanhMuc_NganhHoc faculty = null;
            DanhMuc_KhoiLop grade = null;;
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

            List<DanhMuc_KhoiLop> lGrades = gradeBL.GetListGrades();
            DdlKhoiLop.DataSource = lGrades;
            DdlKhoiLop.DataValueField = "TenKhoiLop";
            DdlKhoiLop.DataTextField = "TenKhoiLop";
            DdlKhoiLop.DataBind();
            if (lGrades.Count > 1)
            {
                DdlKhoiLop.Items.Insert(0, new ListItem("Tất cả", "Tất cả"));
            }

            DdlKhoiLopThem.DataSource = lGrades;
            DdlKhoiLopThem.DataValueField = "TenKhoiLop";
            DdlKhoiLopThem.DataTextField = "TenKhoiLop";
            DdlKhoiLopThem.DataBind();
        }

        private void BindDDLFaculties()
        {
            FacultyBL facultyBL = new FacultyBL(UserSchool);

            List<DanhMuc_NganhHoc> faculties = facultyBL.GetFaculties();
            DdlNganh.DataSource = faculties;
            DdlNganh.DataValueField = "TenNganhHoc";
            DdlNganh.DataTextField = "TenNganhHoc";
            DdlNganh.DataBind();
            if (faculties.Count > 1)
            {
                DdlNganh.Items.Insert(0, new ListItem("Tất cả", "Tất cả"));
            }

            DdlNganhHocThem.DataSource = faculties;
            DdlNganhHocThem.DataValueField = "TenNganhHoc";
            DdlNganhHocThem.DataTextField = "TenNganhHoc";
            DdlNganhHocThem.DataBind();
        }       
        #endregion
    }
}