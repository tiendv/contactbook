using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class LearningResultBL: BaseBL
    {
        private LearningResultDA learningResultDA;

        public LearningResultBL(School_School school)
            : base(school)
        {
            learningResultDA = new LearningResultDA(school);
        }

        public void InsertLearningResult(string learningResultName, List<KeyValuePair<Category_LearningAptitude, Category_Conduct>> detailLearningResults)
        {
            int i = 0;
            while (i < detailLearningResults.Count)
            {
                Category_LearningAptitude learningAptitude = detailLearningResults[i].Key;
                int j = i + 1;
                while (j < detailLearningResults.Count)
                {
                    if (learningAptitude.LearningAptitudeId == detailLearningResults[j].Key.LearningAptitudeId)
                    {
                        Category_Conduct conduct = detailLearningResults[i].Value;
                        if (conduct.ConductId == detailLearningResults[j].Value.ConductId)
                        {
                            detailLearningResults.RemoveAt(j);
                        }
                        else
                        {
                            j++;
                        }
                    }
                    else
                    {
                        j++;
                    }
                }
                i++;
            }

            Category_LearningResult learningResult = new Category_LearningResult();
            learningResult.LearningResultName = learningResultName;

            learningResult = learningResultDA.InsertLearningResult(learningResult);
            Category_DetailedLearningResult detailedLearningResult = null;

            foreach (KeyValuePair<Category_LearningAptitude, Category_Conduct> detailLearningResult in detailLearningResults)
            {
                detailedLearningResult = new Category_DetailedLearningResult();
                detailedLearningResult.LearningResultId = learningResult.LearningResultId;
                detailedLearningResult.LearningAptitudeId = detailLearningResult.Key.LearningAptitudeId;
                detailedLearningResult.ConductId = detailLearningResult.Value.ConductId;

                learningResultDA.InsertDetailLearningResult(detailedLearningResult);
            }
        }

        public void UpdateLearningResult(Category_LearningResult learningResult, string learningResultName, List<KeyValuePair<Category_LearningAptitude, Category_Conduct>> detailLearningResults)
        {
            learningResultDA.DeleteDetailLearningResult(learningResult);

            int i = 0;
            while (i < detailLearningResults.Count)
            {
                Category_LearningAptitude learningAptitude = detailLearningResults[i].Key;
                int j = i + 1;
                while (j < detailLearningResults.Count)
                {
                    if (learningAptitude.LearningAptitudeId == detailLearningResults[j].Key.LearningAptitudeId)
                    {
                        Category_Conduct conduct = detailLearningResults[i].Value;
                        if (conduct.ConductId == detailLearningResults[j].Value.ConductId)
                        {
                            detailLearningResults.RemoveAt(j);
                        }
                        else
                        {
                            j++;
                        }
                    }
                    else
                    {
                        j++;
                    }
                }
                i++;
            }

            learningResultDA.UpdateLearningResult(learningResult, learningResultName);
            Category_DetailedLearningResult detailedLearningResult = null;

            foreach (KeyValuePair<Category_LearningAptitude, Category_Conduct> detailLearningResult in detailLearningResults)
            {
                detailedLearningResult = new Category_DetailedLearningResult();
                detailedLearningResult.LearningResultId = learningResult.LearningResultId;
                detailedLearningResult.LearningAptitudeId = detailLearningResult.Key.LearningAptitudeId;
                detailedLearningResult.ConductId = detailLearningResult.Value.ConductId;

                learningResultDA.InsertDetailLearningResult(detailedLearningResult);
            }
        }
        
        public void DeleteLearningResult(Category_LearningResult learningResult)
        {
            learningResultDA.DeleteLearningResult(learningResult);
        }

        public bool LearningResultNameExists(string learningResultName)
        {
            return learningResultDA.LearningResultNameExists(learningResultName);
        }

        public bool LearningResultNameExists(string oldLearningResultName, string newLearningResultName)
        {
            if (oldLearningResultName == newLearningResultName)
            {
                return false;
            }
            else
            {
                return learningResultDA.LearningResultNameExists(newLearningResultName);
            }
        }

        public bool DetailLearningResultExists(Category_LearningResult learningResult, Category_LearningAptitude learningAptitude, Category_Conduct conduct)
        {
            Category_LearningResult existedLearningResult = GetLearningResult(conduct, learningAptitude);
            if (existedLearningResult == null)
            {
                return false;
            }
            else
            {
                if (learningResult == null) // create new
                {
                    return true;
                }
                else // modify
                {
                    if (learningResult.LearningResultId == existedLearningResult.LearningResultId)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }            
        }

        public List<TabularLearningResult> GetTabularLearningResults(string learningResultName, int pageCurrentIndex, int pageSize, out double totalRecord)
        {
            List<TabularLearningResult> tabularLearningResults = new List<TabularLearningResult>();
            TabularLearningResult tabularLearningResult = null;
            List<Category_LearningResult> learningResults;
            if (CheckUntils.IsAllOrBlank(learningResultName))
            {
                learningResults = learningResultDA.GetLearningResults(pageCurrentIndex, pageSize, out totalRecord);
            }
            else
            {
                learningResults = learningResultDA.GetLearningResults(learningResultName, pageCurrentIndex, pageSize, out totalRecord);
            }

            List<Category_DetailedLearningResult> detailedLearningResults;
            List<TabularDetailLearningResult> tabularDetailLearningResults;
            TabularDetailLearningResult tabularDetailLearningResult;

            foreach (Category_LearningResult learningResult in learningResults)
            {
                tabularLearningResult = new TabularLearningResult();
                tabularLearningResult.LearningResultId = learningResult.LearningResultId;
                tabularLearningResult.LearningResultName = learningResult.LearningResultName;

                tabularDetailLearningResults = new List<TabularDetailLearningResult>();

                detailedLearningResults = learningResultDA.GetDetailedLearningResults(learningResult);

                foreach (Category_DetailedLearningResult detailedLearningResult in detailedLearningResults)
                {
                    tabularDetailLearningResult = new TabularDetailLearningResult();
                    tabularDetailLearningResult.LearningAptitudeId = detailedLearningResult.LearningAptitudeId;
                    tabularDetailLearningResult.LearningAptitudeName = detailedLearningResult.Category_LearningAptitude.LearningAptitudeName;
                    tabularDetailLearningResult.BeginAverageMark = detailedLearningResult.Category_LearningAptitude.BeginAverageMark;
                    tabularDetailLearningResult.EndAverageMark = detailedLearningResult.Category_LearningAptitude.EndAverageMark;
                    tabularDetailLearningResult.LearningAptitudeId = detailedLearningResult.ConductId;
                    tabularDetailLearningResult.ConductName = detailedLearningResult.Category_Conduct.ConductName;

                    tabularDetailLearningResults.Add(tabularDetailLearningResult);
                }

                tabularLearningResult.DetailLearningResults = tabularDetailLearningResults;

                tabularLearningResults.Add(tabularLearningResult);
            }

            return tabularLearningResults;
        }

        public Category_LearningResult GetLearningResult(int LearningResultId)
        {
            return learningResultDA.GetLearningResult(LearningResultId);
        }

        public Category_LearningResult GetLearningResult(Category_Conduct conduct, Category_LearningAptitude learningAptitude)
        {
            return learningResultDA.GetLearningResult(conduct, learningAptitude);
        }

        public List<Category_DetailedLearningResult> GetDetailedLearningResults(Category_LearningResult learningResult)
        {
            return learningResultDA.GetDetailedLearningResults(learningResult);
        }

        public bool IsDeletable(Category_LearningResult learningResult)
        {
            return learningResultDA.IsDeletable(learningResult);
        }        
    }
}
