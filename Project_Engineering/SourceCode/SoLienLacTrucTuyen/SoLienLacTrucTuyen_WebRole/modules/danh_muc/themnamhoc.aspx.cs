﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessLogic;
using System.Text;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class CategoryAddYear : BaseContentPage
    {
        #region Field(s)
        SystemConfigBL systemConfigBL;
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

            systemConfigBL = new SystemConfigBL(UserSchool);
            int iPresentYear = DateTime.Now.Year;
            Configuration_Year lastedYear = systemConfigBL.GetLastedYear();
            if (lastedYear != null)
            {
                if (lastedYear.BeginFirstTermDate.Year >= iPresentYear)
                {

                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(iPresentYear);
                    stringBuilder.Append(" - ");
                    stringBuilder.Append(iPresentYear + 1);
                    LblYearName.Text = stringBuilder.ToString();
                }
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(iPresentYear);
                stringBuilder.Append(" - ");
                stringBuilder.Append(iPresentYear + 1);
                LblYearName.Text = stringBuilder.ToString();
            }

            if (!Page.IsPostBack)
            {
                RetrieveSelectionSession();
            }
        }
        #endregion

        #region Button event handlers
        protected void BtnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            String strYearName = LblYearName.Text.Trim();
            DateTime dtFirstTermBeginDate = DateTime.Parse(TxtFirstTermBeginDate.Text);
            DateTime dtFirstTermEndDate = DateTime.Parse(TxtFirstTermEndDate.Text);
            DateTime dtSecondTermBeginDate = DateTime.Parse(TxtSecondTermBeginDate.Text);
            DateTime dtSecondTermEndDate = DateTime.Parse(TxtSecondTermEndDate.Text);

            systemConfigBL.InsertYear(strYearName, dtFirstTermBeginDate, dtFirstTermEndDate, dtSecondTermBeginDate, dtSecondTermEndDate);

            Session[AppConstant.SESSION_CURRENT_YEAR] = systemConfigBL.GetLastedYear();

            RedirectToPrevPage();
        }

        protected void BtnCancel_Click(object sender, ImageClickEventArgs e)
        {
            RedirectToPrevPage();
        }
        #endregion

        #region Method(s)
        private void SaveSelectionSessions()
        {
            string strYearName = (string)ViewState[AppConstant.VIEWSTATE_SELECTED_YEARNAME];
            AddSession(AppConstant.SESSION_SELECTED_YEARNAME, strYearName);
        }

        private void RetrieveSelectionSession()
        {
            if (CheckSessionKey(AppConstant.SESSION_SELECTED_YEARNAME))
            {
                ViewState[AppConstant.VIEWSTATE_SELECTED_YEARNAME] = (string)GetSession(AppConstant.SESSION_SELECTED_YEARNAME);
                RemoveSession(AppConstant.SESSION_SELECTED_YEARNAME);
            }
        }

        private void RedirectToPrevPage()
        {
            SaveSelectionSessions();
            Response.Redirect(AppConstant.PAGEPATH_CATEGORY_YEARLIST);
        }

        private bool ValidateInputs()
        {
            bool bValid = true;

            if (CheckUntils.IsNullOrBlank(TxtFirstTermBeginDate.Text.Trim()))
            {
                RequiredFirstTermBeginDate.IsValid = false;
                bValid = false;
            }

            if (CheckUntils.IsNullOrBlank(TxtFirstTermEndDate.Text.Trim()))
            {
                RequiredFirstTermEndDate.IsValid = false;
                bValid = false;
            }

            if (CheckUntils.IsNullOrBlank(TxtSecondTermBeginDate.Text.Trim()))
            {
                RequiredSecondTermBeginDate.IsValid = false;
                bValid = false;
            }

            if (CheckUntils.IsNullOrBlank(TxtSecondTermEndDate.Text.Trim()))
            {
                RequiredSecondTermEndDate.IsValid = false;
                bValid = false;
            }

            DateTime dtFirstTermBeginDate = DateTime.Parse(TxtFirstTermBeginDate.Text);
            DateTime dtFirstTermEndDate = DateTime.Parse(TxtFirstTermEndDate.Text);
            DateTime dtSecondTermBeginDate = DateTime.Parse(TxtSecondTermBeginDate.Text);
            DateTime dtSecondTermEndDate = DateTime.Parse(TxtSecondTermEndDate.Text);

            if (CheckUntils.CompareDateWithoutHMS(dtFirstTermBeginDate, dtFirstTermEndDate) >= 0)
            {
                CompareToFirstTermStartDate.IsValid = false;
                bValid = false;
            }

            if (CheckUntils.CompareDateWithoutHMS(dtFirstTermEndDate, dtSecondTermBeginDate) >= 0)
            {
                CompareToFirstTermEndDate.IsValid = false;
                bValid = false;
            }

            if (CheckUntils.CompareDateWithoutHMS(dtSecondTermBeginDate, dtSecondTermEndDate) >= 0)
            {
                CompareToSecondTermStartDate.IsValid = false;
                bValid = false;
            }

            return bValid;
        }
        #endregion
    }
}