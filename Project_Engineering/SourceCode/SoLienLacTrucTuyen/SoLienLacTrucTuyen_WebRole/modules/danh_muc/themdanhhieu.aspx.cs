using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using AjaxControlToolkit;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class ThemDanhHieuPage : BaseContentPage
    {
        #region Fields
        public struct SelectedHocLucHanhKiem
        {
            public int ThuTu { get; set; }
            public int LearningAptitudeId { get; set; }
            public string LearningAptitudeName { get; set; }
            public int ConductId { get; set; }
            public string ConductName { get; set; }
        }
        private DanhHieuBL danhHieuBL;
        public List<SelectedHocLucHanhKiem> SelectedHocLucHanhKiems 
        { 
            get
            {
                if (Session["SltHocLucHanhKiems"] != null)
                {
                    return (List<SelectedHocLucHanhKiem>)Session["SltHocLucHanhKiems"];
                }
                else
                {
                    return new List<SelectedHocLucHanhKiem>();
                }
            }

            set
            {
                Session["SltHocLucHanhKiems"] = value;
            }
        }
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            danhHieuBL = new DanhHieuBL(UserSchool);

            if (!Page.IsPostBack)
            {
                Session.Remove("SltHocLucHanhKiems");

                BindRptDescriptionDanhHieu();
                BindDropdownlists();
            }
        }        
        #endregion

        #region Button click event handlers
        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            string LearningResultName = this.TxtLearningResultName.Text.Trim();

            if (LearningResultName == "")
            {
                LearningResultNameRequiredAdd.IsValid = false;
                TxtLearningResultName.Focus();
                return;
            }
            else
            {
                if (danhHieuBL.DanhHieuExists(LearningResultName))
                {
                    LearningResultNameValidatorAdd.IsValid = false;
                    TxtLearningResultName.Focus();
                    return;
                }
            }

            danhHieuBL.InsertDanhHieu(LearningResultName, new Dictionary<int, int>());

            if (this.CkbAddAfterSave.Checked)
            {
                this.TxtLearningResultName.Text = "";
            }
            else
            {
                RedirectPage();
            }
        }

        protected void BtnCancelAdd_Click(object sender, ImageClickEventArgs e)
        {
            RedirectPage();
        }

        protected void BtnSelect_Click(object sender, ImageClickEventArgs e)
        {
            List<Category_LearningAptitude> sltHocLucs = new List<Category_LearningAptitude>();
            int iSltLearningAptitudeId = Int32.Parse(DdlHocLucAdd.SelectedValue);
            if(iSltLearningAptitudeId == 0)
            {
                LearningAptitudeBL hocLucBL = new LearningAptitudeBL(UserSchool);
                sltHocLucs = hocLucBL.GetListHocLuc(false);
            }
            else
            {
                sltHocLucs.Add(new Category_LearningAptitude{
                    LearningAptitudeId = iSltLearningAptitudeId,
                    LearningAptitudeName = DdlHocLucAdd.SelectedItem.Text
                });
            }

            int iSltConductId = Int32.Parse(DdlHanhKiemAdd.SelectedValue);
            List<SelectedHocLucHanhKiem> sltHocLucHanhKiems = SelectedHocLucHanhKiems;
            foreach(Category_LearningAptitude sltHocLuc in sltHocLucs)
            {
                SelectedHocLucHanhKiem sltHocLucHanhKiem = new SelectedHocLucHanhKiem();
                sltHocLucHanhKiem.ThuTu = sltHocLucHanhKiems.Count;
                sltHocLucHanhKiem.LearningAptitudeId = sltHocLuc.LearningAptitudeId; 
                sltHocLucHanhKiem.LearningAptitudeName = sltHocLuc.LearningAptitudeName;
                sltHocLucHanhKiem.ConductId = iSltConductId;
                sltHocLucHanhKiem.ConductName = DdlHanhKiemAdd.SelectedItem.Text;
                sltHocLucHanhKiems.Add(sltHocLucHanhKiem);
            }

            SelectedHocLucHanhKiems = sltHocLucHanhKiems;
            BindRptDescriptionDanhHieu();
        }

        protected void BtnOKDeleteItem_Click(object sender, ImageClickEventArgs e)
        {            
            List<SelectedHocLucHanhKiem> sltHocLucHanhKiems = SelectedHocLucHanhKiems;
            int i = 0;
            while (i < sltHocLucHanhKiems.Count)
            {
                SelectedHocLucHanhKiem sltHocLucHanhKiem = sltHocLucHanhKiems[i];

                if (this.HdfThuTu.Value.ToString() == sltHocLucHanhKiem.ThuTu.ToString())
                {
                    sltHocLucHanhKiems.RemoveAt(i);
                    break;
                }
            }

            SelectedHocLucHanhKiems = sltHocLucHanhKiems;
            //int GradeId = Int32.Parse(this.HdfGradeId.Value);
            //grades.DeleteKhoiLop(GradeId);
            BindRptDescriptionDanhHieu();
        }

        #endregion

        #region Repeater event handlers
        protected void RptDescriptionDanhHieu_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "CmdDeleteItem":
                    {
                        this.LblConfirmDelete.Text = "Bạn có chắc xóa khối lớp <b>" + e.CommandArgument + "</b> này không?";
                        ModalPopupExtender mPEDelete = (ModalPopupExtender)e.Item.FindControl("MPEDelete");
                        mPEDelete.Show();

                        HiddenField hdfThuTu = (HiddenField)e.Item.FindControl("HdfThuTu");
                        this.HdfThuTu.Value = hdfThuTu.Value;

                        this.HdfRptDescriptionDanhHieuMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        //int GradeId = Int32.Parse(e.CommandArgument.ToString());
                        //Category_Grade khoiLop = grades.GetKhoiLop(GradeId);

                        //TxtSuaGradeName.Text = khoiLop.GradeName;
                        //TxtOrderEdit.Text = khoiLop.ThuTuHienThi.ToString();

                        //ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        //mPEEdit.Show();

                        //this.HdfRptKhoiLopMPEEdit.Value = mPEEdit.ClientID;
                        //this.HdfGradeId.Value = GradeId.ToString();

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion

        #region Methods
        private void BindRptDescriptionDanhHieu()
        {
            RptDescriptionDanhHieu.DataSource = SelectedHocLucHanhKiems;
            RptDescriptionDanhHieu.DataBind();

            bool bDisplayData = (SelectedHocLucHanhKiems.Count != 0);
            PnlPopupConfirmDelete.Visible = bDisplayData;
        }

        private void BindDropdownlists()
        {
            BindDDLHocLuc();
            BindDDLHanhKiem();
        }

        private void BindDDLHocLuc()
        {
            LearningAptitudeBL hocLucBL = new LearningAptitudeBL(UserSchool);
            List<Category_LearningAptitude> hocLucs = hocLucBL.GetListHocLuc(true);
            DdlHocLucAdd.DataSource = hocLucs;
            DdlHocLucAdd.DataValueField = "LearningAptitudeId";
            DdlHocLucAdd.DataTextField = "LearningAptitudeName";
            DdlHocLucAdd.DataBind();
        }

        private void BindDDLHanhKiem()
        {
            ConductBL HanhKiemBL = new ConductBL(UserSchool);
            List<Category_Conduct> HanhKiems = HanhKiemBL.GetListConducts(false);
            DdlHanhKiemAdd.DataSource = HanhKiems;
            DdlHanhKiemAdd.DataValueField = "ConductId";
            DdlHanhKiemAdd.DataTextField = "ConductName";
            DdlHanhKiemAdd.DataBind();
        }

        private void RedirectPage()
        {
            Response.Redirect("danhmucdanhhieu.aspx");
            Session.Remove("SltHocLucHanhKiems");
        }
        #endregion
    }
}