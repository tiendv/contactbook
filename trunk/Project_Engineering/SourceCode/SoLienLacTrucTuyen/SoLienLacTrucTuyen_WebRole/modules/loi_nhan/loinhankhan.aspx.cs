using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EContactBook.BusinessEntity;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using AjaxControlToolkit;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class LoiNhanKhan : BaseContentPage
    {
        #region Fields
        private bool isSearch;
        private MessageBL loiNhanKhanBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (accessDenied)
            {
                // User can not access this page
                return;
            }

            if (sessionExpired)
            {
                FormsAuthentication.SignOut();
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            loiNhanKhanBL = new MessageBL(UserSchool);
            if (!Page.IsPostBack)
            {
                isSearch = false;
                BindDropDownLists();
                InitDates();

                BindRptMessages();
            }
        }
        #endregion

        #region Methods
        private void BindRptMessages()
        {
            Configuration_Year year = new Configuration_Year();
            year.YearId = Int32.Parse(DdlNamHoc.SelectedValue);
            DateTime tuNgay = DateTime.Parse(TxtTuNgay.Text);
            DateTime denNgay = DateTime.Parse(TxtDenNgay.Text);
            string strStudentCode = TxtMaHS.Text;
            ConfigurationMessageStatus messageStatus = null;
            if (DdlXacNhan.SelectedIndex > 0)
            {
                messageStatus = new ConfigurationMessageStatus();
                messageStatus.MessageStatusId = Int32.Parse(DdlXacNhan.SelectedValue);
            }

            double dTotalRecords;
            List<TabularMessage> lstTabularLoiNhanKhan = loiNhanKhanBL.GetTabularMessages(
                year, tuNgay, denNgay,
                strStudentCode, messageStatus, MainDataPager.CurrentIndex, MainDataPager.PageSize, out dTotalRecords);

            if (lstTabularLoiNhanKhan.Count == 0 && dTotalRecords != 0)
            {
                MainDataPager.CurrentIndex--;
                BindRptMessages();
                return;
            }

            bool bDisplayData = (lstTabularLoiNhanKhan.Count != 0) ? true : false;
            ProcessDislayInfo(bDisplayData);

            RptLoiNhanKhan.DataSource = lstTabularLoiNhanKhan;
            RptLoiNhanKhan.DataBind();
            MainDataPager.ItemCount = dTotalRecords;
        }

        private void ProcessDislayInfo(bool bDisplayData)
        {
            PnlPopupConfirmDelete.Visible = bDisplayData;
            PnlPopupEdit.Visible = bDisplayData;
            RptLoiNhanKhan.Visible = bDisplayData;
            LblSearchResult.Visible = !bDisplayData;

            if (LblSearchResult.Visible)
            {
                if (!isSearch)
                {
                    LblSearchResult.Text = "Chưa có thông tin lời nhắn khẩn";
                }
                else
                {
                    LblSearchResult.Text = "Không tìm thấy lời nhắn khẩn";
                }
                MainDataPager.ItemCount = 0;
                MainDataPager.Visible = false;
            }
            else
            {
                MainDataPager.Visible = true;
            }
        }

        private void BindDropDownLists()
        {
            BindDDLNamHoc();
            BindDDLMessageStatuses();
            BindDDLNganhHoc();
            BindDDLKhoiLop();
            BindDDLLopHoc();
        }

        private void BindDDLNamHoc()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<Configuration_Year> years = systemConfigBL.GetListYears();
            DdlNamHoc.DataSource = years;
            DdlNamHoc.DataValueField = "YearId";
            DdlNamHoc.DataTextField = "YearName";
            DdlNamHoc.DataBind();
            DdlNamHoc.SelectedValue = (new SystemConfigBL(UserSchool)).GetLastedYear().ToString();
        }

        private void BindDDLMessageStatuses()
        {
            SystemConfigBL systemConfigBL = new SystemConfigBL(UserSchool);
            List<ConfigurationMessageStatus> messageStatuses = systemConfigBL.GetMessageStatuses();
            DdlXacNhan.DataSource = messageStatuses;
            DdlXacNhan.DataValueField = "MessageStatusId";
            DdlXacNhan.DataTextField = "MessageStatusName";
            DdlXacNhan.DataBind();

            DdlXacNhan.Items.Insert(0, new ListItem("Tất cả", "0")); 
        }

        private void BindDDLKhoiLop()
        {
            GradeBL gradeBL = new GradeBL(UserSchool);
            List<Category_Grade> lGrades = gradeBL.GetListGrades();
            DdlKhoiLopThem.DataSource = lGrades;
            DdlKhoiLopThem.DataValueField = "GradeName";
            DdlKhoiLopThem.DataTextField = "GradeName";
            DdlKhoiLopThem.DataBind();
            if (lGrades.Count > 1)
            {
                DdlKhoiLopThem.Items.Insert(0, new ListItem("Tất cả", "Tất cả"));
            }
        }

        private void BindDDLNganhHoc()
        {
            FacultyBL facultyBL = new FacultyBL(UserSchool);
            List<Category_Faculty> faculties = facultyBL.GetFaculties();
            DdlNganhHocThem.DataSource = faculties;
            DdlNganhHocThem.DataValueField = "FacultyId";
            DdlNganhHocThem.DataTextField = "FacultyName";
            DdlNganhHocThem.DataBind();
            if (faculties.Count > 1)
            {
                DdlNganhHocThem.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void BindDDLLopHoc()
        {
            ClassBL lopHocBL = new ClassBL(UserSchool);
            GradeBL gradeBL = new GradeBL(UserSchool);
            Category_Faculty faculty = null;
            Category_Grade grade = null;
            Configuration_Year currentYear = (new SystemConfigBL(UserSchool)).GetLastedYear();

            int YearId = currentYear.YearId;

            try
            {
                if (DdlNganhHocThem.SelectedIndex > 0)
                {
                    faculty = new Category_Faculty();
                    faculty.FacultyId = Int32.Parse(DdlNganhHocThem.SelectedValue);
                }
            }
            catch (Exception) { }

            try
            {                
                if (DdlKhoiLopThem.SelectedIndex > 0)
                {
                    string gradeName = DdlKhoiLopThem.SelectedValue;                    
                    grade = gradeBL.GetGrade(gradeName);
                }
            }
            catch (Exception) { }


            List<Class_Class> lstLop = lopHocBL.GetListClasses(currentYear, faculty, grade);
            DdlLopThem.DataSource = lstLop;
            DdlLopThem.DataValueField = "ClassId";
            DdlLopThem.DataTextField = "ClassName";
            DdlLopThem.DataBind();

            if (lstLop.Count > 1)
            {
                DdlLopThem.Items.Insert(0, new ListItem("Tất cả", "0"));
            }

            BindDDLHocSinh();
        }

        private void BindDDLHocSinh()
        {
            List<StudentDropdownListItem> lStudents = new List<StudentDropdownListItem>();
            if (DdlLopThem.Items.Count != 0)
            {
                string facultyName = DdlNganhHocThem.SelectedItem.Text;
                Category_Faculty faculty = (new FacultyBL(UserSchool)).GetFaculty(facultyName);

                string strGradeName = DdlKhoiLopThem.SelectedValue;
                Category_Grade grade = (new GradeBL(UserSchool)).GetGrade(strGradeName);

                int iClassId = Int32.Parse(DdlLopThem.SelectedValue);
                Class_Class cls = (new ClassBL(UserSchool)).GetClass(iClassId);

                lStudents = (new StudentBL(UserSchool)).GetStudents(faculty, grade, cls);
            }

            DdlHocSinhThem.DataSource = lStudents;
            DdlHocSinhThem.DataTextField = StudentDropdownListItem.STUDENT_CODE;
            DdlHocSinhThem.DataValueField = StudentDropdownListItem.STUDENT_IN_CLASS_ID;

            DdlHocSinhThem.DataBind();
            if (DdlHocSinhThem.Items.Count > 1)
            {
                DdlHocSinhThem.Items.Insert(0, new ListItem("Tất cả", "0"));
            }
        }

        private void InitDates()
        {
            DateTime today = DateTime.Now;
            DateTime beginDateOfMonth = new DateTime(today.Year, today.Month, 1);
            TxtTuNgay.Text = beginDateOfMonth.ToShortDateString();
            DateTime dateOfNextMonth = today.AddMonths(1);
            DateTime beginDateOfNextMonth = new DateTime(dateOfNextMonth.Year, dateOfNextMonth.Month, 1);
            DateTime endDateOfMonth = beginDateOfNextMonth.AddDays(-1);
            TxtDenNgay.Text = endDateOfMonth.ToShortDateString();

            TxtNgayThem.Text = today.ToShortDateString();
        }
        #endregion

        #region DropDownList event hanlders
        protected void DdlNganhThem_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }

        protected void DdlKhoiLopThem_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLLopHoc();
        }

        protected void DdlLopHocThem_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDDLHocSinh();
        }
        #endregion

        #region Repeater event handlers
        protected void RptLoiNhanKhan_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //LopHocInfo lopHoc = (LopHocInfo)e.Item.DataItem;
                //if (lopHoc != null)
                //{
                //    int ClassId = lopHoc.ClassId;
                //    if (!lopHocBL.CheckCanDeleteLopHoc(ClassId))
                //    {
                //        ImageButton btnDeleteItem = (ImageButton)e.Item.FindControl("BtnDeleteItem");
                //        btnDeleteItem.ImageUrl = "~/Styles/Images/button_delete_disable.png";
                //        btnDeleteItem.Enabled = false;
                //    }
                //}
            }
        }

        protected void RptLoiNhanKhan_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa lời nhắn khẩn <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfRptMaLoiNhanKhan = (HiddenField)e.Item.FindControl("HdfRptMaLoiNhanKhan");
                        this.HdfMaLoiNhanKhan.Value = hdfRptMaLoiNhanKhan.Value;

                        this.HdfRptLoiNhanKhanMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        int maLoiNhanKhan = Int32.Parse(e.CommandArgument.ToString());
                        MessageToParents_Message loiNhanKhan = loiNhanKhanBL.GetMessage(maLoiNhanKhan);

                        LblTieuDeSua.Text = loiNhanKhan.Title;
                        TxtNoiDungSua.Text = TxtNoiDungSua.Text;
                        TxtNgaySua.Text = loiNhanKhan.Date.ToShortDateString();

                        Student_StudentInClass hocSinhLopHoc = (new StudentBL(UserSchool)).GetStudentInClass(loiNhanKhan.StudentInClassId);
                        LblMaHocSinhSua.Text = (new StudentBL(UserSchool)).GetStudent(hocSinhLopHoc.StudentId).StudentCode;                        
                        LblNganhHocSua.Text = hocSinhLopHoc.Class_Class.Category_Faculty.FacultyName;
                        LblKhoiSua.Text = hocSinhLopHoc.Class_Class.Category_Grade.GradeName;
                        LblLopSua.Text = hocSinhLopHoc.Class_Class.ClassName;

                        ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        mPEEdit.Show();

                        this.HdfMaLoiNhanKhan.Value = maLoiNhanKhan.ToString();
                        this.HdfRptLoiNhanKhanMPEEdit.Value = mPEEdit.ClientID;

                        break;
                    }
                case "CmdDetailItem":
                    {
                        //int ClassId = Int32.Parse(e.CommandArgument.ToString());
                        //Class_Class lophoc = lopHocBL.GetLopHoc(ClassId);

                        //LblClassNameChiTiet.Text = lophoc.ClassName;
                        //LblFacultyNameChiTiet.Text = (new facultyBL(UserSchool)).GetNganhHoc(lophoc.FacultyId).FacultyName;
                        //LblGradeNameChiTiet.Text = (new grades(UserSchool)).GetKhoiLop(lophoc.GradeId).GradeName;
                        //LblSiSoChiTiet.Text = lophoc.SiSo.ToString();
                        //ModalPopupExtender mPEDetail = (ModalPopupExtender)e.Item.FindControl("MPEDetail");
                        //mPEDetail.Show();

                        //this.HdfClassId.Value = ClassId.ToString();
                        //this.HdfRptLopHocMPEDetail.Value = mPEDetail.ClientID;
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
            BindRptMessages();
        }

        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            MainDataPager.CurrentIndex = 1;

            string tieuDe = TxtTieuDeThem.Text;
            string noiDung = TxtNoiDungThem.Text;
            DateTime ngay = DateTime.Parse(TxtNgayThem.Text);
            int maHocSinhLopHoc = Int32.Parse(DdlHocSinhThem.SelectedValue);
            if (maHocSinhLopHoc == 0)
            {
                for (int i = 1; i < DdlHocSinhThem.Items.Count; i++)
                {
                    loiNhanKhanBL.InsertMessage(Int32.Parse(DdlHocSinhThem.Items[i].Value),
                        tieuDe, noiDung, ngay);
                }
            }
            else
            {
                loiNhanKhanBL.InsertMessage(maHocSinhLopHoc,
                        tieuDe, noiDung, ngay);
            }

            BindRptMessages();

            TxtTieuDeThem.Text = "";
            TxtNoiDungThem.Text = "";
            TxtNgayThem.Text = DateTime.Now.ToShortDateString();

            if (this.CkbAddAfterSave.Checked)
            {
                this.MPEAdd.Show();
            }
        }

        protected void BtnSaveEdit_Click(object sender, ImageClickEventArgs e)
        {
            int maLoiNhanKhan = Int32.Parse(this.HdfMaLoiNhanKhan.Value);
            loiNhanKhanBL.UpdateMessage(maLoiNhanKhan, TxtNoiDungSua.Text, DateTime.Parse(TxtNgaySua.Text));
            BindRptMessages();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {
            int maLopNhanKhan = Int32.Parse(this.HdfMaLoiNhanKhan.Value);
            loiNhanKhanBL.DeleteMessage(maLopNhanKhan);
            isSearch = false;
            BindRptMessages();
        }
        #endregion

        #region Pager event handlers
        public void MainDataPager_Command(object sender, CommandEventArgs e)
        {
            int currentPageIndex = Convert.ToInt32(e.CommandArgument);
            this.MainDataPager.CurrentIndex = currentPageIndex;
            BindRptMessages();
        }
        #endregion
    }
}