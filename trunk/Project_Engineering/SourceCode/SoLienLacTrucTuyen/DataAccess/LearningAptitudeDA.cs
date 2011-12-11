using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class LearningAptitudeDA : BaseDA
    {
        public LearningAptitudeDA(School_School school)
            : base(school)
        {
        }

        public void InsertHocLuc(Category_LearningAptitude hoclucEn)
        {
            db.Category_LearningAptitudes.InsertOnSubmit(hoclucEn);
            db.SubmitChanges();
        }

        public void UpdateHocLuc(Category_LearningAptitude hoclucEn)
        {
            Category_LearningAptitude hocluc = GetHocLuc(hoclucEn.LearningAptitudeId);
            hocluc.LearningAptitudeName = hocluc.LearningAptitudeName;
            hocluc.BeginAverageMark = hoclucEn.BeginAverageMark;
            hocluc.EndAverageMark = hoclucEn.EndAverageMark;
            db.SubmitChanges();
        }

        public void DeleteHocLuc(int LearningAptitudeId)
        {
            Category_LearningAptitude HocLuc = GetHocLuc(LearningAptitudeId);
            if (HocLuc != null)
            {
                db.Category_LearningAptitudes.DeleteOnSubmit(HocLuc);
                db.SubmitChanges();
            }
        }

        public Category_LearningAptitude GetHocLuc(int LearningAptitudeId)
        {
            IQueryable<Category_LearningAptitude> HocLucs = from hl in db.Category_LearningAptitudes
                                                            where hl.LearningAptitudeId == LearningAptitudeId
                                                            select hl;
            if (HocLucs.Count() != 0)
            {
                return HocLucs.First();
            }
            else
            {
                return null;
            }
        }

        public List<Category_LearningAptitude> GetListHocLuc()
        {
            IQueryable<Category_LearningAptitude> iqHocLuc = from hocLuc in db.Category_LearningAptitudes
                                                             select hocLuc;
            if (iqHocLuc.Count() != 0)
            {
                return iqHocLuc.OrderBy(hocLuc => hocLuc.LearningAptitudeName).ToList();
            }
            else
            {
                return new List<Category_LearningAptitude>();
            }
        }

        public List<Category_LearningAptitude> GetListHocLuc(int pageCurrentIndex, int pageSize)
        {
            IQueryable<Category_LearningAptitude> hocLucs = from hl in db.Category_LearningAptitudes
                                                            select hl;
            hocLucs = hocLucs.OrderBy(hl => hl.LearningAptitudeName);
            hocLucs = hocLucs.Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            return hocLucs.ToList();
        }

        public List<Category_LearningAptitude> GetListHocLuc(string LearningAptitudeName, int pageCurrentIndex, int pageSize)
        {
            IQueryable<Category_LearningAptitude> hocLucs = from hl in db.Category_LearningAptitudes
                                                            where hl.LearningAptitudeName.Contains(LearningAptitudeName)
                                                            select hl;
            hocLucs = hocLucs.OrderBy(n => n.LearningAptitudeName).Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize);
            return hocLucs.ToList();
        }
        public List<Category_LearningAptitude> GetHocLucs(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_LearningAptitude> lConducts = new List<Category_LearningAptitude>();

            IQueryable<Category_LearningAptitude> iqConduct = from cdt in db.Category_LearningAptitudes
                                                              where cdt.SchoolId == school.SchoolId
                                                              select cdt;

            return GetHocLucs(ref iqConduct, pageCurrentIndex, pageSize, out totalRecords);
        }
        private List<Category_LearningAptitude> GetHocLucs(ref IQueryable<Category_LearningAptitude> iqHocLuc, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            totalRecords = iqHocLuc.Count();
            if (totalRecords != 0)
            {
                return iqHocLuc.OrderBy(hocluc => hocluc.LearningAptitudeId)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return new List<Category_LearningAptitude>();
            }
        }
        public double GetHocLucCount()
        {
            IQueryable<Category_LearningAptitude> hocLucs = from hl in db.Category_LearningAptitudes
                                                            select hl;
            return hocLucs.Count();
        }

        public double GetHocLucCount(string LearningAptitudeName)
        {
            IQueryable<Category_LearningAptitude> hocLucs = from hl in db.Category_LearningAptitudes
                                                            where hl.LearningAptitudeName.Contains(LearningAptitudeName)
                                                            select hl;
            return hocLucs.Count();
        }

        public bool CheckExistLearningAptitudeName(int LearningAptitudeId, string LearningAptitudeName)
        {
            IQueryable<Category_LearningAptitude> hocLucs = from hl in db.Category_LearningAptitudes
                                                            where hl.LearningAptitudeName == LearningAptitudeName && hl.LearningAptitudeId != LearningAptitudeId
                                                            select hl;
            if (hocLucs.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckCanDeleteHocLuc(int LearningAptitudeId)
        {
            //IQueryable<Student_DetailedTermSubjectMark> chiTietDiem = from ctd in db.Student_DetailedTermSubjectMarks
            //                                              where ctd.LearningAptitudeId == LearningAptitudeId
            //                                              select ctd;
            //if (chiTietDiem.Count() != 0)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
            return true;
        }

        public Category_LearningAptitude GetHocLuc(double diem)
        {
            IQueryable<Category_LearningAptitude> hocLucs = from hocluc in db.Category_LearningAptitudes
                                                            where hocluc.EndAverageMark <= diem && hocluc.EndAverageMark >= diem
                                                            select hocluc;
            if (hocLucs.Count() != 0)
            {
                return hocLucs.First();
            }
            else
            {
                return null;
            }
        }
        public bool ConductNameExists(string conductName)
        {
            IQueryable<Category_LearningAptitude> iqConduct = from cdt in db.Category_LearningAptitudes
                                                              where cdt.LearningAptitudeName == conductName
                                                              && cdt.SchoolId == school.SchoolId
                                                              select cdt;
            if (iqConduct.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void InsertConduct(Category_LearningAptitude conduct)
        {
            conduct.SchoolId = school.SchoolId;
            db.Category_LearningAptitudes.InsertOnSubmit(conduct);
            db.SubmitChanges();
        }
        public void DeleteConduct(Category_LearningAptitude deletedHocLuc)
        {
            Category_LearningAptitude conduct = null;

            IQueryable<Category_LearningAptitude> iqConduct = from cdt in db.Category_LearningAptitudes
                                                              where cdt.LearningAptitudeId == deletedHocLuc.LearningAptitudeId
                                                                && cdt.SchoolId == school.SchoolId
                                                              select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
                db.Category_LearningAptitudes.DeleteOnSubmit(conduct);
                db.SubmitChanges();
            }
        }
        public Category_LearningAptitude GetConduct(string conductName)
        {
            Category_LearningAptitude conduct = null;

            IQueryable<Category_LearningAptitude> iqConduct = from cdt in db.Category_LearningAptitudes
                                                              where cdt.LearningAptitudeName == conductName
                                                              && cdt.SchoolId == school.SchoolId
                                                              select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
            }

            return conduct;
        }
        public Category_LearningAptitude GetConduct(int conductName)
        {
            Category_LearningAptitude conduct = null;

            IQueryable<Category_LearningAptitude> iqConduct = from cdt in db.Category_LearningAptitudes
                                                              where cdt.LearningAptitudeId == conductName
                                                              && cdt.SchoolId == school.SchoolId
                                                              select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
            }

            return conduct;
        }
        public void UpdateConduct(Category_LearningAptitude editedConduct)
        {
            Category_LearningAptitude conduct = null;

            IQueryable<Category_LearningAptitude> iqConduct = from cdt in db.Category_LearningAptitudes
                                                              where cdt.LearningAptitudeId == editedConduct.LearningAptitudeId
                                                              && cdt.SchoolId == school.SchoolId
                                                              select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
                conduct.LearningAptitudeName = editedConduct.LearningAptitudeName;
                conduct.BeginAverageMark = editedConduct.BeginAverageMark;
                conduct.EndAverageMark = editedConduct.EndAverageMark;
                db.SubmitChanges();
            }
        }
        public bool IsDeletable(string LearningAptitudeName)
        {
            bool bResult = true;
            IQueryable<Student_TermLearningResult> iqTermStudentResult;

            // Kiểm tra có tồn tại Học sinh nào đạt hạnh kiểm chỉ định hay không
            iqTermStudentResult = from termStudentResult in db.Student_TermLearningResults
                                  join hocluc in db.Category_LearningAptitudes on termStudentResult.TermLearningAptitudeId equals hocluc.LearningAptitudeId
                                  where hocluc.LearningAptitudeName == LearningAptitudeName
                                  select termStudentResult;

            if (iqTermStudentResult.Count() != 0)
            {
                bResult = false;
            }

            return bResult;
        }

        public Category_LearningAptitude GetLearningAptitude(double averageMark)
        {
            Category_LearningAptitude learningAptitude = null;

            IQueryable<Category_LearningAptitude> iqLearningAptitude = from lA in db.Category_LearningAptitudes
                                                                       where lA.BeginAverageMark <= averageMark
                                                                       && lA.BeginAverageMark >= averageMark
                                                                       && lA.SchoolId == school.SchoolId
                                                                       select lA;
            if (iqLearningAptitude.Count() != 0)
            {
                learningAptitude = iqLearningAptitude.First();
            }

            return learningAptitude;
        }
    }
}
