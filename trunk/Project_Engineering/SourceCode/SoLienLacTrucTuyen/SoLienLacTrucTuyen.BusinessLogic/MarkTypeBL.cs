using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

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

        public void InsertLoaiDiem(string markTypeName, double markRatio, short maxMarksPerTerm, bool calAverageMark, Category_Grade grade)
        {
            Category_MarkType markType = new Category_MarkType
            {
                MarkTypeName = markTypeName,
                MarkRatio = markRatio,
                MaxQuantity = maxMarksPerTerm,
                IsUsedForCalculatingAvg = calAverageMark,
                GradeId = grade.GradeId
            };

            markTypeDA.InsertMarkType(markType);
        }

        public void DeleteMarkType(Category_MarkType markType)
        {
            markTypeDA.DeleteMarkType(markType);
        }

        public void UpdateMarkType(Category_Grade grade, string editedMarkTypeName, string newMarkTypeName, double newMarkRatio, short maxMarksPerTerm, bool calAverageMark)
        {
            Category_MarkType markType = GetMarkType(grade, editedMarkTypeName);

            markType.MarkTypeName = newMarkTypeName;
            markType.MarkRatio = newMarkRatio;
            markType.MaxQuantity = maxMarksPerTerm;
            markType.IsUsedForCalculatingAvg = calAverageMark;

            markTypeDA.UpdateMarkType(markType);
        }

        public Category_MarkType GetMarkType(Category_Grade grade, string markTypeName)
        {
            return markTypeDA.GetMarkType(grade, markTypeName);
        }

        public Category_MarkType GetMarkType(int markTypeId)
        {
            return markTypeDA.GetMarkType(markTypeId);
        }

        public List<Category_MarkType> GetListMarkTypes(Category_Grade grade)
        {
            return markTypeDA.GetMarkTypes(grade);
        }

        public List<TabularMarkType> GetTabularMarkTypes(string markTypeName, Category_Grade grade, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularMarkType> tabularMarkTypes = new List<TabularMarkType>();
            List<Category_MarkType> lMarkTypes = new List<Category_MarkType>();
            TabularMarkType tabularMarkType = null;

            if (String.Compare(markTypeName, "tất cả", true) == 0 || markTypeName == "")
            {
                lMarkTypes = markTypeDA.GetMarkTypes(grade, pageCurrentIndex, pageSize, out totalRecords);
            }
            else
            {
                Category_MarkType markType = GetMarkType(grade, markTypeName);
                lMarkTypes.Add(markType);
                totalRecords = 1;
            }

            foreach(Category_MarkType markType in lMarkTypes)
            {
                tabularMarkType = new TabularMarkType();
                tabularMarkType.MarkTypeId = markType.MarkTypeId;
                tabularMarkType.GradeId = markType.GradeId;
                tabularMarkType.GradeName = markType.Category_Grade.GradeName;
                tabularMarkType.IsUsedForCalculatingAvg = markType.IsUsedForCalculatingAvg;
                tabularMarkType.MarkRatio = markType.MarkRatio;
                tabularMarkType.MarkTypeName = markType.MarkTypeName;
                tabularMarkType.MaxQuantity = markType.MaxQuantity;

                tabularMarkTypes.Add(tabularMarkType);
            }

            return tabularMarkTypes;
        }

        public bool MarkTypeNameExists(Category_Grade grade, string markTypeName)
        {
            return markTypeDA.MarkTypeExists(grade, markTypeName);
        }

        public bool MarkTypeNameExists(Category_Grade grade, string oldMarkTypeName, string newMarkTypeName)
        {
            bool bExist = false;

            if (oldMarkTypeName == newMarkTypeName)
            {
                bExist = false;
            }
            else
            {
                bExist = markTypeDA.MarkTypeExists(grade, newMarkTypeName);
            }

            return bExist;
        }

        public bool IsDeletable(Category_Grade grade, string markTypeName)
        {
            Category_MarkType markType = GetMarkType(grade, markTypeName);
            StudyingResultBL studyingResultBL = new StudyingResultBL(school);
            return studyingResultBL.DetailTermSubjectMarkExists(markType);
        }

        public bool IsDeletable(Category_MarkType markType)
        {
            StudyingResultBL studyingResultBL = new StudyingResultBL(school);
            return studyingResultBL.DetailTermSubjectMarkExists(markType);
        }

        public Category_MarkType GetAppliedCalAvgMarkType(Category_Grade grade)
        {
            return markTypeDA.GetAppliedCalculateAvgMarkType(grade);
        }
    }
}
