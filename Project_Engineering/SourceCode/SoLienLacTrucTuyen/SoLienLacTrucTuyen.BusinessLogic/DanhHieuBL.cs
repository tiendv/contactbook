using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class DanhHieuBL: BaseBL
    {
        private DanhHieuDA danhHieuDA;

        public DanhHieuBL(School_School school)
            : base(school)
        {
            danhHieuDA = new DanhHieuDA(school);
        }

        public void InsertDanhHieu(string LearningResultName, Dictionary<int, int> dicHanhKiemNHocLuc)
        {
            danhHieuDA.InsertDanhHieu(LearningResultName, dicHanhKiemNHocLuc);
        }

        public void UpdateDanhHieu(int LearningResultId, string LearningResultName)
        {
            danhHieuDA.UpdateDanhHieu(LearningResultId, LearningResultName);
        }

        public void UpdateDanhHieu(int LearningResultId, Dictionary<int, int> dicHanhKiemNHocLuc)
        {
            danhHieuDA.UpdateDanhHieu(LearningResultId, dicHanhKiemNHocLuc);
        }

        public void DeleteDanhHieu(int LearningResultId)
        {
            danhHieuDA.DeleteDanhHieu(LearningResultId);
        }

        public void DeleteChiTietDanhHieu(int LearningResultId, int LearningAptitudeId, int ConductId)
        {
            danhHieuDA.DeleteChiTietDanhHieu(LearningResultId, LearningAptitudeId, ConductId);
        }

        public bool DanhHieuExists(int exceptedLearningResultId, string LearningResultName)
        {
            return danhHieuDA.DanhHieuExists(exceptedLearningResultId, LearningResultName);
        }

        public bool DanhHieuExists(string LearningResultName)
        {
            return danhHieuDA.DanhHieuExists(LearningResultName);
        }

        public List<Category_LearningResult> GetListDanhHieus(string LearningResultName, 
            int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            if (String.Compare(LearningResultName, "tất cả", true) == 0 || LearningResultName == "")
            {
                return danhHieuDA.GetListDanhHieus(pageCurrentIndex, pageSize, out totalRecord);
            }
            else
            {
                return danhHieuDA.GetListDanhHieus(LearningResultName, pageCurrentIndex, pageSize, out totalRecord);
            }
        }

        public Category_LearningResult GetDanhHieu(int LearningResultId)
        {
            return danhHieuDA.GetDanhHieu(LearningResultId);
        }

        public Category_LearningResult GetLearningResult(Category_Conduct conduct, Category_LearningAptitude learningAptitude)
        {
            return danhHieuDA.GetLearningResult(conduct, learningAptitude);
        }

        public bool CanDeleteDanhHieu(int LearningResultId)
        {
            return danhHieuDA.CanDeleteDanhHieu(LearningResultId);
        }
    }
}
