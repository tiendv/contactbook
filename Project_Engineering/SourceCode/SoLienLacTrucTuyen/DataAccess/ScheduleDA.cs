using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoLienLacTrucTuyen.BusinessEntity;

namespace SoLienLacTrucTuyen.DataAccess
{
    public class ScheduleDA : BaseDA
    {
        public ScheduleDA(School school)
            : base(school)
        {
        }

        public void InsertSchedule(LopHoc_MonHocTKB schedule)
        {
            // Insert new schedule
            db.LopHoc_MonHocTKBs.InsertOnSubmit(schedule);
            db.SubmitChanges();
            //---

            // Check and insert DiemMonHocHocKy for HocSinh
            int iClassId = schedule.MaLopHoc;
            int iSubjectId = schedule.MaMonHoc;
            int iTermId = schedule.MaHocKy;
            IQueryable<HocSinh_HocSinhLopHoc> iqStudentsInClass = from stdsInCls in db.HocSinh_HocSinhLopHocs
                                                                  where stdsInCls.MaLopHoc == iClassId
                                                                  select stdsInCls;
            if (iqStudentsInClass.Count() != 0)
            {
                HocSinh_HocSinhLopHoc firstHsLop = iqStudentsInClass.First();
                IQueryable<HocSinh_DiemMonHocHocKy> iqStudentTermSubjectMark;
                iqStudentTermSubjectMark = from stdTermSubjMark in db.HocSinh_DiemMonHocHocKies
                                           where stdTermSubjMark.MaHocSinhLopHoc == firstHsLop.MaHocSinhLopHoc
                                              && stdTermSubjMark.MaMonHoc == iSubjectId
                                           select stdTermSubjMark;
                if (iqStudentTermSubjectMark.Count() == 0)
                {
                    HocSinh_DiemMonHocHocKy studentTermSubjectMark = null;
                    foreach (HocSinh_HocSinhLopHoc studentInClass in iqStudentsInClass)
                    {
                        studentTermSubjectMark = new HocSinh_DiemMonHocHocKy();
                        studentTermSubjectMark.MaHocSinhLopHoc = studentInClass.MaHocSinhLopHoc;
                        studentTermSubjectMark.MaMonHoc = iSubjectId;
                        studentTermSubjectMark.MaHocKy = iTermId;
                        studentTermSubjectMark.DiemTB = -1;

                        db.HocSinh_DiemMonHocHocKies.InsertOnSubmit(studentTermSubjectMark);
                    }

                    db.SubmitChanges();
                }
            }
        }

        public void UpdateSchedule(LopHoc_MonHocTKB editedSchedule, DanhMuc_MonHoc subject, LopHoc_GiaoVien teacher)
        {
            IQueryable<LopHoc_MonHocTKB> iqNewSubjectedSchedule;
            iqNewSubjectedSchedule = from schdl in db.LopHoc_MonHocTKBs
                                     where schdl.MaMonHoc == subject.MaMonHoc
                                     select schdl;
            bool bAdd = (iqNewSubjectedSchedule.Count() != 0) ? false : true;

            LopHoc_MonHocTKB schedule = (from schdl in db.LopHoc_MonHocTKBs
                                         where schdl.MaMonHocTKB == editedSchedule.MaMonHocTKB
                                         select schdl).First();
            int iOriginalSubjectId = schedule.MaMonHoc; // store
            schedule.MaMonHoc = subject.MaMonHoc;
            schedule.MaGiaoVien = teacher.MaGiaoVien;
            db.SubmitChanges();

            IQueryable<LopHoc_MonHocTKB> iqOrginalSubjectedSchedule;
            iqOrginalSubjectedSchedule = from schdl in db.LopHoc_MonHocTKBs
                                         where schdl.MaMonHoc == iOriginalSubjectId
                                         select schdl;
            bool bRemove = (iqOrginalSubjectedSchedule.Count() != 0) ? false : true;

            IQueryable<HocSinh_HocSinhLopHoc> iqStudentsInClass = from stdInCls in db.HocSinh_HocSinhLopHocs
                                                                  where stdInCls.MaLopHoc == schedule.MaLopHoc
                                                                  select stdInCls;

            HocSinh_DiemMonHocHocKy studentTermSubjectMark = null;
            if (bAdd)
            {
                foreach (HocSinh_HocSinhLopHoc studentsInClass in iqStudentsInClass)
                {
                    studentTermSubjectMark = new HocSinh_DiemMonHocHocKy();
                    studentTermSubjectMark.MaHocSinhLopHoc = studentsInClass.MaHocSinhLopHoc;
                    studentTermSubjectMark.MaMonHoc = subject.MaMonHoc;
                    studentTermSubjectMark.MaHocKy = schedule.MaHocKy;
                    studentTermSubjectMark.DiemTB = -1;
                    db.HocSinh_DiemMonHocHocKies.InsertOnSubmit(studentTermSubjectMark);
                }
                db.SubmitChanges();
            }

            if (bRemove)
            {
                foreach (HocSinh_HocSinhLopHoc studentsInClass in iqStudentsInClass)
                {
                    IQueryable<HocSinh_DiemMonHocHocKy> iqStudentTermSubjectMark;
                    iqStudentTermSubjectMark = from stdTermSubjMark in db.HocSinh_DiemMonHocHocKies
                                               where stdTermSubjMark.MaHocSinhLopHoc == studentsInClass.MaHocSinhLopHoc
                                               && stdTermSubjMark.MaMonHoc == iOriginalSubjectId
                                               select stdTermSubjMark;
                    if (iqStudentTermSubjectMark.Count() != 0)
                    {
                        studentTermSubjectMark = iqStudentTermSubjectMark.First();
                        db.HocSinh_DiemMonHocHocKies.DeleteOnSubmit(studentTermSubjectMark);
                    }
                }
                db.SubmitChanges();
            }
        }

        public void DeleteSchedule(LopHoc_MonHocTKB deletedSchedule)
        {
            LopHoc_MonHocTKB schedule = null;

            IQueryable<LopHoc_MonHocTKB> iqSchedule = from schd in db.LopHoc_MonHocTKBs
                                                      where schd.MaMonHocTKB == deletedSchedule.MaMonHocTKB
                                                      select schd;
            if (iqSchedule.Count() != 0)
            {
                schedule = iqSchedule.First();

                int iClasId = schedule.MaLopHoc;
                int iOriginalSubjectId = schedule.MaMonHoc;
                int iTermId = schedule.MaHocKy;

                db.LopHoc_MonHocTKBs.DeleteOnSubmit(schedule);
                db.SubmitChanges();

                IQueryable<DanhMuc_MonHoc> iqScheduledSubjects = from scheduledSubj in db.LopHoc_MonHocTKBs
                                                                 where scheduledSubj.MaMonHoc == iOriginalSubjectId
                                                                 select scheduledSubj.DanhMuc_MonHoc;

                if (iqScheduledSubjects.Count() == 0)
                {
                    IQueryable<HocSinh_DiemMonHocHocKy> iqStudentTermSubjectMark;
                    iqStudentTermSubjectMark = from studTermSubjMark in db.HocSinh_DiemMonHocHocKies
                                               where studTermSubjMark.MaHocKy == iTermId
                                                 && studTermSubjMark.MaMonHoc == iOriginalSubjectId
                                                 && studTermSubjMark.HocSinh_HocSinhLopHoc.MaLopHoc == iClasId
                                               select studTermSubjMark;
                    if (iqStudentTermSubjectMark.Count() != 0)
                    {
                        foreach (HocSinh_DiemMonHocHocKy studentTermSubjectMark in iqStudentTermSubjectMark)
                        {
                            db.HocSinh_DiemMonHocHocKies.DeleteOnSubmit(studentTermSubjectMark);
                        }
                        db.SubmitChanges();
                    }
                }
            }
        }

        public LopHoc_MonHocTKB GetSchedule(int scheduleId)
        {
            LopHoc_MonHocTKB schedule = null;

            IQueryable<LopHoc_MonHocTKB> iqSchedule = from schd in db.LopHoc_MonHocTKBs
                                                      where schd.MaMonHocTKB == scheduleId
                                                      select schd;
            if (iqSchedule.Count() != 0)
            {
                schedule = iqSchedule.First();
            }

            return schedule;
        }

        public LopHoc_MonHocTKB GetSchedule(LopHoc_Lop Class, CauHinh_HocKy term, CauHinh_Thu dayInweek, DanhMuc_Tiet teachingPeriod)
        {
            LopHoc_MonHocTKB schedule = null;
            IQueryable<LopHoc_MonHocTKB> iqSchedule = from schd in db.LopHoc_MonHocTKBs
                                                      where schd.MaLopHoc == Class.MaLopHoc && schd.MaThu == dayInweek.MaThu
                                                      && schd.MaHocKy == term.MaHocKy
                                                      && schd.MaTiet == teachingPeriod.MaTiet
                                                      select schd;

            if (iqSchedule.Count() != 0)
            {
                schedule = iqSchedule.First();
            }

            return schedule;
        }

        public List<LopHoc_MonHocTKB> GetSchedules(LopHoc_Lop Class, CauHinh_HocKy term, CauHinh_Thu dayInweek, CauHinh_Buoi session)
        {
            List<LopHoc_MonHocTKB> schedules = new List<LopHoc_MonHocTKB>();

            IQueryable<LopHoc_MonHocTKB> iqSchedule = from schd in db.LopHoc_MonHocTKBs
                                                      where schd.MaLopHoc == Class.MaLopHoc && schd.MaHocKy == term.MaHocKy
                                                      && schd.MaThu == dayInweek.MaThu && schd.MaBuoi == session.MaBuoi
                                                      select schd;
            if (iqSchedule.Count() != 0)
            {
                schedules = iqSchedule.OrderBy(teachingPeriod => teachingPeriod.MaTiet).ToList();
            }

            return schedules;
        }        

        public List<DanhMuc_MonHoc> GetScheduledSubjects(LopHoc_Lop Class, CauHinh_HocKy term)
        {
            List<DanhMuc_MonHoc> scheduledSubjects = new List<DanhMuc_MonHoc>();

            IQueryable<DanhMuc_MonHoc> iqScheduledSubject;
            iqScheduledSubject = from subj in db.DanhMuc_MonHocs
                                 join schedule in db.LopHoc_MonHocTKBs on subj.MaMonHoc equals schedule.MaMonHoc
                                 where schedule.MaLopHoc == Class.MaLopHoc && schedule.MaHocKy == term.MaHocKy
                                 select subj;

            if (iqScheduledSubject.Count() != 0)
            {
                scheduledSubjects = iqScheduledSubject.OrderBy(subj => subj.TenMonHoc)
                    .GroupBy(c => c.MaMonHoc).Select(g => g.First()).ToList();
            }

            return scheduledSubjects;
        }

        public bool ScheduleExists(LopHoc_Lop Class, DanhMuc_MonHoc subject, CauHinh_HocKy term, CauHinh_Thu dayInweek, CauHinh_Buoi session)
        {
            IQueryable<LopHoc_MonHocTKB> iqSchedule = from schd in db.LopHoc_MonHocTKBs
                                                      where schd.MaLopHoc == Class.MaLopHoc && schd.MaMonHoc == subject.MaMonHoc
                                                      && schd.MaHocKy == term.MaHocKy && schd.MaThu == dayInweek.MaThu
                                                      && schd.MaTiet == session.MaBuoi
                                                      select schd;

            if (iqSchedule.Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ScheduleExists(LopHoc_GiaoVien teacher)
        {
            IQueryable<LopHoc_MonHocTKB> iqSchedule = from schd in db.LopHoc_MonHocTKBs
                                                      where schd.MaGiaoVien == teacher.MaGiaoVien
                                                      select schd;

            if (iqSchedule.Count() != 0)
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
