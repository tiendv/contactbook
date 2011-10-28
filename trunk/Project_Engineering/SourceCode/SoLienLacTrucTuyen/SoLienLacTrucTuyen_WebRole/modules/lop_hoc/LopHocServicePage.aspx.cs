using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using SoLienLacTrucTuyen.BusinessLogic;
using SoLienLacTrucTuyen.DataAccess;
using System.Collections.Specialized;
using SoLienLacTrucTuyen;

namespace SoLienLacTrucTuyen_WebRole
{
    public partial class LopHocServicePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }       

        [WebMethod]
        public static List<LopHoc_Lop> GetListLopHoc(int maNganhHoc, int maKhoiLop, int maNamHoc)
        {
            LopHocBL lophocBL = new LopHocBL();
            return lophocBL.GetListLopHoc(maNganhHoc, maKhoiLop, maNamHoc);
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static AjaxControlToolkit.CascadingDropDownNameValue[] GetTenLopHoc(string knownCategoryValues, string category) 
        {            
            //StringDictionary kv = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            //int maNganhHoc;
            //if (!kv.ContainsKey("MaNganhHoc") || !Int32.TryParse(kv["MaNganhHoc"], out maNganhHoc))
            //{
            //    return null;
            //}
            
            //LopHocBL lophocBL = new LopHocBL();
            //List<LopHoc_Lop> lstLopHoc = lophocBL.GetListLopHocByNganhHoc(maNganhHoc);

            List<AjaxControlToolkit.CascadingDropDownNameValue> values = new List<AjaxControlToolkit.CascadingDropDownNameValue>();
            //foreach (LopHoc_Lop lop in lstLopHoc)
            //{
            //    values.Add(new AjaxControlToolkit.CascadingDropDownNameValue(
            //        (string)lop.TenLopHoc,
            //        (lop.MaLopHoc).ToString()));
            //}

            return values.ToArray();
        }

        [WebMethod]
        public static bool LopHocExists(string tenLopHoc, int maNamHoc)
        {
            tenLopHoc = Uri.UnescapeDataString(tenLopHoc);
            LopHocBL lopHocBL = new LopHocBL();
            return lopHocBL.LopHocExists(tenLopHoc, maNamHoc);
        }

        [WebMethod]
        public static bool LopHocExists(int maLopHoc, string tenLopHoc)
        {
            tenLopHoc = Uri.UnescapeDataString(tenLopHoc);
            LopHocBL lopHocBL = new LopHocBL();
            return lopHocBL.LopHocExists(maLopHoc, tenLopHoc);
        }

        [WebMethod(EnableSession=true)]
        public static void AddCheckedMonHocTKBSang(string maMonHoc)
        {
            string userName = HttpContext.Current.Session["username"].ToString();
            int iMaMonHoc = Int32.Parse(maMonHoc);
            List<int> lstCheckedMonHocSang = new List<int>();
            if (HttpContext.Current.Session[userName + "LstCheckedMonHocSang"] != null)
            {
                lstCheckedMonHocSang = (List<int>)HttpContext.Current.Session[userName + "LstCheckedMonHocSang"];
            }
            lstCheckedMonHocSang.Add(iMaMonHoc);
            HttpContext.Current.Session[userName + "LstCheckedMonHocSang"] = lstCheckedMonHocSang;
        }

        [WebMethod(EnableSession=true)]
        public static void RemoveCheckedMonHocTKBSang(string maMonHoc)
        {
            string userName = HttpContext.Current.Session["username"].ToString();
            int iMaMonHoc = Int32.Parse(maMonHoc);
            List<int> lstCheckedMonHocSang = new List<int>();
            if (HttpContext.Current.Session[userName + "LstCheckedMonHocSang"] != null)
            {
                lstCheckedMonHocSang = (List<int>)HttpContext.Current.Session[userName + "LstCheckedMonHocSang"];
            }
            lstCheckedMonHocSang.Remove(iMaMonHoc);
            HttpContext.Current.Session[userName + "LstCheckedMonHocSang"] = lstCheckedMonHocSang;
        }

        [WebMethod(EnableSession=true)]
        public static void AddCheckedMonHocTKBChieu(string maMonHoc)
        {
            string userName = HttpContext.Current.Session["username"].ToString();
            int iMaMonHoc = Int32.Parse(maMonHoc);
            List<int> lstCheckedMonHocChieu = new List<int>();
            if (HttpContext.Current.Session[userName + "LstCheckedMonHocChieu"] != null)
            {
                lstCheckedMonHocChieu = (List<int>)HttpContext.Current.Session[userName + "LstCheckedMonHocChieu"];
            }
            lstCheckedMonHocChieu.Add(iMaMonHoc);
            HttpContext.Current.Session[userName + "LstCheckedMonHocChieu"] = lstCheckedMonHocChieu;
        }

        [WebMethod(EnableSession=true)]
        public static void RemoveCheckedMonHocTKBChieu(string maMonHoc)
        {
            string userName = HttpContext.Current.Session["username"].ToString();
            int iMaMonHoc = Int32.Parse(maMonHoc);
            List<int> lstCheckedMonHocChieu = new List<int>();
            if (HttpContext.Current.Session[userName + "LstCheckedMonHocChieu"] != null)
            {
                lstCheckedMonHocChieu = (List<int>)HttpContext.Current.Session[userName + "LstCheckedMonHocChieu"];
            }
            lstCheckedMonHocChieu.Remove(iMaMonHoc);
            HttpContext.Current.Session[userName + "LstCheckedMonHocChieu"] = lstCheckedMonHocChieu;
        }
    }
}