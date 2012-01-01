using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class SchoolBL
    {
        SchoolDA schoolDA;

        public SchoolBL()
        {
            schoolDA = new SchoolDA();
        }

        public List<School_School> GetSchools()
        {
            return schoolDA.GetSchools();
        }

        public List<School_School> GetSchools(ConfigurationProvince province, ConfigurationDistrict district, string schoolName, 
            int pageIndex, int pageSize, out double totalRecords)
        {
            List<School_School> schools = new List<School_School>();

            if (district == null) // disctrict is all
            {
                if (province == null) // province is all
                {
                    if (CheckUntils.IsAllOrBlank(schoolName)) // schoolName is all
                    {
                        schools = schoolDA.GetSchools(pageIndex, pageSize, out totalRecords);
                    }
                    else // schoolName is specified
                    {
                        schools = schoolDA.GetSchools(schoolName, pageIndex, pageSize, out totalRecords);
                    }
                }
                else // province is specified, disctrict is all
                {
                    if (CheckUntils.IsAllOrBlank(schoolName)) // schoolName is all
                    {
                        schools = schoolDA.GetSchools(province, pageIndex, pageSize, out totalRecords);
                    }
                    else // schoolName is specified
                    {
                        schools = schoolDA.GetSchools(province, schoolName, pageIndex, pageSize, out totalRecords);
                    }
                }
            }
            else // disctrict is specified
            {
                if (CheckUntils.IsAllOrBlank(schoolName)) // schoolName is all
                {
                    schools = schoolDA.GetSchools(district, pageIndex, pageSize, out totalRecords);
                }
                else // schoolName is specified
                {
                    schools = schoolDA.GetSchools(district, schoolName, pageIndex, pageSize, out totalRecords);
                }
            }

            return schools;
        }

        public void DeleteSchool(List<School_School> schools)
        {
            foreach (School_School school in schools)
            {
                schoolDA.DeleteSchool(school);
            }            
        }

        public School_School InsertSchool(ConfigurationDistrict district, string schoolName, string address, string phone, 
            string email, string password, byte[] logo)
        {
            School_School school = new School_School();
            school.DistrictId = district.DistrictId;
            school.SchoolName = schoolName;
            school.Address = address;
            school.Phone = phone;
            school.Email = email;
            school.Password = password;
            school.Logo = new System.Data.Linq.Binary(logo);
            schoolDA.InsertSchool(school);

            return schoolDA.GetLastedInsertedSchool();



            /*
             * 1. Tạo danh sách nhóm người dùng mặc định
             * 2. Tạo tài khoản 1 admin cho nhóm admin
             * 2.1. Phân quyền cho tài khoản này
             * 3. Tạo danh sách loại điểm mặc định
             * 4. Tạo danh sách tiết mặc định
             */
        }
    }
}
