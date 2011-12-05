using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class MarkTypeDA : BaseDA
    {
        public MarkTypeDA(School_School school)
            : base(school)
        {
        }

        public void InsertMarkType(Category_MarkType markType)
        {
            markType.SchoolId = school.SchoolId;
            db.Category_MarkTypes.InsertOnSubmit(markType);
            db.SubmitChanges();
        }

        public void DeleteMarkType(Category_MarkType markType)
        {
            IQueryable<Category_MarkType> iqMarkType = from markTp in db.Category_MarkTypes
                                                      where markTp.MarkTypeId == markType.MarkTypeId
                                                      && markTp.SchoolId == school.SchoolId
                                                      select markTp;
            if (iqMarkType.Count() != 0)
            {
                db.Category_MarkTypes.DeleteOnSubmit(iqMarkType.First());
            }
            db.SubmitChanges();
        }

        public void UpdateMarkType(Category_MarkType newMarkType)
        {
            IQueryable<Category_MarkType> iqMarkType = from markTp in db.Category_MarkTypes
                                                      where markTp.MarkTypeId == newMarkType.MarkTypeId
                                                      && markTp.SchoolId == school.SchoolId
                                                      select markTp;
            if (iqMarkType.Count() != 0)
            {
                Category_MarkType markType = iqMarkType.First();

                markType.MarkTypeName = newMarkType.MarkTypeName;
                markType.MarkRatio = newMarkType.MarkRatio;
                markType.MaxQuantity = newMarkType.MaxQuantity;
                markType.IsUsedForCalculatingAvg = newMarkType.IsUsedForCalculatingAvg;
            }

            db.SubmitChanges();
        }

        public Category_MarkType GetMarkType(int markTypeId)
        {
            Category_MarkType markType = null;

            IQueryable<Category_MarkType> iqMarkType = from markTp in db.Category_MarkTypes
                                                      where markTp.MarkTypeId == markTypeId
                                                      && markTp.SchoolId == school.SchoolId
                                                      select markTp;
            if (iqMarkType.Count() != 0)
            {
                markType = iqMarkType.First();
            }

            return markType;
        }

        public Category_MarkType GetMarkType(string markTypeName)
        {
            Category_MarkType markType = null;

            IQueryable<Category_MarkType> iqMarkType = from markTp in db.Category_MarkTypes
                                                      where markTp.MarkTypeName == markTypeName
                                                      && markTp.SchoolId == school.SchoolId
                                                      select markTp;
            if (iqMarkType.Count() != 0)
            {
                markType = iqMarkType.First();
            }

            return markType;
        }

        public List<Category_MarkType> GetListMarkTypes()
        {
            List<Category_MarkType> lMarkTypes = new List<Category_MarkType>();

            IQueryable<Category_MarkType> iqMarkType = from markTp in db.Category_MarkTypes
                                                      where markTp.SchoolId == school.SchoolId
                                                      select markTp;

            if (iqMarkType.Count() != 0)
            {
                lMarkTypes = iqMarkType.OrderBy(loaiDiem => loaiDiem.MarkRatio)
                    .ThenBy(loaiDiem => loaiDiem.MarkTypeName).ToList();
            }

            return lMarkTypes;
        }

        public List<Category_MarkType> GetListMarkTypes(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_MarkType> lMarkTypes = new List<Category_MarkType>();

            IQueryable<Category_MarkType> iqMarkType = from markTp in db.Category_MarkTypes
                                                      where markTp.SchoolId == school.SchoolId
                                                      select markTp;

            totalRecords = iqMarkType.Count();
            if (totalRecords != 0)
            {
                lMarkTypes = iqMarkType.OrderBy(loaiDiem => loaiDiem.MarkRatio)
                    .ThenBy(loaiDiem => loaiDiem.MarkTypeName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return lMarkTypes;
        }

        public bool MarkTypeExists(string markTypeName)
        {
            IQueryable<Category_MarkType> iqMarkType = from markTp in db.Category_MarkTypes
                                                      where markTp.MarkTypeName == markTypeName
                                                      && markTp.SchoolId == school.SchoolId
                                                      select markTp;
            if (iqMarkType.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDeletable(Category_MarkType markType)
        {
            IQueryable<Student_DetailedTermSubjectMark> iqHocSinhChiTietDiem;
            iqHocSinhChiTietDiem = from hsChiTietDiem in db.Student_DetailedTermSubjectMarks
                                   where hsChiTietDiem.MarkType == markType.MarkTypeId
                                   && hsChiTietDiem.Category_MarkType.SchoolId == school.SchoolId
                                   select hsChiTietDiem;

            if (iqHocSinhChiTietDiem.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public Category_MarkType GetAppliedCalAvgMarkType()
        {
            Category_MarkType markType = null;

            IQueryable<Category_MarkType> iqMarkType = from markTp in db.Category_MarkTypes
                                                      where markTp.IsUsedForCalculatingAvg == true
                                                      && markType.SchoolId == school.SchoolId
                                                      select markTp;
            if (iqMarkType.Count() != 0)
            {
                markType = iqMarkType.First();
            }

            return markType;
        }
    }
}
