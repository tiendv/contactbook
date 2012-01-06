using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EContactBook.BusinessEntity;
using EContactBook.DataAccess;

namespace SoLienLacTrucTuyen.BusinessLogic
{
    public class StudentBL : BaseBL
    {
        private StudentDA studentDA;

        public StudentBL(School_School school)
            : base(school)
        {
            studentDA = new StudentDA(school);
        }

        public void InsertStudent(Class_Class Class, string studentCode, string studentName,
            bool gender, DateTime studentBithday, string birthplace, string address, string phone,
            string fatherName, string fatherJob, DateTime? fatherBirthday,
            string motherName, string motherJob, DateTime? motherBirthday,
            string patronName, string patronJob, DateTime? patronBirthday)
        {
            ClassBL classBL = new ClassBL(school);
            SystemConfigBL systemConfigBL = new SystemConfigBL(school);
            StudyingResultBL studyingResultBL = new StudyingResultBL(school);
            ScheduleBL scheduleBL = new ScheduleBL(school);
            Category_Conduct conduct = null;
            Category_LearningAptitude studyingAptitude = null;

            // Insert new student to database
            Student_Student student = new Student_Student
            {
                StudentCode = studentCode,
                FullName = studentName,
                Gender = gender,
                StudentBirthday = studentBithday,
                Birthplace = birthplace,
                Address = address,
                ContactPhone = phone,
                FatherName = fatherName,
                FatherJob = fatherJob,
                FatherBirthday = fatherBirthday,
                MotherName = motherName,
                MotherJob = motherJob,
                MotherBirthday = motherBirthday,
                PatronName = patronName,
                PatronJob = patronJob,
                PatronBirthday = patronBirthday,
                SchoolId = school.SchoolId
            };
            studentDA.InsertStudent(student);

            // Update Class' quantity student
            classBL.IncreaseStudentAmount(Class);
            
            // Insert created student to specified class
            Student_Student insertedStudent = studentDA.GetStudent(student.StudentCode);
            Student_StudentInClass lastedStudentInClass = studentDA.InsertStudentInClass(insertedStudent, Class);

            List<Configuration_Term> terms = systemConfigBL.GetListTerms();
            List<Category_Subject> scheduledSubjects;
            foreach (Configuration_Term term in terms)
            {
                conduct = new Category_Conduct();
                conduct.ConductId = -1;
                studyingAptitude = new Category_LearningAptitude();
                studyingAptitude.LearningAptitudeId = -1;

                // insert student's TermSubjectMark
                scheduledSubjects = scheduleBL.GetScheduledSubjects(Class, term);
                foreach (Category_Subject scheduledSubject in scheduledSubjects)
                {
                    studyingResultBL.InsertTermSubjectMark(lastedStudentInClass, term, scheduledSubject);
                }

                // insert new student's TermStudyingResult
                studyingResultBL.InsertTermStudyingResult(term, lastedStudentInClass, -1, conduct, studyingAptitude);
            }

            // Insert new User
        }

        public void UpdateHocSinh(Student_Student editedStudent, Class_Class Class, string studentCode, string studentName,
            bool gender, DateTime studentBithday, string birthplace, string address, string phone,
            string fatherName, string fatherJob, DateTime? fatherBirthday,
            string motherName, string motherJob, DateTime? motherBirthday,
            string patronName, string patronJob, DateTime? patronBirthday)
        {
            editedStudent.StudentCode = studentCode;
            editedStudent.FullName = studentName;
            editedStudent.Gender = gender;
            editedStudent.StudentBirthday = studentBithday;
            editedStudent.Birthplace = birthplace;
            editedStudent.Address = address;
            editedStudent.ContactPhone = phone;
            editedStudent.FatherName = fatherName;
            editedStudent.FatherJob = fatherJob;
            editedStudent.FatherBirthday = fatherBirthday;
            editedStudent.MotherName = motherName;
            editedStudent.MotherJob = motherJob;
            editedStudent.PatronBirthday = motherBirthday;
            editedStudent.PatronName = patronName;
            editedStudent.PatronJob = patronJob;
            editedStudent.PatronBirthday = patronBirthday;
            studentDA.UpdateStudent(editedStudent);

            Student_StudentInClass studentInClass = studentDA.GetLastedStudentInClass(editedStudent);
            studentDA.UpdateStudentInClass(studentInClass, Class);
        }

        public void DeleteStudent(Student_Student deletedStudent)
        {
            StudyingResultBL studyingResultBL = new StudyingResultBL(school);
            AbsentBL absentBL = new AbsentBL(school);
            StudentActivityBL activityBL = new StudentActivityBL(school);
            MessageBL messageBL = new MessageBL(school);
            ParentsCommentBL parentsCommentBL = new ParentsCommentBL(school);

            parentsCommentBL.DeleteParentsComment(deletedStudent);
            messageBL.DeleteMessage(deletedStudent);
            absentBL.DeleteAbsent(deletedStudent);
            activityBL.DeleteStudentActivity(deletedStudent);
            studyingResultBL.DeleteStudyingResult(deletedStudent);
            studentDA.DeleteStudentInClass(deletedStudent);
            studentDA.DeleteStudent(deletedStudent);
        }

        public bool IsDeletable(string studentCode)
        {
            return studentDA.IsDeletable(studentCode);
        }

        public bool StudentCodeExists(string studentCode)
        {
            return studentDA.StudentCodeExists(studentCode);
        }

        public bool StudentCodeExists(string oldStudentCode, string newStudentCode)
        {
            if (oldStudentCode.Trim() == newStudentCode.Trim())
            {
                return false;
            }
            else
            {
                return studentDA.StudentCodeExists(newStudentCode);
            }
        }

        public Student_Student GetStudent(string studentCode)
        {
            return studentDA.GetStudent(studentCode);
        }

        public Student_Student GetStudent(int studentId)
        {
            return studentDA.GetStudent(studentId);
        }

        public Student_StudentInClass GetStudentInClass(int studentInClassId)
        {
            return studentDA.GetStudentInClass(studentInClassId);
        }

        public Student_StudentInClass GetStudentInClass(Student_Student student, Configuration_Year year)
        {
            return studentDA.GetStudentInClass(student, year);
        }

        public List<TabularStudent> GetTabularStudents(Configuration_Year year, Category_Faculty faculty, Category_Grade grade,
            Class_Class Class, string studentCode, string studentName, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            List<TabularStudent> tabularStudents = new List<TabularStudent>();
            List<Student_StudentInClass> studentInClasses = new List<Student_StudentInClass>();
            TabularStudent tabularStudent = null;
            bool bStudentNameIsAll = (string.Compare(studentName, "tất cả", true) == 0) || (studentName == "");

            if ((string.Compare(studentCode, "tất cả", true) != 0) && (studentCode != ""))
            {
                Student_Student student = studentDA.GetStudent(studentCode);
                if (student != null)
                {
                    studentInClasses.Add(studentDA.GetStudentInClass(student, year));
                }

                totalRecords = studentInClasses.Count;
            }
            else
            {
                if (Class != null)
                {
                    if (bStudentNameIsAll)
                    {
                        studentInClasses = studentDA.GetStudentInClasses(Class, pageCurrentIndex, pageSize, out totalRecords);
                    }
                    else
                    {
                        studentInClasses = studentDA.GetStudentInClasses(Class, studentName, pageCurrentIndex, pageSize, out totalRecords);
                    }
                }
                else
                {
                    if (faculty == null)
                    {
                        if (grade == null)
                        {
                            if (bStudentNameIsAll)
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, studentName, pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                        else
                        {
                            if (bStudentNameIsAll)
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, grade, pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, grade, studentName, pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                    }
                    else
                    {
                        if (grade == null)
                        {
                            if (bStudentNameIsAll)
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, faculty, pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, faculty, studentName, pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                        else
                        {
                            if (bStudentNameIsAll)
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, faculty, grade, pageCurrentIndex, pageSize, out totalRecords);
                            }
                            else
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, faculty, grade, studentName, pageCurrentIndex, pageSize, out totalRecords);
                            }
                        }
                    }
                }
            }

            foreach (Student_StudentInClass studentInClass in studentInClasses)
            {
                tabularStudent = new TabularStudent();
                tabularStudent.StudentId = studentInClass.StudentId;
                tabularStudent.StudentCode = studentInClass.Student_Student.StudentCode;
                tabularStudent.FullName = studentInClass.Student_Student.FullName;
                tabularStudent.FacultyName = studentInClass.Class_Class.Category_Faculty.FacultyName;
                tabularStudent.GradeName = studentInClass.Class_Class.Category_Grade.GradeName;
                tabularStudent.ClassName = studentInClass.Class_Class.ClassName;
                tabularStudent.ClassId = studentInClass.ClassId;
                tabularStudent.DateOfBirth = studentInClass.Student_Student.StudentBirthday;
                tabularStudent.StringDateOfBirth = tabularStudent.DateOfBirth.ToString("dd/MM/yyyy");
                tabularStudent.Gender = studentInClass.Student_Student.Gender;
                tabularStudent.StringGender = tabularStudent.Gender == true ? "Nam" : "Nữ"; 
                tabularStudent.StudentInClassId = studentInClass.StudentInClassId;
                tabularStudents.Add(tabularStudent);
            }

            return tabularStudents;
        }

        public List<Student_StudentInClass> GetStudentInClasses(Configuration_Year year, Category_Faculty faculty, Category_Grade grade,
            Class_Class Class)
        {
            List<Student_StudentInClass> studentInClasses = new List<Student_StudentInClass>();

            if (Class != null)
            {
                studentInClasses = studentDA.GetStudentInClasses(Class);
            }
            else
            {
                if (faculty == null)
                {
                    if (grade == null)
                    {
                        studentInClasses = studentDA.GetStudentInClasses(year);
                    }
                    else
                    {
                        studentInClasses = studentDA.GetStudentInClasses(year, grade);
                    }
                }
                else
                {
                    if (grade == null)
                    {
                        studentInClasses = studentDA.GetStudentInClasses(year, faculty);
                    }
                    else
                    {
                        studentInClasses = studentDA.GetStudentInClasses(year, faculty, grade);
                    }
                }
            }

            return studentInClasses;
        }

        public List<TabularStudent> GetTabularStudents(Configuration_Year year, Category_Faculty faculty, Category_Grade grade,
            Class_Class Class, string studentCode, string studentName)
        {
            List<TabularStudent> tabularStudents = new List<TabularStudent>();
            List<Student_StudentInClass> studentInClasses = new List<Student_StudentInClass>();
            TabularStudent tabularStudent = null;
            bool bStudentNameIsAll = (string.Compare(studentName, "tất cả", true) == 0) || (studentName == "");

            if ((string.Compare(studentCode, "tất cả", true) != 0) && (studentCode != ""))
            {
                Student_Student student = studentDA.GetStudent(studentCode);
                if (student != null)
                {
                    studentInClasses.Add(studentDA.GetStudentInClass(student, year));
                }
            }
            else
            {
                if (Class != null)
                {
                    if (bStudentNameIsAll)
                    {
                        studentInClasses = studentDA.GetStudentInClasses(Class);
                    }
                    else
                    {
                        studentInClasses = studentDA.GetStudentInClasses(Class, studentName);
                    }
                }
                else
                {
                    if (faculty == null)
                    {
                        if (grade == null)
                        {
                            if (bStudentNameIsAll)
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year);
                            }
                            else
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, studentName);
                            }
                        }
                        else
                        {
                            if (bStudentNameIsAll)
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, grade);
                            }
                            else
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, grade, studentName);
                            }
                        }
                    }
                    else
                    {
                        if (grade == null)
                        {
                            if (bStudentNameIsAll)
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, faculty);
                            }
                            else
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, faculty, studentName);
                            }
                        }
                        else
                        {
                            if (bStudentNameIsAll)
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, faculty, grade);
                            }
                            else
                            {
                                studentInClasses = studentDA.GetStudentInClasses(year, faculty, grade, studentName);
                            }
                        }
                    }
                }
            }

            foreach (Student_StudentInClass studentInClass in studentInClasses)
            {
                tabularStudent = new TabularStudent();
                tabularStudent.StudentId = studentInClass.StudentId;
                tabularStudent.StudentCode = studentInClass.Student_Student.StudentCode;
                tabularStudent.FullName = studentInClass.Student_Student.FullName;
                tabularStudent.FacultyName = studentInClass.Class_Class.Category_Faculty.FacultyName;
                tabularStudent.GradeName = studentInClass.Class_Class.Category_Grade.GradeName;
                tabularStudent.ClassName = studentInClass.Class_Class.ClassName;
                tabularStudent.ClassId = studentInClass.ClassId;
                tabularStudent.Gender = studentInClass.Student_Student.Gender;
                tabularStudent.StringGender = (tabularStudent.Gender == true) ? "Nam" : "Nữ";
                tabularStudent.DateOfBirth = studentInClass.Student_Student.StudentBirthday;
                tabularStudent.StringDateOfBirth = tabularStudent.DateOfBirth.ToString("dd/MM/yyyy");
                tabularStudents.Add(tabularStudent);
            }

            return tabularStudents;
        }

        public TabularClass GetTabularClass(Configuration_Year year, Student_Student student)
        {
            ClassBL classBL = new ClassBL(school);
            TabularClass tabularClass = null;
            Class_Class Class = studentDA.GetClass(year, student);
            if (Class != null)
            {
                tabularClass = classBL.GetTabularClass(Class);
            }

            return tabularClass;
        }

        public List<StudentDropdownListItem> GetStudents(Category_Faculty faculty, Category_Grade grade, Class_Class Class)
        {
            List<Student_StudentInClass> studentInClasses = new List<Student_StudentInClass>();
            List<StudentDropdownListItem> students = new List<StudentDropdownListItem>();
            StudentDropdownListItem student = null;

            if (Class == null)
            {
                if (faculty == null)
                {
                    if (grade == null)
                    {
                        studentInClasses = studentDA.GetStudentInClasses();
                    }
                    else
                    {
                        studentInClasses = studentDA.GetStudentInClasses(grade);
                    }
                }
                else
                {
                    if (grade == null)
                    {
                        studentInClasses = studentDA.GetStudentInClasses(faculty);
                    }
                    else
                    {
                        studentInClasses = studentDA.GetStudentInClasses(grade, faculty);
                    }
                }
            }
            else
            {
                studentInClasses = studentDA.GetStudentInClasses(Class);
            }

            foreach (Student_StudentInClass studentInClass in studentInClasses)
            {
                student = new StudentDropdownListItem();
                student.StudentId = studentInClass.StudentId;
                student.StudentCode = studentInClass.Student_Student.StudentCode;
                student.StudentName = studentInClass.Student_Student.FullName;
                student.StudentInClassId = studentInClass.StudentInClassId;

                students.Add(student);
            }

            return students;
        }

        public List<Configuration_Year> GetYears(Student_Student student)
        {
            return studentDA.GetYears(student);
        }

        public Class_Class GetClass(Student_Student student, Configuration_Year year)
        {
            return studentDA.GetClass(year, student);
        }

        public Class_Class GetLastedClass(Student_Student student)
        {
            return studentDA.GetLastedClass(student);
        }

        public List<TabularHanhKiemHocSinh> GetListHanhKiemHocSinh(int ClassId, int TermId, int pageCurrentIndex, int pageSize, out double totalRecords)
        {
            return studentDA.GetListHanhKiemHocSinh(ClassId, TermId,
                pageCurrentIndex, pageSize, out totalRecords);
        }

        public void UpdateStudenTermConduct(Class_Class Class, Configuration_Term term, Student_Student student, Category_Conduct conduct)
        {
            studentDA.UpdateStudenTermConduct(Class, term, student, conduct);
        }
    }
}
