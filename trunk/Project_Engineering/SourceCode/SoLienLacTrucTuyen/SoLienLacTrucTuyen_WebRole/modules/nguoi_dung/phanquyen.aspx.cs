using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class AuthenticationPage : BaseContentPage
    {
        #region Fields
        AuthorizationBL authorizationBL;
        private RoleBL roleBL;
        private PhanQuyenBL phanQuyenBL;
        #endregion

        #region Page event handlers
        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            if (isAccessDenied)
            {
                return;
            }

            authorizationBL = new AuthorizationBL(UserSchool);
            roleBL = new RoleBL(UserSchool);
            phanQuyenBL = new PhanQuyenBL(UserSchool);

            if (!Page.IsPostBack)
            {
                BindDropDownList();
                BindRepeater();
            }
        }
        #endregion

        #region Repeater event handlers
        protected void RptPhanQuyen_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.DataItem != null)
                {
                    TabularPhanQuyen tbPhanQuyen = (TabularPhanQuyen)e.Item.DataItem;
                    Repeater RptPhanQuyen = (Repeater)e.Item.FindControl("RptChiTietPhanQuyen");
                    RptPhanQuyen.DataSource = tbPhanQuyen.ListChiTietPhanQuyens;
                    RptPhanQuyen.DataBind();

                    FunctionsBL functionBL = new FunctionsBL();
                    if (tbPhanQuyen.FunctionCategoryName == functionBL.GetHomePageFunctionCategory())
                    {
                        foreach (RepeaterItem item in RptPhanQuyen.Items)
                        {
                            if (item.ItemType == ListItemType.Item 
                                || item.ItemType == ListItemType.AlternatingItem)
                            {
                                CheckBox ckbxView = (CheckBox)item.FindControl("CkbxView");
                                ckbxView.Enabled = false;

                                item.FindControl("CkbxAdd").Visible = false;
                                item.FindControl("CkbxModify").Visible = false;
                                item.FindControl("CkbxDelete").Visible = false;
                            }
                        }
                    }

                    Guid selectedRole = new Guid(DdlRoles.SelectedValue.ToString());
                    if (selectedRole == authorizationBL.GetRoleAdminId())
                    {
                        if (functionBL.GetAdminOnlyFunctionCategories().Contains(tbPhanQuyen.FunctionCategoryName))
                        {
                            foreach (RepeaterItem item in RptPhanQuyen.Items)
                            {
                                if (item.ItemType == ListItemType.Item
                                    || item.ItemType == ListItemType.AlternatingItem)
                                {
                                    CheckBox ckbxView = (CheckBox)item.FindControl("CkbxView");
                                    ckbxView.Enabled = false;
                                    CheckBox ckbxAdd = (CheckBox)item.FindControl("CkbxAdd");
                                    ckbxAdd.Enabled = false;
                                    CheckBox ckbxModify = (CheckBox)item.FindControl("CkbxModify");
                                    ckbxModify.Enabled = false;
                                    CheckBox ckbxDelete = (CheckBox)item.FindControl("CkbxDelete");
                                    ckbxDelete.Enabled = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            BindRepeater();
        }
        
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            Guid role = new Guid(DdlRoles.SelectedValue);
            List<TabularChiTietPhanQuyen> lstTbChiTietPhanQuyens = new List<TabularChiTietPhanQuyen>();

            foreach (RepeaterItem rptItemPhanQuyen in RptPhanQuyen.Items)
            {
                if (rptItemPhanQuyen.ItemType == ListItemType.Item 
                    || rptItemPhanQuyen.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater RptChiTietPhanQuyen = (Repeater)rptItemPhanQuyen.FindControl("RptChiTietPhanQuyen");
                    foreach (RepeaterItem rptItemChiTietPhanQuyen in RptChiTietPhanQuyen.Items)
                    {
                        if (rptItemChiTietPhanQuyen.ItemType == ListItemType.Item 
                            || rptItemChiTietPhanQuyen.ItemType == ListItemType.AlternatingItem)
                        {
                            TabularChiTietPhanQuyen phanQuyenChucNang = new TabularChiTietPhanQuyen();

                            HiddenField hfFunctionId = (HiddenField)rptItemChiTietPhanQuyen.FindControl("HfFunctionId");
                            phanQuyenChucNang.FunctionId = Int32.Parse(hfFunctionId.Value);

                            CheckBox CkbxView = (CheckBox)rptItemChiTietPhanQuyen.FindControl("CkbxView");
                            phanQuyenChucNang.ViewAccessibility = CkbxView.Checked;

                            CheckBox CkbxAdd = (CheckBox)rptItemChiTietPhanQuyen.FindControl("CkbxAdd");
                            phanQuyenChucNang.AddAccessibility = CkbxAdd.Checked;

                            CheckBox CkbxModify = (CheckBox)rptItemChiTietPhanQuyen.FindControl("CkbxModify");
                            phanQuyenChucNang.ModifyAccessibility = CkbxModify.Checked;

                            CheckBox CkbxDelete = (CheckBox)rptItemChiTietPhanQuyen.FindControl("CkbxDelete");
                            phanQuyenChucNang.DeleteAccessibility = CkbxDelete.Checked;

                            lstTbChiTietPhanQuyens.Add(phanQuyenChucNang);
                        }
                    }
                }
            }

            phanQuyenBL.PhanQuyen(role, lstTbChiTietPhanQuyens);

            BindRepeater();            
        }
        #endregion

        #region Methods
        private void BindDropDownList()
        {
            List<TabularRole> tabularRoles = authorizationBL.GetAuthorizedRoles();
            DdlRoles.DataSource = tabularRoles;
            DdlRoles.DataValueField = "RoleId";
            DdlRoles.DataTextField = "DisplayedName";
            DdlRoles.DataBind();
        }

        private void BindRepeater()
        {
            Guid selectedRole = new Guid(DdlRoles.SelectedValue.ToString());
            List<TabularPhanQuyen> lstTbPhanQuyens;
            lstTbPhanQuyens = phanQuyenBL.GetListPhanQuyens(selectedRole);
            RptPhanQuyen.DataSource = lstTbPhanQuyens;
            RptPhanQuyen.DataBind();
        }
        #endregion
    }
}