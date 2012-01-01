using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EContactBook.DataAccess;
using EContactBook.BusinessEntity;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class SchoolBL
    {
        SchoolDA schoolDA;

        public SchoolBL()
        {
            schoolDA = new SchoolDA();
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
        }

        public void DeleteSchool(List<School_School> schools)
        {
            foreach (School_School school in schools)
            {
                schoolDA.DeleteSchool(school);
            }
        }
        
        public List<School_School> GetSchools()
        {
            return schoolDA.GetSchools();
        }

        public List<TabularSchool> GetTabularSchools(ConfigurationProvince province, ConfigurationDistrict district, string schoolName, 
            int pageIndex, int pageSize, out double totalRecords)
        {
            List<TabularSchool> tabularSchools = new List<TabularSchool>();
            TabularSchool tabularSchool = null;
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

            foreach (School_School school in schools)
            {
                tabularSchool = new TabularSchool();
                tabularSchool.SchoolId = school.SchoolId;
                tabularSchool.SchoolName = school.SchoolName;
                tabularSchool.Address = school.Address;
                tabularSchool.Phone = school.Phone;
                tabularSchool.Email = school.Email;
                tabularSchool.Status = (school.Status == true) ? "Đang sử dụng" : "Chưa sử dụng";
                tabularSchool.DistrictName = school.ConfigurationDistrict.DistrictName;
                tabularSchool.ProvinceName = school.ConfigurationDistrict.ConfigurationProvince.ProvinceName;
                tabularSchools.Add(tabularSchool);
            }

            return tabularSchools;
        }

        public School_School GetSupplier()
        {
            return schoolDA.GetSchool(0);
        }

        public bool SchoolNameExists(ConfigurationDistrict district, string schoolName)
        {
            return schoolDA.SchoolNameExists(district, schoolName);
        }

        public bool SchoolNameExists(ConfigurationDistrict district, string oldSchoolName, string newSchoolName)
        {
            if (oldSchoolName == newSchoolName)
            {
                return false;
            }
            else
            {
                return SchoolNameExists(district, newSchoolName);
            }
        }
    }
}
