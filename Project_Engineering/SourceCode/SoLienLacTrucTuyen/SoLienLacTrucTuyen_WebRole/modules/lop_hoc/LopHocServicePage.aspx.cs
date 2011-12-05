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

        //[WebMethod]
        //public static List<Class_Class> GetListLopHoc(int FacultyId, int GradeId, int YearId)
        //{
        //    ClassBL lophocBL = new ClassBL(UserSchool);
        //    return lophocBL.GetListLopHoc(FacultyId, GradeId, YearId);
        //}

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static AjaxControlToolkit.CascadingDropDownNameValue[] GetClassName(string knownCategoryValues, string category) 
        {            
            //StringDictionary kv = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            //int FacultyId;
            //if (!kv.ContainsKey("FacultyId") || !Int32.TryParse(kv["FacultyId"], out FacultyId))
            //{
            //    return null;
            //}
            
            //LopHocBL lophocBL = new LopHocBL(UserSchool);
            //List<Class_Class> lstLopHoc = lophocBL.GetListLopHocByNganhHoc(FacultyId);

            List<AjaxControlToolkit.CascadingDropDownNameValue> values = new List<AjaxControlToolkit.CascadingDropDownNameValue>();
            //foreach (Class_Class lop in lstLopHoc)
            //{
            //    values.Add(new AjaxControlToolkit.CascadingDropDownNameValue(
            //        (string)lop.ClassName,
            //        (lop.ClassId).ToString()));
            //}

            return values.ToArray();
        }

        //[WebMethod]
        //public static bool LopHocExists(string ClassName, int YearId)
        //{
        //    ClassName = Uri.UnescapeDataString(ClassName);
        //    ClassBL lopHocBL = new ClassBL(UserSchool);
        //    Configuration_Year year = new Configuration_Year();
        //    year.YearId = YearId;
        //    return lopHocBL.ClassNameExists(ClassName, year);
        //}

        //[WebMethod]
        //public static bool LopHocExists(int ClassId, string ClassName)
        //{
        //    ClassName = Uri.UnescapeDataString(ClassName);
        //    ClassBL lopHocBL = new ClassBL(UserSchool);
        //    Class_Class Class = new Class_Class();
        //    Class.ClassId = ClassId;
        //    return lopHocBL.ClassNameExists(
        //}

        [WebMethod(EnableSession=true)]
        public static void AddCheckedMonHocTKBSang(string SubjectId)
        {
            string userName = HttpContext.Current.Session["username"].ToString();
            int iSubjectId = Int32.Parse(SubjectId);
            List<int> lstCheckedMonHocSang = new List<int>();
            if (HttpContext.Current.Session[userName + "LstCheckedMonHocSang"] != null)
            {
                lstCheckedMonHocSang = (List<int>)HttpContext.Current.Session[userName + "LstCheckedMonHocSang"];
            }
            lstCheckedMonHocSang.Add(iSubjectId);
            HttpContext.Current.Session[userName + "LstCheckedMonHocSang"] = lstCheckedMonHocSang;
        }

        [WebMethod(EnableSession=true)]
        public static void RemoveCheckedMonHocTKBSang(string SubjectId)
        {
            string userName = HttpContext.Current.Session["username"].ToString();
            int iSubjectId = Int32.Parse(SubjectId);
            List<int> lstCheckedMonHocSang = new List<int>();
            if (HttpContext.Current.Session[userName + "LstCheckedMonHocSang"] != null)
            {
                lstCheckedMonHocSang = (List<int>)HttpContext.Current.Session[userName + "LstCheckedMonHocSang"];
            }
            lstCheckedMonHocSang.Remove(iSubjectId);
            HttpContext.Current.Session[userName + "LstCheckedMonHocSang"] = lstCheckedMonHocSang;
        }

        [WebMethod(EnableSession=true)]
        public static void AddCheckedMonHocTKBChieu(string SubjectId)
        {
            string userName = HttpContext.Current.Session["username"].ToString();
            int iSubjectId = Int32.Parse(SubjectId);
            List<int> lstCheckedMonHocChieu = new List<int>();
            if (HttpContext.Current.Session[userName + "LstCheckedMonHocChieu"] != null)
            {
                lstCheckedMonHocChieu = (List<int>)HttpContext.Current.Session[userName + "LstCheckedMonHocChieu"];
            }
            lstCheckedMonHocChieu.Add(iSubjectId);
            HttpContext.Current.Session[userName + "LstCheckedMonHocChieu"] = lstCheckedMonHocChieu;
        }

        [WebMethod(EnableSession=true)]
        public static void RemoveCheckedMonHocTKBChieu(string SubjectId)
        {
            string userName = HttpContext.Current.Session["username"].ToString();
            int iSubjectId = Int32.Parse(SubjectId);
            List<int> lstCheckedMonHocChieu = new List<int>();
            if (HttpContext.Current.Session[userName + "LstCheckedMonHocChieu"] != null)
            {
                lstCheckedMonHocChieu = (List<int>)HttpContext.Current.Session[userName + "LstCheckedMonHocChieu"];
            }
            lstCheckedMonHocChieu.Remove(iSubjectId);
            HttpContext.Current.Session[userName + "LstCheckedMonHocChieu"] = lstCheckedMonHocChieu;
        }
    }
}