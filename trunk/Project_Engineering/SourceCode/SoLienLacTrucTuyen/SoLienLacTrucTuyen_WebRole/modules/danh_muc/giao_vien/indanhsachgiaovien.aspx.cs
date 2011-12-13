using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SoLienLacTrucTuyen.BusinessLogic;
using EContactBook.DataAccess;
using SoLienLacTrucTuyen.BusinessEntity;
using AjaxControlToolkit;
using CrystalDecisions;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;

namespace SoLienLacTrucTuyen_WebRole.Modules
{
    public partial class PrintTeacherPage : BaseContentPage
    {
        
        #region Variables
        string strTeacherID;
        string strTeacherName;
        TeacherBL teacherBL;
        private ReportDocument RptDocument = new ReportDocument();
        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            DataSet dsGiaoVien = new DataSet();
            if (!Page.IsPostBack)
            {
                List<TabularTeacher> tabularTeachers = new List<TabularTeacher>();
                double dTotalRecords;

                strTeacherID = (string)GetSession(AppConstant.SESSION_TEACHERID);
                RemoveSession(AppConstant.SESSION_TEACHERID);

                strTeacherName = (string)GetSession(AppConstant.SESSION_TEACHERNAME);
                RemoveSession(AppConstant.SESSION_TEACHERNAME);                

                // Get data
                teacherBL = new TeacherBL(UserSchool);
                tabularTeachers = teacherBL.GetTabularTeachers(strTeacherID, strTeacherName,
                1, 20, out dTotalRecords);

                DataTable dtSource = new DataTable();
                dtSource.Columns.Add("MaHienThiGiaoVien", Type.GetType("System.String"));
                dtSource.Columns.Add("HoTen", Type.GetType("System.String"));
                dtSource.Columns.Add("GioiTinh", Type.GetType("System.String"));
                dtSource.Columns.Add("NgaySinh", Type.GetType("System.String"));
                for (int i = 0; i < tabularTeachers.Count; i++)
                {
                    DataRow dr = dtSource.NewRow();
                    dr["MaHienThiGiaoVien"] = tabularTeachers[i].MaHienThiGiaoVien;
                    dr["HoTen"] = tabularTeachers[i].HoTen;                    
                    dr["GioiTinh"] = tabularTeachers[i].StringGioiTinh;
                    dr["NgaySinh"] = tabularTeachers[i].StringNgaySinh;
                    dtSource.Rows.Add(dr);
                }
                dsGiaoVien.Tables.Add(dtSource);
                RptDocument.Load(Server.MapPath("~/modules/report/Rpt_Teachers.rpt"));
                RptDocument.SetDataSource(dsGiaoVien.Tables[0]);
                RptDocument.SetParameterValue("School", UserSchool.SchoolName);
                Session["report1"] = RptDocument;
            }

            if (Session["report1"] != null)
            {
                RptDocument = (ReportDocument)Session["report1"];
            }
            Rpt_DanhSachGiaoVien.ReportSource = RptDocument;
            Rpt_DanhSachGiaoVien.DisplayGroupTree = false;
        }
    }
}