using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class LearningAptitudeBL : BaseBL
    {
        LearningAptitudeDA learningAptitudeDA;

        public LearningAptitudeBL(School_School school)
            : base(school)
        {
            learningAptitudeDA = new LearningAptitudeDA(school);
        }

        public void InsertHocLuc(Category_LearningAptitude hocLucEn)
        {
            learningAptitudeDA.InsertHocLuc(hocLucEn);
        }

        public void UpdateHocLuc(int LearningAptitudeId, string LearningAptitudeName, float BeginAverageMark, float EndAverageMark)
        {
            learningAptitudeDA.UpdateHocLuc(new Category_LearningAptitude()
            {
                LearningAptitudeId = LearningAptitudeId,
                LearningAptitudeName = LearningAptitudeName,
                BeginAverageMark = BeginAverageMark,
                EndAverageMark = EndAverageMark
            });
        }

        public void DeleteHocLuc(int LearningAptitudeId)
        {
            learningAptitudeDA.DeleteHocLuc(LearningAptitudeId);
        }

        public Category_LearningAptitude GetHocLuc(int LearningAptitudeId)
        {
            return learningAptitudeDA.GetHocLuc(LearningAptitudeId);
        }

        public List<Category_LearningAptitude> GetListHocLuc(bool allOptions)
        {
            List<Category_LearningAptitude> hocLucs = learningAptitudeDA.GetListHocLuc();
            if (hocLucs.Count != 0 && allOptions)
            {
                hocLucs.Insert(hocLucs.Count, new Category_LearningAptitude
                {
                    LearningAptitudeId = 0,
                    LearningAptitudeName = "Tất cả"
                });
            }
            return hocLucs;
        }

        public List<Category_LearningAptitude> GetListHocLuc(int pageCurrentIndex, int pageSize)
        {
            return learningAptitudeDA.GetListHocLuc(pageCurrentIndex, pageSize);
        }

        public List<Category_LearningAptitude> GetListHocLuc(string LearningAptitudeName, int pageCurrentIndex, int pageSize)
        {
            return learningAptitudeDA.GetListHocLuc(LearningAptitudeName, pageCurrentIndex, pageSize);
        }
        public List<Category_LearningAptitude> GetListHocLuc(string conductName, int pageIndex, int pageSize, out double totalRecords)
        {
            List<Category_LearningAptitude> learningAptitudes = new List<Category_LearningAptitude>();

            if ((conductName == "") || (string.Compare(conductName, "tất cả", true) == 0))
            {
                learningAptitudes = learningAptitudeDA.GetHocLucs(pageIndex, pageSize, out totalRecords);
            }
            else
            {
                Category_LearningAptitude learningAptitude = GetConduct(conductName);
                if (learningAptitude != null)
                {
                    learningAptitudes.Add(learningAptitude);
                }
                
                totalRecords = learningAptitudes.Count;
            }

            return learningAptitudes;
        }
        public double GetHocLucCount()
        {
            return learningAptitudeDA.GetHocLucCount();
        }

        public double GetHocLucCount(string LearningAptitudeName)
        {
            return learningAptitudeDA.GetHocLucCount(LearningAptitudeName);
        }

        public bool CheckExistHocLuc(int LearningAptitudeId, string LearningAptitudeName)
        {
            return learningAptitudeDA.CheckExistLearningAptitudeName(LearningAptitudeId, LearningAptitudeName);
        }

        public bool CheckCanDeleteHocLuc(int LearningAptitudeId)
        {
            return learningAptitudeDA.CheckCanDeleteHocLuc(LearningAptitudeId);
        }

        public bool ConductNameExists(string conductName)
        {
            return learningAptitudeDA.ConductNameExists(conductName);
        }
        public void InsertConduct(Category_LearningAptitude conduct)
        {
            learningAptitudeDA.InsertConduct(conduct);
        }
        public void DeleteConduct(Category_LearningAptitude conduct)
        {
            learningAptitudeDA.DeleteConduct(conduct);
        }
        public bool ConductNameExists(string oldConductName, string newConductName)
        {
            bool bResult = false;

            if (oldConductName == newConductName)
            {
                bResult = false;
            }
            else
            {
                bResult = learningAptitudeDA.ConductNameExists(newConductName);
            }

            return bResult;
        }
        public void UpdateConduct(int editedConductName, string newConductName, double BeginAverageMark, double EndAverageMark)
        {
            Category_LearningAptitude hocluc = GetConduct(editedConductName);

            hocluc.LearningAptitudeName = newConductName;
            hocluc.BeginAverageMark = BeginAverageMark;
            hocluc.EndAverageMark = EndAverageMark;

            learningAptitudeDA.UpdateConduct(hocluc);
        }
        public Category_LearningAptitude GetConduct(int conductName)
        {
            return learningAptitudeDA.GetConduct(conductName);
        }
        public Category_LearningAptitude GetConduct(string conductName)
        {
            return learningAptitudeDA.GetConduct(conductName);
        }
        public bool IsDeletable(string conductName)
        {
            return learningAptitudeDA.IsDeletable(conductName);
        }
    }
}
