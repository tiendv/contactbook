using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.DataAccess
{
    public class ConductDA : BaseDA
    {
        public ConductDA(School_School school)
            : base(school)
        {

        }

        /// <summary>
        /// Insert new conduct to database
        /// </summary>
        /// <param name="newConduct">conduct that will be inserted</param>
        public void InsertConduct(Category_Conduct newConduct)
        {
            newConduct.SchoolId = school.SchoolId;
            db.Category_Conducts.InsertOnSubmit(newConduct);
            db.SubmitChanges();
        }

        public void UpdateConduct(Category_Conduct editedConduct)
        {
            Category_Conduct conduct = null;

            IQueryable<Category_Conduct> iqConduct = from cdt in db.Category_Conducts
                                                     where cdt.ConductId == editedConduct.ConductId
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
                conduct.ConductName = editedConduct.ConductName;
                db.SubmitChanges();
            }
        }

        public void DeleteConduct(Category_Conduct deletedConduct)
        {
            Category_Conduct conduct = null;

            IQueryable<Category_Conduct> iqConduct = from cdt in db.Category_Conducts
                                                     where cdt.ConductId == deletedConduct.ConductId
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
                db.Category_Conducts.DeleteOnSubmit(conduct);
                db.SubmitChanges();
            }
        }

        public Category_Conduct GetConduct(int conductId)
        {
            Category_Conduct conduct = null;

            IQueryable<Category_Conduct> iqConduct = from cdt in db.Category_Conducts
                                                     where cdt.ConductId == conductId
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
            }

            return conduct;
        }

        public Category_Conduct GetConduct(string conductName)
        {
            Category_Conduct conduct = null;

            IQueryable<Category_Conduct> iqConduct = from cdt in db.Category_Conducts
                                                     where cdt.ConductName == conductName
                                                     && cdt.SchoolId == school.SchoolId
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                conduct = iqConduct.First();
            }

            return conduct;
        }

        public List<Category_Conduct> GetConducts()
        {
            List<Category_Conduct> conducts = new List<Category_Conduct>();

            IQueryable<Category_Conduct> iqConduct = from cdt in db.Category_Conducts
                                                     where cdt.SchoolId == school.SchoolId
                                                     select cdt;

            if (iqConduct.Count() != 0)
            {
                conducts = iqConduct.OrderBy(cdt => cdt.ConductName).ToList();
            }

            return conducts;
        }

        public List<Category_Conduct> GetConducts(int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_Conduct> conducts = new List<Category_Conduct>();

            IQueryable<Category_Conduct> iqConduct = from cdt in db.Category_Conducts
                                                     where cdt.SchoolId == school.SchoolId
                                                     select cdt;

            return GetConducts(ref iqConduct, pageCurrentIndex, pageSize, out totalRecords);
        }

        private List<Category_Conduct> GetConducts(ref IQueryable<Category_Conduct> iqConduct, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<Category_Conduct> conducts = new List<Category_Conduct>();
            totalRecords = iqConduct.Count();
            if (totalRecords != 0)
            {
                conducts = iqConduct.OrderBy(conduct => conduct.ConductName)
                    .Skip((pageCurrentIndex - 1) * pageSize).Take(pageSize).ToList();
            }

            return conducts;
        }

        public bool IsDeletable(string conductName)
        {
            bool bResult = true;
            IQueryable<Student_TermLearningResult> iqTermStudentResult;

            // Kiểm tra có tồn tại Học sinh nào đạt hạnh kiểm chỉ định hay không
            iqTermStudentResult = from termStudentResult in db.Student_TermLearningResults
                                  join conduct in db.Category_Conducts on termStudentResult.TermConductId equals conduct.ConductId
                                  where conduct.ConductName == conductName
                                  select termStudentResult;

            if (iqTermStudentResult.Count() != 0)
            {
                bResult = false;
            }

            return bResult;
        }

        public bool ConductNameExists(string conductName)
        {
            IQueryable<Category_Conduct> iqConduct = from cdt in db.Category_Conducts
                                                     where cdt.ConductName == conductName
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
    }
}
