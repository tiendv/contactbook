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
            public int MaHocLuc { get; set; }
            public string TenHocLuc { get; set; }
            public int MaHanhKiem { get; set; }
            public string TenHanhKiem { get; set; }
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

            danhHieuBL = new DanhHieuBL();

            if (!Page.IsPostBack)
            {
                Session.Remove("SltHocLucHanhKiems");

                BindRptMoTaDanhHieu();
                BindDropdownlists();
            }
        }        
        #endregion

        #region Button click event handlers
        protected void BtnSaveAdd_Click(object sender, ImageClickEventArgs e)
        {
            string tenDanhHieu = this.TxtTenDanhHieu.Text.Trim();

            if (tenDanhHieu == "")
            {
                TenDanhHieuRequiredAdd.IsValid = false;
                TxtTenDanhHieu.Focus();
                return;
            }
            else
            {
                if (danhHieuBL.DanhHieuExists(tenDanhHieu))
                {
                    TenDanhHieuValidatorAdd.IsValid = false;
                    TxtTenDanhHieu.Focus();
                    return;
                }
            }

            danhHieuBL.InsertDanhHieu(tenDanhHieu, new Dictionary<int, int>());

            if (this.CkbAddAfterSave.Checked)
            {
                this.TxtTenDanhHieu.Text = "";
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
            List<DanhMuc_HocLuc> sltHocLucs = new List<DanhMuc_HocLuc>();
            int iSltMaHocLuc = Int32.Parse(DdlHocLucAdd.SelectedValue);
            if(iSltMaHocLuc == 0)
            {
                HocLucBL hocLucBL = new HocLucBL();
                sltHocLucs = hocLucBL.GetListHocLuc(false);
            }
            else
            {
                sltHocLucs.Add(new DanhMuc_HocLuc{
                    MaHocLuc = iSltMaHocLuc,
                    TenHocLuc = DdlHocLucAdd.SelectedItem.Text
                });
            }

            int iSltMaHanhKiem = Int32.Parse(DdlHanhKiemAdd.SelectedValue);
            List<SelectedHocLucHanhKiem> sltHocLucHanhKiems = SelectedHocLucHanhKiems;
            foreach(DanhMuc_HocLuc sltHocLuc in sltHocLucs)
            {
                SelectedHocLucHanhKiem sltHocLucHanhKiem = new SelectedHocLucHanhKiem();
                sltHocLucHanhKiem.ThuTu = sltHocLucHanhKiems.Count;
                sltHocLucHanhKiem.MaHocLuc = sltHocLuc.MaHocLuc; 
                sltHocLucHanhKiem.TenHocLuc = sltHocLuc.TenHocLuc;
                sltHocLucHanhKiem.MaHanhKiem = iSltMaHanhKiem;
                sltHocLucHanhKiem.TenHanhKiem = DdlHanhKiemAdd.SelectedItem.Text;
                sltHocLucHanhKiems.Add(sltHocLucHanhKiem);
            }

            SelectedHocLucHanhKiems = sltHocLucHanhKiems;
            BindRptMoTaDanhHieu();
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
            //int maKhoiLop = Int32.Parse(this.HdfMaKhoiLop.Value);
            //khoiLopBL.DeleteKhoiLop(maKhoiLop);
            BindRptMoTaDanhHieu();
        }

        #endregion

        #region Repeater event handlers
        protected void RptMoTaDanhHieu_ItemCommand(object source, RepeaterCommandEventArgs e)
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

                        this.HdfRptMoTaDanhHieuMPEDelete.Value = mPEDelete.ClientID;

                        break;
                    }
                case "CmdEditItem":
                    {
                        //int maKhoiLop = Int32.Parse(e.CommandArgument.ToString());
                        //DanhMuc_KhoiLop khoiLop = khoiLopBL.GetKhoiLop(maKhoiLop);

                        //TxtSuaTenKhoiLop.Text = khoiLop.TenKhoiLop;
                        //TxtOrderEdit.Text = khoiLop.ThuTuHienThi.ToString();

                        //ModalPopupExtender mPEEdit = (ModalPopupExtender)e.Item.FindControl("MPEEdit");
                        //mPEEdit.Show();

                        //this.HdfRptKhoiLopMPEEdit.Value = mPEEdit.ClientID;
                        //this.HdfMaKhoiLop.Value = maKhoiLop.ToString();

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
        private void BindRptMoTaDanhHieu()
        {
            RptMoTaDanhHieu.DataSource = SelectedHocLucHanhKiems;
            RptMoTaDanhHieu.DataBind();

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
            HocLucBL hocLucBL = new HocLucBL();
            List<DanhMuc_HocLuc> hocLucs = hocLucBL.GetListHocLuc(true);
            DdlHocLucAdd.DataSource = hocLucs;
            DdlHocLucAdd.DataValueField = "MaHocLuc";
            DdlHocLucAdd.DataTextField = "TenHocLuc";
            DdlHocLucAdd.DataBind();
        }

        private void BindDDLHanhKiem()
        {
            ConductBL HanhKiemBL = new ConductBL();
            List<DanhMuc_HanhKiem> HanhKiems = HanhKiemBL.GetListConducts(false);
            DdlHanhKiemAdd.DataSource = HanhKiems;
            DdlHanhKiemAdd.DataValueField = "MaHanhKiem";
            DdlHanhKiemAdd.DataTextField = "TenHanhKiem";
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