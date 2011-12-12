using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class DanhHieuDA : BaseDA
    {
        public DanhHieuDA(School_School school)
            : base(school)
        {
        }

        public void InsertDanhHieu(string LearningResultName, Dictionary<int, int> dicHanhKiemNHocLuc)
        {
            Category_LearningResult danhHieu = new Category_LearningResult
            {
                LearningResultName = LearningResultName
            };
            db.Category_LearningResults.InsertOnSubmit(danhHieu);
            db.SubmitChanges();

            danhHieu = GetLastedDanhHieu();
            foreach (KeyValuePair<int, int> pair in dicHanhKiemNHocLuc)
            {
                Category_DetailedLearningResult ctDanhHieu = new Category_DetailedLearningResult
                {
                    LearningResultId = danhHieu.LearningResultId,
                    LearningAptitudeId = pair.Key,
                    ConductId = pair.Value
                };
                db.Category_DetailedLearningResults.InsertOnSubmit(ctDanhHieu);
            }

            db.SubmitChanges();
        }

        public void UpdateDanhHieu(int LearningResultId, Dictionary<int, int> dicHanhKiemNHocLuc)
        {
            IQueryable<Category_DetailedLearningResult> ctDanhHieus = from ctDHieu in db.Category_DetailedLearningResults
                                                                      where ctDHieu.LearningResultId == LearningResultId
                                                                      select ctDHieu;
            foreach (Category_DetailedLearningResult ctDanhHieu in ctDanhHieus)
            {
                db.Category_DetailedLearningResults.DeleteOnSubmit(ctDanhHieu);
            }
            db.SubmitChanges();

            foreach (KeyValuePair<int, int> pair in dicHanhKiemNHocLuc)
            {
                Category_DetailedLearningResult ctDanhHieu = new Category_DetailedLearningResult
                {
                    LearningResultId = LearningResultId,
                    LearningAptitudeId = pair.Key,
                    ConductId = pair.Value
                };
                db.Category_DetailedLearningResults.InsertOnSubmit(ctDanhHieu);
            }

            db.SubmitChanges();
        }

        public void DeleteDanhHieu(int LearningResultId)
        {
            Category_LearningResult danhHieu = (from dHieu in db.Category_LearningResults
                                                where dHieu.LearningResultId == LearningResultId
                                                select dHieu).First();
            db.Category_LearningResults.DeleteOnSubmit(danhHieu);
            db.SubmitChanges();
        }

        public void DeleteChiTietDanhHieu(int LearningResultId, int LearningAptitudeId, int ConductId)
        {
            Category_DetailedLearningResult ctDanhHieu = (from ctDHieu in db.Category_DetailedLearningResults
                                                          where ctDHieu.LearningResultId == LearningResultId
                                                             && ctDHieu.LearningAptitudeId == LearningAptitudeId
                                                             && ctDHieu.ConductId == ConductId
                                                          select ctDHieu).First();
            db.Category_DetailedLearningResults.DeleteOnSubmit(ctDanhHieu);
            db.SubmitChanges();
        }

        public Category_LearningResult GetLastedDanhHieu()
        {
            IQueryable<Category_LearningResult> danhHieus = from danhHieu in db.Category_LearningResults
                                                            select danhHieu;
            if (danhHieus.Count() != 0)
            {
                return danhHieus.OrderByDescending(danhHieu => danhHieu.LearningResultId).First();
            }
            else
            {
                return null;
            }
        }

        public string GetLearningResultName(int LearningAptitudeId, int ConductId)
        {
            IQueryable<Category_LearningResult> danhHieus = from ctDanhHieu in db.Category_DetailedLearningResults
                                                            join danhHieu in db.Category_LearningResults
                                                               on ctDanhHieu.LearningResultId equals danhHieu.LearningResultId
                                                            where ctDanhHieu.LearningAptitudeId == LearningAptitudeId
                                                               && ctDanhHieu.ConductId == ConductId
                                                            select danhHieu;
            if (danhHieus.Count() != 0)
            {
                return danhHieus.First().LearningResultName;
            }
            else
            {
                return "";
            }
        }

        public bool DanhHieuExists(int exceptedLearningResultId, string LearningResultName)
        {
            IQueryable<Category_LearningResult> danhHieus = from danhHieu in db.Category_LearningResults
                                                            where danhHieu.LearningResultName == LearningResultName
                                                               && danhHieu.LearningResultId != exceptedLearningResultId
                                                            select danhHieu;
            if (danhHieus.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DanhHieuExists(string LearningResultName)
        {
            IQueryable<Category_LearningResult> danhHieus = from danhHieu in db.Category_LearningResults
                                                            where danhHieu.LearningResultName == LearningResultName
                                                            select danhHieu;
            if (danhHieus.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void UpdateDanhHieu(int LearningResultId, string LearningResultName)
        {
            IQueryable<Category_LearningResult> danhHieus = from danhHieu in db.Category_LearningResults
                                                            where danhHieu.LearningResultId == LearningResultId
                                                            select danhHieu;
            danhHieus.First().LearningResultName = LearningResultName;
            db.SubmitChanges();
        }

        public List<Category_LearningResult> GetListDanhHieus(string LearningResultName,
            int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            IQueryable<Category_LearningResult> danhHieus = from danhHieu in db.Category_LearningResults
                                                            where danhHieu.LearningResultName == LearningResultName
                                                            select danhHieu;
            totalRecord = danhHieus.Count();
            if (totalRecord != 0)
            {
                return danhHieus.OrderBy(danhHieu => danhHieu.LearningResultName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<Category_LearningResult>();
            }
        }

        public List<Category_LearningResult> GetListDanhHieus(int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            IQueryable<Category_LearningResult> danhHieus = from danhHieu in db.Category_LearningResults
                                                            select danhHieu;
            totalRecord = danhHieus.Count();
            if (totalRecord != 0)
            {
                return danhHieus.OrderBy(danhHieu => danhHieu.LearningResultName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<Category_LearningResult>();
            }
        }

        public Category_LearningResult GetDanhHieu(int LearningResultId)
        {
            IQueryable<Category_LearningResult> danhHieus = from danhHieu in db.Category_LearningResults
                                                            where danhHieu.LearningResultId == LearningResultId
                                                            select danhHieu;
            if (danhHieus.Count() != 0)
            {
                return danhHieus.First();
            }
            else
            {
                return null;
            }
        }

        public bool CanDeleteDanhHieu(int LearningResultId)
        {
            return true;
        }

        public Category_LearningResult GetLearningResult(Category_Conduct conduct, Category_LearningAptitude learningAptitude)
        {
            Category_LearningResult learningResult = null;
            IQueryable<Category_LearningResult> iqLearningResult = from detailedLearningResult in db.Category_DetailedLearningResults
                                                                   where detailedLearningResult.ConductId == conduct.ConductId
                                                                   && detailedLearningResult.LearningAptitudeId == learningAptitude.LearningAptitudeId
                                                                   select detailedLearningResult.Category_LearningResult;
            if (iqLearningResult.Count() != 0)
            {
                learningResult = iqLearningResult.First();
            }

            return learningResult;
        }
    }
}
