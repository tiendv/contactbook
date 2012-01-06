using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EContactBook.BusinessEntity
{
    public class TabularImportedStudent
    {
        public string StudentCode { get; set; }

        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string StringDateOfBirth { get; set; }

        public bool Gender { get; set; }

        public string StringGender { get; set; }

        public string Address { get; set; }

        public string BirthPlace { get; set; }

        public string Phone { get; set; }

        public string FatherName { get; set; }

        public DateTime? FatherDateOfBirth { get; set; }

        public string StringFatherDateOfBirth { get; set; }

        public string FatherJob { get; set; }

        public string MotherName { get; set; }

        public DateTime? MotherDateOfBirth { get; set; }

        public string StringMotherDateOfBirth { get; set; }

        public string MotherJob { get; set; }

        public string PatronName { get; set; }

        public DateTime? PatronDateOfBirth { get; set; }

        public string StringPatronDateOfBirth { get; set; }

        public string PatronJob { get; set; }

        public string Error { get; set; }
        public string ImportStatus { get; set; }
    }
}
