using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class MarkTypeBL: BaseBL
    {
        MarkTypeDA markTypeDA;

        public MarkTypeBL(School_School school)
            : base(school)
        {
            markTypeDA = new MarkTypeDA(school);
        }

        public void InsertLoaiDiem(string markTypeName, double markRatio, short maxMarksPerTerm, bool calAverageMark)
        {
            Category_MarkType markType = new Category_MarkType
            {
                MarkTypeName = markTypeName,
                MarkRatio = markRatio,
                MaxQuantity = maxMarksPerTerm,
                IsUsedForCalculatingAvg = calAverageMark
            };

            markTypeDA.InsertMarkType(markType);
        }

        public void DeleteMarkType(Category_MarkType markType)
        {
            markTypeDA.DeleteMarkType(markType);
        }

        public void UpdateMarkType(string editedMarkTypeName, string newMarkTypeName, double newMarkRatio, short maxMarksPerTerm, bool calAverageMark)
        {
            Category_MarkType markType = GetMarkType(editedMarkTypeName);

            markType.MarkTypeName = newMarkTypeName;
            markType.MarkRatio = newMarkRatio;
            markType.MaxQuantity = maxMarksPerTerm;
            markType.IsUsedForCalculatingAvg = calAverageMark;

            markTypeDA.UpdateMarkType(markType);
        }

        public Category_MarkType GetMarkType(string markTypeName)
        {
            return markTypeDA.GetMarkType(markTypeName);
        }

        public Category_MarkType GetMarkType(int markTypeId)
        {
            return markTypeDA.GetMarkType(markTypeId);
        }

        public List<Category_MarkType> GetListMarkTypes()
        {
            return markTypeDA.GetMarkTypes();
        }

        public List<Category_MarkType> GetListMarkTypes(string markTypeName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_MarkType> lMarkTypes = new List<Category_MarkType>();

            if (String.Compare(markTypeName, "tất cả", true) == 0 || markTypeName == "")
            {
                lMarkTypes = markTypeDA.GetMarkTypes(pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                Category_MarkType markType = GetMarkType(markTypeName);
                lMarkTypes.Add(markType);
                totalRecords = 1;
            }

            return lMarkTypes;
        }

        public bool MarkTypeNameExists(string markTypeName)
        {
            return markTypeDA.MarkTypeExists(markTypeName);
        }

        public bool MarkTypeNameExists(string oldMarkTypeName, string newMarkTypeName)
        {
            bool bExist = false;

            if (oldMarkTypeName == newMarkTypeName)
            {
                bExist = false;
            }
            else
            {
                bExist = markTypeDA.MarkTypeExists(newMarkTypeName);
            }

            return bExist;
        }

        public bool IsDeletable(string markTypeName)
        {
            Category_MarkType markType = GetMarkType(markTypeName);
            StudyingResultBL studyingResultBL = new StudyingResultBL(school);
            return studyingResultBL.DetailTermSubjectMarkExists(markType);
        }

        public Category_MarkType GetAppliedCalAvgMarkType()
        {
            return markTypeDA.GetAppliedCalculateAvgMarkType();
        }
    }
}
