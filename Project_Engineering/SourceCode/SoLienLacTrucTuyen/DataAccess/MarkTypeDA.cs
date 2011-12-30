using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.DataAccess
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

        public List<Category_MarkType> GetMarkTypes()
        {
            List<Category_MarkType> markTypes = new List<Category_MarkType>();

            IQueryable<Category_MarkType> iqMarkType = from markType in db.Category_MarkTypes
                                                       where markType.SchoolId == school.SchoolId
                                                       select markType;

            if (iqMarkType.Count() != 0)
            {
                markTypes = iqMarkType.OrderBy(loaiDiem => loaiDiem.MarkRatio)
                    .ThenBy(markType => markType.MarkTypeName).ToList();
            }

            return markTypes;
        }

        public List<Category_MarkType> GetMarkTypes(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_MarkType> markTypes = new List<Category_MarkType>();

            IQueryable<Category_MarkType> iqMarkType = from markTp in db.Category_MarkTypes
                                                       where markTp.SchoolId == school.SchoolId
                                                       select markTp;

            totalRecords = iqMarkType.Count();
            if (totalRecords != 0)
            {
                markTypes = iqMarkType.OrderBy(loaiDiem => loaiDiem.MarkRatio)
                    .ThenBy(loaiDiem => loaiDiem.MarkTypeName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return markTypes;
        }

        public bool MarkTypeExists(string markTypeName)
        {
            IQueryable<Category_MarkType> iqMarkType = from markType in db.Category_MarkTypes
                                                       where markType.MarkTypeName == markTypeName
                                                       && markType.SchoolId == school.SchoolId
                                                       select markType;
            if (iqMarkType.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Category_MarkType GetAppliedCalculateAvgMarkType()
        {
            Category_MarkType markType = null;

            IQueryable<Category_MarkType> iqMarkType = from markTp in db.Category_MarkTypes
                                                       where markTp.IsUsedForCalculatingAvg == true
                                                       && markTp.SchoolId == school.SchoolId
                                                       select markTp;
            if (iqMarkType.Count() != 0)
            {
                markType = iqMarkType.First();
            }

            return markType;
        }
    }
}
