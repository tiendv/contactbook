using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using System.Web.Security;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ThemNguoiDung : System.Web.UI.Page
    {
        #region Fields
        private RoleBL roleBL = new RoleBL();
        private UserBL userBL;
        private HocSinhBL hocSinhBL;
        protected string btnSaveClickEvent = string.Empty;
        public string SeletedRole 
        {
            get
            {
                if (ViewState["SeletedRole"] != null)
                {
                    return ViewState["SeletedRole"].ToString();
                }
                else
                {
                    return "";
                }
            }

            set
            {
                ViewState["SeletedRole"] = value;
            }
        }
        #endregion

        #region Page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            userBL = new UserBL();
            hocSinhBL = new HocSinhBL();

            string pageUrl = Page.Request.Path;
            Guid role = userBL.GetRoleId(User.Identity.Name);
            if (!roleBL.ValidateAuthorization(role, pageUrl))
            {
                Response.Redirect("/Modules/ErrorPage/AccessDenied.aspx");
                return;
            }

            Site masterPage = (Site)Page.Master;
            masterPage.UserRole = userBL.GetRoleId(User.Identity.Name);
            masterPage.PageUrl = Page.Request.Path;

            if (!Page.IsPostBack)
            {
                BindDropDownListNhomNguoiDung();

                DropDownList DdlRoles = (DropDownList)SeleteRoleStep.FindControl("DdlRoles");
                if (DdlRoles.Items.Count != 0)
                {
                    SeletedRole = DdlRoles.Items[0].Value;
                    ProcessUI();

                    BindDropDownListNamHoc();
                    BindDropDownListNganhHoc();
                    BindDropDownListKhoiLop();
                }                
            }
        }
        #endregion

        #region Methods
        private void BindDropDownListNhomNguoiDung()
        {   
            DropDownList DdlRoles = (DropDownList)SeleteRoleStep.FindControl("DdlRoles");
            List<aspnet_Role> lstNhomNguoiDung = roleBL.GetListRoles();
            DdlRoles.DataSource = lstNhomNguoiDung;
            DdlRoles.DataValueField = "RoleName";
            DdlRoles.DataTextField = "RoleName";
            DdlRoles.DataBind();
        }

        private void BindDropDownListNamHoc()
        {
            NamHocBL namHocBL = new NamHocBL();
            List<CauHinh_NamHoc> lstNamHocs = namHocBL.GetListNamHoc();

            MultiView multiViewCtrl = (MultiView)SeleteRoleStep.FindControl("MultiViewCtrl");
            View viewGVCN = multiViewCtrl.Views[1];
            
            DropDownList DdlNamHocGVCN = (DropDownList)viewGVCN.FindControl("DdlNamHocGVCN");
            DdlNamHocGVCN.DataSource = lstNamHocs;
            DdlNamHocGVCN.DataValueField = "MaNamHoc";
            DdlNamHocGVCN.DataTextField = "TenNamHoc";
            DdlNamHocGVCN.DataBind();
        }

        private void BindDropDownListNganhHoc()
        {
            FacultyBL nganhHocBL = new FacultyBL();
            List<DanhMuc_NganhHoc> lstNganhHoc = nganhHocBL.GetListNganhHoc();

            MultiView multiViewCtrl = (MultiView)SeleteRoleStep.FindControl("MultiViewCtrl");
            View viewGVCN = multiViewCtrl.Views[1];

            DropDownList DdlNganhHocGVCN = (DropDownList)viewGVCN.FindControl("DdlNganhGVCN");
            DdlNganhHocGVCN.DataSource = lstNganhHoc;
            DdlNganhHocGVCN.DataValueField = "MaNganhHoc";
            DdlNganhHocGVCN.DataTextField = "TenNganhHoc";
            DdlNganhHocGVCN.DataBind();
        }

        private void BindDropDownListKhoiLop()
        {
            KhoiLopBL KhoiLopBL = new KhoiLopBL();
            List<DanhMuc_KhoiLop> lstKhoiLop = KhoiLopBL.GetListKhoiLop();

            MultiView multiViewCtrl = (MultiView)SeleteRoleStep.FindControl("MultiViewCtrl");
            View viewGVCN = multiViewCtrl.Views[1];

            DropDownList DdlKhoiLopGVCN = (DropDownList)viewGVCN.FindControl("DdlKhoiGVCN");

            DdlKhoiLopGVCN.DataSource = lstKhoiLop;
            DdlKhoiLopGVCN.DataValueField = "MaKhoiLop";
            DdlKhoiLopGVCN.DataTextField = "TenKhoiLop";
            DdlKhoiLopGVCN.DataBind();
        }

        private void BackPrevPage()
        {
            Response.Redirect("/Modules/Nguoi_Dung/DanhSachNguoiDung.aspx");
        }
        #endregion

        #region DropDownList event handlers
        protected void DdlRoles_SelectedIndexChanged(object sender, EventArgs e)
        {            
            ProcessUI();
        }

        private void ProcessUI()
        {
            string selectedRoleName = ((DropDownList)SeleteRoleStep.FindControl("DdlRoles")).SelectedValue;
            SeletedRole = selectedRoleName;

            ImageButton NextButton = (ImageButton)RegisterUserWizard.FindControl("StartNavigationTemplateContainerID").FindControl("StartNextButton");
            NextButton.Visible = true;
            Label lblStepError = (Label)SeleteRoleStep.FindControl("LblStepError");
            lblStepError.Text = "";

            Control container = CreateUserStep.ContentTemplateContainer;
            ((Label)container.FindControl("LblSelectedRole")).Text = SeletedRole;
            ((Label)container.FindControl("LblTenNguoiDung")).Text = "Tên người dùng:";
            ((RequiredFieldValidator)container.FindControl("RealNameRequired")).Enabled = true;
            ((HtmlTableRow)container.FindControl("HtmlTrThoiHan")).Style.Add(HtmlTextWriterStyle.Display, "none");
            ((HtmlTableRow)container.FindControl("HtmlTrTenThat")).Style.Add(HtmlTextWriterStyle.Display, "block");
            
            ViewState["SeletedRoleParents"] = false;

            MultiView multiViewCtrl = (MultiView)SeleteRoleStep.FindControl("MultiViewCtrl");
            multiViewCtrl.ActiveViewIndex = 0;

            if (roleBL.IsRoleParents(selectedRoleName))
            {
                multiViewCtrl.ActiveViewIndex = 1;
                Repeater rptRoleBasedFunctions = (Repeater)multiViewCtrl.FindControl("RptRoleBasedFunctions");
                rptRoleBasedFunctions.DataSource = roleBL.GetListRoleParentsBasedFunctions();
                rptRoleBasedFunctions.DataBind();

                ((Label)container.FindControl("LblTenNguoiDung")).Text = "Mã học sinh:";
                ((RequiredFieldValidator)container.FindControl("RealNameRequired")).Enabled = false;
                ((HtmlTableRow)container.FindControl("HtmlTrThoiHan")).Style.Add(HtmlTextWriterStyle.Display, "block");
                ((HtmlTableRow)container.FindControl("HtmlTrTenThat")).Style.Add(HtmlTextWriterStyle.Display, "none");                
                ViewState["SeletedRoleParents"] = true;                
            }
            else
            {
                if (roleBL.IsRoleGiaoVienChuNhiem(selectedRoleName))
                {
                    multiViewCtrl.ActiveViewIndex = 2;

                    View activeView = multiViewCtrl.Views[multiViewCtrl.ActiveViewIndex];
                    DropDownList DdlLopHocGVCN = (DropDownList)activeView.FindControl("DdlLopHocGVCN");
                    if (DdlLopHocGVCN.Items.Count == 0)
                    {
                        NextButton.Visible = false;
                        lblStepError.Text = "Chưa có thông tin lớp học.<br/>Vui lòng bổ sung thông tin lớp học!";
                    }
                }
                else
                {
                    if (roleBL.IsRoleGiaoVienBoMon(selectedRoleName))
                    {
                        multiViewCtrl.ActiveViewIndex = 3;
                    }
                    else
                    {

                    }
                }
            }


            
        }

        protected void DdlNamHocGVCN_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHocGVCN();
        }

        protected void DdlKhoiGVCN_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHocGVCN();
        }

        protected void DdlNganhGVCN_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDropDownListLopHocGVCN();
        }

        protected void DdlLopHocGVCN_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label lblStepError = (Label)SeleteRoleStep.FindControl("LblStepError");
            ImageButton NextButton = (ImageButton)RegisterUserWizard.FindControl(
                "StartNavigationTemplateContainerID").FindControl("StartNextButton");

            DropDownList ddlLopHocGVCN = sender as DropDownList;
            int maLopHoc = Int32.Parse(ddlLopHocGVCN.SelectedValue);
            bool hasGVCN = (new LopHocBL()).HasGiaoVienChuNhiem(maLopHoc);
            if (!hasGVCN)
            {                
                lblStepError.Text = "Lớp học này chưa có giáo viên chủ nhiệm.<br/>"
                    + "Vui lòng bổ sung thông tin giáo viên chủ nhiệm!";            
                NextButton.Visible = false;
            }
            else
            {
                lblStepError.Text = "";
                NextButton.Visible = true;
            }
        }

        private void BindDropDownListLopHocGVCN()
        {
            Label lblStepError = (Label)SeleteRoleStep.FindControl("LblStepError");
            ImageButton NextButton = (ImageButton)RegisterUserWizard.FindControl(
                "StartNavigationTemplateContainerID").FindControl("StartNextButton"); 
            
            MultiView multiViewCtrl = (MultiView)SeleteRoleStep.FindControl("MultiViewCtrl");
            View viewGVCN = multiViewCtrl.Views[1];            
            DropDownList ddlNamHocGVCN = (DropDownList)viewGVCN.FindControl("DdlNamHocGVCN");
            DropDownList ddlNganhHocGVCN = (DropDownList)viewGVCN.FindControl("DdlNganhGVCN");
            DropDownList ddlKhoiLopGVCN = (DropDownList)viewGVCN.FindControl("DdlKhoiGVCN");

            if (ddlNamHocGVCN.Items.Count == 0 || ddlNganhHocGVCN.Items.Count == 0 
                || ddlKhoiLopGVCN.Items.Count == 0)
            {
                lblStepError.Text = "Chưa có thông tin lớp học.<br/>Vui lòng bổ sung thông tin lớp học!";
                NextButton.Visible = false;
                return;
            }

            int maNamHoc = Int32.Parse(ddlNamHocGVCN.SelectedValue);
            int maNganhHoc = Int32.Parse(ddlNganhHocGVCN.SelectedValue);
            int maKhoiLop = Int32.Parse(ddlKhoiLopGVCN.SelectedValue);

            List<LopHoc_Lop> lstLopHocGVCN = (new LopHocBL()).GetListLopHoc(maNganhHoc, maKhoiLop, maNamHoc);
            DropDownList ddlLopHocGVCN = (DropDownList)viewGVCN.FindControl("DdlLopHocGVCN");
            ddlLopHocGVCN.DataSource = lstLopHocGVCN;
            ddlLopHocGVCN.DataValueField = "MaLopHoc";
            ddlLopHocGVCN.DataTextField = "TenLopHoc";
            ddlLopHocGVCN.DataBind();
            if (ddlLopHocGVCN.Items.Count == 0)
            {
                lblStepError.Text = "Chưa có thông tin lớp học.<br/>"
                    + "Vui lòng bổ sung thông tin lớp học!";
                NextButton.Visible = false;
                return;
            }
            else
            {
                int maLopHoc = Int32.Parse(ddlLopHocGVCN.Items[0].Value);
                bool hasGVCN = (new LopHocBL()).HasGiaoVienChuNhiem(maLopHoc);
                if (!hasGVCN)
                {
                    lblStepError.Text = "Lớp học này chưa có giáo viên chủ nhiệm.<br/>" 
                        + "Vui lòng bổ sung thông tin giáo viên chủ nhiệm!";
                    NextButton.Visible = false;
                    return;
                }

                lblStepError.Text = "";
                NextButton.Visible = true;
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnFinish_Click(object sender, ImageClickEventArgs e)
        {
            BackPrevPage();
        }

        protected void BtnNext_Click(object sender, ImageClickEventArgs e)
        {
            List<int> functionIds = new List<int>();
            Repeater rptRoleBasedFunctions = (Repeater)SeleteRoleStep.FindControl("RptRoleBasedFunctions");
            foreach(RepeaterItem rptItem in rptRoleBasedFunctions.Items)
            {
                if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox ChkBxSelectedFunction = (CheckBox)rptItem.FindControl("ChkBxSelectedFunction");
                    if (ChkBxSelectedFunction.Checked)
                    {
                        HiddenField HdfFunctionId = (HiddenField)rptItem.FindControl("HdfFunctionId");
                        functionIds.Add(Int32.Parse(HdfFunctionId.Value));
                    }
                }
            }

            string roleName = roleBL.GetChildRoleParentsByFunctions(functionIds);
            if(roleName != "")
            {
                SeletedRole = roleName;
            }

        }
        #endregion

        #region CreateUserWizard event handlers
        protected void RegisterUserWizard_CreatingUser(object sender, LoginCancelEventArgs e)
        {

        }

        protected void RegisterUserWizard_CreatedUser(object sender, EventArgs e)
        {
            // UserName
            string userName = RegisterUserWizard.UserName;

            // RoleName            
            //Control control = CreateUserStep.ContentTemplateContainer;
            //DropDownList ddlRoles = (DropDownList)SeleteRoleStep.FindControl("DdlRoles");
            //string roleName = ddlRoles.SelectedItem.Text;

            string roleName = SeletedRole;
            
            roleBL.AddUserToRole(userName, roleName);
        }

        protected void RegisterUserWizard_ContinueButtonClick(object sender, ImageClickEventArgs e)
        {
            BackPrevPage();
        }        
        #endregion
    }
}